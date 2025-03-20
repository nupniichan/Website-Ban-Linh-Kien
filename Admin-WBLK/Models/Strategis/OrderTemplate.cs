using System;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public abstract class OrderTemplate
    {
        protected readonly DatabaseContext _context;
        protected readonly IOrderFilterStrategy _filterStrategy;

        public OrderTemplate(DatabaseContext context, IOrderFilterStrategy filterStrategy)
        {
            _context = context;
            _filterStrategy = filterStrategy;
        }

        public async Task<dynamic> GetData()
        {
            // Template Method Pattern
            var query = GetBaseQuery();
            query = ApplyFilter(query);
            var result = await ExecuteQuery(query);
            return ProcessResult(result);
        }

        protected abstract IQueryable<Donhang> GetBaseQuery();
        
        protected abstract Task<dynamic> ExecuteQuery(IQueryable<Donhang> query);
        
        protected abstract dynamic ProcessResult(dynamic result);
        
        protected virtual IQueryable<Donhang> ApplyFilter(IQueryable<Donhang> query)
        {
            return query;
        }
    }
} 