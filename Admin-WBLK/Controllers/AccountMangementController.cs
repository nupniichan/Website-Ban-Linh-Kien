using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;


namespace Admin_WBLK.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public AccountManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: AccountManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Taikhoans.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(a => a.IdTk.ToLower().Contains(searchString) || a.Tentaikhoan.ToLower().Contains(searchString));
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var model = new PaginatedList<Taikhoan>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<object>());

            term = term.ToLower();
            var suggestions = await _context.Taikhoans
                .Where(t => t.IdTk.ToLower().Contains(term) ||
                            t.Tentaikhoan.ToLower().Contains(term))
                .Take(5)
                .Select(t => new { t.IdTk, t.Tentaikhoan })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: AccountManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(m => m.IdTk == id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // GET: AccountManagement/Create
        public IActionResult Create()
        {
            // Generate next IdTk
            var lastAccount = _context.Taikhoans.OrderByDescending(t => t.IdTk).FirstOrDefault();
            var nextId = "TK001";

            if (lastAccount != null && int.TryParse(lastAccount.IdTk.Substring(2), out int lastId))
            {
                nextId = $"TK{(lastId + 1).ToString("D3")}";
            }

            var model = new Taikhoan { IdTk = nextId, Ngaytaotk = DateOnly.FromDateTime(DateTime.Now) };
            return View(model);
        }

        // POST: AccountManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTk, Tentaikhoan, Matkhau, Quyentruycap")] Taikhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
                // Check if the Tentaikhoan already exists
                if (_context.Taikhoans.Any(t => t.Tentaikhoan == taikhoan.Tentaikhoan))
                {
                    ModelState.AddModelError("Tentaikhoan", "Tên tài khoản đã tồn tại.");
                    return View(taikhoan);
                }

                taikhoan.Ngaytaotk = DateOnly.FromDateTime(DateTime.Now);
                taikhoan.Ngaysuadoi = null;
                _context.Add(taikhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taikhoan);
        }

        // GET: AccountManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // POST: AccountManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdTk, Tentaikhoan, Matkhau, Quyentruycap")] Taikhoan taikhoan)
        {
            if (id != taikhoan.IdTk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAccount = await _context.Taikhoans.AsNoTracking().FirstOrDefaultAsync(t => t.IdTk == id);
                    if (existingAccount != null)
                    {
                        taikhoan.Ngaytaotk = existingAccount.Ngaytaotk; // Preserve original creation date
                    }

                    taikhoan.Ngaysuadoi = DateOnly.FromDateTime(DateTime.Now);
                    _context.Update(taikhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaikhoanExists(taikhoan.IdTk))
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

            return View(taikhoan);
        }

        // GET: AccountManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(m => m.IdTk == id);
            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // POST: AccountManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan != null)
            {
                _context.Taikhoans.Remove(taikhoan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaikhoanExists(string id)
        {
            return _context.Taikhoans.Any(e => e.IdTk == id);
        }
        
    }
}