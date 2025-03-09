using System.Collections.Generic;
using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IProductFilterStrategy
    {
        IEnumerable<Sanpham> Filter(IEnumerable<Sanpham> products, string category, string brand);
    }
} 