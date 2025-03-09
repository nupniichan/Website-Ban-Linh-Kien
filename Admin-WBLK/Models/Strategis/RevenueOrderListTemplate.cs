using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class RevenueOrderListTemplate : RevenueDataTemplate
    {
        private readonly int _page;
        private readonly int _pageSize;

        public RevenueOrderListTemplate(
            DatabaseContext context, 
            IRevenueFilterStrategy filterStrategy,
            int page,
            int pageSize)
            : base(context, filterStrategy)
        {
            _page = page;
            _pageSize = pageSize;
        }

        protected override IQueryable<Donhang> GetBaseQuery()
        {
            return _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .AsQueryable();
        }

        protected override async Task<object> ExecuteQuery(IQueryable<Donhang> query)
        {
            // Đếm tổng số đơn hàng trước khi phân trang
            var totalCount = await query.CountAsync();
            
            // Lấy dữ liệu phân trang
            var orders = await query
                .OrderByDescending(o => o.Ngaydathang)
                .Skip((_page - 1) * _pageSize)
                .Take(_pageSize)
                .Select(o => new
                {
                    Id = o.IdDh,
                    CustomerName = o.IdKhNavigation.Hoten,
                    OrderDate = o.Ngaydathang,
                    PaymentMethod = o.Phuongthucthanhtoan,
                    TotalAmount = o.Tongtien,
                    Status = o.Trangthai
                })
                .ToListAsync();

            return new { orders, totalCount };
        }

        protected override object ProcessResult(object result)
        {
            // Không cần xử lý thêm, trả về kết quả trực tiếp
            return result;
        }
    }
} 