using Admin_WBLK.Models;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.Observers;
using Admin_WBLK.Models.Facades;
using Admin_WBLK.Models.Templates;
using Admin_WBLK.Models.Composites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Admin_WBLK.Controllers
{
    public class RevenueManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly RevenueFacade _revenueFacade;
        private readonly IRevenueSubject _revenueSubject;
        
        // Template Method Pattern
        private readonly DailyRevenueReport _dailyRevenueReport;
        private readonly MonthlyRevenueReport _monthlyRevenueReport;
        
        // Composite Pattern
        private readonly RevenueBuilder _revenueBuilder;

        public RevenueManagementController(DatabaseContext context, ILogger<RevenueLogger> logger)
        {
            _context = context;
            
            // Khởi tạo các thành phần
            var filterStrategy = new DefaultRevenueFilterStrategy();
            
            // Khởi tạo Observer Pattern
            _revenueSubject = new RevenueManager();
            var revenueLogger = new RevenueLogger(logger);
            _revenueSubject.Attach(revenueLogger);
            
            // Khởi tạo Facade Pattern
            _revenueFacade = new RevenueFacade(context, filterStrategy);
            
            // Khởi tạo Template Method Pattern
            _dailyRevenueReport = new DailyRevenueReport(context);
            _monthlyRevenueReport = new MonthlyRevenueReport(context);
            
            // Khởi tạo Composite Pattern
            _revenueBuilder = new RevenueBuilder(context);
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue(DateTime? fromDate, DateTime? toDate, string? paymentMethod)
        {
            // Sử dụng Facade Pattern để lấy dữ liệu doanh thu
            return await _revenueFacade.GetRevenueSummary(fromDate, toDate, paymentMethod, this);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 10, string? paymentMethod = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Sử dụng Facade Pattern để lấy danh sách đơn hàng
            return await _revenueFacade.GetOrderList(page, pageSize, paymentMethod, fromDate, toDate, this);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetail(string id)
        {
            // Sử dụng Facade Pattern để lấy chi tiết đơn hàng
            return await _revenueFacade.GetOrderDetail(id, this);
        }
        
        // Template Method Pattern - Báo cáo doanh thu hàng ngày
        [HttpGet]
        public async Task<IActionResult> GetDailyReport(DateTime? fromDate, DateTime? toDate, string? paymentMethod)
        {
            return await _dailyRevenueReport.GenerateReport(fromDate, toDate, paymentMethod, this);
        }
        
        // Template Method Pattern - Báo cáo doanh thu hàng tháng
        [HttpGet]
        public async Task<IActionResult> GetMonthlyReport(DateTime? fromDate, DateTime? toDate, string? paymentMethod)
        {
            return await _monthlyRevenueReport.GenerateReport(fromDate, toDate, paymentMethod, this);
        }
        
        // Composite Pattern - Cấu trúc phân cấp doanh thu
        [HttpGet]
        public async Task<IActionResult> GetRevenueHierarchy(DateTime? fromDate, DateTime? toDate)
        {
            var revenueHierarchy = await _revenueBuilder.BuildRevenueHierarchy(fromDate, toDate);
            
            // Thông báo qua Observer Pattern
            await _revenueSubject.NotifyObservers("Đã tạo báo cáo phân cấp doanh thu");
            
            return Json(revenueHierarchy.GetDetails());
        }
    }
}
