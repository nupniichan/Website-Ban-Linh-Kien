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
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
            {
                int pageSize = 10;
                ViewData["CurrentFilter"] = searchString;

                var query = _context.Magiamgia.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    query = query.Where(m => m.IdMgg.ToLower().Contains(searchString) ||
                                            m.Ten.ToLower().Contains(searchString));
                }

                var totalItems = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

                var model = new PaginatedList<Magiamgia>(items, totalItems, pageNumber, pageSize);
                return View(model);
            }


        // GET: DiscountManagement/Create
        public async Task<IActionResult> Create()
        {
            var discount = new Magiamgia
            {
                Ngaysudung = DateOnly.FromDateTime(DateTime.Today),
                Ngayhethan = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            // Also generate Id if needed
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

            // Auto-generate the discount ID if it's empty:
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
            // Order discounts by descending ID and extract the numeric part.
            var lastDiscount = await _context.Magiamgia
                .OrderByDescending(d => d.IdMgg)
                .FirstOrDefaultAsync();

            if (lastDiscount == null)
            {
                return "MG000001";
            }

            string lastIdNumberPart = lastDiscount.IdMgg.Substring(2); // Remove "MG" prefix
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
        public async Task<IActionResult> Details(string id)
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

        // GET: DiscountManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Magiamgia.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: DiscountManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdMgg,Ten,Ngaysudung,Ngayhethan,Tilechietkhau,Soluong")] Magiamgia discount)
        {
            if (id != discount.IdMgg)
            {
                return NotFound();
            }

            // 1️⃣ Remove validation for Ngaysudung being in the past

            // 2️⃣ Ensure Ngayhethan is after Ngaysudung
            if (discount.Ngayhethan <= discount.Ngaysudung)
            {
                ModelState.AddModelError("Ngayhethan", "Ngày hết hạn phải sau ngày bắt đầu sử dụng.");
            }

            if (!ModelState.IsValid)
            {
                return View(discount);
            }

            try
            {
                _context.Update(discount);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật mã giảm giá thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountExists(discount.IdMgg))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
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

        private bool DiscountExists(string id)
        {
            return _context.Magiamgia.Any(e => e.IdMgg == id);
        }
    }
}