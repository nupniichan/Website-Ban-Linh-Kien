using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Website_Ban_Linh_Kien.Models.Factories;
using Website_Ban_Linh_Kien.Models.Strategies;

namespace Website_Ban_Linh_Kien.Models.Services
{
    // Lớp Service xử lý đăng ký, áp dụng Template Method Pattern
    public class RegistrationService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IValidationStrategy _validationStrategy;

        public RegistrationService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _validationStrategy = new CompositeValidationStrategy();
        }

        // Template Method định nghĩa các bước đăng ký
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                // Bước 1: Validate model state
                if (!ValidateModelState())
                {
                    return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                // Bước 2: Validate input data
                var (isValid, errorResult) = _validationStrategy.Validate(model, _dbContext);
                if (!isValid)
                {
                    return errorResult;
                }

                // Bước 3: Kiểm tra khách hàng theo email và số điện thoại
                var customerCheckResult = CheckExistingCustomer(model);
                if (customerCheckResult != null)
                {
                    return customerCheckResult;
                }

                // Bước 4: Tạo tài khoản mới
                return CreateNewAccount(model);
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, message = "Đã xảy ra lỗi khi đăng ký" });
            }
        }

        // Các phương thức con thực hiện các bước cụ thể
        protected virtual bool ValidateModelState()
        {
            return true; // Trong controller thực tế, sẽ kiểm tra ModelState.IsValid
        }

        protected virtual IActionResult CheckExistingCustomer(RegisterViewModel model)
        {
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
                    return new JsonResult(new { success = false, message = message });
                }
                else
                {
                    // Khách hàng là guest
                    string message = $"Phát hiện bạn đã từng mua hàng với {(customerByEmail != null ? "email" : "số điện thoại")} này.\n\n" +
                                   $"Vui lòng sử dụng cả email và số điện thoại đã mua hàng trước đó:\n" +
                                   $"Email: {existingCustomer.Email}\n" +
                                   $"Số điện thoại: {existingCustomer.Sodienthoai}\n\n" +
                                   "Điều này sẽ giúp liên kết các đơn hàng cũ với tài khoản mới của bạn.";
                    return new JsonResult(new { 
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
                    return new JsonResult(new { 
                        success = false, 
                        message = "Email và số điện thoại này thuộc về hai khách hàng khác nhau.\n" +
                                 "Vui lòng kiểm tra lại thông tin của bạn." 
                    });
                }

                // Kiểm tra xem khách hàng đã có tài khoản chưa
                if (customerByEmail.IdTk != null)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Khách hàng này đã có tài khoản.\n" +
                                 "Vui lòng đăng nhập hoặc sử dụng chức năng quên mật khẩu." 
                    });
                }

                // Tạo tài khoản mới cho khách hàng guest
                var accountIdGenerator = IdGeneratorFactory.CreateAccountIdGenerator();
                string newAccountId = accountIdGenerator.GenerateId(_dbContext.Taikhoans.OrderByDescending(t => t.IdTk).FirstOrDefault()?.IdTk);
                
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

                return new JsonResult(new
                {
                    success = true,
                    redirectUrl = "/Home/Index",
                    message = "Đăng ký thành công!\n" +
                             "Tất cả đơn hàng trước đây của bạn đã được liên kết với tài khoản mới."
                });
            }

            return null; // Không có khách hàng tồn tại
        }

        protected virtual IActionResult CreateNewAccount(RegisterViewModel model)
        {
            // Tạo ID mới cho khách hàng và tài khoản
            var customerIdGenerator = IdGeneratorFactory.CreateCustomerIdGenerator();
            var accountIdGenerator = IdGeneratorFactory.CreateAccountIdGenerator();
            
            string newCustomerId = customerIdGenerator.GenerateId(_dbContext.Khachhangs.OrderByDescending(k => k.IdKh).FirstOrDefault()?.IdKh);
            string newAccountId = accountIdGenerator.GenerateId(_dbContext.Taikhoans.OrderByDescending(t => t.IdTk).FirstOrDefault()?.IdTk);

            // Tạo khách hàng mới
            var newCustomer = new Khachhang
            {
                IdKh = newCustomerId,
                Hoten = model.FullName,
                Email = model.Email,
                Diachi = model.Address,
                Gioitinh = model.Gender,
                Ngaysinh = DateOnly.FromDateTime(model.BirthDate),
                Sodienthoai = model.Phone,
                IdTk = newAccountId,
                Loaikhachhang = 1
            };

            // Tạo tài khoản mới
            var accountForNewCustomer = new Taikhoan
            {
                IdTk = newAccountId,
                Tentaikhoan = model.Username,
                Matkhau = model.Password,
                Ngaytaotk = DateOnly.FromDateTime(DateTime.Now),
                Ngaysuadoi = null,
                Quyentruycap = "KHACHHANG"
            };

            _dbContext.Taikhoans.Add(accountForNewCustomer);
            _dbContext.Khachhangs.Add(newCustomer);
            _dbContext.SaveChanges();

            return new JsonResult(new
            {
                success = true,
                redirectUrl = "/Home/Index",
                message = "Đăng ký tài khoản thành công!"
            });
        }
    }
} 