using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IDiscountSearchStrategy
    {
        IQueryable<Magiamgia> Search(IQueryable<Magiamgia> query, string searchTerm);
    }
} 