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

                // Validate username length
                if (model.Username.Length < 3)
                {
                    return Json(new { success = false, message = "Tên tài khoản phải có ít nhất 3 ký tự" });
                }

                // Validate password
                if (model.Password.Length < 8)
                {
                    return Json(new { success = false, message = "Mật khẩu phải có ít nhất 8 ký tự" });
                }

                // Check for common/weak passwords
                string[] commonPasswords = { "password", "123456", "12345678", "abc123", "qwerty", "111111", "password123" };
                if (commonPasswords.Contains(model.Password.ToLower()))
                {
                    return Json(new { success = false, message = "Mật khẩu quá đơn giản, vui lòng chọn mật khẩu khác" });
                }

                // Validate age
                var today = DateTime.Today;
                var age = today.Year - model.BirthDate.Year;
                if (model.BirthDate > today.AddYears(-age)) age--;

                if (age < 13)
                {
                    return Json(new { success = false, message = "Bạn phải từ 13 tuổi trở lên để đăng ký" });
                }
                if (age > 100)
                {
                    return Json(new { success = false, message = "Tuổi không hợp lệ" });
                }

                // Validate phone number uniqueness
                if (_dbContext.Khachhangs.Any(k => k.Sodienthoai == model.Phone))
                {
                    return Json(new { success = false, message = "Số điện thoại đã được sử dụng" });
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

                // Get the latest account ID and increment
                string newAccountId;
                var lastAccount = _dbContext.Taikhoans
                    .OrderByDescending(t => t.IdTk)
                    .FirstOrDefault();

                if (lastAccount == null)
                {
                    newAccountId = "TK00001";
                }
                else
                {
                    // Extract the number from the last ID and increment
                    int lastNumber = int.Parse(lastAccount.IdTk.Substring(2));
                    newAccountId = $"TK{(lastNumber + 1).ToString("D5")}";
                }

                // Create new account
                var newAccount = new Taikhoan
                {
                    IdTk = newAccountId,
                    Tentaikhoan = model.Username,
                    Matkhau = model.Password,
                    Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                    Ngaysuadoi = null,
                    Quyentruycap = "KHACHHANG"
                };

                // Get the latest customer ID and increment
                string newCustomerId;
                var lastCustomer = _dbContext.Khachhangs
                    .OrderByDescending(k => k.IdKh)
                    .FirstOrDefault();

                if (lastCustomer == null)
                {
                    newCustomerId = "KH00001";
                }
                else
                {
                    // Extract the number from the last ID and increment
                    int lastNumber = int.Parse(lastCustomer.IdKh.Substring(2));
                    newCustomerId = $"KH{(lastNumber + 1).ToString("D5")}";
                }

                // Create new customer with the new ID format
                var newCustomer = new Khachhang
                {
                    IdKh = newCustomerId,
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
