using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public abstract class RevenueDataTemplate
    {
        protected readonly DatabaseContext _context;
        protected readonly IRevenueFilterStrategy _filterStrategy;

        public RevenueDataTemplate(DatabaseContext context, IRevenueFilterStrategy filterStrategy)
        {
            _context = context;
            _filterStrategy = filterStrategy;
        }

        // Template method định nghĩa thuật toán chung
        public async Task<object> GetData(DateTime? fromDate, DateTime? toDate, string paymentMethod)
        {
            // Bước 1: Lấy truy vấn cơ bản
            var query = GetBaseQuery();
            
            // Bước 2: Áp dụng bộ lọc
            query = ApplyFilter(query, fromDate, toDate, paymentMethod);
            
            // Bước 3: Thực hiện truy vấn và chuyển đổi dữ liệu
            var result = await ExecuteQuery(query);
            
            // Bước 4: Xử lý dữ liệu sau truy vấn
            return ProcessResult(result);
        }

        // Các phương thức trừu tượng sẽ được triển khai bởi các lớp con
        protected abstract IQueryable<Donhang> GetBaseQuery();
        protected virtual IQueryable<Donhang> ApplyFilter(IQueryable<Donhang> query, DateTime? fromDate, DateTime? toDate, string paymentMethod)
        {
            return _filterStrategy.Filter(query, fromDate, toDate, paymentMethod);
        }
        protected abstract Task<object> ExecuteQuery(IQueryable<Donhang> query);
        protected abstract object ProcessResult(object result);
    }
} 