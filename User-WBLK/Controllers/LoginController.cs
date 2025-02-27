using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;  // Include models
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _dbContext;

        // Constructor to inject DbContext
        public LoginController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string tentaikhoan, string password, bool remember = false)
        {
            if (string.IsNullOrEmpty(tentaikhoan) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
            }

            // Kiểm tra tài khoản và mật khẩu trong bảng TaiKhoan
            var account = _dbContext.Taikhoans.FirstOrDefault(t => 
                t.Tentaikhoan == tentaikhoan && 
                t.Matkhau == password);

            if (account == null)
            {
                return Json(new { success = false, message = "Tài khoản hoặc mật khẩu không chính xác" });
            }

            // Kiểm tra quyền truy cập
            if (account.Quyentruycap.ToLower() != "khachhang")
            {
                return Json(new { success = false, message = "Bạn không có quyền truy cập vào hệ thống này" });
            }

            // Kiểm tra nếu tài khoản có liên kết với khách hàng
            var customer = _dbContext.Khachhangs.FirstOrDefault(kh => kh.IdTk == account.IdTk);
            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
            }

            // Tạo claims cho authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Tentaikhoan),
                new Claim(ClaimTypes.NameIdentifier, customer.IdKh), // <-- KEY FIX: match "KH000001", etc.
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
