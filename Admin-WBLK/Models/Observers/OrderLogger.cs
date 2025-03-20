using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Models.Observers
{
    public class OrderLogger : IOrderObserver
    {
        private readonly ILogger<OrderLogger> _logger;

        public OrderLogger(ILogger<OrderLogger> logger)
        {
            _logger = logger;
        }

        public Task Update(Donhang order, string action)
        {
            _logger.LogInformation($"Order {action}: ID={order.IdDh}, Status={order.Trangthai}, Amount={order.Tongtien}, Date={DateTime.Now}");
            return Task.CompletedTask;
        }
    }
} 