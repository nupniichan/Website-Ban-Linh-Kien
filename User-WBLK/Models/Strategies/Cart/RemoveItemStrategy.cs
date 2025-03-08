using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Website_Ban_Linh_Kien.Models.Strategies.Cart
{
    public class RemoveItemStrategy : ICartStrategy
    {
        private readonly ILogger<RemoveItemStrategy> _logger;

        public RemoveItemStrategy(ILogger<RemoveItemStrategy> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Execute(DatabaseContext context, string customerId, object data)
        {
            if (data is not RemoveItemData removeData)
            {
                return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                if (customerId == null)
                {
                    return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để xóa sản phẩm khỏi giỏ hàng" });
                }

                var cartItem = await context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .ThenInclude(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == removeData.ProductId);

                if (cartItem == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                }

                context.Chitietgiohangs.Remove(cartItem);
                await context.SaveChangesAsync();

                var cart = await context.Giohangs
                    .Include(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(g => g.IdKh == customerId);
                
                var total = cart?.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham) ?? 0;
                return new JsonResult(new { success = true, cartTotal = total.ToString("N0") });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error removing item from cart: {ex.Message}");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm khỏi giỏ hàng" });
            }
        }
    }

    public class RemoveItemData
    {
        public string ProductId { get; set; }
    }
} 