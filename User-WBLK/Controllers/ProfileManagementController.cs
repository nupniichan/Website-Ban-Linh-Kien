using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    [Authorize] // Require login for all actions in this controller
    public class ProfileManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public ProfileManagementController(DatabaseContext context)
        {
            _context = context;
        }

        protected void SetBreadcrumb(params (string Text, string Url)[] items)
        {
            ViewData["Breadcrumb"] = items.ToList();
        }

        // GET: Profile?edit=true  (if edit is true, the view renders the edit form)
        [HttpGet]
        public async Task<IActionResult> Profile(bool edit = false)
        {
            ViewBag.CurrentTab = "Profile";
            SetBreadcrumb(
                ("Trang chủ", "/"),
                ("Tài khoản", "/account"),
                ("Thông tin tài khoản", null)
            );
            ViewBag.EditMode = edit;  // pass to the view so it can decide the rendering mode

            // Get the logged-in username (assumes Tentaikhoan is used as username)
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Retrieve the account using the username
            var account = await _context.Taikhoans
                .FirstOrDefaultAsync(t => t.Tentaikhoan == username);
            if (account == null)
            {
                return Unauthorized();
            }

            // Retrieve the customer (khách hàng) info including the related account info
            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // POST: Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string Hoten, string Diachi)
        {
            // Retrieve the current user/account as before
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
            if (account == null)
            {
                return Unauthorized();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            // --- Server-side Validation ---
            if (string.IsNullOrWhiteSpace(Hoten))
            {
                ModelState.AddModelError("Hoten", "Tên không được để trống.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Hoten, @"^[\p{L}\s]+$"))
            {
                ModelState.AddModelError("Hoten", "Tên không được chứa ký tự đặc biệt hoặc số.");
            }

            if (string.IsNullOrWhiteSpace(Diachi) || Diachi.Length < 8)
            {
                ModelState.AddModelError("Diachi", "Địa chỉ phải có hơn 8 ký tự.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                // Return the current full record so that non-editable fields show up
                return View(khachhang);
            }

            // Update allowed fields
            khachhang.Hoten = Hoten;
            khachhang.Diachi = Diachi;

            _context.Khachhangs.Update(khachhang);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công!";
            return RedirectToAction("Profile");
        }


        public async Task<IActionResult> OrderHistory(string? orderId, int pageNumber = 1)
        {
            ViewBag.CurrentTab = "OrderHistory";
            SetBreadcrumb(
                ("Trang chủ", "/"),
                ("Tài khoản", "/account"),
                ("Lịch sử giao dịch", null)
            );

            // Get the logged-in username
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Retrieve the account and customer
            var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
            if (account == null)
            {
                return Unauthorized();
            }

            var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(orderId))
            {
                // Detail Mode: load order details including items
                var order = await _context.Donhangs
                    .Include(o => o.Chitietdonhangs)
                        .ThenInclude(ct => ct.IdSpNavigation)
                    .FirstOrDefaultAsync(o => o.IdDh == orderId && o.IdKh == khachhang.IdKh);
                if (order == null)
                {
                    return NotFound();
                }
                ViewBag.DetailOrder = order;
                return View(null);
            }
            else
            {
                // List Mode: create a paginated list
                int pageSize = 10; // Adjust page size as needed
                var query = _context.Donhangs
                    .Where(o => o.IdKh == khachhang.IdKh)
                    .OrderByDescending(o => o.Ngaydathang);
                int count = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var paginatedList = new PaginatedList<Donhang>(items, count, pageNumber, pageSize);
                return View(paginatedList);
            }
        }
    }
}
