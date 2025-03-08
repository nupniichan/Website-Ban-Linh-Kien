using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Website_Ban_Linh_Kien.Models.Factories.Cart
{
    public class UserCartFactory : ICartFactory
    {
        private readonly DatabaseContext _context;

        public UserCartFactory(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Giohang> CreateCart(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty");
            }

            var existingCart = await _context.Giohangs
                .Include(g => g.Chitietgiohangs)
                .FirstOrDefaultAsync(g => g.IdKh == customerId);

            if (existingCart != null)
            {
                return existingCart;
            }

            var newCart = new Giohang
            {
                IdGh = Guid.NewGuid().ToString().Substring(0, 10),
                IdKh = customerId,
                Thoigiancapnhat = DateTime.Now
            };

            _context.Giohangs.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }
    }
} 