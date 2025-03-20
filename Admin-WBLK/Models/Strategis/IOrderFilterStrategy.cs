using System;
using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IOrderFilterStrategy
    {
        IQueryable<Donhang> Filter(
            IQueryable<Donhang> query, 
            string searchString, 
            string trangThai, 
            DateTime? tuNgay, 
            DateTime? denNgay);
    }
} 