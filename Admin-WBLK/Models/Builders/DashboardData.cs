using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models.Builders
{
    /// <summary>
    /// Lớp chứa dữ liệu Dashboard - kết quả của quá trình xây dựng
    /// </summary>
    public class DashboardData
    {
        // Thống kê đơn hàng
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        
        // Thống kê doanh thu
        public decimal TotalRevenue { get; set; }
        
        // Thống kê theo phương thức thanh toán
        public List<PaymentMethodStat> PaymentStats { get; set; } = new List<PaymentMethodStat>();
        
        // Đơn hàng gần đây
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
        
        // Lớp con để lưu trữ thống kê phương thức thanh toán
        public class PaymentMethodStat
        {
            public string Method { get; set; } = string.Empty;
            public int Count { get; set; }
            public decimal Amount { get; set; }
        }
        
        // Lớp con để lưu trữ thông tin đơn hàng gần đây
        public class RecentOrder
        {
            public string IdDh { get; set; } = string.Empty;
            public DateTime? Ngaydathang { get; set; }
            public decimal Tongtien { get; set; }
            public string Trangthai { get; set; } = string.Empty;
            public string Phuongthucthanhtoan { get; set; } = string.Empty;
        }
    }
} 