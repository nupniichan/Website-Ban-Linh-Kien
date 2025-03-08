using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Website_Ban_Linh_Kien.Models.Observers.Cart
{
    public class CartTotalObserver : ICartObserver
    {
        private readonly DatabaseContext _context;
        private decimal _cartTotal;

        public CartTotalObserver(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Update(Giohang cart)
        {
            if (cart == null)
            {
                _cartTotal = 0;
                return;
            }

            // Ensure we have the latest data with product information
            var updatedCart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .ThenInclude(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(g => g.IdGh == cart.IdGh);

            _cartTotal = updatedCart?.Chitietgiohangs.Sum(c => c.IdSpNavigation.Gia * c.Soluongsanpham) ?? 0;
        }

        public decimal GetCartTotal()
        {
            return _cartTotal;
        }

        public string GetFormattedCartTotal()
        {
            return _cartTotal.ToString("N0");
        }
    }
} 