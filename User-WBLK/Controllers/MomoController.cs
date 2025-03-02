using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class MomoController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        // Thuộc tính từ MomoService
        private readonly MomoOptionModel _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public MomoController(DatabaseContext context, IHttpContextAccessor httpContextAccessor, 
                             IOptions<MomoOptionModel> options, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        // Phương thức từ MomoService
        public async Task<MomoCreatePaymentResponse> CreatePaymentAsync(long amount, string orderId, string orderInfo, string extraData = "", string requestType = null)
        {
            var requestId = DateTime.UtcNow.Ticks.ToString();
            
            // Sử dụng requestType từ tham số hoặc từ cấu hình
            string actualRequestType = requestType ?? _options.RequestType;
            
            // Tạo request object theo định dạng mới của Momo API v2
            var request = new MomoCreatePaymentRequest
            {
                PartnerCode = _options.PartnerCode,
                AccessKey = _options.AccessKey,
                RequestId = requestId,
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                ReturnUrl = _options.ReturnUrl,
                NotifyUrl = _options.NotifyUrl,
                RequestType = actualRequestType,
                ExtraData = extraData
            };
            
            // Tạo chuỗi hash theo định dạng mới của Momo API v2
            var rawHash = $"accessKey={request.AccessKey}" +
                         $"&amount={request.Amount}" +
                         $"&extraData={request.ExtraData}" +
                         $"&ipnUrl={request.NotifyUrl}" +
                         $"&orderId={request.OrderId}" +
                         $"&orderInfo={request.OrderInfo}" +
                         $"&partnerCode={request.PartnerCode}" +
                         $"&redirectUrl={request.ReturnUrl}" +
                         $"&requestId={request.RequestId}" +
                         $"&requestType={request.RequestType}";

            // Tạo chữ ký
            var signature = ComputeHmacSha256(rawHash, _options.SecretKey);
            request.Signature = signature;

            var client = _httpClientFactory.CreateClient();
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Gọi API và xử lý response
            var response = await client.PostAsync(_options.MomoApiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            // Log response để debug
            Console.WriteLine($"Momo API Response: {responseContent}");
            
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(responseContent);
        }

        // Phương thức từ MomoService
        public bool ValidateSignature(string rawHash, string signature)
        {
            var hash = ComputeHmacSha256(rawHash, _options.SecretKey);
            return hash.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        // Phương thức từ MomoService
        public string GetAccessKey()
        {
            return _options.AccessKey;
        }

        // Phương thức từ MomoService
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(key))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                Console.WriteLine($"CreatePayment request: {JsonConvert.SerializeObject(request)}");
                
                // Lưu thông tin đơn hàng vào session để sử dụng sau khi thanh toán
                if (!string.IsNullOrEmpty(request.ShippingAddress))
                {
                    HttpContext.Session.SetString("ShippingAddress", request.ShippingAddress);
                    Console.WriteLine($"Saved shipping address to session: {request.ShippingAddress}");
                }
                
                if (!string.IsNullOrEmpty(request.CartItems))
                {
                    HttpContext.Session.SetString("CartItems", request.CartItems);
                    Console.WriteLine($"Saved cart items to session: {request.CartItems}");
                }
                
                // Lưu thêm OrderId vào session
                HttpContext.Session.SetString("OrderId", request.OrderId);
                HttpContext.Session.SetString("OrderAmount", request.Amount.ToString());
                
                // Tạo đơn hàng TRƯỚC KHI thanh toán, tương tự như CheckOutController
                if (!string.IsNullOrEmpty(request.CartItems))
                {
                    try
                    {
                        var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(request.CartItems);
                        
                        // Kiểm tra xem đơn hàng đã tồn tại chưa
                        var existingOrder = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == request.OrderId);
                        if (existingOrder == null)
                        {
                            // Lấy thông tin khách hàng
                            string customerId = "KH000001"; // Mặc định
                            if (User.Identity?.IsAuthenticated == true)
                            {
                                var userCustomerId = User.FindFirstValue("CustomerId");
                                if (!string.IsNullOrEmpty(userCustomerId))
                                {
                                    customerId = userCustomerId;
                                }
                            }
                            
                            // Tạo đơn hàng mới
                            var donhang = new Donhang
                            {
                                IdDh = request.OrderId,
                                Ngaydathang = DateTime.Now,
                                Diachigiaohang = request.ShippingAddress,
                                Tongtien = request.Amount,
                                Trangthai = "Chờ xác nhận",
                                Phuongthucthanhtoan = "Momo",
                                IdKh = customerId
                            };
                            
                            _context.Donhangs.Add(donhang);
                            await _context.SaveChangesAsync();
                            
                            // Tạo chi tiết đơn hàng
                            int detailStartId = 1;
                            var lastDetailId = await _context.Chitietdonhangs
                                .OrderByDescending(c => c.Idchitietdonhang)
                                .Select(c => c.Idchitietdonhang)
                                .FirstOrDefaultAsync();
                                
                            if (lastDetailId != null && int.TryParse(lastDetailId.Substring(4), out int currentId))
                            {
                                detailStartId = currentId + 1;
                            }
                            
                            foreach (var item in cartItems)
                            {
                                var product = await _context.Sanphams.FindAsync(item.ProductId);
                                if (product != null)
                                {
                                    var chitietdonhang = new Chitietdonhang
                                    {
                                        Idchitietdonhang = $"CTDH{detailStartId:D5}",
                                        IdDh = request.OrderId,
                                        IdSp = item.ProductId,
                                        Soluongsanpham = item.Quantity,
                                        Dongia = item.Price
                                    };
                                    
                                    _context.Chitietdonhangs.Add(chitietdonhang);
                                    
                                    // Cập nhật số lượng tồn kho
                                    product.Soluongton -= item.Quantity;
                                    product.Damuahang += item.Quantity;
                                    
                                    detailStartId++;
                                }
                            }
                            
                            await _context.SaveChangesAsync();
                            Console.WriteLine($"Created order {request.OrderId} with {cartItems.Count} items");
                        }
                        else
                        {
                            Console.WriteLine($"Order {request.OrderId} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating order: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        // Tiếp tục xử lý thanh toán ngay cả khi tạo đơn hàng thất bại
                    }
                }
                
                Console.WriteLine($"Using RequestType: {request.RequestType}");
                
                // Sử dụng phương thức CreatePaymentAsync đã được gộp vào controller
                var response = await CreatePaymentAsync(
                    request.Amount,
                    request.OrderId,
                    request.OrderInfo,
                    request.ExtraData,
                    request.RequestType);

                if (response.ErrorCode != 0)
                {
                    return BadRequest(new { message = response.Message });
                }

                return Ok(new { payUrl = response.PayUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatePayment: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult PaymentCallback(string partnerCode, string orderId, string requestId, 
            long amount, string orderInfo, string orderType, string transId, 
            int resultCode, string message, string payType, long responseTime, string extraData, string signature)
        {
            Console.WriteLine($"Received Momo callback: {JsonConvert.SerializeObject(new { partnerCode, orderId, requestId, amount, orderInfo, orderType, transId, resultCode, message, payType, responseTime, extraData, signature })}");
            Console.WriteLine($"ResultCode: {resultCode}, Message: {message}");
            
            // Kiểm tra kết quả thanh toán
            if (resultCode == 0) // Thanh toán thành công
            {
                try {
                    // Cập nhật trạng thái đơn hàng
                    var order = _context.Donhangs.FirstOrDefault(d => d.IdDh == orderId);
                    if (order != null)
                    {
                        order.Trangthai = "Đã thanh toán";
                        _context.SaveChanges();
                        Console.WriteLine($"Updated order status for {orderId} to 'Đã thanh toán'");
                        
                        // Tạo thanh toán
                        var existingPayment = _context.Thanhtoans.FirstOrDefault(t => t.IdDh == orderId);
                        if (existingPayment == null)
                        {
                            var thanhtoan = new Thanhtoan
                            {
                                IdTt = Guid.NewGuid().ToString().Substring(0, 10),
                                IdDh = orderId,
                                Trangthai = "Thành công",
                                Tienthanhtoan = amount,
                                Ngaythanhtoan = DateTime.Now,
                                Mathanhtoan = transId,
                                Noidungthanhtoan = $"Thanh toán qua Momo, mã giao dịch: {transId}"
                            };
                            
                            _context.Thanhtoans.Add(thanhtoan);
                            _context.SaveChanges();
                            Console.WriteLine($"Created payment record for order {orderId}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: Order {orderId} not found");
                    }
                    
                    // Chuyển hướng đến trang thành công với tham số clearCart
                    return RedirectToAction("PaymentSuccess", "PaymentResult", new { 
                        orderId = orderId, 
                        transId = transId,
                        amount = amount,
                        clearCart = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PaymentCallback: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    
                    // Chuyển hướng đến trang lỗi với thông báo cụ thể
                    return RedirectToAction("PaymentFailed", "PaymentResult", new { 
                        errorMessage = "Thanh toán thành công nhưng xảy ra lỗi khi xử lý đơn hàng: " + ex.Message 
                    });
                }
            }
            else
            {
                Console.WriteLine($"Payment failed with code: {resultCode}, message: {message}");
                return RedirectToAction("PaymentFailed", "PaymentResult", new { 
                    errorMessage = "Thanh toán thất bại: " + message 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MomoNotify([FromBody] MomoIPN ipn)
        {
            try
            {
                Console.WriteLine($"Received Momo notify: {JsonConvert.SerializeObject(ipn)}");
                
                // Kiểm tra ipn null
                if (ipn == null)
                {
                    Console.WriteLine("Error: MomoIPN object is null");
                    return BadRequest(new { message = "Invalid IPN data" });
                }
                
                // Xử lý thông báo từ Momo
                if (ipn.ResultCode == 0)
                {
                    // Thanh toán thành công
                    // Ghi log và cập nhật trạng thái thanh toán
                    Console.WriteLine($"Payment successful for order {ipn.OrderId}, transaction {ipn.TransId}");
                    
                    // Cập nhật trạng thái đơn hàng
                    var order = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == ipn.OrderId);
                    if (order != null)
                    {
                        order.Trangthai = "Đã thanh toán";
                        
                        // Lấy thông tin khách hàng
                        var customer = await _context.Khachhangs.FindAsync(order.IdKh);
                        if (customer != null)
                        {
                            // Xóa giỏ hàng của khách hàng
                            var cart = await _context.Giohangs
                                .Include(g => g.Chitietgiohangs)
                                .Where(g => g.IdKh == customer.IdKh)
                                .OrderByDescending(g => g.Thoigiancapnhat)
                                .FirstOrDefaultAsync();
                            
                            if (cart != null)
                            {
                                // Xóa chi tiết giỏ hàng
                                _context.Chitietgiohangs.RemoveRange(cart.Chitietgiohangs);
                                
                                // Xóa giỏ hàng
                                _context.Giohangs.Remove(cart);
                                
                                Console.WriteLine($"Removed cart for customer {customer.IdKh}");
                            }
                        }
                        
                        // Tạo thanh toán nếu chưa tồn tại
                        var existingPayment = await _context.Thanhtoans.FirstOrDefaultAsync(t => t.IdDh == ipn.OrderId);
                        if (existingPayment == null)
                        {
                            var thanhtoan = new Thanhtoan
                            {
                                IdTt = Guid.NewGuid().ToString().Substring(0, 10),
                                IdDh = ipn.OrderId,
                                Trangthai = "Thành công",
                                Tienthanhtoan = ipn.Amount,
                                Ngaythanhtoan = DateTime.Now,
                                Mathanhtoan = ipn.TransId,
                                Noidungthanhtoan = $"Thanh toán qua Momo, mã giao dịch: {ipn.TransId}"
                            };
                            
                            _context.Thanhtoans.Add(thanhtoan);
                        }
                        
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"Updated order status for {ipn.OrderId} to 'Đã thanh toán' and cleared cart");
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: Order {ipn.OrderId} not found");
                    }
                }
                else
                {
                    // Thanh toán thất bại
                    Console.WriteLine($"Payment failed for order {ipn.OrderId}, code: {ipn.ResultCode}, message: {ipn.Message}");
                }
                
                return Ok(new { message = "IPN received" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MomoNotify: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

    public class PaymentRequest
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ExtraData { get; set; }
        public string RequestType { get; set; }
        public string ShippingAddress { get; set; }
        public string CartItems { get; set; }
    }

    public class CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 