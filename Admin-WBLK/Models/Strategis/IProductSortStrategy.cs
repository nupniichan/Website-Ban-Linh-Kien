using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IProductSortStrategy
    {
        IQueryable<Sanpham> Sort(IQueryable<Sanpham> query, string sortOrder);
    }
} 