using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Website_Ban_Linh_Kien.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Website_Ban_Linh_Kien.Models.Commands;
using Website_Ban_Linh_Kien.Models.Facades;
using Website_Ban_Linh_Kien.Models.Observers;

namespace Website_Ban_Linh_Kien.Controllers
{
    [Authorize] // Require login for all actions in this controller
    public class ProfileManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ProfileFacade _profileFacade;
        private readonly IProfileSubject _profileSubject;

        public ProfileManagementController(DatabaseContext context)
        {
            _context = context;
            _profileFacade = new ProfileFacade(context);
            
            // Khởi tạo Subject và đăng ký các Observer
            _profileSubject = new ProfileSubject();
            
            // Đăng ký các Observer
            _profileSubject.RegisterObserver(new LoggingObserver());
            _profileSubject.RegisterObserver(new NotificationObserver(context));
            _profileSubject.RegisterObserver(new StatisticsObserver(context));
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

            // Sử dụng Facade để lấy thông tin tài khoản và khách hàng
            var account = await _profileFacade.GetAccountByUsername(username);
            if (account == null)
            {
                return Unauthorized();
            }

            var khachhang = await _profileFacade.GetCustomerFullInfo(account.IdTk);
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
            // Sử dụng Command Pattern để xử lý cập nhật hồ sơ
            var command = new UpdateProfileCommand(
                _context,
                User.Identity.Name,
                Hoten,
                Diachi,
                ModelState,
                this,
                _profileSubject
            );

            return await command.Execute();
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

            // Sử dụng Facade để lấy thông tin tài khoản và khách hàng
            var account = await _profileFacade.GetAccountByUsername(username);
            if (account == null)
            {
                return Unauthorized();
            }

            var khachhang = await _profileFacade.GetCustomerByAccountId(account.IdTk);
            if (khachhang == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(orderId))
            {
                // Detail Mode: load order details including items
                var order = await _profileFacade.GetOrderDetail(orderId, khachhang.IdKh);
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
                var count = await _profileFacade.CountCustomerOrders(khachhang.IdKh);
                var items = await _profileFacade.GetCustomerOrders(khachhang.IdKh, pageNumber, pageSize);
                var paginatedList = new PaginatedList<Donhang>(items, count, pageNumber, pageSize);
                return View(paginatedList);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(string Idchitietdonhang, int rating, string comment)
        {
            // Sử dụng Command Pattern để xử lý gửi đánh giá
            var command = new SubmitReviewCommand(
                _context,
                User.Identity.Name,
                Idchitietdonhang,
                rating,
                comment,
                _profileSubject
            );

            return await command.Execute();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(string orderId, string LydoHuy)
        {
            // Sử dụng Command Pattern để xử lý hủy đơn hàng
            var command = new CancelOrderCommand(
                _context,
                User.Identity.Name,
                orderId,
                LydoHuy,
                _profileSubject
            );

            return await command.Execute();
        }
    }
}
