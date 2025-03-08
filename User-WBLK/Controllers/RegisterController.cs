using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using System.Text.RegularExpressions;
using Website_Ban_Linh_Kien.Models.Services;
using Website_Ban_Linh_Kien.Models.Factories;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly RegistrationService _registrationService;

        public RegisterController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _registrationService = new RegistrationService(dbContext);
        }

        // GET: RegisterController
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel model)
        {
            // Sử dụng RegistrationService để xử lý đăng ký
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }
            
            return _registrationService.Register(model);
        }

        // Các phương thức GenerateNewAccountId và GenerateNewCustomerId không còn cần thiết
        // vì đã được chuyển sang các lớp Factory
    }
}
