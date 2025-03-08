using System;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class RevenueOrderDetailTemplate : RevenueDataTemplate
    {
        private readonly string _orderId;

        public RevenueOrderDetailTemplate(
            DatabaseContext context, 
            IRevenueFilterStrategy filterStrategy,
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

        protected override async Task<object> ExecuteQuery(IQueryable<Donhang> query)
        {
            var order = await query
                .Select(o => new
                {
                    Id = o.IdDh,
                    OrderDate = o.Ngaydathang,
                    PaymentMethod = o.Phuongthucthanhtoan,
                    Status = o.Trangthai,
                    CustomerName = o.IdKhNavigation.Hoten,
                    CustomerEmail = o.IdKhNavigation.Email,
                    CustomerPhone = o.IdKhNavigation.Sodienthoai,
                    CustomerAddress = o.Diachigiaohang,
                    TotalAmount = o.Tongtien,
                    Items = o.Chitietdonhangs.Select(od => new
                    {
                        ProductName = od.IdSpNavigation.Tensanpham,
                        Price = od.Dongia,
                        Quantity = od.Soluongsanpham
                    }).ToList(),
                    Payment = o.Thanhtoans.Select(p => new
                    {
                        Id = p.IdTt,
                        Status = p.Trangthai,
                        Amount = p.Tienthanhtoan,
                        PaymentDate = p.Ngaythanhtoan,
                        Content = p.Noidungthanhtoan,
                        Code = p.Mathanhtoan
                    }).FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        protected override object ProcessResult(object result)
        {
            // Không cần xử lý thêm, trả về kết quả trực tiếp
            return result;
        }

        // Ghi đè phương thức ApplyFilter vì không cần lọc theo ngày và phương thức thanh toán
        protected override IQueryable<Donhang> ApplyFilter(IQueryable<Donhang> query, DateTime? fromDate, DateTime? toDate, string paymentMethod)
        {
            // Không áp dụng bộ lọc vì đã lọc theo ID đơn hàng
            return query;
        }
    }
} 