using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IDiscountSortStrategy
    {
        IQueryable<Magiamgia> Sort(IQueryable<Magiamgia> query);
    }
} 