using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Commands;
using Admin_WBLK.Models.Factories;
using Admin_WBLK.Models.Observers;
using Admin_WBLK.Models.Strategis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Facades
{
    public class DiscountFacade
    {
        private readonly DatabaseContext _context;
        private readonly IDiscountFactory _discountFactory;
        private readonly IDiscountSearchStrategy _searchStrategy;
        private readonly IDiscountSortStrategy _sortStrategy;
        private readonly IDiscountSubject _discountSubject;

        public DiscountFacade(
            DatabaseContext context,
            IDiscountFactory discountFactory,
            IDiscountSearchStrategy searchStrategy,
            IDiscountSortStrategy sortStrategy,
            IDiscountSubject discountSubject)
        {
            _context = context;
            _discountFactory = discountFactory;
            _searchStrategy = searchStrategy;
            _sortStrategy = sortStrategy;
            _discountSubject = discountSubject;
        }

        public async Task<PaginatedList<Magiamgia>> GetDiscounts(string searchString, int pageNumber, int pageSize)
        {
            var query = _context.Magiamgia.AsQueryable();
            
            // Áp dụng Strategy Pattern cho tìm kiếm
            query = _searchStrategy.Search(query, searchString);
            
            // Áp dụng Strategy Pattern cho sắp xếp
            query = _sortStrategy.Sort(query);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedList<Magiamgia>(items, totalItems, pageNumber, pageSize);
        }

        public async Task<Magiamgia> GetDiscountById(string id)
        {
            return await _context.Magiamgia.FirstOrDefaultAsync(m => m.IdMgg == id);
        }

        public async Task<Magiamgia> CreateEmptyDiscount()
        {
            return await _discountFactory.CreateDiscount(
                "", 
                System.DateOnly.FromDateTime(System.DateTime.Today), 
                System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(1)),
                0,
                0
            );
        }

        public async Task<IActionResult> CreateDiscount(
            Magiamgia discount, 
            Controller controller, 
            ITempDataDictionary tempData)
        {
            var command = new CreateDiscountCommand(_context, discount, _discountFactory, controller, tempData);
            var result = await command.Execute();
            
            if (result is RedirectToActionResult)
            {
                await _discountSubject.NotifyObservers(discount, "Tạo mới");
            }
            
            return result;
        }

        public async Task<IActionResult> UpdateDiscount(
            string id,
            Magiamgia discount, 
            Controller controller, 
            ITempDataDictionary tempData)
        {
            var command = new UpdateDiscountCommand(_context, discount, id, controller, tempData);
            var result = await command.Execute();
            
            if (result is RedirectToActionResult)
            {
                await _discountSubject.NotifyObservers(discount, "Cập nhật");
            }
            
            return result;
        }

        public async Task<IActionResult> DeleteDiscount(string id, Controller controller)
        {
            var deleteOperation = new DeleteDiscountOperation(_context, controller);
            var result = await deleteOperation.ProcessDiscount(id);
            
            var discount = await GetDiscountById(id);
            if (discount != null)
            {
                await _discountSubject.NotifyObservers(discount, "Xóa");
            }
            
            return result;
        }
    }
} 