using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Models.Observers
{
    public class RevenueLogger : IRevenueObserver
    {
        private readonly ILogger<RevenueLogger> _logger;

        public RevenueLogger(ILogger<RevenueLogger> logger)
        {
            _logger = logger;
        }

        public Task Update(Donhang order, string action)
        {
            _logger.LogInformation($"{DateTime.Now}: {action} đơn hàng {order.IdDh} - Giá trị: {order.Tongtien} - Phương thức thanh toán: {order.Phuongthucthanhtoan}");
            return Task.CompletedTask;
        }

        public Task Update(string message)
        {
            _logger.LogInformation($"{DateTime.Now}: {message}");
            return Task.CompletedTask;
        }
    }
} 