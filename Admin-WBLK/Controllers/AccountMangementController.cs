using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Admin_WBLK.Models;


namespace Admin_WBLK.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public AccountManagementController(DatabaseContext context)
        {
            _context = context;
        }
                
        // Validation helper method
        private bool IsValidInput(string input, bool isPassword = false)
        {
            if (string.IsNullOrWhiteSpace(input) || input.StartsWith(" ") || input.EndsWith(" "))
            {
                return false;
            }

            if (!isPassword)
            {
                return Regex.IsMatch(input, "^[a-zA-Z0-9_]+$");
            }

            // Password validation: At least 8 chars, one uppercase, one number, one special character
            return Regex.IsMatch(input, @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        }

        // GET: AccountManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Taikhoans.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(a => a.IdTk.ToLower().Contains(searchString) || a.Tentaikhoan.ToLower().Contains(searchString));
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var model = new PaginatedList<Taikhoan>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<object>());

            term = term.ToLower();
            var suggestions = await _context.Taikhoans
                .Where(t => t.IdTk.ToLower().Contains(term) ||
                            t.Tentaikhoan.ToLower().Contains(term))
                .Take(5)
                .Select(t => new { t.IdTk, t.Tentaikhoan })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: AccountManagement/Details/5
        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(m => m.IdTk == id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            // Check the role and get the linked record
            if (taikhoan.Quyentruycap.Equals("nhanvienkho", StringComparison.OrdinalIgnoreCase) ||
                taikhoan.Quyentruycap.Equals("nhanvienkinhdoanh", StringComparison.OrdinalIgnoreCase) ||
                taikhoan.Quyentruycap.Equals("nhanvienmarketing", StringComparison.OrdinalIgnoreCase))
            {
                var employee = await _context.Nhanviens.FirstOrDefaultAsync(n => n.Idtk == taikhoan.IdTk);
                if (employee != null)
                {
                    ViewBag.LinkedName = employee.Hoten;
                    ViewBag.LinkedId = employee.IdNv;
                    ViewBag.LinkedUrl = Url.Action("Details", "EmployeeManagement", new { id = employee.IdNv });
                }
            }
            else if (taikhoan.Quyentruycap.Equals("khachhang", StringComparison.OrdinalIgnoreCase))
            {
                var customer = await _context.Khachhangs.FirstOrDefaultAsync(c => c.IdTk == taikhoan.IdTk);
                if (customer != null)
                {
                    ViewBag.LinkedName = customer.Hoten;
                    ViewBag.LinkedId = customer.IdKh;
                    ViewBag.LinkedUrl = Url.Action("Details", "CustomerManagement", new { id = customer.IdKh });
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(taikhoan);
        }

        private string GetAlphabetSequence(int number)
        {
            string result = "";
            while (number > 0)
            {
                number--; // Adjust to zero-based
                char letter = (char)('A' + (number % 26));
                result = letter + result;
                number /= 26;
            }
            return result;
        }

        // GET: AccountManagement/Create
        public IActionResult Create()
        {
            // Generate next IdTk
            var lastAccount = _context.Taikhoans.OrderByDescending(t => t.IdTk).FirstOrDefault();
            var nextId = "TK000001";

            if (lastAccount != null && int.TryParse(lastAccount.IdTk.Substring(2), out int lastId))
            {
                nextId = $"TK{(lastId + 1).ToString("D6")}";
            }

            var model = new Taikhoan { IdTk = nextId, Ngaytaotk = DateOnly.FromDateTime(DateTime.Now) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdTk,Tentaikhoan,Matkhau,Quyentruycap")] Taikhoan taikhoan, 
            string actionType)
        {
            // Validate account inputs
            if (!IsValidInput(taikhoan.Tentaikhoan))
            {
                ModelState.AddModelError("Tentaikhoan", "Tên tài khoản không hợp lệ. Không chứa khoảng trắng ở đầu/cuối và chỉ gồm chữ, số, dấu gạch dưới.");
            }
            if (!IsValidInput(taikhoan.Matkhau, true))
            {
                ModelState.AddModelError("Matkhau", "Mật khẩu phải có ít nhất 8 ký tự, ít nhất một chữ cái viết hoa, một số và một ký tự đặc biệt, không có khoảng trắng.");
            }
            if (_context.Taikhoans.Any(t => t.Tentaikhoan == taikhoan.Tentaikhoan))
            {
                ModelState.AddModelError("Tentaikhoan", "Tên tài khoản đã tồn tại.");
            }
            if (!ModelState.IsValid)
            {
                return View(taikhoan);
            }
            
            taikhoan.Ngaytaotk = DateOnly.FromDateTime(DateTime.Now);
            taikhoan.Ngaysuadoi = null;
            _context.Add(taikhoan);
            await _context.SaveChangesAsync();

            // For employees:
            if (actionType == "AccountAndEmployee" &&
            (taikhoan.Quyentruycap.Equals("nhanvienkho", StringComparison.OrdinalIgnoreCase) ||
                taikhoan.Quyentruycap.Equals("nhanvienkinhdoanh", StringComparison.OrdinalIgnoreCase) ||
                taikhoan.Quyentruycap.Equals("nhanvienmarketing", StringComparison.OrdinalIgnoreCase)))
            {
                string newNhanvienId = await GenerateNhanvienId();
                string baseName = "Nhân viên ";
                int count = _context.Nhanviens.Count(n => n.Hoten.StartsWith(baseName));
                string sequence = GetAlphabetSequence(count + 1);

                var newNhanvien = new Nhanvien
                {
                    IdNv = newNhanvienId,
                    Hoten = baseName + sequence,
                    Chucvu = taikhoan.Quyentruycap,
                    Luong = 0,
                    Gioitinh = "Chưa xác định",
                    Sodienthoai = "0000000000",
                    Email = "abc@gmail.com",
                    Diachi = "Road A",
                    Ngayvaolam = DateOnly.FromDateTime(DateTime.Now),
                    Idtk = taikhoan.IdTk
                };

                _context.Nhanviens.Add(newNhanvien);
                await _context.SaveChangesAsync();

                TempData["CreatedNhanvien"] = newNhanvien.IdNv;
            }
            // For customers:
            else if (actionType == "AccountAndCustomer" &&
                    taikhoan.Quyentruycap.Equals("khachhang", StringComparison.OrdinalIgnoreCase))
            {
                string newCustomerId = GenerateCustomerId();
                string baseName = "Khách hàng ";
                int count = _context.Khachhangs.Count(k => k.Hoten.StartsWith(baseName));
                string sequence = GetAlphabetSequence(count + 1);

                var newCustomer = new Khachhang
                {
                    IdKh = newCustomerId,
                    Hoten = baseName + sequence,
                    // Set default values (adjust these defaults as needed)
                    Email = "default@gmail.com",
                    Diachi = "Default Address",
                    Sodienthoai = "0000000000",
                    Gioitinh = "Chưa xác định",
                    Ngaysinh = DateOnly.FromDateTime(new DateTime(1990, 1, 1)),
                    IdTk = taikhoan.IdTk
                };

                _context.Khachhangs.Add(newCustomer);
                await _context.SaveChangesAsync();

                TempData["CreatedCustomer"] = newCustomer.IdKh;

                // Instead of immediately redirecting to Index,
                // redirect to a confirmation view asking if the user wants to edit the customer.
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(Index));
        }


        private string GenerateCustomerId()
        {
            var lastCustomer = _context.Khachhangs.OrderByDescending(k => k.IdKh).FirstOrDefault();
            if (lastCustomer == null)
                return "KH000001";
            string numStr = lastCustomer.IdKh.Substring(2);
            if (int.TryParse(numStr, out int lastNumber))
            {
                return $"KH{(lastNumber + 1):D6}";
            }
            else
            {
                throw new Exception($"Cannot parse customer ID number from {lastCustomer.IdKh}");
            }
        }

        private async Task<string> GenerateNhanvienId()
        {
            var lastNhanvien = await _context.Nhanviens
                                    .OrderByDescending(n => n.IdNv)
                                    .FirstOrDefaultAsync();
            if (lastNhanvien == null)
            {
                return "NV0001";
            }
            int lastNumber = int.Parse(lastNhanvien.IdNv.Substring(2));
            return $"NV{(lastNumber + 1):D4}";
        }


        // GET: AccountManagement/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(taikhoan);
        }

        // POST: AccountManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdTk,Tentaikhoan,Matkhau,Quyentruycap")] Taikhoan taikhoan, string returnUrl)
        {
            if (id != taikhoan.IdTk)
            {
                return NotFound();
            }

            // Validate input as before...
            if (!IsValidInput(taikhoan.Tentaikhoan))
            {
                ModelState.AddModelError("Tentaikhoan", "Tên tài khoản không hợp lệ. Không chứa khoảng trắng ở đầu/cuối và chỉ gồm chữ, số, dấu gạch dưới.");
            }
            if (!string.IsNullOrEmpty(taikhoan.Matkhau) && !IsValidInput(taikhoan.Matkhau, true))
            {
                ModelState.AddModelError("Matkhau", "Mật khẩu phải có ít nhất 8 ký tự, ít nhất một chữ cái viết hoa, một số và một ký tự đặc biệt, không có khoảng trắng.");
            }

            if (!ModelState.IsValid)
            {
                return View(taikhoan);
            }

            try
            {
                // Retrieve the existing account from the database (without tracking)
                var existingAccount = await _context.Taikhoans.AsNoTracking().FirstOrDefaultAsync(t => t.IdTk == id);
                if (existingAccount == null)
                {
                    return NotFound();
                }

                // Define employee roles allowed for change
                var nhanvienRoles = new[] { "nhanvienkho", "nhanvienkinhdoanh", "nhanvienmarketing" };

                // If the existing account is either "khachhang" or "quantrivien", its role cannot be changed.
                if (existingAccount.Quyentruycap == "khachhang" || existingAccount.Quyentruycap == "quantrivien")
                {
                    if (!string.Equals(existingAccount.Quyentruycap, taikhoan.Quyentruycap, StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("Quyentruycap", $"Không được thay đổi quyền truy cập từ '{existingAccount.Quyentruycap}' sang '{taikhoan.Quyentruycap}'.");
                        return View(taikhoan);
                    }
                }
                // Otherwise, if the account is in one of the employee roles, allow changes only within the employee roles.
                else if (nhanvienRoles.Contains(existingAccount.Quyentruycap, StringComparer.OrdinalIgnoreCase))
                {
                    if (!nhanvienRoles.Contains(taikhoan.Quyentruycap, StringComparer.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("Quyentruycap", $"Nếu tài khoản hiện tại là nhân viên (hiện là '{existingAccount.Quyentruycap}'), chỉ được chuyển sang các quyền nhân viên: nhanvienkho, nhanvienkinhdoanh, hoặc nhanvienmarketing. Bạn đã cố gắng chuyển sang '{taikhoan.Quyentruycap}'.");
                        return View(taikhoan);
                    }
                }

                // Preserve original creation date
                taikhoan.Ngaytaotk = existingAccount.Ngaytaotk;
                // If password is left blank, retain the original password.
                if (string.IsNullOrWhiteSpace(taikhoan.Matkhau))
                {
                    taikhoan.Matkhau = existingAccount.Matkhau;
                }

                taikhoan.Ngaysuadoi = DateOnly.FromDateTime(DateTime.Now);
                _context.Taikhoans.Update(taikhoan);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật tài khoản thành công!";
                
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaikhoanExists(taikhoan.IdTk))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }





        // GET: AccountManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(m => m.IdTk == id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // POST: AccountManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan != null)
            {
                _context.Taikhoans.Remove(taikhoan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaikhoanExists(string id)
        {
            return _context.Taikhoans.Any(e => e.IdTk == id);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Taikhoan loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest.Tentaikhoan) || string.IsNullOrWhiteSpace(loginRequest.Matkhau))
                {
                    return BadRequest(new { message = "Tên tài khoản và mật khẩu không được để trống." });
                }

                // Find user by username
                var user = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == loginRequest.Tentaikhoan);
                if (user == null)
                {
                    return Unauthorized(new { message = "Tài khoản không tồn tại." });
                }

                // Restrict access if the role is khachhang
                if (user.Quyentruycap.Equals("khachhang", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(403, new { message = "Bạn không có quyền truy cập!" });
                }

                if (user.Matkhau != loginRequest.Matkhau)
                {
                    return Unauthorized(new { message = "Mật khẩu không chính xác." });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Tentaikhoan),
                    new Claim(ClaimTypes.Role, user.Quyentruycap),
                    new Claim("UserId", user.IdTk)
                };

                // For employee roles, retrieve the employee's name from the Nhanvien table
                if (user.Quyentruycap.Equals("nhanvienkho", StringComparison.OrdinalIgnoreCase) ||
                    user.Quyentruycap.Equals("nhanvienkinhdoanh", StringComparison.OrdinalIgnoreCase) ||
                    user.Quyentruycap.Equals("nhanvienmarketing", StringComparison.OrdinalIgnoreCase))
                {
                    var employee = await _context.Nhanviens.FirstOrDefaultAsync(n => n.Idtk == user.IdTk);
                    if (employee != null)
                    {
                        claims.Add(new Claim("LinkedName", employee.Hoten));
                    }
                }
                // (Optionally, for customers you could do the same if needed.)
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Json(new { role = user.Quyentruycap });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }




        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(); // Trả về phản hồi thành công
        }
    }
}