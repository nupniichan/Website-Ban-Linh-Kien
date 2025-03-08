using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace Admin_WBLK.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly DatabaseContext _context;

        // JsonSerializer options to not escape Unicode characters
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public ProductManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: ProductManagement
        public async Task<IActionResult> Index(string searchString, string loaiSp, string thuongHieu, string sortOrder = "newest", int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLoaiSp"] = loaiSp;
            ViewData["CurrentThuongHieu"] = thuongHieu;
            ViewData["CurrentSort"] = sortOrder;

            // Retrieve all products
            var query = _context.Sanphams.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(s => s.IdSp.ToLower().Contains(searchString) ||
                                         s.Tensanpham.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(thuongHieu))
            {
                query = query.Where(s => s.Thuonghieu == thuongHieu);
            }

            // Sorting
            query = sortOrder switch
            {
                "oldest" => query.OrderBy(s => s.IdSp),
                _ => query.OrderByDescending(s => s.IdSp)
            };

            var items = await query.ToListAsync();

            if (!string.IsNullOrEmpty(loaiSp))
            {
                if (loaiSp == "PC" || loaiSp == "Laptop" || loaiSp == "Monitor")
                {
                    items = items.Where(s => s.Loaisanpham == loaiSp).ToList();
                }
                else
                {
                    items = items.Where(s =>
                    {
                        var specs = JsonSerializer.Deserialize<Dictionary<string, string>>(s.Thongsokythuat, _jsonOptions);
                        return specs != null &&
                               ((specs.ContainsKey("Danh mục") && specs["Danh mục"] == loaiSp) ||
                                (specs.ContainsKey("Loại ổ cứng") && specs["Loại ổ cứng"] == loaiSp));
                    }).ToList();
                }
            }

            // Prepare category list
            var allProducts = await _context.Sanphams.ToListAsync();
            var danhMucs = allProducts
                .Select(s =>
                {
                    if (s.Loaisanpham == "PC" || s.Loaisanpham == "Laptop" || s.Loaisanpham == "Monitor")
                        return s.Loaisanpham;

                    var specs = JsonSerializer.Deserialize<Dictionary<string, string>>(s.Thongsokythuat, _jsonOptions);
                    if (specs != null)
                    {
                        if (specs.ContainsKey("Danh mục")) return specs["Danh mục"];
                        if (specs.ContainsKey("Loại ổ cứng")) return specs["Loại ổ cứng"];
                    }
                    return null;
                })
                .Where(dm => dm != null)
                .Distinct()
                .ToList();

            ViewBag.LoaiSps = danhMucs;
            ViewBag.ThuongHieus = await _context.Sanphams.Select(s => s.Thuonghieu).Distinct().ToListAsync();

            // Pagination
            var totalItems = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var model = new PaginatedList<Sanpham>(pagedItems, totalItems, pageNumber, pageSize);
            return View(model);
        }

        // GET: ProductManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var sanpham = await _context.Sanphams.FirstOrDefaultAsync(m => m.IdSp == id);
            if (sanpham == null)
                return NotFound();

            return View(sanpham);
        }

        // GET: ProductManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSp,Tensanpham,Gia,Soluongton,Thuonghieu,Mota,Thongsokythuat,Loaisanpham,Hinhanh,Soluotxem,Damuahang")] Sanpham sanpham, IFormFile? imageFile)
        {
            try
            {
                // Remove fields that are auto-set
                ModelState.Remove("IdSp");
                ModelState.Remove("Hinhanh");
                ModelState.Remove("Soluotxem");
                ModelState.Remove("Damuahang");

                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                        foreach (var error in modelState.Errors)
                            Console.WriteLine(error.ErrorMessage);
                    return View(sanpham);
                }

                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Vui lòng chọn hình ảnh sản phẩm");
                    return View(sanpham);
                }

                // Auto-increment product ID
                string newId = "SP00001";
                var lastProduct = await _context.Sanphams.OrderByDescending(p => p.IdSp)
                                                         .Select(p => new { p.IdSp })
                                                         .FirstOrDefaultAsync();

                if (lastProduct != null && !string.IsNullOrEmpty(lastProduct.IdSp))
                {
                    int lastNumber = int.Parse(lastProduct.IdSp.Substring(2));
                    newId = $"SP{(lastNumber + 1):D5}";
                }

                sanpham.IdSp = newId;

                // Process image
                var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductImage");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await imageFile.CopyToAsync(stream);

                sanpham.Hinhanh = "/Images/ProductImage/" + fileName;

                // Process technical specifications
                var thongSoKyThuat = Request.Form["thongsokythuat"].ToString();
                if (!string.IsNullOrEmpty(thongSoKyThuat))
                    sanpham.Thongsokythuat = thongSoKyThuat;
                else
                {
                    ModelState.AddModelError("Thongsokythuat", "Thông số kỹ thuật không được để trống");
                    return View(sanpham);
                }

                _context.Sanphams.Add(sanpham);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(sanpham);
            }
        }

        // GET: ProductManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null)
                return NotFound();

            return View(sanpham);
        }

        // POST: ProductManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSp,Tensanpham,Gia,Soluongton,Loaisanpham,Thuonghieu,Hinhanh,Mota,Thongsokythuat")] Sanpham sanpham, IFormFile? imageFile)
        {
            try
            {
                if (id != sanpham.IdSp)
                    return NotFound();

                ModelState.Remove("IdNvNavigation");
                ModelState.Remove("IdNv");

                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                        foreach (var error in modelState.Errors)
                            Console.WriteLine(error.ErrorMessage);
                    return View(sanpham);
                }

                var existingProduct = await _context.Sanphams.AsNoTracking()
                                                           .FirstOrDefaultAsync(s => s.IdSp == id);
                if (existingProduct == null)
                    return NotFound();

                // Handle image update
                if (imageFile != null && imageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingProduct.Hinhanh))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            existingProduct.Hinhanh.TrimStart('/').Replace("/", "\\"));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductImage");
                    Directory.CreateDirectory(uploadPath);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await imageFile.CopyToAsync(stream);

                    sanpham.Hinhanh = $"/Images/ProductImage/{fileName}";
                }
                else
                {
                    sanpham.Hinhanh = existingProduct.Hinhanh;
                }

                // Process technical specifications
                var specs = new Dictionary<string, string>();
                var thongSoKyThuatJson = Request.Form["thongsokythuat"].ToString();
                if (!string.IsNullOrEmpty(thongSoKyThuatJson))
                {
                    try
                    {
                        specs = JsonSerializer.Deserialize<Dictionary<string, string>>(thongSoKyThuatJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi deserialize thông số kỹ thuật: " + ex.Message);
                    }
                }

                sanpham.Thongsokythuat = specs.Any() 
                    ? JsonSerializer.Serialize(specs, _jsonOptions)
                    : existingProduct?.Thongsokythuat;

                _context.Update(sanpham);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit action: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(sanpham);
            }
        }

        // GET: ProductManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var sanpham = await _context.Sanphams.FirstOrDefaultAsync(m => m.IdSp == id);
            if (sanpham == null)
                return NotFound();

            return View(sanpham);
        }

        // POST: ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
                _context.Sanphams.Remove(sanpham);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanphamExists(string id)
        {
            return _context.Sanphams.Any(e => e.IdSp == id);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            term = term.ToLower();
            try
            {
                var suggestions = await _context.Sanphams
                    .Where(s => s.IdSp.ToLower().Contains(term) ||
                                s.Tensanpham.ToLower().Contains(term))
                    .Take(5)
                    .Select(s => new
                    {
                        s.IdSp,
                        s.Tensanpham,
                        MaSP = "Mã SP: " + s.IdSp
                    })
                    .ToListAsync();

                return Json(suggestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchSuggestions: {ex.Message}");
                return Json(new List<object>());
            }
        }
    }
}
