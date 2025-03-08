using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Website_Ban_Linh_Kien.Models.Commands.Cart
{
    public class AddToCartCommand : ICartCommand
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AddToCartCommand> _logger;
        private readonly string _customerId;
        private readonly string _productId;
        private readonly int _quantity;

        public AddToCartCommand(
            DatabaseContext context,
            ILogger<AddToCartCommand> logger,
            string customerId,
            string productId,
            int quantity)
        {
            _context = context;
            _logger = logger;
            _customerId = customerId;
            _productId = productId;
            _quantity = quantity;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                if (_customerId == null)
                {
                    return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
                }

                // Check product existence
                var product = await _context.Sanphams.FindAsync(_productId);
                if (product == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Check requested quantity is at least 1 and does not exceed available stock
                if (_quantity <= 0 || _quantity > product.Soluongton)
                {
                    return new JsonResult(new { success = false, message = "Số lượng không hợp lệ" });
                }

                // Enforce the maximum limit of 5 for the same product
                const int maxAllowed = 5;

                // Find or create the cart
                var cart = await _context.Giohangs
                    .Include(g => g.Chitietgiohangs)
                    .FirstOrDefaultAsync(g => g.IdKh == _customerId);

                if (cart == null)
                {
                    cart = new Giohang
                    {
                        IdGh = Guid.NewGuid().ToString().Substring(0, 10),
                        IdKh = _customerId,
                        Thoigiancapnhat = DateTime.Now
                    };
                    _context.Giohangs.Add(cart);
                    await _context.SaveChangesAsync();
                }

                // Check if the product is already in the cart
                var cartItem = await _context.Chitietgiohangs
                    .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == _productId);

                if (cartItem != null)
                {
                    if (cartItem.Soluongsanpham >= maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = "Tổng số lượng sản phẩm này trong giỏ hàng không thể vượt quá 5." });
                    }
                    if (cartItem.Soluongsanpham + _quantity > maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = $"Bạn chỉ có thể mua tối đa {maxAllowed} sản phẩm của loại này." });
                    }

                    // Increase quantity if within limit
                    cartItem.Soluongsanpham += _quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    // For a new item, ensure quantity does not exceed maxAllowed
                    if (_quantity > maxAllowed)
                    {
                        return new JsonResult(new { success = false, message = $"Sản phẩm chỉ cho phép mua tối đa {maxAllowed}." });
                    }
                    _context.Chitietgiohangs.Add(new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = _productId,
                        Soluongsanpham = _quantity,
                        Thoigiancapnhat = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error adding to cart: {ex.Message}");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra khi thêm vào giỏ hàng" });
            }
        }
    }
} 