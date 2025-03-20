using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Models.Builders
{
    /// <summary>
    /// Triển khai Builder Pattern để xây dựng dữ liệu Dashboard
    /// </summary>
    public class DashboardBuilder : IDashboardBuilder
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<DashboardBuilder> _logger;
        private readonly DashboardData _dashboardData;
        
        public DashboardBuilder(DatabaseContext context, ILogger<DashboardBuilder> logger)
        {
            _context = context;
            _logger = logger;
            _dashboardData = new DashboardData();
        }
        
        /// <summary>
        /// Xây dựng thống kê đơn hàng
        /// </summary>
        public async Task<IDashboardBuilder> BuildOrderStatistics()
        {
            try
            {
                _dashboardData.TotalOrders = await _context.Donhangs.CountAsync();
                _dashboardData.CompletedOrders = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")
                    .CountAsync();
                _dashboardData.PendingOrders = await _context.Donhangs
                    .Where(d => d.Trangthai == "Đang giao")
                    .CountAsync();
                
                _logger.LogInformation($"Đã xây dựng thống kê đơn hàng: Tổng {_dashboardData.TotalOrders}, Hoàn thành {_dashboardData.CompletedOrders}, Đang giao {_dashboardData.PendingOrders}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xây dựng thống kê đơn hàng: {ex.Message}");
            }
            
            return this;
        }
        
        /// <summary>
        /// Xây dựng thống kê doanh thu
        /// </summary>
        public async Task<IDashboardBuilder> BuildRevenueStatistics()
        {
            try
            {
                _dashboardData.TotalRevenue = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")
                    .SumAsync(d => d.Tongtien);
                
                _logger.LogInformation($"Đã xây dựng thống kê doanh thu: {_dashboardData.TotalRevenue}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xây dựng thống kê doanh thu: {ex.Message}");
            }
            
            return this;
        }
        
        /// <summary>
        /// Xây dựng thống kê theo phương thức thanh toán
        /// </summary>
        public async Task<IDashboardBuilder> BuildPaymentMethodStatistics()
        {
            try
            {
                var paymentStats = await _context.Donhangs
                    .Where(d => d.Trangthai == "Giao thành công")
                    .GroupBy(d => d.Phuongthucthanhtoan ?? "Không xác định")
                    .Select(g => new DashboardData.PaymentMethodStat
                    {
                        Method = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(d => d.Tongtien)
                    })
                    .ToListAsync();
                
                _dashboardData.PaymentStats = paymentStats;
                
                foreach (var stat in paymentStats)
                {
                    _logger.LogInformation($"Phương thức thanh toán: {stat.Method}, Số lượng: {stat.Count}, Tổng tiền: {stat.Amount}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xây dựng thống kê phương thức thanh toán: {ex.Message}");
            }
            
            return this;
        }
        
        /// <summary>
        /// Xây dựng danh sách đơn hàng gần đây
        /// </summary>
        public async Task<IDashboardBuilder> BuildRecentOrders(int count = 5)
        {
            try
            {
                var recentOrders = await _context.Donhangs
                    .Where(d => !string.IsNullOrEmpty(d.Trangthai))
                    .OrderByDescending(d => d.Ngaydathang ?? DateTime.MinValue)
                    .Take(count)
                    .Select(d => new DashboardData.RecentOrder
                    {
                        IdDh = d.IdDh,
                        Ngaydathang = d.Ngaydathang,
                        Tongtien = d.Tongtien,
                        Trangthai = d.Trangthai ?? "Không xác định",
                        Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "Không xác định"
                    })
                    .ToListAsync();
                
                _dashboardData.RecentOrders = recentOrders;
                _logger.LogInformation($"Đã xây dựng danh sách {recentOrders.Count} đơn hàng gần đây");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xây dựng danh sách đơn hàng gần đây: {ex.Message}");
            }
            
            return this;
        }
        
        /// <summary>
        /// Trả về kết quả cuối cùng
        /// </summary>
        public Task<DashboardData> Build()
        {
            _logger.LogInformation("Hoàn thành xây dựng dữ liệu Dashboard");
            return Task.FromResult(_dashboardData);
        }
    }
} 