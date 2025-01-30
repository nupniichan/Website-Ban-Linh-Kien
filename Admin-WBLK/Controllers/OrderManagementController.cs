using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;

namespace Admin_WBLK.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public OrderManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: OrderManagement
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string trangThai, DateOnly? ngayDat, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentTrangThai"] = trangThai;
            ViewData["CurrentNgayDat"] = ngayDat?.ToString("yyyy-MM-dd");

            var query = _context.Donhangs
                .Include(d => d.IdKhNavigation)    // Join với bảng khachhang
                .Include(d => d.IdNvNavigation)    // Join với bảng nhanvien
                .Include(d => d.IdMggNavigation)   // Join với bảng magiamgia
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(d => d.IdDh.ToLower().Contains(searchString) ||
                                       d.IdKhNavigation.Hoten.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.Trangthai == trangThai);
            }

            if (ngayDat.HasValue)
            {
                query = query.Where(d => d.Ngaydathang == ngayDat.Value);
            }

            // Lấy danh sách trạng thái để làm dropdown filter
            ViewBag.TrangThais = new List<string> 
            { 
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang giao",
                "Đã giao",
                "Đã hủy",
                "Yêu cầu hủy"
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            var model = new PaginatedList<Donhang>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<object>());

            term = term.ToLower();
            var suggestions = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Where(d => d.IdDh.ToLower().Contains(term) ||
                           d.IdKhNavigation.Hoten.ToLower().Contains(term))
                .Take(5)
                .Select(d => new { d.IdDh, CustomerName = d.IdKhNavigation.Hoten })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: OrderManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)    // Include thông tin khách hàng
                .Include(d => d.IdNvNavigation)    // Include thông tin nhân viên
                .Include(d => d.IdMggNavigation)   // Include thông tin mã giảm giá
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // GET: OrderManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKh,Diachigiaohang,Phuongthucthanhtoan,IdMgg")] Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                donhang.IdDh = GenerateOrderId(); // Tạo mã đơn hàng tự động
                donhang.Trangthai = "Chờ xác nhận";
                
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donhang);
        }

        private string GenerateOrderId()
        {
            // Logic tạo mã đơn hàng tự động
            return "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        // API endpoints for AJAX calls
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo(string id)
        {
            var customer = await _context.Khachhangs
                .Where(k => k.IdKh == id)
                .Select(k => new { k.IdKh, k.Hoten })
                .FirstOrDefaultAsync();
            return Json(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string id)
        {
            var product = await _context.Sanphams
                .Where(s => s.IdSp == id)
                .Select(s => new { s.IdSp, s.TenSp, s.Gia, s.SoLuongTon })
                .FirstOrDefaultAsync();
            return Json(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscountInfo(string id)
        {
            try 
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                Console.WriteLine($"Debug - Today: {today:dd/MM/yyyy}");

                var discount = await _context.Magiamgia
                    .Where(m => m.IdMgg == id)
                    .FirstOrDefaultAsync();

                if (discount == null)
                {
                    return Json(new { success = false, message = "Mã giảm giá không tồn tại" });
                }

                if (today < discount.Ngaysudung)
                {
                    return Json(new { 
                        success = false, 
                        message = $"Mã giảm giá chưa đến ngày sử dụng (Bắt đầu từ: {discount.Ngaysudung:dd/MM/yyyy})" 
                    });
                }

                if (today > discount.Ngayhethan)
                {
                    return Json(new { 
                        success = false, 
                        message = $"Mã giảm giá đã hết hạn (Hết hạn ngày: {discount.Ngayhethan:dd/MM/yyyy})" 
                    });
                }

                if (discount.Soluong <= 0)
                {
                    return Json(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });
                }

                return Json(new { 
                    success = true,
                    tilechietkhau = discount.Tilechietkhau * 100,
                    message = $"Áp dụng giảm giá {discount.Tilechietkhau * 100}% (Có hiệu lực đến {discount.Ngayhethan:dd/MM/yyyy})"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDiscountInfo: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi kiểm tra mã giảm giá" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffInfo(string id)
        {
            var staff = await _context.Nhanviens
                .Where(n => n.IdNv == id)
                .Select(n => new { n.IdNv, n.Hoten })
                .FirstOrDefaultAsync();
            return Json(staff);
        }

        // GET: OrderManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdNvNavigation)
                .Include(d => d.IdMggNavigation)
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: OrderManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang != null)
            {
                _context.Donhangs.Remove(donhang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa đơn hàng thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool DonhangExists(string id)
        {
            return _context.Donhangs.Any(e => e.IdDh == id);
        }

        // GET: OrderManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdNvNavigation)
                .Include(d => d.IdMggNavigation)
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: OrderManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDh,IdKh,IdNv,Trangthai,Tongtien,Diachigiaohang,Ngaydathang,Phuongthucthanhtoan,IdMgg")] Donhang donhang)
        {
            if (id != donhang.IdDh)
            {
                return NotFound();
            }

            try
            {
                // Bỏ qua validation cho các trường sẽ được tự động set
                ModelState.Remove("IdKhNavigation");
                ModelState.Remove("IdNvNavigation");
                ModelState.Remove("IdMggNavigation");

                if (!ModelState.IsValid)
                {
                    return View(donhang);
                }

                // Lấy đơn hàng hiện tại từ database
                var existingOrder = await _context.Donhangs.AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Cập nhật các trường có thể sửa
                existingOrder.IdKh = donhang.IdKh;
                existingOrder.IdNv = donhang.IdNv;
                existingOrder.Trangthai = donhang.Trangthai;
                existingOrder.Diachigiaohang = donhang.Diachigiaohang;
                existingOrder.Phuongthucthanhtoan = donhang.Phuongthucthanhtoan;
                existingOrder.IdMgg = donhang.IdMgg;
                existingOrder.Ngaydathang = donhang.Ngaydathang;
                existingOrder.Tongtien = donhang.Tongtien;

                _context.Update(existingOrder);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonhangExists(donhang.IdDh))
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
                return View(donhang);
            }
        }

        // GET: OrderManagement/UpdateStatus/5
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(string id, string newStatus)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newStatus))
            {
                return BadRequest();
            }

            try
            {
                var donhang = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (donhang == null)
                {
                    return NotFound();
                }

                // Kiểm tra tính hợp lệ của trạng thái mới
                var validTransitions = new Dictionary<string, string[]>
                {
                    { "Chờ xác nhận", new[] { "Đã xác nhận" } },
                    { "Đã xác nhận", new[] { "Đang giao" } },
                    { "Đang giao", new[] { "Đã giao" } },
                    { "Yêu cầu hủy", new[] { "Đã hủy" } }
                };

                if (!validTransitions.ContainsKey(donhang.Trangthai) || 
                    !validTransitions[donhang.Trangthai].Contains(newStatus))
                {
                    TempData["Error"] = "Không thể chuyển trạng thái đơn hàng!";
                    return RedirectToAction(nameof(Index));
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Cập nhật trạng thái đơn hàng
                    donhang.Trangthai = newStatus;

                    // Xử lý các logic bổ sung theo trạng thái
                    switch (newStatus)
                    {
                        case "Đã xác nhận":
                            // Kiểm tra và cập nhật số lượng tồn kho
                            foreach (var detail in donhang.Chitietdonhangs)
                            {
                                var product = await _context.Sanphams.FindAsync(detail.IdSp);
                                if (product.SoLuongTon < detail.Soluong)
                                {
                                    throw new Exception($"Sản phẩm {product.TenSp} không đủ số lượng trong kho!");
                                }
                            }
                            break;

                        case "Đã giao":
                            // Cập nhật số lượng tồn kho
                            foreach (var detail in donhang.Chitietdonhangs)
                            {
                                var product = await _context.Sanphams.FindAsync(detail.IdSp);
                                product.SoLuongTon -= detail.Soluong;
                                _context.Update(product);
                            }

                            // Cập nhật số lượng mã giảm giá nếu có
                            if (!string.IsNullOrEmpty(donhang.IdMgg))
                            {
                                var discount = await _context.Magiamgia.FindAsync(donhang.IdMgg);
                                if (discount != null && discount.Soluong > 0)
                                {
                                    discount.Soluong--;
                                    _context.Update(discount);
                                }
                            }
                            break;

                        case "Đã hủy":
                            // Hoàn trả số lượng tồn kho nếu đã xác nhận trước đó
                            if (donhang.Trangthai == "Đã xác nhận" || donhang.Trangthai == "Đang giao")
                            {
                                foreach (var detail in donhang.Chitietdonhangs)
                                {
                                    var product = await _context.Sanphams.FindAsync(detail.IdSp);
                                    product.SoLuongTon += detail.Soluong;
                                    _context.Update(product);
                                }
                            }
                            break;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["Success"] = $"Đã cập nhật trạng thái đơn hàng thành '{newStatus}'!";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = $"Lỗi khi cập nhật trạng thái: {ex.Message}";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}