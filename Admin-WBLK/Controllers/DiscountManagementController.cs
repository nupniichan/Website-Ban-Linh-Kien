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

namespace Admin_WBLK.Controllers
{
    public class DiscountManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IDiscountSearchStrategy _searchStrategy;
        private readonly IDiscountSortStrategy _sortStrategy;
        private readonly IDiscountFactory _discountFactory;

        public DiscountManagementController(DatabaseContext context)
        {
            _context = context;
            _searchStrategy = new DefaultDiscountSearchStrategy();
            _sortStrategy = new DefaultDiscountSortStrategy();
            _discountFactory = new DiscountFactory(context);
        }

        // GET: DiscountManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            
            var query = _context.Magiamgia.AsQueryable();

            // Áp dụng Strategy Pattern cho tìm kiếm
            query = _searchStrategy.Search(query, searchString);
            
            // Áp dụng Strategy Pattern cho sắp xếp
            query = _sortStrategy.Sort(query);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var model = new PaginatedList<Magiamgia>(items, totalItems, pageNumber, pageSize);
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
            // Sử dụng Factory Pattern để tạo mã giảm giá mới
            var discount = await _discountFactory.CreateDiscount(
                "", 
                DateOnly.FromDateTime(DateTime.Today), 
                DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                0,
                0
            );
            
            return View(discount);
        }

        // POST: DiscountManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMgg,Ten,Ngaysudung,Ngayhethan,Tilechietkhau,Soluong")] Magiamgia discount)
        {
            // Server-side date validation:
            if (discount.Ngaysudung < DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("Ngaysudung", "Ngày bắt đầu sử dụng không được trong quá khứ.");
            }
            if (discount.Ngayhethan <= discount.Ngaysudung)
            {
                ModelState.AddModelError("Ngayhethan", "Ngày hết hạn phải sau ngày bắt đầu sử dụng.");
            }
            
            if (!ModelState.IsValid)
            {
                return View(discount);
            }

            if (string.IsNullOrEmpty(discount.IdMgg))
            {
                discount.IdMgg = await _discountFactory.GenerateNextDiscountId();
            }

            _context.Add(discount);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm mã giảm giá thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: DiscountManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magiamgia = await _context.Magiamgia
                .FirstOrDefaultAsync(m => m.IdMgg == id);
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

            var magiamgia = await _context.Magiamgia.FindAsync(id);
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
            if (id != magiamgia.IdMgg)
            {
                return NotFound();
            }

            if (magiamgia.Ngayhethan <= magiamgia.Ngaysudung)
            {
                ModelState.AddModelError("Ngayhethan", "Ngày hết hạn phải sau ngày bắt đầu sử dụng.");
            }

            if (!ModelState.IsValid)
            {
                return View(magiamgia);
            }

            try
            {
                _context.Update(magiamgia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MagiamgiaExists(magiamgia.IdMgg))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: DiscountManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Magiamgia
                .FirstOrDefaultAsync(m => m.IdMgg == id);

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
            // Sử dụng Template Method Pattern cho thao tác xóa
            var deleteOperation = new DeleteDiscountOperation(_context, this);
            return await deleteOperation.ProcessDiscount(id);
        }

        private bool MagiamgiaExists(string id)
        {
            return _context.Magiamgia.Any(e => e.IdMgg == id);
        }
    }
}
