using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultProductSearchStrategy : IProductSearchStrategy
    {
        public IQueryable<Sanpham> Search(IQueryable<Sanpham> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            searchTerm = searchTerm.ToLower();
            return query.Where(s => s.IdSp.ToLower().Contains(searchTerm) ||
                                   s.Tensanpham.ToLower().Contains(searchTerm));
        }
    }
} 