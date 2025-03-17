using System.Diagnostics;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Builders;
using Admin_WBLK.Models.Observers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Admin_WBLK.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DatabaseContext _context;
        private readonly IDashboardBuilder _dashboardBuilder;
        private readonly DashboardDirector _dashboardDirector;
        private readonly IRevenueSubject _revenueSubject;

        public DashboardController(
            ILogger<DashboardController> logger, 
            DatabaseContext context,
            IDashboardBuilder dashboardBuilder,
            DashboardDirector dashboardDirector,
            IRevenueSubject revenueSubject)
        {
            _logger = logger;
            _context = context;
            _dashboardBuilder = dashboardBuilder;
            _dashboardDirector = dashboardDirector;
            _revenueSubject = revenueSubject;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Sử dụng Builder Pattern để xây dựng dữ liệu dashboard
                var dashboardData = await _dashboardDirector.BuildFullDashboard(_dashboardBuilder);
                
                // Thông báo qua Observer Pattern
                await _revenueSubject.NotifyObservers("Đã tạo báo cáo dashboard");
                
                // Truyền dữ liệu vào view
                ViewBag.TotalOrders = dashboardData.TotalOrders;
                ViewBag.CompletedOrders = dashboardData.CompletedOrders;
                ViewBag.PendingOrders = dashboardData.PendingOrders;
                ViewBag.TotalRevenue = dashboardData.TotalRevenue;
                ViewBag.PaymentStats = dashboardData.PaymentStats;
                ViewBag.RecentOrders = dashboardData.RecentOrders;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong Dashboard Index: {ex.Message}");
                throw;
            }
        }
        
        // Endpoint để lấy dashboard tối thiểu (chỉ thống kê đơn hàng và doanh thu)
        [HttpGet]
        public async Task<IActionResult> MinimalDashboard()
        {
            try
            {
                var dashboardData = await _dashboardDirector.BuildMinimalDashboard(_dashboardBuilder);
                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong MinimalDashboard: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        
        // Endpoint để lấy dashboard tùy chỉnh
        [HttpGet]
        public async Task<IActionResult> CustomDashboard(
            bool includeOrderStats = true,
            bool includeRevenueStats = true,
            bool includePaymentStats = false,
            bool includeRecentOrders = false,
            int recentOrdersCount = 5)
        {
            try
            {
                var dashboardData = await _dashboardDirector.BuildCustomDashboard(
                    _dashboardBuilder,
                    includeOrderStats,
                    includeRevenueStats,
                    includePaymentStats,
                    includeRecentOrders,
                    recentOrdersCount);
                    
                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong CustomDashboard: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
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
