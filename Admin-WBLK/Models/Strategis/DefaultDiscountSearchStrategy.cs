using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultDiscountSearchStrategy : IDiscountSearchStrategy
    {
        public IQueryable<Magiamgia> Search(IQueryable<Magiamgia> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            searchTerm = searchTerm.ToLower();
            return query.Where(m => m.IdMgg.ToLower().Contains(searchTerm) ||
                                   m.Ten.ToLower().Contains(searchTerm));
        }
    }
} 