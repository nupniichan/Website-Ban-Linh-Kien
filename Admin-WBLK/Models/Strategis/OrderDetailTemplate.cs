using System;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class OrderDetailTemplate : OrderTemplate
    {
        private readonly string _orderId;

        public OrderDetailTemplate(
            DatabaseContext context, 
            IOrderFilterStrategy filterStrategy,
            string orderId)
            : base(context, filterStrategy)
        {
            _orderId = orderId;
        }

        protected override IQueryable<Donhang> GetBaseQuery()
        {
            return _context.Donhangs
                .Include(o => o.IdKhNavigation)
                .Include(o => o.Chitietdonhangs)
                .ThenInclude(od => od.IdSpNavigation)
                .Include(o => o.Thanhtoans)
                .Where(o => o.IdDh == _orderId);
        }

        protected override async Task<dynamic> ExecuteQuery(IQueryable<Donhang> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        protected override dynamic ProcessResult(dynamic result)
        {
            // Không cần xử lý thêm, trả về kết quả trực tiếp
            return result;
        }

        // Ghi đè phương thức ApplyFilter vì không cần lọc theo ngày và phương thức thanh toán
        protected override IQueryable<Donhang> ApplyFilter(IQueryable<Donhang> query)
        {
            // Không áp dụng bộ lọc vì đã lọc theo ID đơn hàng
            return query;
        }
    }
} 