using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Templates
{
    public abstract class RevenueReportTemplate
    {
        protected readonly DatabaseContext _context;
        
        public RevenueReportTemplate(DatabaseContext context)
        {
            _context = context;
        }
        
        // Template method định nghĩa thuật toán
        public async Task<IActionResult> GenerateReport(DateTime? fromDate, DateTime? toDate, string paymentMethod, Controller controller)
        {
            // Các bước của thuật toán
            var data = await CollectData(fromDate, toDate, paymentMethod);
            var processedData = ProcessData(data);
            var formattedData = FormatData(processedData);
            return PresentData(formattedData, controller);
        }
        
        // Các bước cụ thể được triển khai bởi lớp con
        protected abstract Task<object> CollectData(DateTime? fromDate, DateTime? toDate, string paymentMethod);
        
        protected abstract object ProcessData(object rawData);
        
        // Bước có triển khai mặc định, có thể ghi đè
        protected virtual object FormatData(object processedData)
        {
            return processedData;
        }
        
        // Bước cuối cùng để trả về kết quả
        protected abstract IActionResult PresentData(object formattedData, Controller controller);
    }
} 