using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Admin_WBLK.Controllers
{
    public class DiscountManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public DiscountManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: DiscountManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            
            ViewData["CurrentFilter"] = searchString;
            
            var query = _context.Magiamgia.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(m => m.IdMgg.ToLower().Contains(searchString) ||
                                       m.Ten.ToLower().Contains(searchString));
            }

            // Default sorting by IdMgg
            query = query.OrderByDescending(m => m.IdMgg);

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
            var discount = new Magiamgia
            {
                Ngaysudung = DateOnly.FromDateTime(DateTime.Today),
                Ngayhethan = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            discount.IdMgg = await GenerateNextDiscountId();
            
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
                discount.IdMgg = await GenerateNextDiscountId();
            }

            _context.Add(discount);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm mã giảm giá thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GenerateNextDiscountId()
        {
            var lastDiscount = await _context.Magiamgia
                .OrderByDescending(d => d.IdMgg)
                .FirstOrDefaultAsync();

            if (lastDiscount == null)
            {
                return "MG000001";
            }

            string lastIdNumberPart = lastDiscount.IdMgg.Substring(2);
            if (int.TryParse(lastIdNumberPart, out int number))
            {
                number++;
                return "MG" + number.ToString("D6");
            }
            else
            {
                return "MG000001";
            }
        }

        // GET: DiscountManagement/Details/5
        public async Task<IActionResult> Details(string id, string returnUrl)
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

            ViewData["ReturnUrl"] = returnUrl;
            return View(magiamgia);
        }

        // GET: DiscountManagement/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
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

            ViewData["ReturnUrl"] = returnUrl;
            return View(magiamgia);
        }

        // POST: DiscountManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdMgg,Ten,Tilechietkhau,Ngaysudung,Ngayhethan,Soluong")] Magiamgia magiamgia, string returnUrl)
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
                
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
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
            var discount = await _context.Magiamgia.FindAsync(id);
            if (discount != null)
            {
                _context.Magiamgia.Remove(discount);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa mã giảm giá thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MagiamgiaExists(string id)
        {
            return _context.Magiamgia.Any(e => e.IdMgg == id);
        }
    }
}