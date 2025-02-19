using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;

        public CartController(DatabaseContext context)
        {
            _context = context;
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
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? productType = null;
            
            if (customerId != null)
            {
                var product = await _context.Sanphams
                    .Select(p => new { p.IdSp, p.Soluongton, p.Loaisanpham })
                    .FirstOrDefaultAsync(p => p.IdSp == productId);

                if (product == null || quantity <= 0 || quantity > product.Soluongton * 0.2)
                {
                    TempData["Error"] = "Số lượng sản phẩm không hợp lệ!";
                    return RedirectToProductDetail(productId, product?.Loaisanpham);
                }

                productType = product.Loaisanpham;

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

                var cartItem = await _context.Chitietgiohangs
                    .FirstOrDefaultAsync(c => c.IdGh == cart.IdGh && c.IdSp == productId);

                if (cartItem == null)
                {
                    cartItem = new Chitietgiohang
                    {
                        IdGh = cart.IdGh,
                        IdSp = productId,
                        Soluongsanpham = quantity,
                        Thoigiancapnhat = DateTime.Now
                    };
                    _context.Chitietgiohangs.Add(cartItem);
                }
                else
                {
                    cartItem.Soluongsanpham += quantity;
                    cartItem.Thoigiancapnhat = DateTime.Now;
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng!";
                }
            }
            
            return RedirectToProductDetail(productId, productType);
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
    }
}
