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
        // =============== IdNV tui đang set tạm nào có login đồ xong tui đổi lại ===============
        private readonly DatabaseContext _context;

        // Thêm options cho JsonSerializer để không escape Unicode characters
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

            var query = _context.Sanphams.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(s => s.IdSp.ToLower().Contains(searchString) || 
                                        s.Tensanpham.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(loaiSp))
            {
                query = query.Where(s => s.Loaisanpham == loaiSp);
            }

            if (!string.IsNullOrEmpty(thuongHieu))
            {
                query = query.Where(s => s.Thuonghieu == thuongHieu);
            }

            // Sắp xếp theo tùy chọn người dùng
            switch (sortOrder)
            {
                case "oldest":
                    query = query.OrderBy(s => s.IdSp);
                    break;
                case "newest":
                default:
                    query = query.OrderByDescending(s => s.IdSp);
                    break;
            }

            ViewBag.LoaiSps = await _context.Sanphams.Select(s => s.Loaisanpham).Distinct().ToListAsync();
            ViewBag.ThuongHieus = await _context.Sanphams.Select(s => s.Thuonghieu).Distinct().ToListAsync();

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            var model = new PaginatedList<Sanpham>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }

        // GET: ProductManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .FirstOrDefaultAsync(m => m.IdSp == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // GET: ProductManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSp,Tensanpham,Gia,Soluongton,Thuonghieu,Mota,Thongsokythuat,Loaisanpham,Hinhanh,Soluotxem,Damuahang")] Sanpham sanpham, IFormFile? imageFile)
        {
            try
            {
                // Bỏ qua validation cho các trường sẽ được tự động set
                ModelState.Remove("IdSp");
                ModelState.Remove("Hinhanh");
                ModelState.Remove("Soluotxem");
                ModelState.Remove("Damuahang");

                if (!ModelState.IsValid)
                {
                    // Debug: In ra các lỗi validation
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
                    return View(sanpham);
                }

                // Kiểm tra và xử lý hình ảnh
                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Vui lòng chọn hình ảnh sản phẩm");
                    return View(sanpham);
                }

                // Tạo ID sản phẩm tự động tăng
                string newId = "SP00001";
                var lastProduct = await _context.Sanphams
                    .OrderByDescending(p => p.IdSp)
                    .Select(p => new { p.IdSp })
                    .FirstOrDefaultAsync();

                if (lastProduct != null && !string.IsNullOrEmpty(lastProduct.IdSp))
                {
                    int lastNumber = int.Parse(lastProduct.IdSp.Substring(2));
                    newId = $"SP{(lastNumber + 1):D5}";
                }

                sanpham.IdSp = newId;

                // Xử lý hình ảnh
                var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductImage");
                
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                sanpham.Hinhanh = "/Images/ProductImage/" + fileName;

                // Lấy thông số kỹ thuật từ form và xử lý
                var thongSoKyThuat = Request.Form["thongsokythuat"].ToString();
                if (!string.IsNullOrEmpty(thongSoKyThuat))
                {
                    sanpham.Thongsokythuat = thongSoKyThuat;
                }
                else
                {
                    ModelState.AddModelError("Thongsokythuat", "Thông số kỹ thuật không được để trống");
                    return View(sanpham);
                }

                // Thêm và lưu vào database
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
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null)
            {
                return NotFound();
            }
            
            return View(sanpham);
        }

        // POST: ProductManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSp,Tensanpham,Gia,Soluongton,Thuonghieu,Mota,Thongsokythuat,Loaisanpham,Hinhanh")] Sanpham sanpham, IFormFile? imageFile)
        {
            if (id != sanpham.IdSp)
            {
                return NotFound();
            }

            try
            {
                // Bỏ qua validation cho các trường sẽ được tự động set
                ModelState.Remove("IdNvNavigation");
                ModelState.Remove("IdNv");
                
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
                    return View(sanpham);
                }

                // Lấy sản phẩm hiện tại từ database
                var existingProduct = await _context.Sanphams.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.IdSp == id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Xử lý hình ảnh nếu có upload file mới
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Xóa file ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(existingProduct.Hinhanh))
                    {
                        var oldImagePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            existingProduct.Hinhanh.TrimStart('/')
                        );
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu file ảnh mới
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductImage");
                    
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    sanpham.Hinhanh = "/Images/ProductImage/" + fileName;
                }
                else
                {
                    // Giữ nguyên đường dẫn hình ảnh cũ
                    sanpham.Hinhanh = existingProduct.Hinhanh;
                }

                // Debug logging
                Console.WriteLine("Thông số kỹ thuật từ form: " + Request.Form["thongsokythuat"]);

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

                // Nếu có specs mới, cập nhật với options để không escape Unicode
                if (specs.Any())
                {
                    sanpham.Thongsokythuat = JsonSerializer.Serialize(specs, _jsonOptions);
                }
                else
                {
                    sanpham.Thongsokythuat = existingProduct?.Thongsokythuat;
                }

                // Cập nhật sản phẩm
                _context.Update(sanpham);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanphamExists(sanpham.IdSp))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(sanpham);
            }
        }

        // GET: ProductManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .FirstOrDefaultAsync(m => m.IdSp == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // POST: ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                _context.Sanphams.Remove(sanpham);
            }

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
            if (string.IsNullOrEmpty(term)) return Json(new List<object>());

            term = term.ToLower();
            var suggestions = await _context.Sanphams
                .Where(s => s.IdSp.ToLower().Contains(term) || 
                            s.Tensanpham.ToLower().Contains(term))
                .Take(5)
                .Select(s => new { s.IdSp, s.Tensanpham })
                .ToListAsync();

            return Json(suggestions);
        }
    }
}
