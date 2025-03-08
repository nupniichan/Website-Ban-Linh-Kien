using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;
using Website_Ban_Linh_Kien.Models.Observers;

namespace Website_Ban_Linh_Kien.Models.Commands
{
    // Interface cho các lệnh
    public interface IProfileCommand
    {
        Task<IActionResult> Execute();
    }

    // Lớp cơ sở cho các lệnh liên quan đến hồ sơ người dùng
    public abstract class ProfileCommandBase : IProfileCommand
    {
        protected readonly DatabaseContext _context;
        protected readonly string _username;
        protected readonly IProfileSubject _subject;

        public ProfileCommandBase(DatabaseContext context, string username, IProfileSubject subject)
        {
            _context = context;
            _username = username;
            _subject = subject;
        }

        // Template Method định nghĩa các bước thực hiện lệnh
        public async Task<IActionResult> Execute()
        {
            try
            {
                // Bước 1: Xác thực người dùng
                if (string.IsNullOrEmpty(_username))
                {
                    return new UnauthorizedResult();
                }

                // Bước 2: Lấy thông tin tài khoản
                var account = await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == _username);
                if (account == null)
                {
                    return new UnauthorizedResult();
                }

                // Bước 3: Lấy thông tin khách hàng
                var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == account.IdTk);
                if (khachhang == null)
                {
                    return new NotFoundResult();
                }

                // Bước 4: Thực hiện hành động cụ thể
                return await PerformAction(account, khachhang);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                return new JsonResult(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        // Phương thức trừu tượng để các lớp con triển khai
        protected abstract Task<IActionResult> PerformAction(Taikhoan account, Khachhang khachhang);
    }

    // Lệnh cập nhật thông tin hồ sơ
    public class UpdateProfileCommand : ProfileCommandBase
    {
        private readonly string _hoten;
        private readonly string _diachi;
        private readonly ModelStateDictionary _modelState;
        private readonly Controller _controller;

        public UpdateProfileCommand(
            DatabaseContext context, 
            string username, 
            string hoten, 
            string diachi, 
            ModelStateDictionary modelState,
            Controller controller,
            IProfileSubject subject) : base(context, username, subject)
        {
            _hoten = hoten;
            _diachi = diachi;
            _modelState = modelState;
            _controller = controller;
        }

        protected override async Task<IActionResult> PerformAction(Taikhoan account, Khachhang khachhang)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(_hoten))
            {
                _modelState.AddModelError("Hoten", "Tên không được để trống.");
            }
            else if (!Regex.IsMatch(_hoten, @"^[\p{L}\s]+$"))
            {
                _modelState.AddModelError("Hoten", "Tên không được chứa ký tự đặc biệt hoặc số.");
            }

            if (string.IsNullOrWhiteSpace(_diachi) || _diachi.Length < 8)
            {
                _modelState.AddModelError("Diachi", "Địa chỉ phải có hơn 8 ký tự.");
            }

            if (!_modelState.IsValid)
            {
                // Cần load lại thông tin đầy đủ của khách hàng
                var fullKhachhang = await _context.Khachhangs
                    .Include(k => k.IdTkNavigation)
                    .Include(k => k.IdXephangvipNavigation)
                    .FirstOrDefaultAsync(k => k.IdTk == account.IdTk);

                _controller.ViewBag.EditMode = true;
                return _controller.View(fullKhachhang);
            }

            // Cập nhật thông tin
            khachhang.Hoten = _hoten;
            khachhang.Diachi = _diachi;

            _context.Khachhangs.Update(khachhang);
            await _context.SaveChangesAsync();

            // Thông báo cho các observer
            await _subject.NotifyObservers(ProfileEventType.ProfileUpdated, khachhang);

            _controller.TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công!";
            return _controller.RedirectToAction("Profile");
        }
    }

    // Lệnh gửi đánh giá sản phẩm
    public class SubmitReviewCommand : ProfileCommandBase
    {
        private readonly string _idChiTietDonHang;
        private readonly int _rating;
        private readonly string _comment;

        public SubmitReviewCommand(
            DatabaseContext context, 
            string username, 
            string idChiTietDonHang, 
            int rating, 
            string comment,
            IProfileSubject subject) : base(context, username, subject)
        {
            _idChiTietDonHang = idChiTietDonHang;
            _rating = rating;
            _comment = comment;
        }

        protected override async Task<IActionResult> PerformAction(Taikhoan account, Khachhang khachhang)
        {
            // Retrieve the specific order detail record by its ID (including its parent order)
            var orderDetail = await _context.Chitietdonhangs
                .Include(ct => ct.IdDhNavigation)
                .FirstOrDefaultAsync(ct => ct.Idchitietdonhang == _idChiTietDonHang);

            if (orderDetail == null)
            {
                return new JsonResult(new { success = false, message = "Order detail not found." });
            }

            // Verify the order belongs to the logged-in customer
            var order = orderDetail.IdDhNavigation;
            if (order == null || order.IdKh != khachhang.IdKh)
            {
                return new JsonResult(new { success = false, message = "Order not eligible for review." });
            }

            // Only allow review if order status is "Giao thành công"
            if (order.Trangthai != "Giao thành công")
            {
                return new JsonResult(new { success = false, message = "Only orders with 'Giao thành công' status can be reviewed." });
            }

            // Prevent duplicate reviews for the same order detail
            if (orderDetail.IdDg != null)
            {
                return new JsonResult(new { success = false, message = "Sản phẩm này đã được đánh giá." });
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
                Sosao = _rating,
                Noidung = _comment,
                Ngaydanhgia = DateTime.Now,
                IdKh = khachhang.IdKh
            };

            _context.Danhgia.Add(review);
            await _context.SaveChangesAsync();

            // Tie the review to the specific order detail
            orderDetail.IdDg = newReviewId;
            _context.Chitietdonhangs.Update(orderDetail);
            await _context.SaveChangesAsync();

            // Thông báo cho các observer
            await _subject.NotifyObservers(ProfileEventType.ReviewSubmitted, review);

            return new JsonResult(new { success = true, message = "Cảm ơn bạn đã đánh giá!", reviewed = true });
        }
    }

    // Lệnh hủy đơn hàng
    public class CancelOrderCommand : ProfileCommandBase
    {
        private readonly string _orderId;
        private readonly string _lydoHuy;

        public CancelOrderCommand(
            DatabaseContext context, 
            string username, 
            string orderId, 
            string lydoHuy,
            IProfileSubject subject) : base(context, username, subject)
        {
            _orderId = orderId;
            _lydoHuy = lydoHuy;
        }

        protected override async Task<IActionResult> PerformAction(Taikhoan account, Khachhang khachhang)
        {
            // Check if cancellation reason is provided
            if (string.IsNullOrWhiteSpace(_lydoHuy))
            {
                return new JsonResult(new { success = false, message = "Vui lòng nhập lý do hủy đơn." });
            }

            // Find the order and verify it belongs to the customer
            var order = await _context.Donhangs.FirstOrDefaultAsync(o => o.IdDh == _orderId && o.IdKh == khachhang.IdKh);
            if (order == null)
            {
                return new JsonResult(new { success = false, message = "Order not found." });
            }

            // Only allow cancellation if order status is "Chờ xác nhận", "Đã duyệt đơn", or "Đã thanh toán"
            if (order.Trangthai != "Chờ xác nhận" && order.Trangthai != "Đã duyệt đơn" && order.Trangthai != "Đã thanh toán")
            {
                return new JsonResult(new { success = false, message = "Không thể hủy đơn hàng ở trạng thái hiện tại." });
            }

            // Update the order's status and record the cancellation reason
            order.Trangthai = "Hủy đơn";
            order.LydoHuy = _lydoHuy;
            _context.Donhangs.Update(order);
            await _context.SaveChangesAsync();

            // Thông báo cho các observer
            await _subject.NotifyObservers(ProfileEventType.OrderCancelled, order);

            return new JsonResult(new { success = true, message = "Đơn hàng đã được hủy." });
        }
    }
} 