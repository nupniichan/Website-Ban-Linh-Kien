using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class OrderListTemplate : OrderTemplate
    {
        private readonly int _page;
        private readonly int _pageSize;
        private readonly string _searchString;
        private readonly string _trangThai;
        private readonly DateTime? _tuNgay;
        private readonly DateTime? _denNgay;

        public OrderListTemplate(
            DatabaseContext context, 
            IOrderFilterStrategy filterStrategy,
            int page,
            int pageSize,
            string searchString,
            string trangThai,
            DateTime? tuNgay,
            DateTime? denNgay)
            : base(context, filterStrategy)
        {
            _page = page;
            _pageSize = pageSize;
            _searchString = searchString;
            _trangThai = trangThai;
            _tuNgay = tuNgay;
            _denNgay = denNgay;
        }

        protected override IQueryable<Donhang> GetBaseQuery()
        {
            return _context.Donhangs
                .OrderByDescending(d => d.Ngaydathang)
                .AsQueryable();
        }

        protected override IQueryable<Donhang> ApplyFilter(IQueryable<Donhang> query)
        {
            return _filterStrategy.Filter(query, _searchString, _trangThai, _tuNgay, _denNgay);
        }

        protected override async Task<dynamic> ExecuteQuery(IQueryable<Donhang> query)
        {
            // Đếm tổng số đơn hàng trước khi phân trang
            var totalItems = await query.CountAsync();
            
            // Lấy dữ liệu phân trang
            var items = await query
                .Skip((_page - 1) * _pageSize)
                .Take(_pageSize)
                .Include(d => d.IdKhNavigation)
                .Include(d => d.Chitietdonhangs)
                .Select(d => new Donhang
                {
                    IdDh = d.IdDh ?? "",
                    IdKh = d.IdKh,
                    Trangthai = d.Trangthai ?? "",
                    Tongtien = d.Tongtien != 0 ? d.Tongtien : d.Chitietdonhangs.Sum(c => c.Soluongsanpham * c.Dongia),
                    Diachigiaohang = d.Diachigiaohang ?? "",
                    Ngaydathang = d.Ngaydathang,
                    Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "",
                    IdKhNavigation = d.IdKhNavigation,
                    Ghichu = d.Ghichu,
                    LydoHuy = d.LydoHuy
                })
                .ToListAsync();

            return new { Items = items, TotalItems = totalItems };
        }

        protected override dynamic ProcessResult(dynamic result)
        {
            // Không cần xử lý thêm, trả về kết quả trực tiếp
            return result;
        }
    }
} 