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
            try
            {
                // Thống kê đơn hàng
                var totalOrders = await _context.Donhangs.CountAsync();
                var completedOrders = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")
                    .CountAsync();
                var pendingOrders = await _context.Donhangs
                    .Where(d => d.Trangthai == "Đang giao")
                    .CountAsync();

                // Doanh thu - bỏ điều kiện HasValue vì Tongtien là decimal không phải decimal?
                var totalRevenue = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")
                    .SumAsync(d => d.Tongtien);

                // Thống kê theo phương thức thanh toán - thêm logging để debug
                var paymentStats = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")  // Bỏ điều kiện check null của phương thức
                    .GroupBy(d => d.Phuongthucthanhtoan ?? "Không xác định")  // Xử lý null ngay tại đây
                    .Select(g => new
                    {
                        Method = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(d => d.Tongtien)
                    })
                    .ToListAsync();

                // Thêm logging chi tiết
                foreach (var stat in paymentStats)
                {
                    _logger.LogInformation($"Payment Method: {stat.Method}, Count: {stat.Count}, Amount: {stat.Amount}");
                }

                // Đơn hàng gần đây
                var recentOrders = await _context.Donhangs
                    .Where(d => !string.IsNullOrEmpty(d.Trangthai))
                    .OrderByDescending(d => d.Ngaydathang ?? DateTime.MinValue)
                    .Take(5)
                    .Select(d => new
                    {
                        IdDh = d.IdDh,
                        Ngaydathang = d.Ngaydathang,
                        Tongtien = d.Tongtien,  // Bỏ ?? 0 vì Tongtien là decimal
                        Trangthai = d.Trangthai ?? "Không xác định",
                        Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "Không xác định"
                    })
                    .ToListAsync();

                // Log để debug
                _logger.LogInformation($"Total Orders: {totalOrders}");
                _logger.LogInformation($"Completed Orders: {completedOrders}");
                _logger.LogInformation($"Payment Stats: {JsonSerializer.Serialize(paymentStats)}");

                ViewBag.TotalOrders = totalOrders;
                ViewBag.CompletedOrders = completedOrders;
                ViewBag.PendingOrders = pendingOrders;
                ViewBag.TotalRevenue = totalRevenue;
                ViewBag.PaymentStats = paymentStats;
                ViewBag.RecentOrders = recentOrders;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Dashboard Index: {ex.Message}");
                throw;
            }
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
