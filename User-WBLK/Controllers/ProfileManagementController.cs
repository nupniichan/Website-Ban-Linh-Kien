using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Website_Ban_Linh_Kien.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Diagnostics;

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

            // Retrieve the customer (khách hàng) info including the related account info and rank info
            var khachhang = await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .Include(k => k.IdXephangvipNavigation)
                .FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(
            string Hoten, 
            string Diachi, 
            DateTime? Ngaysinh, 
            string Gioitinh,
            string newPassword, 
            string confirmPassword,
            string Sodienthoai)  // Phone parameter
        {
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
                .Include(k => k.IdXephangvipNavigation)
                .FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            // --- Validation for Profile Fields ---
            if (string.IsNullOrWhiteSpace(Hoten))
            {
                ModelState.AddModelError("Hoten", "Tên không được để trống.");
            }
            else if (!Regex.IsMatch(Hoten, @"^[\p{L}\s]+$"))
            {
                ModelState.AddModelError("Hoten", "Tên không được chứa ký tự đặc biệt hoặc số.");
            }

            if (string.IsNullOrWhiteSpace(Diachi) || Diachi.Length < 8)
            {
                ModelState.AddModelError("Diachi", "Địa chỉ phải có hơn 8 ký tự.");
            }

            // --- Phone Number Validation ---
            if (!string.IsNullOrWhiteSpace(Sodienthoai) && Sodienthoai != "0000000000")
            {
                if (!Regex.IsMatch(Sodienthoai, @"^0\d{9,10}$"))
                {
                    ModelState.AddModelError("Sodienthoai", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại hợp lệ.");
                }
            }

            // --- Optional Password Change ---
            var pwdPlaceholder = "********";
            if (!string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(confirmPassword))
            {
                // If the user didn't change the placeholder, treat it as not wanting to change the password.
                if (newPassword == pwdPlaceholder)
                {
                    newPassword = "";
                    confirmPassword = "";
                }
                else
                {
                    if (newPassword != confirmPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu xác nhận không khớp.");
                    }
                    else if (newPassword.Length < 8)
                    {
                        ModelState.AddModelError("", "Mật khẩu phải có ít nhất 8 ký tự.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View(khachhang);
            }

            // Update profile fields
            khachhang.Hoten = Hoten;
            khachhang.Diachi = Diachi;
            khachhang.Ngaysinh = Ngaysinh.HasValue ? DateOnly.FromDateTime(Ngaysinh.Value) : null;
            khachhang.Gioitinh = string.IsNullOrWhiteSpace(Gioitinh) ? null : Gioitinh;

            // Update phone if current value is default and a new phone is provided.
            if (khachhang.Sodienthoai == "0000000000" && !string.IsNullOrWhiteSpace(Sodienthoai))
            {
                khachhang.Sodienthoai = Sodienthoai;
            }

            // Update password only if newPassword is provided after checking against placeholder.
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                account.Matkhau = newPassword;
                _context.Taikhoans.Update(account);
            }

            _context.Khachhangs.Update(khachhang);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating profile: " + ex.ToString());
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật thông tin: " + ex.ToString();
                ViewBag.EditMode = true;
                return View(khachhang);
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(string Idchitietdonhang, int rating, string comment)
        {
            try
            {
                // Get the logged-in user's account
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
                if (account == null)
                {
                    return Json(new { success = false, message = "User account not found." });
                }

                var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
                if (khachhang == null)
                {
                    return Json(new { success = false, message = "Customer not found." });
                }

                // Retrieve the specific order detail record by its ID (including its parent order)
                var orderDetail = await _context.Chitietdonhangs
                    .Include(ct => ct.IdDhNavigation)
                    .FirstOrDefaultAsync(ct => ct.Idchitietdonhang == Idchitietdonhang);

                if (orderDetail == null)
                {
                    return Json(new { success = false, message = "Order detail not found." });
                }

                // Verify the order belongs to the logged-in customer
                var order = orderDetail.IdDhNavigation;
                if (order == null || order.IdKh != khachhang.IdKh)
                {
                    return Json(new { success = false, message = "Order not eligible for review." });
                }

                // Only allow review if order status is "Giao thành công"
                if (order.Trangthai != "Giao thành công")
                {
                    return Json(new { success = false, message = "Only orders with 'Giao thành công' status can be reviewed." });
                }

                // Prevent duplicate reviews for the same order detail
                if (orderDetail.IdDg != null)
                {
                    return Json(new { success = false, message = "Sản phẩm này đã được đánh giá." });
                }

                // Generate a new review ID (e.g., "DG000001")
                var lastReview = await _context.Danhgia.OrderByDescending(d => d.IdDg).FirstOrDefaultAsync();
                int nextNumber = 1;
                if (lastReview != null && lastReview.IdDg?.StartsWith("DG") == true)
                {
                    var numericPart = lastReview.IdDg.Substring(2);
                    if (int.TryParse(numericPart, out int parsed))
                    {
                        nextNumber = parsed + 1;
                    }
                }
                var newReviewId = "DG" + nextNumber.ToString("D6");

                // Create the review for this product
                var review = new Danhgia
                {
                    IdDg = newReviewId,
                    Sosao = rating,
                    Noidung = comment,
                    Ngaydanhgia = DateTime.Now,
                    IdKh = khachhang.IdKh
                };

                _context.Danhgia.Add(review);
                await _context.SaveChangesAsync();

                // Tie the review to the specific order detail
                orderDetail.IdDg = newReviewId;
                _context.Chitietdonhangs.Update(orderDetail);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!", reviewed = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[SubmitReview ERROR]: " + ex.ToString());
                return Json(new { success = false, message = "Có lỗi xảy ra khi gửi đánh giá: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(string orderId, string LydoHuy)
        {
            try
            {
                // Check if cancellation reason is provided
                if (string.IsNullOrWhiteSpace(LydoHuy))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do hủy đơn." });
                }

                // Get the logged-in user's account
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
                if (account == null)
                {
                    return Json(new { success = false, message = "User account not found." });
                }

                var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
                if (khachhang == null)
                {
                    return Json(new { success = false, message = "Customer not found." });
                }

                // Find the order and verify it belongs to the customer
                var order = await _context.Donhangs.FirstOrDefaultAsync(o => o.IdDh == orderId && o.IdKh == khachhang.IdKh);
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found." });
                }

                // Only allow cancellation if order status is "Chờ xác nhận", "Đã duyệt đơn", or "Đã thanh toán"
                if (order.Trangthai != "Chờ xác nhận" && order.Trangthai != "Đã duyệt đơn" && order.Trangthai != "Đã thanh toán")
                {
                    return Json(new { success = false, message = "Không thể hủy đơn hàng ở trạng thái hiện tại." });
                }

                // Update the order's status and record the cancellation reason
                order.Trangthai = "Hủy đơn";
                order.LydoHuy = LydoHuy;
                _context.Donhangs.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đơn hàng đã được hủy." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CancelOrder ERROR]: " + ex.ToString());
                return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đơn: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Reviews(int pageNumber = 1)
        {
            ViewBag.CurrentTab = "Review";
            SetBreadcrumb(
                ("Trang chủ", "/"),
                ("Tài khoản", "/account"),
                ("Đánh giá", null)
            );

            // Get the logged-in username
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Retrieve the account and then the customer
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

            int pageSize = 5;

            var query = _context.Danhgia
                .Where(r => r.IdKh == khachhang.IdKh)
                .OrderByDescending(r => r.Ngaydanhgia)
                .Include(r => r.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdSpNavigation);

            int count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Assuming you have a PaginatedList<T> helper class:
            var paginatedList = new PaginatedList<Danhgia>(items, count, pageNumber, pageSize);

            return View(paginatedList);
        
        }
        // GET: /ProfileManagement/CheckProfileCompleteness
        [HttpGet]
        public async Task<IActionResult> CheckProfileCompleteness()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { incomplete = false, missingFields = new string[] { } });
            }

            var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
            if (account == null)
            {
                return Json(new { incomplete = false, missingFields = new string[] { } });
            }

            var customer = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
            if (customer == null)
            {
                return Json(new { incomplete = false, missingFields = new string[] { } });
            }

            var missing = new List<string>();

            if (string.IsNullOrEmpty(customer.Diachi) || customer.Diachi == "Vui lòng đổi địa chỉ")
                missing.Add("Địa chỉ");
            if (string.IsNullOrEmpty(customer.Sodienthoai) || customer.Sodienthoai == "0000000000")
                missing.Add("Số điện thoại");
            if (!customer.Ngaysinh.HasValue)
                missing.Add("Ngày sinh");
            if (string.IsNullOrEmpty(customer.Gioitinh))
                missing.Add("Giới tính");
            if (string.IsNullOrEmpty(account.Matkhau))
                missing.Add("Mật khẩu");

            bool incompleteFlag = missing.Any();

            return Json(new { incomplete = incompleteFlag, missingFields = missing });
        }

    }
}
