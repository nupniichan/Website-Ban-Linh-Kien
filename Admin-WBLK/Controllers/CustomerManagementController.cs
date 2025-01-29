using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Diagnostics;
using System.Text.Json;

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
        public async Task<IActionResult> Index()
        {
            var khachhangs = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .ToListAsync();
            return View(khachhangs);
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
                Console.WriteLine("Create action started");

                // Bỏ qua validation cho các trường navigation và IdTk
                ModelState.Remove("IdTkNavigation");
                ModelState.Remove("IdTk");
                ModelState.Remove("IdKh");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"Validation error: {error.ErrorMessage}");
                        }
                    }
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

                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                    }
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

        