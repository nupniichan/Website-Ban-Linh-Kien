using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.Factories;
using Admin_WBLK.Models.Commands;
using Admin_WBLK.Models.Observers;
using Admin_WBLK.Models.Facades;
using Microsoft.Extensions.Logging;

namespace Admin_WBLK.Controllers
{
    public class DiscountManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly DiscountFacade _discountFacade;

        public DiscountManagementController(DatabaseContext context, ILogger<DiscountLogger> logger)
        {
            _context = context;
            
            // Khởi tạo các thành phần
            var searchStrategy = new DefaultDiscountSearchStrategy();
            var sortStrategy = new DefaultDiscountSortStrategy();
            var discountFactory = new DiscountFactory(context);
            
            // Khởi tạo Observer Pattern
            var discountManager = new DiscountManager();
            var discountLogger = new DiscountLogger(logger);
            discountManager.Attach(discountLogger);
            
            // Khởi tạo Facade Pattern
            _discountFacade = new DiscountFacade(
                context,
                discountFactory,
                searchStrategy,
                sortStrategy,
                discountManager
            );
        }

        // GET: DiscountManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            
            // Sử dụng Facade Pattern để lấy danh sách mã giảm giá
            var model = await _discountFacade.GetDiscounts(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            term = term.ToLower();
            var suggestions = await _context.Magiamgia
                .Where(m => m.IdMgg.ToLower().Contains(term) ||
                           m.Ten.ToLower().Contains(term))
                .OrderByDescending(m => m.IdMgg)
                .Take(5)
                .Select(m => new
                {
                    idMgg = m.IdMgg,
                    ten = m.Ten,
                    tilechietkhau = m.Tilechietkhau
                })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: DiscountManagement/Create
        public async Task<IActionResult> Create()
        {
            // Sử dụng Facade Pattern để tạo mã giảm giá mới
            var discount = await _discountFacade.CreateEmptyDiscount();
            return View(discount);
        }

        // POST: DiscountManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMgg,Ten,Ngaysudung,Ngayhethan,Tilechietkhau,Soluong")] Magiamgia discount)
        {
            // Sử dụng Command Pattern thông qua Facade để tạo mã giảm giá
            return await _discountFacade.CreateDiscount(discount, this, TempData);
        }

        // GET: DiscountManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Sử dụng Facade Pattern để lấy thông tin mã giảm giá
            var magiamgia = await _discountFacade.GetDiscountById(id);
            if (magiamgia == null)
            {
                return NotFound();
            }

            return View(magiamgia);
        }

        // GET: DiscountManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Sử dụng Facade Pattern để lấy thông tin mã giảm giá
            var magiamgia = await _discountFacade.GetDiscountById(id);
            if (magiamgia == null)
            {
                return NotFound();
            }

            return View(magiamgia);
        }

        // POST: DiscountManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdMgg,Ten,Tilechietkhau,Ngaysudung,Ngayhethan,Soluong")] Magiamgia magiamgia)
        {
            // Sử dụng Command Pattern thông qua Facade để cập nhật mã giảm giá
            return await _discountFacade.UpdateDiscount(id, magiamgia, this, TempData);
        }

        // GET: DiscountManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Sử dụng Facade Pattern để lấy thông tin mã giảm giá
            var discount = await _discountFacade.GetDiscountById(id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: DiscountManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Sử dụng Template Method Pattern thông qua Facade để xóa mã giảm giá
            return await _discountFacade.DeleteDiscount(id, this);
        }

        private bool MagiamgiaExists(string id)
        {
            return _context.Magiamgia.Any(e => e.IdMgg == id);
        }
    }
}
