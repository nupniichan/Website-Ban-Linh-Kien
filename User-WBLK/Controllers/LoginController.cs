using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // TODO: Thêm logic xác thực đăng nhập
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
            }

            // Giả sử đăng nhập thành công
            return Json(new { 
                success = true, 
                redirectUrl = Url.Action("Index", "Home"),
                message = "Đăng nhập thành công"
            });
        }
    }
}
