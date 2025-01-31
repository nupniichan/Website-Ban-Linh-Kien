using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;

namespace Admin_WBLK.Controllers
{
    public class RequestManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public RequestManagementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: RequestManagement
        public async Task<IActionResult> Index(string searchString, string filterType = "", string fromDate = "", string toDate = "", int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentType"] = filterType;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;

            var query = _context.Doitradhs
                                .Include(r => r.IdKhNavigation)
                                .Include(r => r.IdDhNavigation)
                                .AsQueryable();

            // Xử lý tìm kiếm theo mã yêu cầu, mã khách hàng, mã đơn hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                string lowerSearchString = searchString.ToLower();
                query = query.Where(r => r.Id.ToLower().Contains(lowerSearchString) || 
                                        (r.IdKhNavigation != null && r.IdKhNavigation.IdKh.ToLower().Contains(lowerSearchString)) || 
                                        (r.IdDh != null && r.IdDh.ToLower().Contains(lowerSearchString)));
            }

            // Lọc theo trạng thái (nếu có)
            if (!string.IsNullOrEmpty(filterType))
            {
                query = query.Where(r => r.Trangthai.Trim().ToLower() == filterType.Trim().ToLower());
            }

            // Lọc theo ngày (nếu có)
            if (DateOnly.TryParse(fromDate, out var fromDateParsed))
            {
                query = query.Where(r => r.Ngayyeucau >= fromDateParsed);
            }

            if (DateOnly.TryParse(toDate, out var toDateParsed))
            {
                query = query.Where(r => r.Ngayyeucau <= toDateParsed);
            }

            // Phân trang
            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.Ngayyeucau)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var model = new PaginatedList<Doitradh>(items, totalItems, pageNumber, pageSize);
            return View(model);
        }


        // POST: RequestManagement/Accept
        [HttpPost("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string id, string note)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid Request ID.");
            }

            var request = await _context.Doitradhs.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            request.Trangthai = "Chấp nhận";
            request.Ngayxuly = DateOnly.FromDateTime(DateTime.Now);
            request.Ghichu = note;

            _context.Update(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Yêu cầu đã được chấp nhận thành công.";
            return RedirectToAction(nameof(Index));
        }

        // POST: RequestManagement/Reject
        [HttpPost("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string id, string note)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid Request ID.");
            }

            var request = await _context.Doitradhs.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            request.Trangthai = "Từ chối";
            request.Ngayxuly = DateOnly.FromDateTime(DateTime.Now);
            request.Ghichu = note;

            _context.Update(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Yêu cầu đã bị từ chối thành công.";
            return RedirectToAction(nameof(Index));
        }
        // GET: RequestManagement/Create
        public IActionResult Create()
        {
            // Generate next Id
            var lastRequest = _context.Doitradhs.OrderByDescending(t => t.Id).FirstOrDefault();
            var nextId = "DTR001";

            if (lastRequest != null && int.TryParse(lastRequest.Id.Substring(3), out int lastId))
            {
                nextId = $"DTR{(lastId + 1).ToString("D3")}";
            }

            var model = new Doitradh { Id = nextId, Ngayyeucau = DateOnly.FromDateTime(DateTime.Now), Trangthai = "Chờ xử lý" };

            ViewData["IdKh"] = new SelectList(_context.Khachhangs.Select(k => new { IdKh = k.IdKh, DisplayName = k.Hoten + " - " + k.IdKh }), "IdKh", "DisplayName");
            ViewData["IdNv"] = new SelectList(_context.Nhanviens.Select(n => new { IdNv = n.IdNv, DisplayName = n.Hoten + " - " + n.IdNv }), "IdNv", "DisplayName");
            ViewData["IdDh"] = new SelectList(_context.Donhangs.Select(d => new { IdDh = d.IdDh, DisplayName = "Đơn hàng " + d.IdDh }), "IdDh", "DisplayName");
        

            return View(model);
        }

        // POST: RequestManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Trangthai,Lydo,Ghichu,IdKh,IdNv,IdDh")] Doitradh request)
        {
            if (string.IsNullOrWhiteSpace(request.Lydo))
            {
                ModelState.AddModelError("Lydo", "Lý do không hợp lệ. Không được để trống.");
            }

            // Explicitly ignore navigation properties in validation
            ModelState.Remove("IdDhNavigation");
            ModelState.Remove("IdKhNavigation");
            ModelState.Remove("IdNvNavigation");

            if (!ModelState.IsValid)
            {
                // Debugging: Output missing fields
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                ViewData["IdKh"] = new SelectList(_context.Khachhangs.Select(k => new { IdKh = k.IdKh, DisplayName = k.Hoten + " - " + k.IdKh }), "IdKh", "DisplayName");
                ViewData["IdNv"] = new SelectList(_context.Nhanviens.Select(n => new { IdNv = n.IdNv, DisplayName = n.Hoten + " - " + n.IdNv }), "IdNv", "DisplayName");
                ViewData["IdDh"] = new SelectList(_context.Donhangs.Select(d => new { IdDh = d.IdDh, DisplayName = "Đơn hàng " + d.IdDh }), "IdDh", "DisplayName");
                
                return View(request);
            }

            request.Ngayyeucau = DateOnly.FromDateTime(DateTime.Now);
            request.Trangthai = "Chờ xử lý";

            _context.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: RequestManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Doitradhs.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // Thiết lập danh sách trạng thái
            ViewData["TrangthaiList"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Chờ xử lý", Text = "Chờ xử lý" },
                new SelectListItem { Value = "Chấp nhận", Text = "Chấp nhận" },
                new SelectListItem { Value = "Từ chối", Text = "Từ chối" }
            }, "Value", "Text", request.Trangthai);

            // Các dropdown khác
            ViewData["IdKh"] = new SelectList(_context.Khachhangs.Select(k => new { IdKh = k.IdKh, DisplayName = k.Hoten + " - " + k.IdKh }), "IdKh", "DisplayName", request.IdKh);
            ViewData["IdNv"] = new SelectList(_context.Nhanviens.Select(n => new { IdNv = n.IdNv, DisplayName = n.Hoten + " - " + n.IdNv }), "IdNv", "DisplayName", request.IdNv);
            ViewData["IdDh"] = new SelectList(_context.Donhangs.Select(d => new { IdDh = d.IdDh, DisplayName = "Đơn hàng " + d.IdDh }), "IdDh", "DisplayName", request.IdDh);

            return View(request);
        }


        // POST: RequestManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Lydo,Ghichu,IdKh,IdNv,IdDh,Trangthai")] Doitradh request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            // Explicitly ignore navigation properties in validation
            ModelState.Remove("IdDhNavigation");
            ModelState.Remove("IdKhNavigation");
            ModelState.Remove("IdNvNavigation");

            // Luôn đảm bảo thiết lập ViewData
            ViewData["TrangthaiList"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Chờ xử lý", Text = "Chờ xử lý" },
                new SelectListItem { Value = "Chấp nhận", Text = "Chấp nhận" },
                new SelectListItem { Value = "Từ chối", Text = "Từ chối" }
            }, "Value", "Text", request.Trangthai);

            if (!ModelState.IsValid)
            {
                // Các dropdown khác
                ViewData["IdKh"] = new SelectList(_context.Khachhangs.Select(k => new { IdKh = k.IdKh, DisplayName = k.Hoten + " - " + k.IdKh }), "IdKh", "DisplayName", request.IdKh);
                ViewData["IdNv"] = new SelectList(_context.Nhanviens.Select(n => new { IdNv = n.IdNv, DisplayName = n.Hoten + " - " + n.IdNv }), "IdNv", "DisplayName", request.IdNv);
                ViewData["IdDh"] = new SelectList(_context.Donhangs.Select(d => new { IdDh = d.IdDh, DisplayName = "Đơn hàng " + d.IdDh }), "IdDh", "DisplayName", request.IdDh);

                return View(request);
            }

            try
            {
                var existingRequest = await _context.Doitradhs.FindAsync(id);
                if (existingRequest == null)
                {
                    return NotFound();
                }

                // Logic xử lý cập nhật trạng thái và ngày xử lý
                if (request.Trangthai == "Chờ xử lý")
                {
                    request.Ngayxuly = null;
                }
                else
                {
                    request.Ngayxuly = DateOnly.FromDateTime(DateTime.Now);
                }
                request.Ngayyeucau = existingRequest.Ngayyeucau;

                // Cập nhật
                _context.Entry(existingRequest).CurrentValues.SetValues(request);
                _context.Entry(existingRequest).Property(r => r.Ngayyeucau).IsModified = false;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Doitradhs.Any(e => e.Id == id))
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




        private bool RequestExists(string id)
        {
            return _context.Doitradhs.Any(e => e.Id == id);
        }
    }
    
}
