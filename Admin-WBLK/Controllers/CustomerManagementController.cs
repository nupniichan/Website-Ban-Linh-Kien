using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Admin_WBLK.Controllers
{
    public class CustomerManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public CustomerManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: CustomerManagement
        public async Task<IActionResult> Index(string searchString, string sortOrder, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var query = _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .AsQueryable();

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                query = query.Where(k => 
                    k.IdKh.ToLower().Contains(searchString) ||
                    k.Hoten.ToLower().Contains(searchString) ||
                    k.Email.ToLower().Contains(searchString) ||
                    k.Sodienthoai.Contains(searchString)
                );
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(k => k.Hoten);
                    break;
                default:
                    query = query.OrderBy(k => k.Hoten);
                    break;
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedList<Khachhang>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<object>());

            term = term.ToLower();
            var suggestions = await _context.Khachhangs
                .Where(k => k.IdKh.ToLower().Contains(term) ||
                           k.Hoten.ToLower().Contains(term) ||
                           k.Email.ToLower().Contains(term))
                .Take(5)
                .Select(k => new { k.IdKh, k.Hoten, k.Email })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: CustomerManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(m => m.IdKh == id);

            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // GET: CustomerManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hoten,Email,Diachi,Sodienthoai,Gioitinh,Ngaysinh")] Khachhang khachhang)
        {
            try
            {
                // Bỏ qua validation cho các trường navigation và IdTk
                ModelState.Remove("IdTkNavigation");
                ModelState.Remove("IdTk");
                ModelState.Remove("IdKh");

                // Custom validation
                if (string.IsNullOrEmpty(khachhang.Hoten) || khachhang.Hoten.Length < 2 || khachhang.Hoten.Length > 50)
                {
                    ModelState.AddModelError("Hoten", "Họ tên phải từ 2 đến 50 ký tự");
                }

                if (string.IsNullOrEmpty(khachhang.Diachi) || khachhang.Diachi.Length < 5 || khachhang.Diachi.Length > 200)
                {
                    ModelState.AddModelError("Diachi", "Địa chỉ phải từ 5 đến 200 ký tự");
                }

                if (!Regex.IsMatch(khachhang.Sodienthoai, @"^0\d{9}$"))
                {
                    ModelState.AddModelError("Sodienthoai", "Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)");
                }

                // Validate ngày sinh (giống nhau cho cả Create và Edit)
                var today = DateOnly.FromDateTime(DateTime.Today);
                var birthDate = khachhang.Ngaysinh; // Đã là DateOnly

                // Kiểm tra ngày sinh không được là ngày hiện tại hoặc trong tương lai
                if (birthDate >= today)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không thể là ngày hiện tại hoặc trong tương lai");
                    return View(khachhang);
                }

                // Tính tuổi chính xác
                var age = today.Year - birthDate.Year;
                if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
                {
                    age--;
                }

                if (age < 18)
                {
                    ModelState.AddModelError("Ngaysinh", "Bạn phải đủ 18 tuổi");
                }
                else if (age > 100)
                {
                    ModelState.AddModelError("Ngaysinh", "Tuổi không được quá 100");
                }

                if (!ModelState.IsValid)
                {
                    return View(khachhang);
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (await _context.Taikhoans.AnyAsync(t => t.Tentaikhoan == khachhang.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng");
                    return View(khachhang);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Tạo ID cho khách hàng
                    khachhang.IdKh = GenerateCustomerId();

                    // Tạo tài khoản mới cho khách hàng
                    var taikhoan = new Taikhoan
                    {
                        IdTk = GenerateAccountId(),
                        Tentaikhoan = khachhang.Email,
                        Matkhau = "123456",
                        Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                        Quyentruycap = "KhachHang"
                    };

                    Console.WriteLine($"Created account: {System.Text.Json.JsonSerializer.Serialize(taikhoan)}");

                    // Lưu tài khoản vào database
                    _context.Taikhoans.Add(taikhoan);
                    await _context.SaveChangesAsync();

                    // Gán IdTk cho khách hàng
                    khachhang.IdTk = taikhoan.IdTk;

                    // Lưu khách hàng vào database
                    _context.Add(khachhang);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    Console.WriteLine("Customer created successfully");
                    TempData["Success"] = "Thêm khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Transaction error: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(khachhang);
            }
        }

        private string GenerateAccountId()
        {
            Console.WriteLine("Generating new account ID...");
            try
            {
                // Lấy ID tài khoản cuối cùng
                var lastAccount = _context.Taikhoans
                    .OrderByDescending(t => t.IdTk)
                    .FirstOrDefault();

                Console.WriteLine($"Last account found: {(lastAccount?.IdTk ?? "none")}");

                if (lastAccount == null)
                {
                    return "TK00001";
                }

                // Lấy số từ chuỗi TKxxxxx
                string numStr = lastAccount.IdTk.Substring(2);
                if (int.TryParse(numStr, out int lastNumber))
                {
                    string newId = $"TK{(lastNumber + 1):D5}";
                    Console.WriteLine($"Generated new ID: {newId}");

                    // Kiểm tra xem ID mới đã tồn tại chưa
                    while (_context.Taikhoans.Any(t => t.IdTk == newId))
                    {
                        lastNumber++;
                        newId = $"TK{lastNumber:D5}";
                        Console.WriteLine($"ID existed, trying next ID: {newId}");
                    }

                    return newId;
                }
                else
                {
                    throw new Exception($"Cannot parse number from ID: {lastAccount.IdTk}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating account ID: {ex.Message}");
                throw;
            }
        }

        private string GenerateCustomerId()
        {
            Console.WriteLine("Generating new customer ID...");
            try
            {
                var lastCustomer = _context.Khachhangs
                    .OrderByDescending(k => k.IdKh)
                    .FirstOrDefault();

                Console.WriteLine($"Last customer found: {(lastCustomer?.IdKh ?? "none")}");

                if (lastCustomer == null)
                {
                    return "KH00001";
                }

                string numStr = lastCustomer.IdKh.Substring(2);
                if (int.TryParse(numStr, out int lastNumber))
                {
                    string newId = $"KH{(lastNumber + 1):D5}";
                    Console.WriteLine($"Generated new ID: {newId}");

                    while (_context.Khachhangs.Any(k => k.IdKh == newId))
                    {
                        lastNumber++;
                        newId = $"KH{lastNumber:D5}";
                        Console.WriteLine($"ID existed, trying next ID: {newId}");
                    }

                    return newId;
                }
                else
                {
                    throw new Exception($"Cannot parse number from ID: {lastCustomer.IdKh}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating customer ID: {ex.Message}");
                throw;
            }
        }

        // GET: CustomerManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(m => m.IdKh == id);

            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // POST: CustomerManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdKh,Hoten,Email,Diachi,Sodienthoai,Gioitinh,Ngaysinh")] Khachhang khachhang)
        {
            if (id != khachhang.IdKh)
            {
                return NotFound();
            }

            try
            {
                // Bỏ qua validation cho các trường navigation và IdTk
                ModelState.Remove("IdTkNavigation");
                ModelState.Remove("IdTk");

                // Custom validation
                if (string.IsNullOrEmpty(khachhang.Hoten) || khachhang.Hoten.Length < 2 || khachhang.Hoten.Length > 50)
                {
                    ModelState.AddModelError("Hoten", "Họ tên phải từ 2 đến 50 ký tự");
                }

                if (string.IsNullOrEmpty(khachhang.Diachi) || khachhang.Diachi.Length < 5 || khachhang.Diachi.Length > 200)
                {
                    ModelState.AddModelError("Diachi", "Địa chỉ phải từ 5 đến 200 ký tự");
                }

                if (!Regex.IsMatch(khachhang.Sodienthoai, @"^0\d{9}$"))
                {
                    ModelState.AddModelError("Sodienthoai", "Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)");
                }

                // Validate ngày sinh (giống nhau cho cả Create và Edit)
                var today = DateOnly.FromDateTime(DateTime.Today);
                var birthDate = khachhang.Ngaysinh; // Đã là DateOnly

                // Kiểm tra ngày sinh không được là ngày hiện tại hoặc trong tương lai
                if (birthDate >= today)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không thể là ngày hiện tại hoặc trong tương lai");
                    return View(khachhang);
                }

                // Tính tuổi chính xác
                var age = today.Year - birthDate.Year;
                if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
                {
                    age--;
                }

                if (age < 18)
                {
                    ModelState.AddModelError("Ngaysinh", "Bạn phải đủ 18 tuổi");
                }
                else if (age > 100)
                {
                    ModelState.AddModelError("Ngaysinh", "Tuổi không được quá 100");
                }

                if (!ModelState.IsValid)
                {
                    return View(khachhang);
                }

                // Lấy khách hàng hiện tại từ database
                var existingCustomer = await _context.Khachhangs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(k => k.IdKh == id);

                if (existingCustomer == null)
                {
                    return NotFound();
                }

                // Giữ nguyên IdTk từ khách hàng cũ
                khachhang.IdTk = existingCustomer.IdTk;

                // Cập nhật thông tin khách hàng
                _context.Entry(khachhang).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachhangExists(khachhang.IdKh))
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
                return View(khachhang);
            }
        }

        // GET: CustomerManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(m => m.IdKh == id);

            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // POST: CustomerManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try 
            {
                var khachhang = await _context.Khachhangs.FindAsync(id);
                if (khachhang != null)
                {
                    _context.Khachhangs.Remove(khachhang);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa khách hàng thành công!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Không thể xóa khách hàng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool KhachhangExists(string id)
        {
            return _context.Khachhangs.Any(e => e.IdKh == id);
        }
    }
}

        