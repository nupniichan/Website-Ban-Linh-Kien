using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;
using System.Security.Claims;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class PaymentResultController : Controller
    {
        private readonly DatabaseContext _context;

        public PaymentResultController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PaymentSuccess(string orderId, string transId, long amount, bool clearCart = true)
        {
            // Ghi log để debug
            Console.WriteLine($"PaymentSuccess called with orderId: {orderId}, transId: {transId}, amount: {amount}, clearCart: {clearCart}");
            
            // Lưu thông tin vào TempData để hiển thị trên trang
            TempData["OrderId"] = orderId;
            TempData["TransactionInfo"] = $"Mã giao dịch: {transId}, Số tiền: {amount}";
            TempData["ClearCart"] = clearCart; // Thêm flag để JavaScript biết cần làm mới giỏ hàng
            
            // Kiểm tra xem đơn hàng đã tồn tại chưa
            if (!string.IsNullOrEmpty(orderId))
            {
                var payment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == orderId);
                
                if (payment != null)
                {
                    Console.WriteLine($"Found payment for order: {orderId}");
                    ViewData["Payment"] = payment;
                }
                else
                {
                    Console.WriteLine($"No payment found for order: {orderId}");
                }
                
                // Nếu cần làm mới giỏ hàng
                if (clearCart)
                {
                    try
                    {
                        // Nếu người dùng đã đăng nhập, xóa giỏ hàng trong database
                        if (User.Identity?.IsAuthenticated == true)
                        {
                            var customerId = User.FindFirst("CustomerId")?.Value;
                            if (!string.IsNullOrEmpty(customerId))
                            {
                                var cart = await _context.Giohangs
                                    .Include(g => g.Chitietgiohangs)
                                    .Where(g => g.IdKh == customerId)
                                    .OrderByDescending(g => g.Thoigiancapnhat)
                                    .FirstOrDefaultAsync();
                                
                                if (cart != null)
                                {
                                    _context.Chitietgiohangs.RemoveRange(cart.Chitietgiohangs);
                                    _context.Giohangs.Remove(cart);
                                    await _context.SaveChangesAsync();
                                    Console.WriteLine($"Cleared cart for customer {customerId}");
                                }
                            }
                        }
                        
                        // Xóa session giỏ hàng
                        HttpContext.Session.Remove("CartItems");
                        Console.WriteLine("Cleared cart from session");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error clearing cart: {ex.Message}");
                    }
                }
            }
            
            // Ngăn chặn chuyển hướng tự động
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");
            
            return View();
        }

        public IActionResult PaymentFailed(string errorMessage)
        {
            // Ghi log để debug
            Console.WriteLine($"PaymentFailed called with errorMessage: {errorMessage}");
            
            // Lưu thông tin vào TempData để hiển thị trên trang
            TempData["ErrorMessage"] = errorMessage;
            
            // Ngăn chặn chuyển hướng tự động
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");
            
            return View();
        }
    }
}
