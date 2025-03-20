using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Factories
{
    public class DiscountFactory : IDiscountFactory
    {
        private readonly DatabaseContext _context;

        public DiscountFactory(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Magiamgia> CreateDiscount(string name, DateOnly startDate, DateOnly endDate, decimal discountRate, int quantity)
        {
            var discount = new Magiamgia
            {
                IdMgg = await GenerateNextDiscountId(),
                Ten = name,
                Ngaysudung = startDate,
                Ngayhethan = endDate,
                Tilechietkhau = discountRate,
                Soluong = quantity
            };

            return discount;
        }

        public async Task<string> GenerateNextDiscountId()
        {
            var lastDiscount = await _context.Magiamgia
                .OrderByDescending(d => d.IdMgg)
                .FirstOrDefaultAsync();

            if (lastDiscount == null)
            {
                return "MG000001";
            }

            string lastIdNumberPart = lastDiscount.IdMgg.Substring(2);
            if (int.TryParse(lastIdNumberPart, out int number))
            {
                number++;
                return "MG" + number.ToString("D6");
            }
            else
            {
                return "MG000001";
            }
        }
    }
} 