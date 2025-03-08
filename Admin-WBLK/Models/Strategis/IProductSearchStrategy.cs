using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IProductSearchStrategy
    {
        IQueryable<Sanpham> Search(IQueryable<Sanpham> query, string searchTerm);
    }
} 