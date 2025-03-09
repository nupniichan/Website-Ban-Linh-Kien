using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Strategis;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Facades
{
    public class RevenueFacade
    {
        private readonly DatabaseContext _context;
        private readonly IRevenueFilterStrategy _filterStrategy;

        public RevenueFacade(
            DatabaseContext context,
            IRevenueFilterStrategy filterStrategy)
        {
            _context = context;
            _filterStrategy = filterStrategy;
        }

        public async Task<JsonResult> GetRevenueSummary(
            DateTime? fromDate, 
            DateTime? toDate, 
            string paymentMethod,
            Controller controller)
        {
            try
            {
                var template = new RevenueSummaryTemplate(_context, _filterStrategy);
                var result = await template.GetData(fromDate, toDate, paymentMethod);
                return controller.Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRevenueSummary - Exception: {ex.Message}");
                return controller.Json(new { error = ex.Message });
            }
        }

        public async Task<JsonResult> GetOrderList(
            int page, 
            int pageSize, 
            string paymentMethod, 
            DateTime? fromDate, 
            DateTime? toDate,
            Controller controller)
        {
            try
            {
                var template = new RevenueOrderListTemplate(_context, _filterStrategy, page, pageSize);
                var result = await template.GetData(fromDate, toDate, paymentMethod);
                return controller.Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderList - Exception: {ex.Message}");
                return controller.Json(new { error = ex.Message });
            }
        }

        public async Task<JsonResult> GetOrderDetail(
            string id,
            Controller controller)
        {
            try
            {
                var template = new RevenueOrderDetailTemplate(_context, _filterStrategy, id);
                var result = await template.GetData(null, null, null);
                
                if (result == null)
                {
                    return controller.Json(new { error = "Order not found" });
                }
                
                return controller.Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderDetail - Exception: {ex.Message}");
                return controller.Json(new { error = ex.Message });
            }
        }
    }
} 