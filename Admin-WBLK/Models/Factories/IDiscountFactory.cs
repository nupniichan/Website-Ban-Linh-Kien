using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Factories
{
    public interface IDiscountFactory
    {
        Task<Magiamgia> CreateDiscount(string name, DateOnly startDate, DateOnly endDate, decimal discountRate, int quantity);
        Task<string> GenerateNextDiscountId();
    }
} 