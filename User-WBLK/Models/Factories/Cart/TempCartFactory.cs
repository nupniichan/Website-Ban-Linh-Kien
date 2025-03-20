using System;
using System.Threading.Tasks;

namespace Website_Ban_Linh_Kien.Models.Factories.Cart
{
    public class TempCartFactory : ICartFactory
    {
        private readonly DatabaseContext _context;

        public TempCartFactory(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Giohang> CreateCart(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty");
            }

            var tempCart = new Giohang
            {
                IdGh = "TEMP_" + Guid.NewGuid().ToString().Substring(0, 10),
                IdKh = customerId,
                Thoigiancapnhat = DateTime.Now
            };

            _context.Giohangs.Add(tempCart);
            await _context.SaveChangesAsync();

            return tempCart;
        }
    }
} 