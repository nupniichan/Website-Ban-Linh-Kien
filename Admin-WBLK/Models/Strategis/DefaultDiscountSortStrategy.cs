using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultDiscountSortStrategy : IDiscountSortStrategy
    {
        public IQueryable<Magiamgia> Sort(IQueryable<Magiamgia> query)
        {
            return query.OrderByDescending(m => m.IdMgg);
        }
    }
} 