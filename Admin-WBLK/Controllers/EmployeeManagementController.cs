using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;

namespace Admin_WBLK.Controllers
{
    public class EmployeeManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private const int PageSize = 10;

        public EmployeeManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: EmployeeManagement
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Nhanviens.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(n => n.IdNv.ToLower().Contains(searchString) ||
                                         n.Hoten.ToLower().Contains(searchString));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                                .OrderBy(n => n.IdNv)
                                .Skip((pageNumber - 1) * PageSize)
                                .Take(PageSize)
                                .ToListAsync();

            var model = new PaginatedList<Nhanvien>(items, totalItems, pageNumber, PageSize);
            return View(model);
        }

        // GET: EmployeeManagement/SearchSuggestions
        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new object[0]);

            term = term.ToLower();
            var suggestions = await _context.Nhanviens
                .Where(n => n.IdNv.ToLower().Contains(term) ||
                            n.Hoten.ToLower().Contains(term))
                .OrderBy(n => n.IdNv)
                .Take(5)
                .Select(n => new { n.IdNv, n.Hoten })
                .ToListAsync();

            return Json(suggestions);
        }

        // GET: EmployeeManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var employee = await _context.Nhanviens.FirstOrDefaultAsync(n => n.IdNv == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: EmployeeManagement/Create
        public async Task<IActionResult> Create()
        {
            // Generate new employee ID
            string newId = await GenerateNhanvienId();
            ViewBag.NewEmployeeId = newId;
            return View();
        }

        // POST: EmployeeManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNv,Hoten,Chucvu,Luong,Gioitinh,Sodienthoai,Email,Diachi,Ngayvaolam,Idtk")] Nhanvien employee)
        {
            if (ModelState.IsValid)
            {
                // If IdNv not provided, generate it.
                if (string.IsNullOrEmpty(employee.IdNv))
                {
                    employee.IdNv = await GenerateNhanvienId();
                }
                _context.Nhanviens.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: EmployeeManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var employee = await _context.Nhanviens.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: EmployeeManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdNv,Hoten,Chucvu,Luong,Gioitinh,Sodienthoai,Email,Diachi,Ngayvaolam,Idtk")] Nhanvien employee)
        {
            if (id != employee.IdNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Nhanviens.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.IdNv))
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
            return View(employee);
        }

        // GET: EmployeeManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var employee = await _context.Nhanviens.FirstOrDefaultAsync(e => e.IdNv == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: EmployeeManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Nhanviens.FindAsync(id);
            if (employee != null)
            {
                _context.Nhanviens.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
            return _context.Nhanviens.Any(e => e.IdNv == id);
        }

        // Helper method to generate a new employee ID (format "NV####")
        private async Task<string> GenerateNhanvienId()
        {
            var lastEmployee = await _context.Nhanviens
                .OrderByDescending(n => n.IdNv)
                .FirstOrDefaultAsync();
            if (lastEmployee == null)
            {
                return "NV0001";
            }

            // Assuming the ID format is "NV" followed by a 4-digit number.
            int lastNumber = int.Parse(lastEmployee.IdNv.Substring(2));
            return $"NV{(lastNumber + 1):D4}";
        }
    }
}
