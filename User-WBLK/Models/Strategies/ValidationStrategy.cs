using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Website_Ban_Linh_Kien.Models.Strategies
{
    // Interface cho các chiến lược validation
    public interface IValidationStrategy
    {
        (bool isValid, IActionResult errorResult) Validate(RegisterViewModel model, DatabaseContext dbContext);
    }

    // Chiến lược kiểm tra độ dài username
    public class UsernameValidationStrategy : IValidationStrategy
    {
        public (bool isValid, IActionResult errorResult) Validate(RegisterViewModel model, DatabaseContext dbContext)
        {
            if (model.Username.Length < 3)
            {
                return (false, new JsonResult(new { success = false, message = "Tên tài khoản phải có ít nhất 3 ký tự" }));
            }

            // Kiểm tra username đã tồn tại chưa
            if (dbContext.Taikhoans.Any(t => t.Tentaikhoan == model.Username))
            {
                return (false, new JsonResult(new { success = false, message = "Tên tài khoản đã được sử dụng" }));
            }

            return (true, null);
        }
    }

    // Chiến lược kiểm tra mật khẩu
    public class PasswordValidationStrategy : IValidationStrategy
    {
        public (bool isValid, IActionResult errorResult) Validate(RegisterViewModel model, DatabaseContext dbContext)
        {
            if (model.Password.Length < 8)
            {
                return (false, new JsonResult(new { success = false, message = "Mật khẩu phải có ít nhất 8 ký tự" }));
            }

            // Check for common/weak passwords
            string[] commonPasswords = { "password", "123456", "12345678", "abc123", "qwerty", "111111", "password123" };
            if (commonPasswords.Contains(model.Password.ToLower()))
            {
                return (false, new JsonResult(new { success = false, message = "Mật khẩu quá đơn giản, vui lòng chọn mật khẩu khác" }));
            }

            return (true, null);
        }
    }

    // Chiến lược kiểm tra tuổi
    public class AgeValidationStrategy : IValidationStrategy
    {
        public (bool isValid, IActionResult errorResult) Validate(RegisterViewModel model, DatabaseContext dbContext)
        {
            var today = DateTime.Today;
            var age = today.Year - model.BirthDate.Year;
            if (model.BirthDate > today.AddYears(-age)) age--;

            if (age < 13)
            {
                return (false, new JsonResult(new { success = false, message = "Bạn phải từ 13 tuổi trở lên để đăng ký" }));
            }
            if (age > 100)
            {
                return (false, new JsonResult(new { success = false, message = "Tuổi không hợp lệ" }));
            }

            return (true, null);
        }
    }

    // Composite Strategy kết hợp tất cả các chiến lược validation
    public class CompositeValidationStrategy : IValidationStrategy
    {
        private readonly List<IValidationStrategy> _strategies;

        public CompositeValidationStrategy()
        {
            _strategies = new List<IValidationStrategy>
            {
                new UsernameValidationStrategy(),
                new PasswordValidationStrategy(),
                new AgeValidationStrategy()
            };
        }

        public (bool isValid, IActionResult errorResult) Validate(RegisterViewModel model, DatabaseContext dbContext)
        {
            foreach (var strategy in _strategies)
            {
                var (isValid, errorResult) = strategy.Validate(model, dbContext);
                if (!isValid)
                {
                    return (isValid, errorResult);
                }
            }

            return (true, null);
        }
    }
} 