using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using System.Text.RegularExpressions;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public RegisterController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: RegisterController
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel model)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                // Check if username exists
                if (_dbContext.Taikhoans.Any(t => t.Tentaikhoan == model.Username))
                {
                    return Json(new { success = false, message = "Tên tài khoản đã được sử dụng" });
                }

                // Check if email exists
                if (_dbContext.Khachhangs.Any(k => k.Email == model.Email))
                {
                    return Json(new { success = false, message = "Email đã được sử dụng" });
                }

                // Create new account
                var newAccount = new Taikhoan
                {
                    IdTk = Guid.NewGuid().ToString(),
                    Tentaikhoan = model.Username,
                    Matkhau = model.Password,
                    Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                    Ngaysuadoi = null,
                    Quyentruycap = "KhachHang"
                };

                // Create new customer
                var newCustomer = new Khachhang
                {
                    IdKh = Guid.NewGuid().ToString(),
                    Hoten = model.FullName,
                    Email = model.Email,
                    Diachi = model.Address,
                    Gioitinh = model.Gender,
                    Ngaysinh = DateOnly.FromDateTime(model.BirthDate),
                    Sodienthoai = model.Phone,
                    IdTk = newAccount.IdTk
                };

                _dbContext.Taikhoans.Add(newAccount);
                _dbContext.Khachhangs.Add(newCustomer);
                _dbContext.SaveChanges();

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Home"),
                    message = "Đăng ký thành công"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi đăng ký" });
            }
        }
    }
}
