using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public LoginController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Local Login
        [HttpPost]
        public async Task<IActionResult> Login(string tentaikhoan, string password, bool remember = false)
        {
            if (string.IsNullOrEmpty(tentaikhoan) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
            }

            // Check credentials in Taikhoans
            var account = _dbContext.Taikhoans
                .FirstOrDefault(t => t.Tentaikhoan == tentaikhoan && t.Matkhau == password);

            if (account == null)
            {
                return Json(new { success = false, message = "Tài khoản hoặc mật khẩu không chính xác" });
            }

            // Ensure it's a "khachhang"
            if (account.Quyentruycap.ToLower() != "khachhang")
            {
                return Json(new { success = false, message = "Bạn không có quyền truy cập vào hệ thống này" });
            }

            // Find linked Khachhang
            var customer = _dbContext.Khachhangs
                .FirstOrDefault(kh => kh.IdTk == account.IdTk);
            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
            }

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Tentaikhoan),
                new Claim(ClaimTypes.NameIdentifier, customer.IdKh),
                new Claim(ClaimTypes.Role, account.Quyentruycap),
                new Claim("CustomerId", customer.IdKh),
                new Claim("AccountId", account.IdTk),
                new Claim("Email", customer.Email),
                new Claim("FullName", customer.Hoten),
                new Claim("CreatedDate", account.Ngaytaotk.ToString()),
                new Claim("LastModified", account.Ngaysuadoi?.ToString() ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = remember,
                ExpiresUtc = remember ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Home"),
                message = "Đăng nhập thành công"
            });
        }

        // POST: Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Initiate external login (Google or Facebook)
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Login", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        // Handle the external login callback
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login");
            }

            // 1. Authenticate using the "External" scheme
            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // 2. Extract user info from Google/Facebook claims
            var externalPrincipal = result.Principal;
            var email = externalPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName = externalPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Nhà cung cấp không trả về email.");
                return RedirectToAction("Login");
            }

            // 3. Check if a Khachhang with this email exists
            var existingKhachhang = _dbContext.Khachhangs
                .Include(kh => kh.IdTkNavigation)
                .FirstOrDefault(kh => kh.Email == email);

            if (existingKhachhang == null)
            {
                // 3a. No existing user -> create new Taikhoan + Khachhang
                // Use the same incremental ID approach as your RegisterController
                var newCustomerId = GenerateNewCustomerId();
                var newAccountId = GenerateNewAccountId();

                var newAccount = new Taikhoan
                {
                    IdTk = newAccountId,
                    Tentaikhoan = email,          // or "google_" + email if you prefer
                    Matkhau = "",                // no local password
                    Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                    Ngaysuadoi = null,
                    Quyentruycap = "KHACHHANG"
                };

                var newCustomer = new Khachhang
                {
                    IdKh = newCustomerId,
                    Hoten = fullName ?? "",
                    Email = email,
                    Diachi = "Vui lòng đổi địa chỉ",
                    Sodienthoai = "0000000000",
                    Gioitinh = null,
                    Ngaysinh = null,
                    IdTk = newAccountId,
                    Loaikhachhang = 1
                };

                _dbContext.Taikhoans.Add(newAccount);
                _dbContext.Khachhangs.Add(newCustomer);
                _dbContext.SaveChanges();

                existingKhachhang = newCustomer;
            }

            // 4. Sign in using the existing or newly created Taikhoan
            var account = existingKhachhang.IdTkNavigation;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Tentaikhoan),
                new Claim(ClaimTypes.NameIdentifier, existingKhachhang.IdKh),
                new Claim(ClaimTypes.Role, account.Quyentruycap),
                new Claim("CustomerId", existingKhachhang.IdKh),
                new Claim("AccountId", account.IdTk),
                new Claim("Email", existingKhachhang.Email ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // 5. Clear the external cookie
            await HttpContext.SignOutAsync("External");

            // 6. Redirect to returnUrl or homepage
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        // Use the same approach as your RegisterController for incremental IDs
        private string GenerateNewAccountId()
        {
            var lastAccount = _dbContext.Taikhoans
                .OrderByDescending(t => t.IdTk)
                .FirstOrDefault();

            if (lastAccount == null)
            {
                // If no records, start with "TK00001"
                return "TK00001";
            }

            // Example: "TK00012" -> parse "00012" -> 12 -> +1 -> 13 -> "TK00013"
            int lastNumber = int.Parse(lastAccount.IdTk.Substring(2));
            return $"TK{(lastNumber + 1).ToString("D5")}";
        }

        private string GenerateNewCustomerId()
        {
            var lastCustomer = _dbContext.Khachhangs
                .OrderByDescending(k => k.IdKh)
                .FirstOrDefault();

            if (lastCustomer == null)
            {
                // If no records, start with "KH000001"
                return "KH000001";
            }

            // Example: "KH000123" -> parse "000123" -> 123 -> +1 -> 124 -> "KH000124"
            int lastNumber = int.Parse(lastCustomer.IdKh.Substring(2));
            return $"KH{(lastNumber + 1).ToString("D6")}";
        }
    }
}
