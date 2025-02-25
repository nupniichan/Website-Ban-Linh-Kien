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

                // Kiểm tra username trước
                if (_dbContext.Taikhoans.Any(t => t.Tentaikhoan == model.Username))
                {
                    return Json(new { success = false, message = "Tên tài khoản đã được sử dụng" });
                }

                // Kiểm tra khách hàng theo email và số điện thoại
                var customerByEmail = _dbContext.Khachhangs
                    .FirstOrDefault(k => k.Email == model.Email);
                var customerByPhone = _dbContext.Khachhangs
                    .FirstOrDefault(k => k.Sodienthoai == model.Phone);

                // Trường hợp 1: Có một trong hai thông tin nhưng không phải cả hai
                if ((customerByEmail != null && customerByPhone == null) || 
                    (customerByEmail == null && customerByPhone != null))
                {
                    var existingCustomer = customerByEmail ?? customerByPhone;
                    
                    if (existingCustomer.IdTk != null)
                    {
                        // Khách hàng đã có tài khoản
                        string message = customerByEmail != null ? 
                            "Email này đã được đăng ký tài khoản" : 
                            "Số điện thoại này đã được đăng ký tài khoản";
                        return Json(new { success = false, message = message });
                    }
                    else
                    {
                        // Khách hàng là guest
                        string message = $"Phát hiện bạn đã từng mua hàng với {(customerByEmail != null ? "email" : "số điện thoại")} này.\n\n" +
                                       $"Vui lòng sử dụng cả email và số điện thoại đã mua hàng trước đó:\n" +
                                       $"Email: {existingCustomer.Email}\n" +
                                       $"Số điện thoại: {existingCustomer.Sodienthoai}\n\n" +
                                       "Điều này sẽ giúp liên kết các đơn hàng cũ với tài khoản mới của bạn.";
                        return Json(new { 
                            success = false, 
                            message = message,
                            isGuest = true,
                            guestInfo = new {
                                email = existingCustomer.Email,
                                phone = existingCustomer.Sodienthoai
                            }
                        });
                    }
                }

                // Trường hợp 2: Cả email và số điện thoại đều tồn tại
                if (customerByEmail != null && customerByPhone != null)
                {
                    // Kiểm tra xem có phải cùng một khách hàng không
                    if (customerByEmail.IdKh != customerByPhone.IdKh)
                    {
                        return Json(new { 
                            success = false, 
                            message = "Email và số điện thoại này thuộc về hai khách hàng khác nhau.\n" +
                                     "Vui lòng kiểm tra lại thông tin của bạn." 
                        });
                    }

                    // Kiểm tra xem khách hàng đã có tài khoản chưa
                    if (customerByEmail.IdTk != null)
                    {
                        return Json(new { 
                            success = false, 
                            message = "Khách hàng này đã có tài khoản.\n" +
                                     "Vui lòng đăng nhập hoặc sử dụng chức năng quên mật khẩu." 
                        });
                    }

                    // Tạo tài khoản mới cho khách hàng guest
                    string newAccountId = GenerateNewAccountId();
                    var newAccount = new Taikhoan
                    {
                        IdTk = newAccountId,
                        Tentaikhoan = model.Username,
                        Matkhau = model.Password,
                        Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                        Ngaysuadoi = null,
                        Quyentruycap = "KHACHHANG"
                    };

                    // Cập nhật thông tin khách hàng
                    customerByEmail.Hoten = model.FullName;
                    customerByEmail.Diachi = model.Address;
                    customerByEmail.Gioitinh = model.Gender;
                    customerByEmail.Ngaysinh = DateOnly.FromDateTime(model.BirthDate);
                    customerByEmail.IdTk = newAccount.IdTk;
                    customerByEmail.Loaikhachhang = 1;

                    _dbContext.Taikhoans.Add(newAccount);
                    _dbContext.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Index", "Home"),
                        message = "Đăng ký thành công!\n" +
                                 "Tất cả đơn hàng trước đây của bạn đã được liên kết với tài khoản mới."
                    });
                }

                // Trường hợp 3: Kiểm tra xem email hoặc số điện thoại đã tồn tại trong bảng tài khoản
                var existingAccount = _dbContext.Taikhoans
                    .FirstOrDefault(t => t.Tentaikhoan == model.Username);
                if (existingAccount != null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Email hoặc số điện thoại này đã được đăng ký tài khoản.\n" +
                                 "Vui lòng sử dụng thông tin khác." 
                    });
                }

                // Trường hợp 4: Khách hàng hoàn toàn mới
                string newCustomerId = GenerateNewCustomerId();
                var newCustomer = new Khachhang
                {
                    IdKh = newCustomerId,
                    Hoten = model.FullName,
                    Email = model.Email,
                    Diachi = model.Address,
                    Gioitinh = model.Gender,
                    Ngaysinh = DateOnly.FromDateTime(model.BirthDate),
                    Sodienthoai = model.Phone,
                    IdTk = GenerateNewAccountId(),
                    Loaikhachhang = 1
                };

                var accountForNewCustomer = new Taikhoan
                {
                    IdTk = newCustomer.IdTk,
                    Tentaikhoan = model.Username,
                    Matkhau = model.Password,
                    Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                    Ngaysuadoi = null,
                    Quyentruycap = "KHACHHANG"
                };

                _dbContext.Taikhoans.Add(accountForNewCustomer);
                _dbContext.Khachhangs.Add(newCustomer);
                _dbContext.SaveChanges();

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Home"),
                    message = "Đăng ký tài khoản thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi đăng ký" });
            }
        }

        private string GenerateNewAccountId()
        {
            var lastAccount = _dbContext.Taikhoans
                .OrderByDescending(t => t.IdTk)
                .FirstOrDefault();

            if (lastAccount == null)
            {
                return "TK00001";
            }

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
                return "KH000001";
            }

            int lastNumber = int.Parse(lastCustomer.IdKh.Substring(2));
            return $"KH{(lastNumber + 1).ToString("D6")}";
        }
    }
}
