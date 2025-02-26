using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using Website_Ban_Linh_Kien.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class MomoController : Controller
    {
        private readonly IMomoService _momoService;
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MomoController(IMomoService momoService, DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _momoService = momoService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
                
                Console.WriteLine($"Using RequestType: {request.RequestType}");
                
                var response = await _momoService.CreatePaymentAsync(
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
                    // Lấy thông tin đơn hàng từ session
                    var shippingAddress = HttpContext.Session.GetString("ShippingAddress");
                    var cartItemsJson = HttpContext.Session.GetString("CartItems");
                    var sessionOrderId = HttpContext.Session.GetString("OrderId");
                    
                    Console.WriteLine($"Session data - ShippingAddress: {shippingAddress}");
                    Console.WriteLine($"Session data - CartItems: {cartItemsJson}");
                    Console.WriteLine($"Session data - OrderId: {sessionOrderId}");
                    
                    // Nếu session bị mất, chỉ ghi nhận thông tin và chuyển hướng đến trang thành công
                    if (string.IsNullOrEmpty(cartItemsJson))
                    {
                        Console.WriteLine("WARNING: CartItems is null or empty in session");
                        
                        // KHÔNG tạo thanh toán ở đây vì đơn hàng chưa tồn tại
                        // Chuyển hướng đến trang thành công với thông tin tối thiểu
                        return RedirectToAction("PaymentSuccess", "PaymentResult", new { 
                            orderId = orderId, 
                            transId = transId,
                            amount = amount
                        });
                    }
                    
                    // Xử lý tạo đơn hàng nếu có đủ thông tin
                    // ... (code tạo đơn hàng)
                    
                    return RedirectToAction("PaymentSuccess", "PaymentResult", new { 
                        orderId = orderId, 
                        transId = transId,
                        amount = amount
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
        public IActionResult MomoNotify([FromBody] MomoIPN ipn)
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
                    
                    // Kiểm tra xem đơn hàng đã tồn tại chưa
                    var existingOrder = _context.Donhangs.FirstOrDefault(d => d.IdDh == ipn.OrderId);
                    if (existingOrder != null)
                    {
                        // Chỉ tạo thanh toán nếu đơn hàng đã tồn tại
                        var existingPayment = _context.Thanhtoans.FirstOrDefault(t => t.IdDh == ipn.OrderId);
                        if (existingPayment == null)
                        {
                            var thanhtoan = new Thanhtoan
                            {
                                IdTt = Guid.NewGuid().ToString(),
                                IdDh = ipn.OrderId,
                                Trangthai = "Thành công",
                                Tienthanhtoan = ipn.Amount,
                                Ngaythanhtoan = DateTime.Now,
                                // Các trường khác tùy theo model của bạn
                                Mathanhtoan = ipn.TransId,
                                Noidungthanhtoan = $"Thanh toán qua Momo, mã giao dịch: {ipn.TransId}"
                            };
                            
                            _context.Thanhtoans.Add(thanhtoan);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: Order {ipn.OrderId} does not exist, cannot create payment");
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