using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Website_Ban_Linh_Kien.Models.Strategies.Cart
{
    public class UpdateQuantityStrategy : ICartStrategy
    {
        private readonly ILogger<UpdateQuantityStrategy> _logger;

        public UpdateQuantityStrategy(ILogger<UpdateQuantityStrategy> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Execute(DatabaseContext context, string customerId, object data)
        {
            if (data is not UpdateQuantityData updateData)
            {
                return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                if (customerId == null)
                {
                    return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
                }

                var cartItem = await context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .ThenInclude(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == updateData.ProductId);

                if (cartItem == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                }

                const int maxAllowed = 5;
                
                // If the user tries to update quantity above maxAllowed, return an error
                if (updateData.Quantity > maxAllowed)
                {
                    return new JsonResult(new { success = false, message = $"Sản phẩm chỉ cho phép mua tối đa {maxAllowed}." });
                }
                
                if (updateData.Quantity < 1)
                {
                    return new JsonResult(new { success = false, message = "Số lượng không thể nhỏ hơn 1." });
                }
                
                if (updateData.Quantity > 0)
                {
                    cartItem.Soluongsanpham = updateData.Quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    context.Chitietgiohangs.Remove(cartItem);
                }
                
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
                _logger?.LogError($"Error updating cart quantity: {ex.Message}");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra khi cập nhật giỏ hàng" });
            }
        }
    }

    public class UpdateQuantityData
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
} 