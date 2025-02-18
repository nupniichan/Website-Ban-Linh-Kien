using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Controllers
{
    public class RevenueManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public RevenueManagementController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue(DateTime? fromDate, DateTime? toDate, string? paymentMethod)
        {
            var query = _context.Donhangs.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(d => d.Ngaydathang >= fromDate);
            
            if (toDate.HasValue)
                query = query.Where(d => d.Ngaydathang <= toDate);
            
            if (!string.IsNullOrEmpty(paymentMethod))
                query = query.Where(d => d.Phuongthucthanhtoan == paymentMethod);

            // Chỉ lấy đơn hàng đã hoàn thành
            query = query.Where(d => d.Trangthai == "Giao thành công");

            var result = await query
                .GroupBy(d => d.Phuongthucthanhtoan)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    TotalAmount = g.Sum(d => d.Tongtien),
                    OrderCount = g.Count()
                })
                .ToListAsync();

            return Json(result);
        }
    }
}
