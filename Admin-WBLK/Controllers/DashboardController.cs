using System.Diagnostics;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Admin_WBLK.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DatabaseContext _context;

        public DashboardController(ILogger<DashboardController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Thống kê đơn hàng
            var totalOrders = await _context.Donhangs.CountAsync();
            var completedOrders = await _context.Donhangs
                .Where(d => d.Trangthai == "Giao thành công")
                .CountAsync();
            var pendingOrders = await _context.Donhangs
                .Where(d => d.Trangthai == "Đang giao")
                .CountAsync();

            // Doanh thu
            var totalRevenue = await _context.Donhangs
                .Where(d => d.Trangthai == "Giao thành công")
                .SumAsync(d => d.Tongtien);

            // Thống kê theo phương thức thanh toán
            var paymentStats = await _context.Donhangs
                .Where(d => d.Trangthai == "Giao thành công")
                .GroupBy(d => d.Phuongthucthanhtoan)
                .Select(g => new
                {
                    Method = g.Key,
                    Count = g.Count(),
                    Amount = g.Sum(d => d.Tongtien)
                })
                .ToListAsync();

            // Thêm log để kiểm tra
            _logger.LogInformation($"Payment Stats: {JsonSerializer.Serialize(paymentStats)}");

            // Đơn hàng gần đây
            var recentOrders = await _context.Donhangs
                .OrderByDescending(d => d.Ngaydathang)
                .Take(5)
                .Select(d => new
                {
                    d.IdDh,
                    d.Ngaydathang,
                    d.Tongtien,
                    d.Trangthai,
                    d.Phuongthucthanhtoan
                })
                .ToListAsync();

            ViewBag.TotalOrders = totalOrders;
            ViewBag.CompletedOrders = completedOrders;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.PaymentStats = paymentStats;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
