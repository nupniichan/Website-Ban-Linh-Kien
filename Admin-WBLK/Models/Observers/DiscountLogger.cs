using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Models.Observers
{
    public class DiscountLogger : IDiscountObserver
    {
        private readonly ILogger<DiscountLogger> _logger;

        public DiscountLogger(ILogger<DiscountLogger> logger)
        {
            _logger = logger;
        }

        public Task Update(Magiamgia discount, string action)
        {
            _logger.LogInformation($"{DateTime.Now}: {action} mã giảm giá {discount.IdMgg} - {discount.Ten}");
            return Task.CompletedTask;
        }
    }
} 