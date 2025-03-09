using System;
using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultRevenueFilterStrategy : IRevenueFilterStrategy
    {
        public IQueryable<Donhang> Filter(IQueryable<Donhang> query, DateTime? fromDate, DateTime? toDate, string paymentMethod)
        {
            if (fromDate.HasValue)
            {
                // Đảm bảo lấy từ đầu ngày
                var fromDateStart = fromDate.Value.Date;
                query = query.Where(d => d.Ngaydathang >= fromDateStart);
            }
            
            if (toDate.HasValue)
            {
                // Đảm bảo lấy đến cuối ngày
                var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.Ngaydathang <= toDateEnd);
            }

            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query = query.Where(d => d.Phuongthucthanhtoan == paymentMethod);
            }

            return query;
        }
    }
} 