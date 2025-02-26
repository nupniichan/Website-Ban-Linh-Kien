﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Website_Ban_Linh_Kien.Models;
using System;
using System.Linq;
using System.Security.Claims;

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
                .Include(k => k.IdXephangvipNavigation)
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
            else if (!Regex.IsMatch(Hoten, @"^[\p{L}\s]+$"))
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

                // Check that the order belongs to the logged-in customer
                var order = orderDetail.IdDhNavigation;
                if (order == null || order.IdKh != khachhang.IdKh)
                {
                    return Json(new { success = false, message = "Order not eligible for review." });
                }

                // Only allow reviews for orders with status "Giao thành công"
                if (order.Trangthai != "Giao thành công")
                {
                    return Json(new { success = false, message = "Only orders with 'Giao thành công' status can be reviewed." });
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

                // Create the review
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

            // Tie the same review to *all* items in the order
            var allDetails = _context.Chitietdonhangs.Where(ct => ct.IdDh == order.IdDh).ToList();
            foreach (var detail in allDetails)
            {
                detail.IdDg = newReviewId;
            }
            _context.Chitietdonhangs.UpdateRange(allDetails);
            await _context.SaveChangesAsync();


                return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!", reviewed = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[SubmitReview ERROR]: " + ex.Message);
                return Json(new { success = false, message = "Có lỗi xảy ra khi gửi đánh giá: " + ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(string orderId)
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

                // Find the order and verify it belongs to the customer
                var order = await _context.Donhangs.FirstOrDefaultAsync(o => o.IdDh == orderId && o.IdKh == khachhang.IdKh);
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found." });
                }

                // Only allow cancellation if order status is "Chờ xác nhận" or "Đã duyệt đơn"
                if (order.Trangthai != "Chờ xác nhận" && order.Trangthai != "Đã duyệt đơn")
                {
                    return Json(new { success = false, message = "Order cannot be cancelled in its current status." });
                }

                // Update the order's status to "Hủy đơn"
                order.Trangthai = "Hủy đơn";
                _context.Donhangs.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đơn hàng đã được hủy." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[CancelOrder ERROR]: " + ex.Message);
                return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đơn: " + ex.Message });
            }
        }

    }
}
