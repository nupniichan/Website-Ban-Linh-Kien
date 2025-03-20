using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Website_Ban_Linh_Kien.Models.Observers.Cart
{
    public class CartCountObserver : ICartObserver
    {
        private readonly DatabaseContext _context;
        private int _cartCount;

        public CartCountObserver(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Update(Giohang cart)
        {
            if (cart == null)
            {
                _cartCount = 0;
                return;
            }

            // Ensure we have the latest data
            var updatedCart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .FirstOrDefaultAsync(g => g.IdGh == cart.IdGh);

            _cartCount = updatedCart?.Chitietgiohangs.Sum(c => c.Soluongsanpham) ?? 0;
        }

        public int GetCartCount()
        {
            return _cartCount;
        }
    }
} 