using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOutController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PlaceOrder(OrderInfo orderInfo)
        {
            // Xử lý đặt hàng ở đây
            return RedirectToAction("OrderConfirmation");
        }
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderInfo
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string PaymentMethod { get; set; }
        public string DiscountCode { get; set; }
        public bool IsHomeDelivery { get; set; }
    }
}
