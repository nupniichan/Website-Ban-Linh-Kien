using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Models.Strategies.Cart
{
    public class CartContext
    {
        private ICartStrategy _strategy;
        private readonly DatabaseContext _context;
        
        public CartContext(DatabaseContext context)
        {
            _context = context;
        }
        
        public void SetStrategy(ICartStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public async Task<IActionResult> ExecuteStrategy(string customerId, object data)
        {
            return await _strategy.Execute(_context, customerId, data);
        }
    }
} 