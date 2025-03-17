using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Templates
{
    public class DailyRevenueReport : RevenueReportTemplate
    {
        public DailyRevenueReport(DatabaseContext context) : base(context)
        {
        }
        
        protected override async Task<object> CollectData(DateTime? fromDate, DateTime? toDate, string paymentMethod)
        {
            var query = _context.Donhangs.AsQueryable();
            
            if (fromDate.HasValue)
                query = query.Where(d => d.Ngaydathang >= fromDate.Value);
                
            if (toDate.HasValue)
                query = query.Where(d => d.Ngaydathang <= toDate.Value.AddDays(1));
                
            if (!string.IsNullOrEmpty(paymentMethod))
                query = query.Where(d => d.Phuongthucthanhtoan == paymentMethod);
                
            return await query
                .Where(d => d.Trangthai == "Giao thành công" || d.Trangthai == "Đã kết thúc")
                .GroupBy(d => d.Ngaydathang.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(d => d.Tongtien),
                    OrderCount = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToListAsync();
        }
        
        protected override object ProcessData(object rawData)
        {
            var dailyData = (IEnumerable<dynamic>)rawData;
            
            // Tính toán thêm các chỉ số như tăng trưởng, trung bình, v.v.
            var result = new
            {
                DailyRevenue = dailyData,
                TotalRevenue = dailyData.Sum(d => (decimal)d.TotalRevenue),
                TotalOrders = dailyData.Sum(d => (int)d.OrderCount),
                AverageOrderValue = dailyData.Any() ? dailyData.Sum(d => (decimal)d.TotalRevenue) / dailyData.Sum(d => (int)d.OrderCount) : 0
            };
            
            return result;
        }
        
        protected override IActionResult PresentData(object formattedData, Controller controller)
        {
            return controller.Json(formattedData);
        }
    }
} 