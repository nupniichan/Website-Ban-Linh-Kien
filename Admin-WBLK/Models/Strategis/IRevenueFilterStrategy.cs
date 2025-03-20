using System;
using System.Linq;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public interface IRevenueFilterStrategy
    {
        IQueryable<Donhang> Filter(IQueryable<Donhang> query, DateTime? fromDate, DateTime? toDate, string paymentMethod);
    }
} 