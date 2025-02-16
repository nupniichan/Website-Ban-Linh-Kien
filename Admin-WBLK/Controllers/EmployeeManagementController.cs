using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Admin_WBLK.Models;
using System.Text.RegularExpressions;
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
            
            // Create a new Nhanvien instance and assign the generated ID
            var employee = new Nhanvien
            {
                IdNv = newId
            };

            // Populate dropdown for accounts
            ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan");

            return View(employee);
        }



        // POST: EmployeeManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNv,Hoten,Chucvu,Luong,Gioitinh,Sodienthoai,Email,Diachi,Ngayvaolam,Idtk")] Nhanvien employee)
        {
            // Validate "Họ tên": must not be empty and cannot contain special characters.
            if (string.IsNullOrWhiteSpace(employee.Hoten) ||
                !Regex.IsMatch(employee.Hoten, @"^[\p{L}\s]+$"))
            {
                ModelState.AddModelError("Hoten", "Họ tên không được chứa ký tự đặc biệt và không được để trống.");
            }
            // Validate Email: must be a valid Gmail address.
            if (string.IsNullOrWhiteSpace(employee.Email) || 
                !Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", "Email phải có định dạng @gmail.com");
            }
            // Validate "Lương": must be >= 0.
            if (employee.Luong < 0)
            {
                ModelState.AddModelError("Luong", "Lương phải luôn >= 0.");
            }

            // Validate "Số điện thoại": must start with 0 and have at least 10 digits.
            if (string.IsNullOrWhiteSpace(employee.Sodienthoai) ||
                !Regex.IsMatch(employee.Sodienthoai, @"^0\d{9,}$"))
            {
                ModelState.AddModelError("Sodienthoai", "Số điện thoại phải theo cấu trúc 0... và có ít nhất 10 số.");
            }

            // Validate "Địa chỉ": must be at least 10 characters.
            if (string.IsNullOrWhiteSpace(employee.Diachi) || employee.Diachi.Length < 10)
            {
                ModelState.AddModelError("Diachi", "Địa chỉ phải có ít nhất 10 ký tự.");
            }

            // Validate "Tài khoản" (Account) if provided.
            if (!string.IsNullOrEmpty(employee.Idtk))
            {
                // Check if this account is already tied to a customer.
                if (_context.Khachhangs.Any(k => k.IdTk == employee.Idtk))
                {
                    ModelState.AddModelError("Idtk", "Tài khoản đó dành cho khách hàng");
                }
                // Check if this account is already tied to any employee.
                if (_context.Nhanviens.Any(n => n.Idtk == employee.Idtk))
                {
                    ModelState.AddModelError("Idtk", "Tài khoản đó thuộc về nhân viên khác");
                }
            }

            if (ModelState.IsValid)
            {
                // If IdNv is not provided, generate a new one.
                if (string.IsNullOrEmpty(employee.IdNv))
                {
                    employee.IdNv = await GenerateNhanvienId();
                }
                _context.Nhanviens.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate the account dropdown before returning to the view.
            ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", employee.Idtk);
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

            // Populate dropdown for accounts and select the current one if any.
            ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", employee.Idtk);
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

            // Validate "Họ tên": must not be empty and cannot contain special characters.
            if (string.IsNullOrWhiteSpace(employee.Hoten) ||
                !Regex.IsMatch(employee.Hoten, @"^[\p{L}\s]+$"))
            {
                ModelState.AddModelError("Hoten", "Họ tên không được chứa ký tự đặc biệt và không được để trống.");
            }
            // Validate Email: must be a valid Gmail address.
            if (string.IsNullOrWhiteSpace(employee.Email) || 
                !Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", "Email phải có định dạng @gmail.com");
            }
            // Validate "Lương": must be >= 0.
            if (employee.Luong < 0)
            {
                ModelState.AddModelError("Luong", "Lương phải luôn >= 0.");
            }

            // Validate "Số điện thoại": must start with 0 and have at least 10 digits.
            if (string.IsNullOrWhiteSpace(employee.Sodienthoai) ||
                !Regex.IsMatch(employee.Sodienthoai, @"^0\d{9,}$"))
            {
                ModelState.AddModelError("Sodienthoai", "Số điện thoại phải theo cấu trúc 0... và có ít nhất 10 số.");
            }

            // Validate "Địa chỉ": must be at least 10 characters.
            if (string.IsNullOrWhiteSpace(employee.Diachi) || employee.Diachi.Length < 10)
            {
                ModelState.AddModelError("Diachi", "Địa chỉ phải có ít nhất 10 ký tự.");
            }

            // Validate "Tài khoản" (Account) if provided.
            if (!string.IsNullOrEmpty(employee.Idtk))
            {
                // Check if this account is already tied to a customer.
                if (_context.Khachhangs.Any(k => k.IdTk == employee.Idtk))
                {
                    ModelState.AddModelError("Idtk", "Tài khoản đó dành cho khách hàng");
                }
                // Check if this account is already tied to a different employee.
                if (_context.Nhanviens.Any(n => n.Idtk == employee.Idtk && n.IdNv != employee.IdNv))
                {
                    ModelState.AddModelError("Idtk", "Tài khoản đó thuộc về nhân viên khác");
                }
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

            // Re-populate the account dropdown before returning to the view.
            ViewBag.Accounts = new SelectList(_context.Taikhoans, "IdTk", "Tentaikhoan", employee.Idtk);
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
