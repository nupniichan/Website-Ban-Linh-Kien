using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class PaymentResultController : Controller
    {
        // GET: PaymentResultController
        public ActionResult PaymentSuccess()
        {
            return View();
        }
        public ActionResult PaymentFailed()
        {
            return View();
        }
    }
}
