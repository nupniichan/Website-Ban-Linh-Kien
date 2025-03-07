using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<CartController>? _logger;

        public CartController(DatabaseContext context, ILogger<CartController>? logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var customerId = User.FindFirstValue("CustomerId");
            if (customerId != null)
            {
                var cart = await _context.Giohangs
                    .Include(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(g => g.IdKh == customerId);
                return View(cart);
            }
            return View(null);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            try
            {
                var customerId = User.FindFirstValue("CustomerId");
                if (customerId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
                }

                // Check product existence
                var product = await _context.Sanphams.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Check requested quantity is at least 1 and does not exceed available stock
                if (quantity <= 0 || quantity > product.Soluongton)
                {
                    return Json(new { success = false, message = "Số lượng không hợp lệ" });
                }

                // Enforce the maximum limit of 5 for the same product
                const int maxAllowed = 5;

                // Find or create the cart
                var cart = await _context.Giohangs
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
                    _context.Giohangs.Add(cart);
                    await _context.SaveChangesAsync();
                }

                // Check if the product is already in the cart
                var cartItem = await _context.Chitietgiohangs
                    .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == productId);

                if (cartItem != null)
                {
                    if (cartItem.Soluongsanpham >= maxAllowed)
                    {
                        return Json(new { success = false, message = "Tổng số lượng sản phẩm này trong giỏ hàng không thể vượt quá 5." });
                    }
                    if (cartItem.Soluongsanpham + quantity > maxAllowed)
                    {
                        return Json(new { success = false, message = $"Bạn chỉ có thể mua tối đa {maxAllowed} sản phẩm của loại này." });
                    }

                    // Increase quantity if within limit
                    cartItem.Soluongsanpham += quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    // For a new item, ensure quantity does not exceed maxAllowed
                    if (quantity > maxAllowed)
                    {
                        return Json(new { success = false, message = $"Sản phẩm chỉ cho phép mua tối đa {maxAllowed}." });
                    }
                    _context.Chitietgiohangs.Add(new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = productId,
                        Soluongsanpham = quantity,
                        Thoigiancapnhat = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error adding to cart: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm vào giỏ hàng" });
            }
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {
            var customerId = User.FindFirstValue("CustomerId");

            if (customerId != null)
            {
                var cartItem = await _context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == productId);

                if (cartItem != null)
                {
                    if (quantity > 0)
                    {
                        cartItem.Soluongsanpham = quantity;
                        cartItem.Thoigiancapnhat = DateTime.Now;
                    }
                    else
                    {
                        _context.Chitietgiohangs.Remove(cartItem);
                    }

                    await _context.SaveChangesAsync();
                }            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/RemoveItem
        [HttpPost]
        public async Task<IActionResult> RemoveItem(string productId)
        {
            var customerId = User.FindFirstValue("CustomerId");

            if (customerId != null)
            {
                var cartItem = await _context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == productId);

                if (cartItem != null)
                {
                    _context.Chitietgiohangs.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var customerId = User.FindFirstValue("CustomerId");
            var cart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .FirstOrDefaultAsync(g => g.IdKh == customerId);

            int count = cart?.Chitietgiohangs.Sum(c => c.Soluongsanpham) ?? 0;
            return Json(count);
        }

        [HttpPost]
        public async Task<IActionResult> MergeGuestCart([FromBody] List<MergeCartItemDto> items)
        {
            var customerId = User.FindFirstValue("CustomerId");
            if (customerId == null)
            {
                return Json(new { success = false, message = "Chưa đăng nhập" });
            }

            try
            {
                var customer = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdKh == customerId);
                if (customer == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                }

                var cart = await _context.Giohangs
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
                    _context.Giohangs.Add(cart);
                    await _context.SaveChangesAsync();
                }

                foreach (var item in items)
                {
                    var product = await _context.Sanphams
                        .FirstOrDefaultAsync(p => p.IdSp == item.ProductId);

                    if (product == null || item.Quantity <= 0)
                    {
                        _logger?.LogWarning($"Invalid product {item.ProductId} or quantity {item.Quantity}");
                        continue;
                    }

                    var cartItem = await _context.Chitietgiohangs
                        .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == item.ProductId);

                    if (cartItem != null)
                    {
                        cartItem.Soluongsanpham += item.Quantity;
                        cartItem.Thoigiancapnhat = DateTime.Now;
                    }
                    else
                    {
                        var newCartItem = new Chitietgiohang
                        {
                            IdGh = cart.IdGh,
                            IdSp = item.ProductId,
                            Soluongsanpham = item.Quantity,
                            Thoigiancapnhat = DateTime.Now
                        };
                        _context.Chitietgiohangs.Add(newCartItem);
                    }
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã merge giỏ hàng thành công!" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error merging cart: {ex.Message}");
                return Json(new { success = false, message = "Lỗi khi merge giỏ hàng: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(string productId, int quantity)
        {
            try
            {
                var customerId = User.FindFirstValue("CustomerId");
                if (customerId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng" });
                }

                var product = await _context.Sanphams.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                if (quantity <= 0 || quantity > product.Soluongton)
                {
                    return Json(new { success = false, message = "Số lượng không hợp lệ" });
                }

                var tempCart = new Giohang
                {
                    IdGh = "TEMP_" + Guid.NewGuid().ToString().Substring(0, 10),
                    IdKh = customerId,
                    Thoigiancapnhat = DateTime.Now
                };

                _context.Giohangs.Add(tempCart);

                _context.Chitietgiohangs.Add(new Chitietgiohang
                {
                    IdGh = tempCart.IdGh,
                    IdSp = productId,
                    Soluongsanpham = quantity,
                    Thoigiancapnhat = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return Json(new { success = true, redirectUrl = $"/Checkout/Index?cartId={tempCart.IdGh}" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error in buy now: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi xử lý mua ngay" });
            }
        }

        // ------------------------------
        // New AJAX endpoint for updating quantity (logged-in users)
        // ------------------------------
        // POST: Cart/UpdateQuantityAjax (for logged-in users)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantityAjax([FromBody] UpdateCartDto dto)
        {
            const int maxAllowed = 5;
            var customerId = User.FindFirstValue("CustomerId");
            if (customerId != null)
            {
                var cartItem = await _context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .ThenInclude(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == dto.ProductId);
                if (cartItem != null)
                {
                    // If the user tries to update quantity above maxAllowed, return an error
                    if (dto.Quantity > maxAllowed)
                    {
                        return Json(new { success = false, message = $"Sản phẩm chỉ cho phép mua tối đa {maxAllowed}." });
                    }
                    if (dto.Quantity < 1)
                    {
                        return Json(new{ success = false, message = "Số lượng không thể nhỏ hơn 1."});
                    }
                    if (dto.Quantity > 0)
                    {
                        cartItem.Soluongsanpham = dto.Quantity;
                        cartItem.Thoigiancapnhat = DateTime.Now;
                    }
                    else
                    {
                        _context.Chitietgiohangs.Remove(cartItem);
                    }
                    await _context.SaveChangesAsync();

                    var cart = await _context.Giohangs
                        .Include(g => g.Chitietgiohangs)
                        .ThenInclude(c => c.IdSpNavigation)
                        .FirstOrDefaultAsync(g => g.IdKh == customerId);
                    var total = cart?.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham) ?? 0;
                    return Json(new { success = true, cartTotal = total.ToString("N0") });
                }
            }
            return Json(new { success = false, message = "Không tìm thấy giỏ hàng hoặc sản phẩm" });
        }

        // ------------------------------
        // New AJAX endpoint for removing an item (logged-in users)
        // ------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemAjax([FromBody] RemoveCartDto dto)
        {
            var customerId = User.FindFirstValue("CustomerId");
            if (customerId != null)
            {
                var cartItem = await _context.Chitietgiohangs
                    .Include(c => c.IdGhNavigation)
                    .ThenInclude(g => g.Chitietgiohangs)
                    .ThenInclude(c => c.IdSpNavigation)
                    .FirstOrDefaultAsync(c => c.IdGhNavigation.IdKh == customerId && c.IdSp == dto.ProductId);
                if (cartItem != null)
                {
                    _context.Chitietgiohangs.Remove(cartItem);
                    await _context.SaveChangesAsync();

                    var cart = await _context.Giohangs
                        .Include(g => g.Chitietgiohangs)
                        .ThenInclude(c => c.IdSpNavigation)
                        .FirstOrDefaultAsync(g => g.IdKh == customerId);
                    var total = cart?.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham) ?? 0;
                    return Json(new { success = true, cartTotal = total.ToString("N0") });
                }
            }
            return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
        }

        [HttpPost]
        public async Task<IActionResult> AddMultipleToCart([FromBody] List<string> productIds)
        {
            try
            {
                var customerId = User.FindFirstValue("CustomerId");
                if (customerId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
                }

                // Kiểm tra xem khách hàng đã có giỏ hàng chưa
                var cart = await _context.Giohangs.FirstOrDefaultAsync(g => g.IdKh == customerId);
                if (cart == null)
                {
                    // Tạo giỏ hàng mới nếu chưa có
                    cart = new Giohang
                    {
                        IdGh = Guid.NewGuid().ToString().Substring(0, 10),
                        IdKh = customerId,
                        Thoigiancapnhat = DateTime.Now
                    };
                    _context.Giohangs.Add(cart);
                    await _context.SaveChangesAsync();
                }

                int successCount = 0;
                List<string> failedProducts = new List<string>();

                foreach (var productId in productIds)
                {
                    // Kiểm tra sản phẩm có tồn tại không
                    var product = await _context.Sanphams.FindAsync(productId);
                    if (product == null)
                    {
                        failedProducts.Add(productId);
                        continue;
                    }

                    // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
                    var cartItem = await _context.Chitietgiohangs
                        .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == productId);

                    if (cartItem != null)
                    {
                        // Nếu đã có, tăng số lượng lên 1
                        cartItem.Soluongsanpham += 1;
                        cartItem.Thoigiancapnhat = DateTime.Now;
                    }
                    else
                    {
                        // Nếu chưa có, thêm mới vào giỏ hàng
                        cartItem = new Chitietgiohang
                        {
                            IdGh = cart.IdGh,
                            IdSp = productId,
                            Soluongsanpham = 1,
                            Thoigiancapnhat = DateTime.Now
                        };
                        _context.Chitietgiohangs.Add(cartItem);
                    }

                    successCount++;
                }

                await _context.SaveChangesAsync();

                // Lấy số lượng sản phẩm trong giỏ hàng
                var cartCount = await _context.Chitietgiohangs
                    .Where(c => c.IdGh == cart.IdGh)
                    .SumAsync(c => c.Soluongsanpham);

                return Json(new { 
                    success = true, 
                    message = $"Đã thêm {successCount} sản phẩm vào giỏ hàng", 
                    cartCount = cartCount,
                    failedProducts = failedProducts
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding multiple products to cart");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm sản phẩm vào giỏ hàng" });
            }
        }
    }

    // DTO classes for AJAX calls
    public class UpdateCartDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class RemoveCartDto
    {
        public string ProductId { get; set; }
    }
    
    public class MergeCartItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}