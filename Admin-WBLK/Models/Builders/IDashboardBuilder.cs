using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Builders
{
    /// <summary>
    /// Interface cho Builder Pattern để xây dựng dữ liệu Dashboard
    /// </summary>
    public interface IDashboardBuilder
    {
        /// <summary>
        /// Xây dựng thống kê đơn hàng (tổng số, hoàn thành, đang xử lý)
        /// </summary>
        Task<IDashboardBuilder> BuildOrderStatistics();
        
        /// <summary>
        /// Xây dựng thống kê doanh thu
        /// </summary>
        Task<IDashboardBuilder> BuildRevenueStatistics();
        
        /// <summary>
        /// Xây dựng thống kê theo phương thức thanh toán
        /// </summary>
        Task<IDashboardBuilder> BuildPaymentMethodStatistics();
        
        /// <summary>
        /// Xây dựng danh sách đơn hàng gần đây
        /// </summary>
        Task<IDashboardBuilder> BuildRecentOrders(int count = 5);
        
        /// <summary>
        /// Trả về kết quả cuối cùng
        /// </summary>
        Task<DashboardData> Build();
    }
} 