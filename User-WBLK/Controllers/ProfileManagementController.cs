using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProfileManagementController : Controller
    {
        // GET: ProfileManagementController
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}
