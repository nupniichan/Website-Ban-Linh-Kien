using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Templates
{
    public class MonthlyRevenueReport : RevenueReportTemplate
    {
        public MonthlyRevenueReport(DatabaseContext context) : base(context)
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
                .GroupBy(d => new { Year = d.Ngaydathang.Value.Year, Month = d.Ngaydathang.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(d => d.Tongtien),
                    OrderCount = g.Count()
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToListAsync();
        }
        
        protected override object ProcessData(object rawData)
        {
            var monthlyData = (IEnumerable<dynamic>)rawData;
            
            // Tính toán thêm các chỉ số như tăng trưởng, trung bình, v.v.
            var result = new
            {
                MonthlyRevenue = monthlyData,
                TotalRevenue = monthlyData.Sum(d => (decimal)d.TotalRevenue),
                TotalOrders = monthlyData.Sum(d => (int)d.OrderCount),
                AverageOrderValue = monthlyData.Any() ? monthlyData.Sum(d => (decimal)d.TotalRevenue) / monthlyData.Sum(d => (int)d.OrderCount) : 0
            };
            
            return result;
        }
        
        protected override object FormatData(object processedData)
        {
            // Định dạng dữ liệu để hiển thị tháng dưới dạng "MM/YYYY"
            dynamic data = processedData;
            var formattedMonthlyData = ((IEnumerable<dynamic>)data.MonthlyRevenue).Select(m => new
            {
                Period = $"{m.Month:D2}/{m.Year}",
                m.TotalRevenue,
                m.OrderCount
            });
            
            return new
            {
                MonthlyRevenue = formattedMonthlyData,
                data.TotalRevenue,
                data.TotalOrders,
                data.AverageOrderValue
            };
        }
        
        protected override IActionResult PresentData(object formattedData, Controller controller)
        {
            return controller.Json(formattedData);
        }
    }
} 