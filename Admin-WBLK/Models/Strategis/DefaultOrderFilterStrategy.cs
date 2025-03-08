using System;
using System.Linq;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultOrderFilterStrategy : IOrderFilterStrategy
    {
        private readonly DatabaseContext _context;

        public DefaultOrderFilterStrategy(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable<Donhang> Filter(
            IQueryable<Donhang> query, 
            string searchString, 
            string trangThai, 
            DateTime? tuNgay, 
            DateTime? denNgay)
        {
            // Áp dụng các bộ lọc
            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.Trangthai == trangThai);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(d => d.Ngaydathang >= tuNgay.Value);
            }

            if (denNgay.HasValue)
            {
                var endOfDay = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.Ngaydathang <= endOfDay);
            }

            // Xử lý tìm kiếm theo mã đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                
                // Tìm theo mã đơn hàng
                var orderIdQuery = query.Where(d => d.IdDh.ToLower().Contains(searchString));
                
                // Tìm theo tên khách hàng
                var customerNameQuery = _context.Khachhangs
                    .Where(k => k.Hoten.ToLower().Contains(searchString))
                    .Select(k => k.IdKh);
                
                var customerOrdersQuery = query.Where(d => customerNameQuery.Contains(d.IdKh));
                
                // Kết hợp hai kết quả
                query = orderIdQuery.Union(customerOrdersQuery);
            }

            return query;
        }
    }
} 