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

namespace Admin_WBLK.Controllers
{
    public class ProductManagementController : Controller
    {
        // =============== IdNV tui đang set tạm nào có login đồ xong tui đổi lại ===============
        private readonly DatabaseContext _context;

        public ProductManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: ProductManagement
        public async Task<IActionResult> Index(string searchString, string loaiSp, string thuongHieu, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLoaiSp"] = loaiSp;
            ViewData["CurrentThuongHieu"] = thuongHieu;

            var query = _context.Sanphams.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(s => s.IdSp.ToLower().Contains(searchString) || 
                                        s.TenSp.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(loaiSp))
            {
                query = query.Where(s => s.LoaiSp == loaiSp);
            }

            if (!string.IsNullOrEmpty(thuongHieu))
            {
                query = query.Where(s => s.ThuongHieu == thuongHieu);
            }

            ViewBag.LoaiSps = await _context.Sanphams.Select(s => s.LoaiSp).Distinct().ToListAsync();
            ViewBag.ThuongHieus = await _context.Sanphams.Select(s => s.ThuongHieu).Distinct().ToListAsync();

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
                .Include(s => s.IdNvNavigation)
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
            ViewData["IdNv"] = "NV001";
            return View();
        }

        // POST: ProductManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSp,TenSp,Gia,SoLuongTon,ThuongHieu,MoTa,ThongSoKyThuat,LoaiSp,IdNv,hinh_anh")] Sanpham sanpham, IFormFile? imageFile)
        {
            try
            {
                // Bỏ qua validation cho các trường sẽ được tự động set
                ModelState.Remove("IdSp");
                ModelState.Remove("IdNv");
                ModelState.Remove("IdNvNavigation");
                ModelState.Remove("hinh_anh");
                // Bỏ ThongSoKyThuat khỏi danh sách remove validation
                // ModelState.Remove("ThongSoKyThuat");

                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
                    ViewData["IdNv"] = "NV001";
                    return View(sanpham);
                }

                // Kiểm tra và xử lý hình ảnh
                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Vui lòng chọn hình ảnh sản phẩm");
                    ViewData["IdNv"] = "NV001";
                    return View(sanpham);
                }

                // Set các giá trị mặc định
                sanpham.IdNv = "NV001";

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

                sanpham.hinh_anh = "/Images/ProductImage/" + fileName;

                // Lấy thông số kỹ thuật từ form và xử lý
                var thongSoKyThuat = Request.Form["ThongSoKyThuat"].ToString();
                if (!string.IsNullOrEmpty(thongSoKyThuat))
                {
                    sanpham.ThongSoKyThuat = thongSoKyThuat;
                }
                else
                {
                    ModelState.AddModelError("ThongSoKyThuat", "Thông số kỹ thuật không được để trống");
                    ViewData["IdNv"] = "NV001";
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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                ViewData["IdNv"] = "NV001";
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
            
            // Set IdNv mặc định là "NV001"
            ViewData["IdNv"] = "NV001";
            return View(sanpham);
        }

        // POST: ProductManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSp,TenSp,Gia,SoLuongTon,ThuongHieu,MoTa,ThongSoKyThuat,LoaiSp,IdNv,hinh_anh")] Sanpham sanpham, IFormFile? imageFile)
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

                // Set IdNv cố định là "NV001"
                sanpham.IdNv = "NV001";

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
                    if (!string.IsNullOrEmpty(existingProduct.hinh_anh))
                    {
                        var oldImagePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            existingProduct.hinh_anh.TrimStart('/')
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

                    sanpham.hinh_anh = "/Images/ProductImage/" + fileName;
                }
                else
                {
                    // Giữ nguyên đường dẫn hình ảnh cũ
                    sanpham.hinh_anh = existingProduct.hinh_anh;
                }

                // Xử lý thông số kỹ thuật
                var thongSoKyThuat = Request.Form["ThongSoKyThuat"].ToString();
                if (!string.IsNullOrEmpty(thongSoKyThuat))
                {
                    sanpham.ThongSoKyThuat = thongSoKyThuat;
                }
                else
                {
                    ModelState.AddModelError("ThongSoKyThuat", "Thông số kỹ thuật không được để trống");
                    return View(sanpham);
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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
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
                .Include(s => s.IdNvNavigation)
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
                            s.TenSp.ToLower().Contains(term))
                .Take(5)
                .Select(s => new { s.IdSp, s.TenSp })
                .ToListAsync();

            return Json(suggestions);
        }
    }
}
