using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class RegisterController : Controller
    {
        // GET: RegisterController
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel model)
        {
            // Giả lập response thành công để test giao diện
            return Json(new { 
                success = true, 
                redirectUrl = Url.Action("Index", "Home"),
                message = "Đăng ký thành công"
            });
        }
    }

    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
