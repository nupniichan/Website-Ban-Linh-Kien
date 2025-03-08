using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultProductSortStrategy : IProductSortStrategy
    {
        public IQueryable<Sanpham> Sort(IQueryable<Sanpham> query, string sortOrder)
        {
            return sortOrder switch
            {
                "oldest" => query.OrderBy(s => s.IdSp),
                "price_asc" => query.OrderBy(s => s.Gia),
                "price_desc" => query.OrderByDescending(s => s.Gia),
                "name_asc" => query.OrderBy(s => s.Tensanpham),
                "name_desc" => query.OrderByDescending(s => s.Tensanpham),
                "views" => query.OrderByDescending(s => s.Soluotxem),
                "sales" => query.OrderByDescending(s => s.Damuahang),
                _ => query.OrderByDescending(s => s.IdSp) // newest by default
            };
        }
    }
} 