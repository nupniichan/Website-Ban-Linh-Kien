using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Website_Ban_Linh_Kien.Models.Strategies.Cart
{
    public class AddToCartStrategy : ICartStrategy
    {
        private readonly ILogger<AddToCartStrategy> _logger;

        public AddToCartStrategy(ILogger<AddToCartStrategy> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Execute(DatabaseContext context, string customerId, object data)
        {
            if (data is not AddToCartData addToCartData)
            {
                return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                if (customerId == null)
                {
                    return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
                }

                // Check product existence
                var product = await context.Sanphams.FindAsync(addToCartData.ProductId);
                if (product == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Check requested quantity is at least 1 and does not exceed available stock
                if (addToCartData.Quantity <= 0 || addToCartData.Quantity > product.Soluongton)
                {
                    return new JsonResult(new { success = false, message = "Số lượng không hợp lệ" });
                }

                // Enforce the maximum limit of 5 for the same product
                const int maxAllowed = 5;

                // Find or create the cart
                var cart = await context.Giohangs
                    .Include(g => g.Chitietgiohangs)
                    .FirstOrDefaultAsync(g => g.IdKh == customerId);

                if (cart == null)
                {
                    cart = new Giohang
                    {
                        IdGh = Guid.NewGuid().ToString().Substring(0, 10),
                        IdKh = customerId,
                        Thoigiancapnhat = DateTime.Now
                    };
                    context.Giohangs.Add(cart);
                    await context.SaveChangesAsync();
                }

                // Check if the product is already in the cart
                var cartItem = await context.Chitietgiohangs
                    .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == addToCartData.ProductId);

                if (cartItem != null)
                {
                    if (cartItem.Soluongsanpham >= maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = "Tổng số lượng sản phẩm này trong giỏ hàng không thể vượt quá 5." });
                    }
                    if (cartItem.Soluongsanpham + addToCartData.Quantity > maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = $"Bạn chỉ có thể mua tối đa {maxAllowed} sản phẩm của loại này." });
                    }

                    // Increase quantity if within limit
                    cartItem.Soluongsanpham += addToCartData.Quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    // For a new item, ensure quantity does not exceed maxAllowed
                    if (addToCartData.Quantity > maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = $"Sản phẩm chỉ cho phép mua tối đa {maxAllowed}." });
                    }
                    context.Chitietgiohangs.Add(new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = addToCartData.ProductId,
                        Soluongsanpham = addToCartData.Quantity,
                        Thoigiancapnhat = DateTime.Now
                    });
                }

                await context.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error adding to cart: {ex.Message}");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra khi thêm vào giỏ hàng" });
            }
        }
    }

    public class AddToCartData
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
} 