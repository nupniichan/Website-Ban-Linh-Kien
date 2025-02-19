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

                // Kiểm tra sản phẩm
                var product = await _context.Sanphams.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Kiểm tra số lượng
                if (quantity <= 0 || quantity > product.Soluongton)
                {
                    return Json(new { success = false, message = "Số lượng không hợp lệ" });
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
                    // Cập nhật số lượng nếu đã có
                    cartItem.Soluongsanpham += quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }
                else
                {
                    // Thêm mới nếu chưa có
                    _context.Chitietgiohangs.Add(new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = productId,
                        Soluongsanpham = quantity,
                        Thoigiancapnhat = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();

                // Trả về thông báo thành công
                TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng";
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
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
                }
            }

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
    }
    public class MergeCartItemDto
    {
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    }
}