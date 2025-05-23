using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                query = query.Where(k =>
                    k.IdKh.ToLower().Contains(searchString) ||
                    k.Hoten.ToLower().Contains(searchString) ||
                    k.Email.ToLower().Contains(searchString) ||
                    k.Sodienthoai.Contains(searchString));
            }

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
            var items = await query.Skip((pageNumber - 1) * pageSize)
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
        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .FirstOrDefaultAsync(m => m.IdKh == id);
            if (khachhang == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(khachhang);
        }

        // GET: CustomerManagement/Create
        public IActionResult Create()
        {
            ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan");
            return View();
        }

        // POST: CustomerManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hoten,Email,Diachi,Sodienthoai,Gioitinh,Ngaysinh,IdTk")] Khachhang khachhang)
        {
            try
            {
                // Remove navigation/ID validations
                ModelState.Remove("IdTkNavigation");
                ModelState.Remove("IdKh");

                // If an account is selected, validate it:
                if (!string.IsNullOrEmpty(khachhang.IdTk))
                {
                    if (_context.Nhanviens.Any(n => n.Idtk == khachhang.IdTk))
                        ModelState.AddModelError("IdTk", "Tài khoản này thuộc về nhân viên.");
                    if (_context.Khachhangs.Any(c => c.IdTk == khachhang.IdTk))
                        ModelState.AddModelError("IdTk", "Tài khoản này đã thuộc về khách hàng khác.");
                    var account = await _context.Taikhoans.FirstOrDefaultAsync(a => a.IdTk == khachhang.IdTk);
                    if (account != null && account.Quyentruycap.Equals("quantrivien", StringComparison.OrdinalIgnoreCase))
                        ModelState.AddModelError("IdTk", "Tài khoản quản trị viên không được liên kết với khách hàng.");
                }

                // Custom validation for customer fields
                if (string.IsNullOrEmpty(khachhang.Hoten) || khachhang.Hoten.Length < 2 || khachhang.Hoten.Length > 50)
                    ModelState.AddModelError("Hoten", "Họ tên phải từ 2 đến 50 ký tự");
                if (string.IsNullOrEmpty(khachhang.Diachi) || khachhang.Diachi.Length < 5 || khachhang.Diachi.Length > 200)
                    ModelState.AddModelError("Diachi", "Địa chỉ phải từ 5 đến 200 ký tự");
                if (!Regex.IsMatch(khachhang.Sodienthoai, @"^0\d{9}$"))
                    ModelState.AddModelError("Sodienthoai", "Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)");

                var today = DateOnly.FromDateTime(DateTime.Today);
                if (!khachhang.Ngaysinh.HasValue)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không được để trống");
                    return View(khachhang);
                }
                var birthDate = khachhang.Ngaysinh.Value;
                if (birthDate >= today)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không thể là ngày hiện tại hoặc trong tương lai");
                    return View(khachhang);
                }
                var age = today.Year - birthDate.Year;
                if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
                    age--;
                if (age < 18)
                    ModelState.AddModelError("Ngaysinh", "Bạn phải đủ 18 tuổi");
                else if (age > 100)
                    ModelState.AddModelError("Ngaysinh", "Tuổi không được quá 100");

                if (!ModelState.IsValid)
                {
                    ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", khachhang.IdTk);
                    return View(khachhang);
                }

                // Check if the email (used as account username) is already used in Taikhoans.
                if (await _context.Taikhoans.AnyAsync(t => t.Tentaikhoan == khachhang.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng");
                    ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", khachhang.IdTk);
                    return View(khachhang);
                }

                // Generate new customer ID and save
                khachhang.IdKh = GenerateCustomerId();
                _context.Khachhangs.Add(khachhang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", khachhang.IdTk);
                return View(khachhang);
            }
        }

        private string GenerateCustomerId()
        {
            var lastCustomer = _context.Khachhangs.OrderByDescending(k => k.IdKh).FirstOrDefault();
            if (lastCustomer == null)
                return "KH00001";
            string numStr = lastCustomer.IdKh.Substring(2);
            if (int.TryParse(numStr, out int lastNumber))
                return $"KH{(lastNumber + 1):D5}";
            else
                throw new Exception($"Cannot parse customer ID number from {lastCustomer.IdKh}");
        }

        // GET: CustomerManagement/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
            return View(khachhang);
        }

        // POST: CustomerManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdKh,Hoten,Email,Sodienthoai,Diachi,Gioitinh,Ngaysinh,IdTk")] Khachhang khachhang, string returnUrl = null)
        {
            Console.WriteLine("Edit POST method called");
            Console.WriteLine($"ID: {id}, Customer ID: {khachhang.IdKh}, Return URL: {returnUrl}");
            
            if (id != khachhang.IdKh)
            {
                Console.WriteLine("ID mismatch");
                return NotFound();
            }

            try
            {
                // Thêm dòng debug để xem dữ liệu đầu vào
                Console.WriteLine($"Editing customer: {JsonSerializer.Serialize(khachhang)}");
                
                // Kiểm tra xem form có gửi đúng dữ liệu không
                Console.WriteLine($"Form data - Name: {khachhang.Hoten}, Email: {khachhang.Email}, Phone: {khachhang.Sodienthoai}");
                Console.WriteLine($"Form data - Address: {khachhang.Diachi}, Gender: {khachhang.Gioitinh}, DOB: {khachhang.Ngaysinh}");
                Console.WriteLine($"Form data - Account ID: {khachhang.IdTk}");
                
                // Xóa các validation không cần thiết
                ModelState.Remove("IdTkNavigation");
                ModelState.Remove("returnUrl");
                
                // Kiểm tra ModelState
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid:");
                    foreach (var state in ModelState)
                    {
                        if (state.Value.Errors.Count > 0)
                        {
                            Console.WriteLine($"- {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                        }
                    }
                }
                
                // Validate linked account if provided
                if (!string.IsNullOrEmpty(khachhang.IdTk))
                {
                    if (_context.Nhanviens.Any(n => n.Idtk == khachhang.IdTk))
                        ModelState.AddModelError("IdTk", "Tài khoản này thuộc về nhân viên.");
                    if (_context.Khachhangs.Any(c => c.IdTk == khachhang.IdTk && c.IdKh != id))
                        ModelState.AddModelError("IdTk", "Tài khoản này đã thuộc về khách hàng khác.");
                    var account = await _context.Taikhoans.FirstOrDefaultAsync(a => a.IdTk == khachhang.IdTk);
                    if (account != null && account.Quyentruycap.Equals("quantrivien", StringComparison.OrdinalIgnoreCase))
                        ModelState.AddModelError("IdTk", "Tài khoản quản trị viên không được liên kết với khách hàng.");
                }

                // Custom validations for Khachhang fields
                if (string.IsNullOrEmpty(khachhang.Hoten) || khachhang.Hoten.Length < 2 || khachhang.Hoten.Length > 50)
                    ModelState.AddModelError("Hoten", "Họ tên phải từ 2 đến 50 ký tự");
                if (string.IsNullOrEmpty(khachhang.Diachi) || khachhang.Diachi.Length < 5 || khachhang.Diachi.Length > 200)
                    ModelState.AddModelError("Diachi", "Địa chỉ phải từ 5 đến 200 ký tự");
                if (!Regex.IsMatch(khachhang.Sodienthoai, @"^0\d{9}$"))
                    ModelState.AddModelError("Sodienthoai", "Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)");

                var today = DateOnly.FromDateTime(DateTime.Today);
                if (!khachhang.Ngaysinh.HasValue)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không được để trống");
                    ViewData["ReturnUrl"] = returnUrl;
                    ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
                    return View(khachhang);
                }
                var birthDate = khachhang.Ngaysinh.Value;
                if (birthDate >= today)
                {
                    ModelState.AddModelError("Ngaysinh", "Ngày sinh không thể là ngày hiện tại hoặc trong tương lai");
                    ViewData["ReturnUrl"] = returnUrl;
                    ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
                    return View(khachhang);
                }
                var age = today.Year - birthDate.Year;
                if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
                    age--;
                if (age < 18)
                    ModelState.AddModelError("Ngaysinh", "Bạn phải đủ 18 tuổi");
                else if (age > 100)
                    ModelState.AddModelError("Ngaysinh", "Tuổi không được quá 100");

                if (!ModelState.IsValid)
                {
                    ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
                    ViewData["ReturnUrl"] = returnUrl;
                    return View(khachhang);
                }

                // Thay đổi cách cập nhật dữ liệu
                var existingCustomer = await _context.Khachhangs.FindAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                // Cập nhật từng trường thay vì cập nhật toàn bộ entity
                existingCustomer.Hoten = khachhang.Hoten;
                existingCustomer.Email = khachhang.Email;
                existingCustomer.Sodienthoai = khachhang.Sodienthoai;
                existingCustomer.Diachi = khachhang.Diachi;
                existingCustomer.Gioitinh = khachhang.Gioitinh;
                existingCustomer.Ngaysinh = khachhang.Ngaysinh;
                existingCustomer.IdTk = khachhang.IdTk;

                // Đánh dấu entity đã được sửa đổi
                _context.Entry(existingCustomer).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật thông tin khách hàng thành công!";
                
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency error: {ex.Message}");
                if (!KhachhangExists(khachhang.IdKh))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu đã bị thay đổi bởi người khác. Vui lòng thử lại.");
                    ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
                    ViewData["ReturnUrl"] = returnUrl;
                    return View(khachhang);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                ViewBag.Accounts = new SelectList(_context.Taikhoans.Where(t => t.Quyentruycap == "khachhang"), "IdTk", "Tentaikhoan", khachhang.IdTk);
                ViewData["ReturnUrl"] = returnUrl;
                return View(khachhang);
            }
        }

        // GET: CustomerManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(m => m.IdKh == id);
            if (khachhang == null) return NotFound();
            return View(khachhang);
        }

        // POST: CustomerManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var khachhang = await _context.Khachhangs
                    .Include(k => k.Donhangs)
                    .FirstOrDefaultAsync(k => k.IdKh == id);

                if (khachhang == null)
                    return NotFound();

                if (khachhang.Donhangs.Any())
                {
                    TempData["Error"] = "Không thể xóa khách hàng này vì họ đang có đơn hàng.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Khachhangs.Remove(khachhang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa khách hàng thành công!";
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
