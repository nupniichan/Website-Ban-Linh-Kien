using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Strategis
{
    public abstract class DiscountOperationTemplate
    {
        protected readonly DatabaseContext _context;

        public DiscountOperationTemplate(DatabaseContext context)
        {
            _context = context;
        }

        // Template method định nghĩa thuật toán chung
        public async Task<IActionResult> ProcessDiscount(string id)
        {
            // Bước 1: Kiểm tra id
            if (string.IsNullOrEmpty(id))
            {
                return GetNotFoundResult();
            }

            // Bước 2: Tìm mã giảm giá
            var discount = await FindDiscount(id);
            if (discount == null)
            {
                return GetNotFoundResult();
            }

            // Bước 3: Xử lý mã giảm giá (khác nhau giữa các thao tác)
            var result = await ProcessDiscountOperation(discount);

            // Bước 4: Trả về kết quả
            return GetResult(result, discount);
        }

        // Các phương thức trừu tượng sẽ được triển khai bởi các lớp con
        protected abstract Task<Magiamgia> FindDiscount(string id);
        protected abstract Task<bool> ProcessDiscountOperation(Magiamgia discount);
        protected abstract IActionResult GetResult(bool operationResult, Magiamgia discount);
        protected abstract IActionResult GetNotFoundResult();
    }
} 