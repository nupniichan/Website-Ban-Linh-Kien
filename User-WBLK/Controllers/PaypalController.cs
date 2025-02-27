using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class PaypalController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly bool _isSandbox;
        private readonly string _baseUrl;
        private readonly IMemoryCache _cache;

        public PaypalController(DatabaseContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
            
            // Đọc cấu hình từ appsettings.json
            _clientId = _configuration["PayPal:ClientId"] ?? "YOUR_PAYPAL_CLIENT_ID";
            _secret = _configuration["PayPal:Secret"] ?? "YOUR_PAYPAL_SECRET";
            _isSandbox = _configuration.GetValue<bool>("PayPal:UseSandbox", true);
            
            // Chọn URL dựa trên môi trường
            _baseUrl = _isSandbox 
                ? "https://api-m.sandbox.paypal.com" 
                : "https://api-m.paypal.com";
        }

        // Lấy tỷ giá USD/VND từ Vietcombank
        private async Task<decimal> GetExchangeRateFromVCB()
        {
            try
            {
                // Kiểm tra cache trước
                if (_cache.TryGetValue("USD_VND_RATE", out decimal cachedRate))
                {
                    return cachedRate;
                }

                using var client = new HttpClient();
                var response = await client.GetAsync("https://portal.vietcombank.com.vn/Usercontrols/TVPortal.TyGia/pXML.aspx");
                
                if (!response.IsSuccessStatusCode)
                {
                    // Nếu không lấy được, sử dụng giá trị mặc định
                    return 25000m;
                }
                
                var content = await response.Content.ReadAsStringAsync();
                
                // Phân tích XML để lấy tỷ giá USD/VND
                var xml = XDocument.Parse(content);
                var usdElement = xml.Descendants("Exrate")
                    .FirstOrDefault(e => e.Attribute("CurrencyCode")?.Value == "USD");
                
                if (usdElement != null)
                {
                    // Lấy giá bán (Sell) và chuyển đổi sang decimal
                    var sellRateStr = usdElement.Attribute("Sell")?.Value;
                    if (!string.IsNullOrEmpty(sellRateStr))
                    {
                        // Xóa dấu phẩy và khoảng trắng
                        sellRateStr = sellRateStr.Replace(",", "").Replace(".", "").Replace(" ", "");
                        
                        if (decimal.TryParse(sellRateStr, out decimal rate))
                        {
                            // Lưu vào cache trong 1 giờ
                            var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                            
                            _cache.Set("USD_VND_RATE", rate, cacheOptions);
                            
                            Console.WriteLine($"Tỷ giá USD/VND từ Vietcombank: {rate}");
                            return rate;
                        }
                    }
                }
                
                // Nếu không lấy được tỷ giá, sử dụng giá trị mặc định
                return 25000m;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy tỷ giá từ Vietcombank: {ex.Message}");
                return 25000m; // Giá trị mặc định nếu có lỗi
            }
        }

        // Lấy token xác thực từ PayPal
        private async Task<string> GetAccessToken()
        {
            try
            {
                var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
                
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });
                
                var response = await _httpClient.PostAsync($"{_baseUrl}/v1/oauth2/token", content);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PayPal Authentication Error: {responseContent}");
                    throw new Exception($"PayPal Authentication Failed: {response.StatusCode}");
                }
                
                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                
                return tokenResponse.GetProperty("access_token").GetString() ?? "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PayPal Authentication Exception: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaypalPaymentRequest request)
        {
            try
            {
                // Kiểm tra xem đơn hàng có tồn tại không
                var orderExists = await _context.Donhangs.AnyAsync(d => d.IdDh == request.OrderId);
                if (!orderExists)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại" });
                }

                // Lấy tỷ giá USD/VND từ Vietcombank
                var exchangeRate = await GetExchangeRateFromVCB();
                
                // Chuyển đổi VND sang USD
                var usdAmount = request.Amount / exchangeRate;
                
                // Làm tròn đến 2 chữ số thập phân
                usdAmount = Math.Round(usdAmount, 2);

                // Lấy token xác thực
                var accessToken = await GetAccessToken();
                
                // Chuẩn bị request tạo order
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                // Tạo URL callback
                var returnUrl = Url.Action("PaymentSuccess", "Paypal", null, Request.Scheme);
                var cancelUrl = Url.Action("PaymentCancel", "Paypal", null, Request.Scheme);
                
                // Tạo payload cho PayPal API
                var orderRequest = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                        new
                        {
                            amount = new
                            {
                                currency_code = "USD",
                                value = usdAmount.ToString("0.00") // Chuyển đổi VND sang USD với tỷ giá từ Vietcombank
                            },
                            description = request.OrderInfo,
                            custom_id = request.OrderId
                        }
                    },
                    application_context = new
                    {
                        brand_name = "Website Bán Linh Kiện",
                        landing_page = "BILLING",
                        user_action = "PAY_NOW",
                        return_url = returnUrl,
                        cancel_url = cancelUrl
                    }
                };
                
                var jsonContent = JsonSerializer.Serialize(orderRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                // Gọi API tạo order
                var response = await _httpClient.PostAsync($"{_baseUrl}/v2/checkout/orders", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PayPal API Error: {responseContent}");
                    return Json(new { success = false, message = "Lỗi khi tạo đơn hàng PayPal" });
                }
                
                // Parse response
                var orderResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                var orderId = orderResponse.GetProperty("id").GetString();
                
                // Tìm link approve để redirect người dùng
                var links = orderResponse.GetProperty("links").EnumerateArray();
                string approveUrl = "";
                
                foreach (var link in links)
                {
                    if (link.GetProperty("rel").GetString() == "approve")
                    {
                        approveUrl = link.GetProperty("href").GetString() ?? "";
                        break;
                    }
                }
                
                if (string.IsNullOrEmpty(approveUrl))
                {
                    return Json(new { success = false, message = "Không tìm thấy URL thanh toán" });
                }
                
                // Lưu thông tin đơn hàng vào TempData
                TempData["PayPalOrderId"] = orderId;
                TempData["PendingOrderData"] = JsonSerializer.Serialize(request);
                
                // Kiểm tra xem đã có bản ghi thanh toán cho đơn hàng này chưa
                var existingPayment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == request.OrderId && t.Mathanhtoan == orderId);
                    
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
                        Noidungthanhtoan = $"Thanh toán PayPal cho đơn hàng {request.OrderId} | Tỷ giá: 1 USD = {exchangeRate} VND",
                        Mathanhtoan = orderId
                    };
                    
                    _context.Thanhtoans.Add(payment);
                    await _context.SaveChangesAsync();
                }
                
                return Json(new { 
                    success = true, 
                    payUrl = approveUrl,
                    exchangeRate = exchangeRate,
                    usdAmount = usdAmount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating PayPal payment: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo thanh toán PayPal" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(string token)
        {
            try
            {
                // Lấy PayPal Order ID từ TempData
                if (TempData["PayPalOrderId"] is not string paypalOrderId)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                // Lấy thông tin đơn hàng từ TempData
                if (TempData["PendingOrderData"] is not string pendingOrderJson)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                var pendingOrder = JsonSerializer.Deserialize<PaypalPaymentRequest>(pendingOrderJson);
                if (pendingOrder == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                // Lấy access token
                var accessToken = await GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                
                // Gọi API để capture payment
                var response = await _httpClient.PostAsync(
                    $"{_baseUrl}/v2/checkout/orders/{paypalOrderId}/capture",
                    new StringContent("{}", Encoding.UTF8, "application/json")
                );
                
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"PayPal Capture Response: {responseContent}");
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PayPal Capture Error: {responseContent}");
                    return RedirectToAction("PaymentFailed", "PaymentResult", new { errorMessage = "Lỗi khi xác nhận thanh toán PayPal" });
                }
                
                // Parse response
                var captureResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                var status = captureResponse.GetProperty("status").GetString();
                
                // Lấy Transaction ID từ response
                string transactionId = "";
                if (captureResponse.TryGetProperty("purchase_units", out var purchaseUnits))
                {
                    var firstUnit = purchaseUnits.EnumerateArray().FirstOrDefault();
                    if (firstUnit.TryGetProperty("payments", out var payments))
                    {
                        if (payments.TryGetProperty("captures", out var captures))
                        {
                            var firstCapture = captures.EnumerateArray().FirstOrDefault();
                            if (firstCapture.TryGetProperty("id", out var id))
                            {
                                transactionId = id.GetString() ?? "";
                                Console.WriteLine($"Found Transaction ID: {transactionId}");
                            }
                        }
                    }
                }
                
                if (status != "COMPLETED")
                {
                    // Cập nhật trạng thái thanh toán thành "Thanh toán thất bại"
                    var payment = await _context.Thanhtoans
                        .FirstOrDefaultAsync(t => t.IdDh == pendingOrder.OrderId && t.Mathanhtoan == paypalOrderId);
                    
                    if (payment != null)
                    {
                        payment.Trangthai = "Thanh toán thất bại";
                        _context.Thanhtoans.Update(payment);
                    }
                    
                    // Cập nhật trạng thái đơn hàng
                    var orderRecord = await _context.Donhangs.FindAsync(pendingOrder.OrderId);
                    if (orderRecord != null)
                    {
                        orderRecord.Phuongthucthanhtoan = "PayPal";
                        orderRecord.Trangthai = "Thanh toán không thành công";
                        _context.Donhangs.Update(orderRecord);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction("PaymentFailed", "PaymentResult", new { errorMessage = "Thanh toán PayPal chưa hoàn tất" });
                }
                
                // Cập nhật bản ghi thanh toán thay vì tạo mới
                var existingPayment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == pendingOrder.OrderId && t.Mathanhtoan == paypalOrderId);
                    
                if (existingPayment != null)
                {
                    existingPayment.Trangthai = "Đã thanh toán";
                    existingPayment.Ngaythanhtoan = DateTime.Now;
                    
                    // Cập nhật Mathanhtoan thành Transaction ID nếu có
                    if (!string.IsNullOrEmpty(transactionId))
                    {
                        existingPayment.Mathanhtoan = transactionId;
                        existingPayment.Noidungthanhtoan = $"Thanh toán PayPal cho đơn hàng {pendingOrder.OrderId}";
                    }
                    
                    _context.Thanhtoans.Update(existingPayment);
                }
                else
                {
                    // Tạo bản ghi thanh toán nếu không tìm thấy
                    var newPaymentId = await GenerateNewPaymentId();
                    var payment = new Thanhtoan
                    {
                        IdTt = newPaymentId,
                        IdDh = pendingOrder.OrderId,
                        Trangthai = "Đã thanh toán",
                        Tienthanhtoan = pendingOrder.Amount,
                        Ngaythanhtoan = DateTime.Now,
                        Noidungthanhtoan = !string.IsNullOrEmpty(transactionId)
                            ? $"Thanh toán PayPal cho đơn hàng {pendingOrder.OrderId} | Capture ID: {transactionId}"
                            : $"Thanh toán PayPal cho đơn hàng {pendingOrder.OrderId}",
                        Mathanhtoan = !string.IsNullOrEmpty(transactionId) ? transactionId : paypalOrderId
                    };
                    
                    _context.Thanhtoans.Add(payment);
                }
                
                // Cập nhật trạng thái đơn hàng
                var orderEntity = await _context.Donhangs.FindAsync(pendingOrder.OrderId);
                if (orderEntity != null)
                {
                    orderEntity.Phuongthucthanhtoan = "PayPal";
                    orderEntity.Trangthai = "Đã thanh toán";
                    _context.Donhangs.Update(orderEntity);
                }
                
                await _context.SaveChangesAsync();
                
                // Chuyển hướng đến trang thành công với thông tin thanh toán
                return RedirectToAction("PaymentSuccess", "PaymentResult", new { 
                    orderId = pendingOrder.OrderId, 
                    transId = !string.IsNullOrEmpty(transactionId) ? transactionId : paypalOrderId, 
                    amount = pendingOrder.Amount,
                    clearCart = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing PayPal success: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return RedirectToAction("PaymentFailed", "PaymentResult", new { errorMessage = "Có lỗi xảy ra khi xử lý thanh toán PayPal" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> PaymentCancel()
        {
            try
            {
                // Lấy PayPal Order ID từ TempData
                if (TempData["PayPalOrderId"] is not string paypalOrderId)
                {
                    return RedirectToAction("Index", "Checkout");
                }
                
                // Lấy thông tin đơn hàng từ TempData
                if (TempData["PendingOrderData"] is not string pendingOrderJson)
                {
                    return RedirectToAction("Index", "Checkout");
                }
                
                var pendingOrder = JsonSerializer.Deserialize<PaypalPaymentRequest>(pendingOrderJson);
                if (pendingOrder == null)
                {
                    return RedirectToAction("Index", "Checkout");
                }
                
                // Cập nhật trạng thái thanh toán thành "Thanh toán thất bại"
                var payment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == pendingOrder.OrderId && t.Mathanhtoan == paypalOrderId);
                
                if (payment != null)
                {
                    payment.Trangthai = "Thanh toán thất bại";
                    _context.Thanhtoans.Update(payment);
                }
                
                // Cập nhật trạng thái đơn hàng
                var orderEntity = await _context.Donhangs.FindAsync(pendingOrder.OrderId);
                if (orderEntity != null)
                {
                    orderEntity.Trangthai = "Thanh toán không thành công";
                    _context.Donhangs.Update(orderEntity);
                }
                
                await _context.SaveChangesAsync();
                
                return RedirectToAction("PaymentFailed", "PaymentResult", new { errorMessage = "Thanh toán PayPal đã bị hủy" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing PayPal cancel: {ex.Message}");
                return RedirectToAction("PaymentFailed", "PaymentResult", new { errorMessage = "Có lỗi xảy ra khi xử lý hủy thanh toán PayPal" });
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
    
    public class PaypalPaymentRequest
    {
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ShippingAddress { get; set; }
        public string CartItems { get; set; }
    }
}
