using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
            
            // Đảm bảo extraData không bao giờ là null
            string actualExtraData = string.IsNullOrEmpty(extraData) ? "" : extraData;
            
            // Thêm timestamp vào orderId để tránh trùng lặp
            string uniqueOrderId = $"{orderId}_{DateTime.Now.Ticks}";
            
            // Tạo request object theo định dạng mới của Momo API v2
            var request = new MomoCreatePaymentRequest
            {
                PartnerCode = _options.PartnerCode,
                AccessKey = _options.AccessKey,
                RequestId = requestId,
                Amount = amount,
                OrderId = uniqueOrderId,
                OrderInfo = orderInfo,
                ReturnUrl = _options.ReturnUrl,
                NotifyUrl = _options.NotifyUrl,
                RequestType = actualRequestType,
                ExtraData = actualExtraData
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
            
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(responseContent);
            
            // Lưu trữ ánh xạ giữa uniqueOrderId và orderId gốc để xử lý callback
            HttpContext.Session.SetString($"MomoOriginalOrderId_{uniqueOrderId}", orderId);
            
            return momoResponse;
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
                
                // Kiểm tra xem đơn hàng có tồn tại không
                var orderExists = await _context.Donhangs.AnyAsync(d => d.IdDh == request.OrderId);
                
                if (!orderExists)
                {
                    Console.WriteLine($"Order {request.OrderId} not found");
                    return BadRequest(new { success = false, message = $"Không tìm thấy đơn hàng {request.OrderId}" });
                }
                
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
                
                // Lưu thông tin người nhận vào session
                if (!string.IsNullOrEmpty(request.ReceiverName))
                {
                    HttpContext.Session.SetString("ReceiverName", request.ReceiverName);
                    Console.WriteLine($"Saved receiver name to session: {request.ReceiverName}");
                }
                
                if (!string.IsNullOrEmpty(request.ReceiverPhone))
                {
                    HttpContext.Session.SetString("ReceiverPhone", request.ReceiverPhone);
                    Console.WriteLine($"Saved receiver phone to session: {request.ReceiverPhone}");
                }
                
                if (!string.IsNullOrEmpty(request.Email))
                {
                    HttpContext.Session.SetString("Email", request.Email);
                    Console.WriteLine($"Saved email to session: {request.Email}");
                }
                
                // Lưu thêm OrderId vào session
                HttpContext.Session.SetString("OrderId", request.OrderId);
                HttpContext.Session.SetString("OrderAmount", request.Amount.ToString());
                
                // Kiểm tra xem đã có bản ghi thanh toán cho đơn hàng này chưa
                var existingPayment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == request.OrderId);
                    
                if (existingPayment == null)
                {
                    // Tạo bản ghi thanh toán với trạng thái "Chờ thanh toán"
                    var newPaymentId = await GenerateNewPaymentId();
                    var payment = new Thanhtoan
                    {
                        IdTt = newPaymentId,
                        IdDh = request.OrderId,
                        Trangthai = "Chờ thanh toán",
                        Tienthanhtoan = request.Amount,
                        Ngaythanhtoan = DateTime.Now,
                        Noidungthanhtoan = $"Thanh toán Momo cho đơn hàng {request.OrderId}",
                        Mathanhtoan = DateTime.Now.Ticks.ToString() // Tạm thời sử dụng timestamp, sẽ cập nhật sau khi có transId từ Momo
                    };
                    
                    _context.Thanhtoans.Add(payment);
                    await _context.SaveChangesAsync();
                }
                
                Console.WriteLine($"Using RequestType: {request.RequestType}");
                
                // Đảm bảo ExtraData không null
                string extraData = string.IsNullOrEmpty(request.ExtraData) ? "" : request.ExtraData;
                
                // Sử dụng phương thức CreatePaymentAsync đã được gộp vào controller
                var response = await CreatePaymentAsync(
                    request.Amount,
                    request.OrderId,
                    request.OrderInfo,
                    extraData,
                    request.RequestType);

                if (response.ErrorCode != 0)
                {
                    return BadRequest(new { success = false, message = response.Message });
                }

                return Ok(new { success = true, payUrl = response.PayUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatePayment: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallback(string partnerCode, string orderId, string requestId, 
            long amount, string orderInfo, string orderType, string transId, 
            int resultCode, string message, string payType, long responseTime, string extraData, string signature)
        {
            Console.WriteLine($"Received Momo callback: {JsonConvert.SerializeObject(new { partnerCode, orderId, requestId, amount, orderInfo, orderType, transId, resultCode, message, payType, responseTime, extraData, signature })}");
            Console.WriteLine($"ResultCode: {resultCode}, Message: {message}");
            
            // Lấy orderId gốc từ session
            string originalOrderId = HttpContext.Session.GetString($"MomoOriginalOrderId_{orderId}");
            if (string.IsNullOrEmpty(originalOrderId))
            {
                // Nếu không tìm thấy trong session, thử tách orderId từ định dạng "orderId_timestamp"
                int underscoreIndex = orderId.LastIndexOf('_');
                if (underscoreIndex > 0)
                {
                    originalOrderId = orderId.Substring(0, underscoreIndex);
                }
                else
                {
                    originalOrderId = orderId; // Sử dụng orderId gốc nếu không tìm thấy dấu gạch dưới
                }
            }
            
            // Kiểm tra kết quả thanh toán
            if (resultCode == 0) // Thanh toán thành công
            {
                try {
                    // Cập nhật trạng thái đơn hàng
                    var order = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == originalOrderId);
                    if (order != null)
                    {
                        // Cập nhật trạng thái đơn hàng thành "Đã thanh toán"
                        order.Trangthai = "Đã thanh toán";
                        order.Phuongthucthanhtoan = "Momo";
                        _context.Donhangs.Update(order);
                        
                        // Cập nhật bản ghi thanh toán thay vì tạo mới
                        var existingPayment = await _context.Thanhtoans.FirstOrDefaultAsync(t => t.IdDh == originalOrderId);
                        
                        if (existingPayment != null)
                        {
                            existingPayment.Trangthai = "Đã thanh toán";
                            existingPayment.Ngaythanhtoan = DateTime.Now;
                            existingPayment.Mathanhtoan = transId;
                            existingPayment.Noidungthanhtoan = $"Thanh toán qua Momo, mã giao dịch: {transId}";
                            
                            _context.Thanhtoans.Update(existingPayment);
                        }
                        else
                        {
                            // Tạo mã thanh toán mới sử dụng phương thức GenerateNewPaymentId
                            var paymentId = await GenerateNewPaymentId();
                            
                            var thanhtoan = new Thanhtoan
                            {
                                IdTt = paymentId,
                                IdDh = originalOrderId,
                                Trangthai = "Đã thanh toán",
                                Tienthanhtoan = amount,
                                Ngaythanhtoan = DateTime.Now,
                                Mathanhtoan = transId,
                                Noidungthanhtoan = $"Thanh toán qua Momo, mã giao dịch: {transId}"
                            };
                            
                            _context.Thanhtoans.Add(thanhtoan);
                        }
                        
                        // Lưu các thay đổi vào cơ sở dữ liệu
                        await _context.SaveChangesAsync();
                        
                        // Xóa giỏ hàng nếu khách hàng đã đăng nhập
                        if (order.IdKh != null)
                        {
                            var cart = await _context.Giohangs
                                .Include(g => g.Chitietgiohangs)
                                .Where(g => g.IdKh == order.IdKh)
                                .OrderByDescending(g => g.Thoigiancapnhat)
                                .FirstOrDefaultAsync();
                                
                            if (cart != null)
                            {
                                _context.Chitietgiohangs.RemoveRange(cart.Chitietgiohangs);
                                _context.Giohangs.Remove(cart);
                            }
                        }
                        
                        Console.WriteLine($"Updated order status for {originalOrderId} to 'Đã thanh toán'");
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: Order {originalOrderId} not found");
                        return RedirectToAction("PaymentFailed", "PaymentResult", new { 
                            errorMessage = "Không tìm thấy đơn hàng" 
                        });
                    }
                    
                    // Chuyển hướng đến trang thành công với tham số clearCart
                    return RedirectToAction("PaymentSuccess", "PaymentResult", new { 
                        orderId = originalOrderId, 
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
                try
                {
                    // Cập nhật trạng thái đơn hàng thành "Thanh toán không thành công"
                    var order = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == originalOrderId);
                    if (order != null)
                    {
                        order.Trangthai = "Thanh toán không thành công";
                        
                        // Cập nhật bản ghi thanh toán
                        var payment = await _context.Thanhtoans.FirstOrDefaultAsync(t => t.IdDh == originalOrderId);
                        if (payment != null)
                        {
                            payment.Trangthai = "Thanh toán thất bại";
                            _context.Thanhtoans.Update(payment);
                        }
                        
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating failed payment status: {ex.Message}");
                }
                
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
                    return BadRequest(new { success = false, message = "Invalid IPN data" });
                }
                
                // Lấy orderId gốc từ session hoặc từ định dạng "orderId_timestamp"
                string originalOrderId = HttpContext.Session.GetString($"MomoOriginalOrderId_{ipn.OrderId}");
                if (string.IsNullOrEmpty(originalOrderId))
                {
                    // Nếu không tìm thấy trong session, thử tách orderId từ định dạng "orderId_timestamp"
                    int underscoreIndex = ipn.OrderId.LastIndexOf('_');
                    if (underscoreIndex > 0)
                    {
                        originalOrderId = ipn.OrderId.Substring(0, underscoreIndex);
                    }
                    else
                    {
                        originalOrderId = ipn.OrderId; // Sử dụng orderId gốc nếu không tìm thấy dấu gạch dưới
                    }
                }
                
                // Xử lý thông báo từ Momo
                if (ipn.ResultCode == 0)
                {
                    // Thanh toán thành công
                    Console.WriteLine($"Payment successful for order {originalOrderId}, transaction {ipn.TransId}");
                    
                    // Cập nhật trạng thái đơn hàng
                    var order = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == originalOrderId);
                    if (order != null)
                    {
                        order.Trangthai = "Đã thanh toán";
                        order.Phuongthucthanhtoan = "Momo";
                        
                        // Cập nhật bản ghi thanh toán thay vì tạo mới
                        var existingPayment = await _context.Thanhtoans.FirstOrDefaultAsync(t => t.IdDh == originalOrderId);
                        
                        if (existingPayment != null)
                        {
                            existingPayment.Trangthai = "Đã thanh toán";
                            existingPayment.Ngaythanhtoan = DateTime.Now;
                            
                            // Cập nhật Mathanhtoan thành Transaction ID
                            existingPayment.Mathanhtoan = ipn.TransId;
                            existingPayment.Noidungthanhtoan = $"Thanh toán qua Momo cho đơn hàng {originalOrderId}";
                            
                            _context.Thanhtoans.Update(existingPayment);
                        }
                        else
                        {
                            // Tạo bản ghi thanh toán nếu không tìm thấy
                            var newPaymentId = await GenerateNewPaymentId();
                            var payment = new Thanhtoan
                            {
                                IdTt = newPaymentId,
                                IdDh = originalOrderId,
                                Trangthai = "Đã thanh toán",
                                Tienthanhtoan = ipn.Amount,
                                Ngaythanhtoan = DateTime.Now,
                                Mathanhtoan = ipn.TransId,
                                Noidungthanhtoan = $"Thanh toán qua Momo cho đơn hàng {originalOrderId}"
                            };
                            
                            _context.Thanhtoans.Add(payment);
                        }
                        
                        // Xóa giỏ hàng của khách hàng
                        if (order.IdKh != null)
                        {
                            var cart = await _context.Giohangs
                                .Include(g => g.Chitietgiohangs)
                                .Where(g => g.IdKh == order.IdKh)
                                .OrderByDescending(g => g.Thoigiancapnhat)
                                .FirstOrDefaultAsync();
                            
                            if (cart != null)
                            {
                                _context.Chitietgiohangs.RemoveRange(cart.Chitietgiohangs);
                                _context.Giohangs.Remove(cart);
                            }
                        }
                        
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"Updated order status for {originalOrderId} to 'Đã thanh toán' and cleared cart");
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: Order {originalOrderId} not found");
                    }
                }
                else
                {
                    // Thanh toán thất bại
                    Console.WriteLine($"Payment failed for order {originalOrderId}, code: {ipn.ResultCode}, message: {ipn.Message}");
                    
                    // Cập nhật trạng thái đơn hàng thành "Thanh toán không thành công"
                    var order = await _context.Donhangs.FirstOrDefaultAsync(d => d.IdDh == originalOrderId);
                    if (order != null)
                    {
                        order.Trangthai = "Thanh toán không thành công";
                        
                        // Cập nhật bản ghi thanh toán
                        var payment = await _context.Thanhtoans.FirstOrDefaultAsync(t => t.IdDh == originalOrderId);
                        if (payment != null)
                        {
                            payment.Trangthai = "Thanh toán thất bại";
                            _context.Thanhtoans.Update(payment);
                        }
                        
                        await _context.SaveChangesAsync();
                    }
                }
                
                // Trả về kết quả cho Momo
                return Ok(new { success = true, message = "IPN received" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MomoNotify: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        private async Task<string> GenerateNewPaymentId()
        {
            var lastPaymentId = await _context.Thanhtoans
                .OrderByDescending(t => t.IdTt)
                .Select(t => t.IdTt)
                .FirstOrDefaultAsync();
                
            int nextId = 1;
            if (lastPaymentId != null && lastPaymentId.StartsWith("TT") && 
                int.TryParse(lastPaymentId.Substring(2), out int currentId))
            {
                nextId = currentId + 1;
            }
            
            return $"TT{nextId:D6}";
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
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string Email { get; set; }
    }

    public class CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 