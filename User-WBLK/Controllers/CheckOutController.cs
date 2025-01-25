using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CheckOutController : BaseController
    {
        private const string STORE_ADDRESS = "123 Nguyễn Văn A, Quận 1, TP.HCM";

        public ActionResult Index()
        {
            SetBreadcrumb(
                ("Checkout", "/checkout"),
                ("Checkout", null)
            );
            
            var model = new CheckoutViewModel
            {
                DeliveryMethod = DeliveryMethod.HomeDelivery, // Mặc định chọn giao hàng
                StoreAddress = STORE_ADDRESS
            };
            
            // Có thể thêm code để lấy thông tin người dùng đã đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                // Lấy thông tin từ user profile
                model.ReceiverName = User.Identity.Name;
                // model.ReceiverPhone = currentUser.Phone;
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveDeliveryInfo(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Xử lý địa chỉ giao hàng
            string fullAddress = model.DeliveryMethod == DeliveryMethod.StorePickup 
                ? STORE_ADDRESS 
                : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}";

            // TODO: Lưu thông tin đơn hàng vào database
            // Tạm thời chuyển hướng đến trang thanh toán
            return RedirectToAction("Payment");
        }
    }
}
