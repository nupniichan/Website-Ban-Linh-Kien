using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class SharedController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 