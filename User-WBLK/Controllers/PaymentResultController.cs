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
        public async Task<ActionResult> PaymentSuccess()
        {
            var orderId = TempData["OrderId"]?.ToString();
            if (orderId != null)
            {
                var payment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == orderId);
                
                if (payment != null)
                {
                    return View(payment);
                }
            }
            return View();
        }

        public async Task<ActionResult> PaymentFailed()
        {
            var orderId = TempData["OrderId"]?.ToString();
            if (orderId != null)
            {
                var payment = await _context.Thanhtoans
                    .FirstOrDefaultAsync(t => t.IdDh == orderId);
                
                if (payment != null)
                {
                    return View(payment);
                }
            }
            return View();
        }
    }
}
