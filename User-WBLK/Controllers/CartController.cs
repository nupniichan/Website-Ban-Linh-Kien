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

                var product = await _context.Sanphams.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Sử dụng hàm validation chung
                if (!ValidateQuantity(quantity, product, out string errorMessage))
                {
                    return Json(new { success = false, message = errorMessage });
                }

                // Tìm hoặc tạo giỏ hàng
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

                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                var cartItem = await _context.Chitietgiohangs
                    .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == productId);

                if (cartItem != null)
                {
                    // Kiểm tra tổng số lượng sau khi cộng thêm
                    int newQuantity = cartItem.Soluongsanpham + quantity;
                    if (newQuantity > product.Soluongton)
                    {
                        return Json(new { 
                            success = false, 
                            message = $"Tổng số lượng ({newQuantity}) vượt quá số lượng tồn kho ({product.Soluongton})" 
                        });
                    }

                    cartItem.Soluongsanpham = newQuantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    _context.Chitietgiohangs.Add(new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = productId,
                        Soluongsanpham = quantity,
                        Thoigiancapnhat = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
                return Json(new { 
                    success = true, 
                    message = "Đã thêm sản phẩm vào giỏ hàng",
                    cartTotal = await GetCartTotal(customerId)
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error adding to cart: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm vào giỏ hàng" });
            }
        }

        private IActionResult RedirectToProductDetail(string productId, string? productType)
        {
            if (string.IsNullOrEmpty(productType))
                return RedirectToAction("Components", "ProductDetail", new { id = productId });

            switch (productType.ToLower())
            {
                case "pc":
                    return RedirectToAction("PC", "ProductDetail", new { id = productId });
                case "laptop":
                    return RedirectToAction("Laptop", "ProductDetail", new { id = productId });
                case "storage":
                    return RedirectToAction("Storage", "ProductDetail", new { id = productId });
                case "monitor":
                    return RedirectToAction("Monitor", "ProductDetail", new { id = productId });
                case "network":
                    return RedirectToAction("Network", "ProductDetail", new { id = productId });
                default:
                    if (productType.StartsWith("audio"))
                        return RedirectToAction("Audio", "ProductDetail", new { category = productType, id = productId });
                    if (productType.StartsWith("peripherals"))
                        return RedirectToAction("Peripherals", "ProductDetail", new { category = productType, id = productId });
                    return RedirectToAction("Components", "ProductDetail", new { category = productType, id = productId });
            }
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {
            try 
            {
                var customerId = User.FindFirstValue("CustomerId");
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
                    return Json(new { 
                        success = false, 
                        message = "Số lượng phải lớn hơn 0",
                        validQuantity = 1
                    });
                }

                if (quantity > product.Soluongton)
                {
                    return Json(new { 
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
                    cartItem.Soluongsanpham = quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                    await _context.SaveChangesAsync();

                    var total = await GetCartTotal(customerId);
                    return Json(new { 
                        success = true, 
                        message = "Cập nhật số lượng thành công",
                        newQuantity = quantity,
                        total = total
                    });
                }

                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
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
                foreach (var item in items)
                {
                    // Kiểm tra sản phẩm và số lượng
                    var product = await _context.Sanphams.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        return Json(new { success = false, message = $"Không tìm thấy sản phẩm {item.ProductId}" });
                    }

                    if (item.Quantity <= 0)
                    {
                        return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });
                    }

                    if (item.Quantity > product.Soluongton)
                    {
                        return Json(new { 
                            success = false, 
                            message = $"Sản phẩm {product.Tensanpham} vượt quá số lượng tồn kho ({product.Soluongton})" 
                        });
                    }
                }

                // Kiểm tra xem khách hàng có tồn tại không
                var customer = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdKh == customerId);
                if (customer == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                }

                // Tìm hoặc tạo giỏ hàng cho khách hàng
                var cart = await _context.Giohangs
                    .Include(g => g.Chitietgiohangs)
                    .FirstOrDefaultAsync(g => g.IdKh == customerId);

                if (cart == null)
                {
                    // Tạo mới giỏ hàng
                    cart = new Giohang
                    {
                        IdGh = Guid.NewGuid().ToString().Substring(0, 10),
                        IdKh = customerId,
                        Thoigiancapnhat = DateTime.Now
                    };
                    _context.Giohangs.Add(cart);
                    // Lưu để có IdGh
                    await _context.SaveChangesAsync();
                }

                // Thêm từng sản phẩm vào giỏ hàng
                foreach (var item in items)
                {
                    var cartItem = await _context.Chitietgiohangs
                        .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == item.ProductId);

                    if (cartItem != null)
                    {
                        // Cập nhật số lượng nếu sản phẩm đã tồn tại
                        cartItem.Soluongsanpham += item.Quantity;
                        cartItem.Thoigiancapnhat = DateTime.Now;
                    }
                    else
                    {
                        // Thêm mới sản phẩm vào giỏ hàng
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

                // Kiểm tra sản phẩm
                var product = await _context.Sanphams.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Sử dụng hàm validation chung
                if (!ValidateQuantity(quantity, product, out string errorMessage))
                {
                    return Json(new { success = false, message = errorMessage });
                }

                // Tạo giỏ hàng tạm thời cho việc mua ngay
                var tempCart = new Giohang
                {
                    IdGh = "TEMP_" + Guid.NewGuid().ToString().Substring(0, 10),
                    IdKh = customerId,
                    Thoigiancapnhat = DateTime.Now
                };

                _context.Giohangs.Add(tempCart);
                
                // Thêm sản phẩm vào giỏ hàng tạm thời
                _context.Chitietgiohangs.Add(new Chitietgiohang
                {
                    IdGh = tempCart.IdGh,
                    IdSp = productId,
                    Soluongsanpham = quantity,
                    Thoigiancapnhat = DateTime.Now
                });

                await _context.SaveChangesAsync();

                // Chuyển đến trang thanh toán với giỏ hàng tạm thời
                return Json(new { success = true, redirectUrl = $"/Checkout/Index?cartId={tempCart.IdGh}" });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error in buy now: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi xử lý mua ngay" });
            }
        }

        // Thêm phương thức tính tổng giỏ hàng
        private async Task<decimal> GetCartTotal(string customerId)
        {
            var cart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .ThenInclude(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(g => g.IdKh == customerId);

            if (cart?.Chitietgiohangs == null)
                return 0;

            return cart.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham);
        }
    }
    public class MergeCartItemDto
    {
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    }
}