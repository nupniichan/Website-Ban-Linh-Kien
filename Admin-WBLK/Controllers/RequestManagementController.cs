using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string searchString, string filterType = "", int pageNumber = 1)
{
        int pageSize = 10;
        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentType"] = filterType;

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

            request.Trangthai = "Chấp nhận đổi";
            request.Ngayxuly = DateTime.Now;
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

            request.Trangthai = "Từ chối đổi";
            request.Ngayxuly = DateTime.Now;
            request.Ghichu = note;

            _context.Update(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Yêu cầu đã bị từ chối thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
