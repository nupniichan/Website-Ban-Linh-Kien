using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class PaymentResultController : Controller
    {
        private readonly DatabaseContext _context;

        public PaymentResultController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: PaymentResultController
        public ActionResult PaymentSuccess()
        {
            // Hiển thị thông tin đơn hàng từ TempData nếu có
            return View();
        }
        public ActionResult PaymentFailed()
        {
            return View();
        }
    }
}
