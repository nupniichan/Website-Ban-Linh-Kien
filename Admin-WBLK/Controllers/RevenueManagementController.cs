using Admin_WBLK.Models;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.Observers;
using Admin_WBLK.Models.Facades;
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
    }
}
