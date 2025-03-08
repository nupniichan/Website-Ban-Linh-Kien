using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class RevenueSummaryTemplate : RevenueDataTemplate
    {
        public RevenueSummaryTemplate(DatabaseContext context, IRevenueFilterStrategy filterStrategy)
            : base(context, filterStrategy)
        {
        }

        protected override IQueryable<Donhang> GetBaseQuery()
        {
            return _context.Donhangs.AsQueryable();
        }

        protected override async Task<object> ExecuteQuery(IQueryable<Donhang> query)
        {
            var result = await query
                .GroupBy(d => d.Phuongthucthanhtoan)
                .Select(g => new
                {
                    paymentMethod = g.Key,
                    totalAmount = g.Sum(d => d.Tongtien),
                    orderCount = g.Count(),
                    successOrderCount = g.Count(d => d.Trangthai == "Giao thành công")
                })
                .ToListAsync();

            return result;
        }

        protected override object ProcessResult(object result)
        {
            var revenueData = result as IEnumerable<dynamic>;
            int totalSuccessOrders = 0;

            if (revenueData != null)
            {
                foreach (dynamic item in revenueData)
                {
                    totalSuccessOrders += item.successOrderCount;
                }
            }

            return new { 
                revenueData = result,
                totalSuccessOrders = totalSuccessOrders
            };
        }
    }
} 