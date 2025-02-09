using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Text.Json;

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
                .Include(d => d.IdKhNavigation)    
                .Include(d => d.IdMggNavigation)   
                .Select(d => new Donhang
                {
                    IdDh = d.IdDh,
                    Trangthai = d.Trangthai,
                    Tongtien = d.Tongtien,
                    Diachigiaohang = d.Diachigiaohang,
                    Ngaydathang = d.Ngaydathang,
                    Phuongthucthanhtoan = d.Phuongthucthanhtoan,
                    IdKh = d.IdKh,
                    IdMgg = d.IdMgg,
                    Ghichu = d.Ghichu,
                    LydoHuy = d.LydoHuy,
                    IdKhNavigation = d.IdKhNavigation,
                    IdMggNavigation = d.IdMggNavigation
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(d => d.IdDh.ToLower().Contains(searchString) ||
                                       (d.IdKhNavigation != null && 
                                        d.IdKhNavigation.Hoten.ToLower().Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.Trangthai == trangThai);
            }

            if (ngayDat.HasValue)
            {
                var ngayDatDateTime = ngayDat.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(d => d.Ngaydathang == ngayDatDateTime.Date);
            }

            // Lấy danh sách trạng thái để làm dropdown filter
            ViewBag.TrangThais = new List<string> 
            { 
                "Đặt hàng thành công",
                "Đã duyệt đơn",
                "Đang giao",
                "Giao thành công",
                "Không giao",
                "Hủy đơn"
            };

            try 
            {
                var totalItems = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

                var model = new PaginatedList<Donhang>(items, totalItems, pageNumber, pageSize);
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
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
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
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
        public async Task<IActionResult> Create([Bind("IdKh,Diachigiaohang,Phuongthucthanhtoan,IdMgg,Ghichu,LydoHuy,Tongtien,Trangthai")] Donhang donhang, 
            string chitietdonhangs, string? Mathanhtoan, string? NoiDungThanhToan)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                // Kiểm tra mã thanh toán nếu là thanh toán online
                if (donhang.Phuongthucthanhtoan != "COD")
                {
                    if (string.IsNullOrEmpty(Mathanhtoan))
                    {
                        throw new Exception("Vui lòng nhập mã thanh toán cho phương thức thanh toán online!");
                    }

                    var existingPayment = await _context.Thanhtoans
                        .FirstOrDefaultAsync(t => t.Mathanhtoan == Mathanhtoan);
                    if (existingPayment != null)
                    {
                        throw new Exception("Mã thanh toán này đã tồn tại trong hệ thống!");
                    }
                }

                // Tạo mã đơn hàng mới
                donhang.IdDh = await GenerateOrderId();
                donhang.Ngaydathang = DateTime.Now;

                // Thêm đơn hàng
                _context.Donhangs.Add(donhang);

                // Kiểm tra và cập nhật số lượng tồn kho
                if (!string.IsNullOrEmpty(chitietdonhangs))
                {
                    var chitietList = JsonSerializer.Deserialize<List<Chitietdonhang>>(chitietdonhangs);
                    foreach (var chitiet in chitietList)
                    {
                        chitiet.IdDh = donhang.IdDh;
                        var product = await _context.Sanphams.FindAsync(chitiet.IdSp);
                        if (product == null)
                        {
                            throw new Exception($"Không tìm thấy sản phẩm {chitiet.IdSp}!");
                        }

                        // Kiểm tra số lượng tồn
                        if (product.Soluongton < chitiet.Soluong)
                        {
                            throw new Exception($"Sản phẩm {product.Tensanpham} chỉ còn {product.Soluongton} sản phẩm!");
                        }

                        // Trừ số lượng tồn ngay khi tạo đơn hàng
                        product.Soluongton -= chitiet.Soluong;
                        _context.Update(product);

                        _context.Chitietdonhangs.Add(chitiet);
                    }
                }

                await _context.SaveChangesAsync();

                // Thêm thông tin thanh toán nếu là thanh toán online
                if (!string.IsNullOrEmpty(Mathanhtoan) && donhang.Phuongthucthanhtoan != "COD")
                {
                    var thanhtoan = new Thanhtoan
                    {
                        IdTt = await GeneratePaymentId(),
                        Mathanhtoan = Mathanhtoan,
                        Trangthai = "Chờ xác nhận",
                        Tienthanhtoan = donhang.Tongtien,
                        Ngaythanhtoan = DateTime.Now,
                        Noidungthanhtoan = NoiDungThanhToan ?? "",
                        IdDh = donhang.IdDh
                    };
                    _context.Thanhtoans.Add(thanhtoan);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                
                TempData["Success"] = "Tạo đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(donhang);
            }
        }

        private async Task<string> GenerateOrderId()
        {
            var lastOrder = await _context.Donhangs
                .OrderByDescending(d => Convert.ToInt32(d.IdDh.Substring(2)))
                .FirstOrDefaultAsync();

            if (lastOrder == null)
            {
                return "DH000001";
            }

            int lastNumber = Convert.ToInt32(lastOrder.IdDh.Substring(2));
            return $"DH{(lastNumber + 1):D6}";
        }

        private async Task<string> GeneratePaymentId()
        {
            string newId = "TT000001";
            var lastPayment = await _context.Thanhtoans
                .OrderByDescending(t => t.IdTt)
                .Select(t => new { t.IdTt })
                .FirstOrDefaultAsync();

            if (lastPayment != null)
            {
                int lastNumber = int.Parse(lastPayment.IdTt.Substring(2));
                newId = $"TT{(lastNumber + 1):D6}";
            }

            return newId;
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
            try 
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Json(new { success = false, message = "Mã sản phẩm không được để trống" });
                }

                // Thêm log để debug
                Console.WriteLine($"Searching for product with ID: {id}");

                var product = await _context.Sanphams
                    .Where(s => s.IdSp == id) // Bỏ Trim() và ToUpper() vì có thể gây lỗi
                    .Select(s => new { 
                        s.IdSp, 
                        s.Tensanpham, 
                        s.Gia, 
                        s.Soluongton,
                        success = true 
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    Console.WriteLine($"Product not found: {id}");
                    return Json(new { success = false, message = $"Không tìm thấy sản phẩm với mã {id}" });
                }

                Console.WriteLine($"Product found: {product.Tensanpham}");
                return Json(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProductInfo: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi tìm sản phẩm" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscountInfo(string id, bool isEdit = false)
        {
            try 
            {
                var discount = await _context.Magiamgia
                    .Where(m => m.IdMgg == id)
                    .FirstOrDefaultAsync();

                if (discount == null)
                {
                    return Json(new { success = false, message = "Mã giảm giá không tồn tại" });
                }

                // Nếu đang ở chế độ Edit, bỏ qua kiểm tra hạn sử dụng
                if (isEdit)
                {
                    return Json(new { 
                        success = true,
                        tilechietkhau = discount.Tilechietkhau,
                        message = $"Áp dụng giảm giá {discount.Tilechietkhau}%"
                    });
                }

                // Kiểm tra các điều kiện cho trang Create
                var today = DateOnly.FromDateTime(DateTime.Now);

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
                    tilechietkhau = discount.Tilechietkhau,
                    message = $"Áp dụng giảm giá {discount.Tilechietkhau}% (Có hiệu lực đến {discount.Ngayhethan:dd/MM/yyyy})"
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
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            // Load thông tin thanh toán nếu có
            if (donhang.Phuongthucthanhtoan != "COD")
            {
                var thanhtoan = await _context.Thanhtoans
                    .Where(t => t.IdDh == id)
                    .OrderByDescending(t => t.Ngaythanhtoan)
                    .FirstOrDefaultAsync();

                if (thanhtoan != null)
                {
                    ViewBag.Mathanhtoan = thanhtoan.Mathanhtoan;
                    ViewBag.TrangthaiThanhtoan = thanhtoan.Trangthai;
                    ViewBag.Noidungthanhtoan = thanhtoan.Noidungthanhtoan;
                }
            }

            return View(donhang);
        }

        // POST: OrderManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDh,IdKh,Trangthai,Tongtien,Diachigiaohang,Ngaydathang,Phuongthucthanhtoan,IdMgg,Ghichu,LydoHuy")] Donhang donhang,
            string? Mathanhtoan, string? TrangthaiThanhtoan, string? Noidungthanhtoan)
        {
            if (id != donhang.IdDh)
            {
                return NotFound();
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Bỏ qua validation cho các trường không cần thiết
                ModelState.Remove("IdKhNavigation");
                ModelState.Remove("IdMggNavigation");
                ModelState.Remove("Chitietdonhangs");

                // Lấy đơn hàng hiện tại và chi tiết
                var existingOrder = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin đơn hàng
                donhang.Chitietdonhangs = existingOrder.Chitietdonhangs;
                _context.Entry(existingOrder).State = EntityState.Detached;
                _context.Update(donhang);

                // Xử lý thông tin thanh toán
                if (donhang.Phuongthucthanhtoan != "COD" && !string.IsNullOrEmpty(Mathanhtoan))
                {
                    var thanhtoan = await _context.Thanhtoans
                        .FirstOrDefaultAsync(t => t.IdDh == id);

                    if (thanhtoan == null)
                    {
                        thanhtoan = new Thanhtoan
                        {
                            IdTt = await GeneratePaymentId(),
                            Mathanhtoan = Mathanhtoan,
                            IdDh = id,
                            Trangthai = TrangthaiThanhtoan ?? "Chờ thanh toán",
                            Tienthanhtoan = donhang.Tongtien,
                            Ngaythanhtoan = DateTime.Now,
                            Noidungthanhtoan = Noidungthanhtoan ?? ""
                        };
                        _context.Thanhtoans.Add(thanhtoan);
                    }
                    else\
                    {
                        thanhtoan.Mathanhtoan = Mathanhtoan;
                        thanhtoan.Trangthai = TrangthaiThanhtoan ?? thanhtoan.Trangthai;
                        thanhtoan.Tienthanhtoan = donhang.Tongtien;
                        thanhtoan.Noidungthanhtoan = Noidungthanhtoan ?? thanhtoan.Noidungthanhtoan;
                        _context.Update(thanhtoan);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(donhang);
            }
        }

        public class ChitietdonhangDTO
        {
            public string IdSp { get; set; }
            public int Soluong { get; set; }
            public decimal Dongia { get; set; }
        }

        // API endpoint để lấy chi tiết đơn hàng
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(string id)
        {
            var details = await _context.Chitietdonhangs
                .Include(c => c.IdSp)
                .Where(c => c.IdDh == id)
                .Select(c => new
                {
                    idSp = c.IdSp,
                    tenSp = c.IdSpNavigation.Tensanpham,
                    soluong = c.Soluong,
                    dongia = c.Dongia
                })
                .ToListAsync();

            return Json(details);
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
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                var donhang = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (donhang == null)
                {
                    return NotFound();
                }

                // Cập nhật Dictionary các trạng thái hợp lệ theo flow mới
                var validTransitions = new Dictionary<string, string[]>
                {
                    { "Đặt hàng thành công", new[] { "Đã duyệt đơn", "Hủy đơn" } },
                    { "Đã duyệt đơn", new[] { "Đang giao" } },
                    { "Đang giao", new[] { "Giao thành công", "Không giao" } },
                    { "Giao thành công", Array.Empty<string>() }, // Trạng thái cuối
                    { "Không giao", Array.Empty<string>() }, // Trạng thái cuối
                    { "Hủy đơn", Array.Empty<string>() } // Trạng thái cuối
                };

                // Kiểm tra tính hợp lệ của việc chuyển trạng thái
                if (!validTransitions.ContainsKey(donhang.Trangthai) || 
                    !validTransitions[donhang.Trangthai].Contains(newStatus))
                {
                    TempData["Error"] = $"Không thể chuyển trạng thái từ '{donhang.Trangthai}' sang '{newStatus}'!";
                    return RedirectToAction(nameof(Index));
                }

                // Xử lý các logic bổ sung theo trạng thái
                switch (newStatus)
                {
                    case "Hủy đơn":
                    case "Không giao":
                        // Hoàn trả số lượng tồn kho
                        foreach (var chitiet in donhang.Chitietdonhangs)
                        {
                            var product = await _context.Sanphams.FindAsync(chitiet.IdSp);
                            if (product != null)
                            {
                                product.Soluongton += chitiet.Soluong;
                                _context.Update(product);
                            }
                        }
                        break;
                }

                // Cập nhật trạng thái đơn hàng
                donhang.Trangthai = newStatus;
                _context.Update(donhang);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"Đã cập nhật trạng thái đơn hàng thành '{newStatus}'!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscountSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            var today = DateOnly.FromDateTime(DateTime.Now);
            var suggestions = await _context.Magiamgia
                .Where(m => (m.IdMgg.Contains(term) || m.Ten.Contains(term)) &&
                            m.Ngaysudung <= today &&
                            m.Ngayhethan >= today &&
                            m.Soluong > 0 &&
                            m.Trangthai == true)
                .Take(5)
                .Select(m => new
                {
                    id = m.IdMgg,
                    ten = m.Ten,
                    tilechietkhau = m.Tilechietkhau,
                    ngayhethan = m.Ngayhethan.ToString("dd/MM/yyyy"),
                    soluong = m.Soluong
                })
                .ToListAsync();

            return Json(suggestions);
        }
    }
}