using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Models.Builders
{
    /// <summary>
    /// Director trong Builder Pattern - điều khiển quá trình xây dựng Dashboard
    /// </summary>
    public class DashboardDirector
    {
        private readonly ILogger<DashboardDirector> _logger;
        
        public DashboardDirector(ILogger<DashboardDirector> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Xây dựng dashboard đầy đủ
        /// </summary>
        public async Task<DashboardData> BuildFullDashboard(IDashboardBuilder builder)
        {
            _logger.LogInformation("Bắt đầu xây dựng dashboard đầy đủ");
            
            await builder.BuildOrderStatistics();
            await builder.BuildRevenueStatistics();
            await builder.BuildPaymentMethodStatistics();
            await builder.BuildRecentOrders();
            
            return await builder.Build();
        }
        
        /// <summary>
        /// Xây dựng dashboard tối thiểu (chỉ thống kê đơn hàng và doanh thu)
        /// </summary>
        public async Task<DashboardData> BuildMinimalDashboard(IDashboardBuilder builder)
        {
            _logger.LogInformation("Bắt đầu xây dựng dashboard tối thiểu");
            
            await builder.BuildOrderStatistics();
            await builder.BuildRevenueStatistics();
            
            return await builder.Build();
        }
        
        /// <summary>
        /// Xây dựng dashboard tùy chỉnh
        /// </summary>
        public async Task<DashboardData> BuildCustomDashboard(
            IDashboardBuilder builder,
            bool includeOrderStats = true,
            bool includeRevenueStats = true,
            bool includePaymentStats = false,
            bool includeRecentOrders = false,
            int recentOrdersCount = 5)
        {
            _logger.LogInformation("Bắt đầu xây dựng dashboard tùy chỉnh");
            
            if (includeOrderStats)
                await builder.BuildOrderStatistics();
                
            if (includeRevenueStats)
                await builder.BuildRevenueStatistics();
                
            if (includePaymentStats)
                await builder.BuildPaymentMethodStatistics();
                
            if (includeRecentOrders)
                await builder.BuildRecentOrders(recentOrdersCount);
                
            return await builder.Build();
        }
    }
} 