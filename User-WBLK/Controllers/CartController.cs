using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;
using Microsoft.Extensions.Logging;

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

        private bool ValidateQuantity(int quantity, Sanpham product, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            // Kiểm tra số âm và số 0
            if (quantity <= 0)
            {
                errorMessage = "Số lượng phải lớn hơn 0";
                return false;
            }

            // Kiểm tra vượt tồn kho
            if (quantity > product.Soluongton)
            {
                errorMessage = $"Số lượng không thể vượt quá {product.Soluongton}";
                return false;
            }

            return true;
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

                // Validate quantity using the existing ValidateQuantity method
                string validationErrorMessage;
                if (!ValidateQuantity(quantity, product, out validationErrorMessage))
                {
                    return Json(new { success = false, message = validationErrorMessage });
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
                        return Json(new { success = false, message = "Sản phẩm đã đạt số lượng tối đa (5)." });
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
            try
            {
                var customerId = User.FindFirstValue("CustomerId");

                if (customerId != null)
                {
                    // var customerId = User.FindFirstValue("CustomerId");
                    if (customerId == null)
                    {
                        return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
                    }

                    // Kiểm tra sản phẩm
                    var product = await _context.Sanphams.FindAsync(productId);
                    if (product == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                    }

                    // Kiểm tra số lượng
                    if (quantity <= 0)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Số lượng phải lớn hơn 0",
                            validQuantity = 1
                        });
                    }

                    if (quantity > product.Soluongton)
                    {
                        return Json(new
                        {
                            success = false,
                            message = $"Số lượng không thể vượt quá {product.Soluongton}",
                            validQuantity = product.Soluongton
                        });
                    }

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

                        var total = await GetCartTotal(customerId);
                        return Json(new
                        {
                            success = true,
                            message = "Cập nhật số lượng thành công",
                            newQuantity = quantity,
                            total = total
                        });
                    }

                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                }
                
                // Add default return for when customerId is null
                return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error updating quantity: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật số lượng" });
            }
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

                // Validate quantity using the existing ValidateQuantity method
                string validationErrorMessage;
                if (!ValidateQuantity(quantity, product, out validationErrorMessage))
                {
                    return Json(new { success = false, message = validationErrorMessage });
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
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                // Nếu người dùng đã đăng nhập
                if (User.Identity?.IsAuthenticated == true)
                {
                    var customerId = User.FindFirst("CustomerId")?.Value;
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        // Xóa giỏ hàng của khách hàng
                        var cart = await _context.Giohangs
                            .Include(g => g.Chitietgiohangs)
                            .Where(g => g.IdKh == customerId)
                            .OrderByDescending(g => g.Thoigiancapnhat)
                            .FirstOrDefaultAsync();
                        
                        if (cart != null)
                        {
                            // Xóa chi tiết giỏ hàng
                            _context.Chitietgiohangs.RemoveRange(cart.Chitietgiohangs);
                            
                            // Xóa giỏ hàng
                            _context.Giohangs.Remove(cart);
                            
                            await _context.SaveChangesAsync();
                            Console.WriteLine($"Cleared cart for customer {customerId}");
                        }
                    }
                }
                
                // Xóa session giỏ hàng
                HttpContext.Session.Remove("CartItems");
                
                return Ok(new { success = true, message = "Cart cleared successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Add the missing GetCartTotal method
        private async Task<decimal> GetCartTotal(string customerId)
        {
            var cart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .ThenInclude(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(g => g.IdKh == customerId);
            
            return cart?.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham) ?? 0;
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
