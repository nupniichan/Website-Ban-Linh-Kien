using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Linq;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DatabaseContext _context;
        private const string STORE_ADDRESS = "123 Nguyễn Văn A, Quận 1, TP.HCM";

        public CheckoutController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new CheckoutViewModel
            {
                StoreAddress = STORE_ADDRESS,
                DeliveryMethod = DeliveryMethod.HomeDelivery
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                // Lấy CustomerId từ Claims
                var customerId = User.FindFirstValue("CustomerId");
                if (!string.IsNullOrEmpty(customerId))
                {
                    // Lấy thông tin khách hàng từ database
                    var customer = await _context.Khachhangs
                        .Include(k => k.Giohangs)
                            .ThenInclude(g => g.Chitietgiohangs)
                                .ThenInclude(c => c.IdSpNavigation)
                        .FirstOrDefaultAsync(k => k.IdKh == customerId);

                    if (customer != null)
                    {
                        // Điền thông tin khách hàng vào viewModel
                        viewModel.CustomerId = customer.IdKh;
                        viewModel.ReceiverName = customer.Hoten;
                        viewModel.Email = customer.Email;
                        viewModel.ReceiverPhone = customer.Sodienthoai;

                        // Lấy giỏ hàng hiện tại của khách hàng
                        var currentCart = customer.Giohangs
                            .OrderByDescending(g => g.Thoigiancapnhat)
                            .FirstOrDefault();

                        if (currentCart?.Chitietgiohangs != null)
                        {
                            foreach (var item in currentCart.Chitietgiohangs)
                            {
                                viewModel.Items.Add(new CheckoutItemViewModel
                                {
                                    ProductId = item.IdSp,
                                    ProductName = item.IdSpNavigation.Tensanpham,
                                    ImageUrl = item.IdSpNavigation.Hinhanh,
                                    Quantity = item.Soluongsanpham,
                                    Price = item.IdSpNavigation.Gia
                                });
                            }
                        }

                        // Nếu có địa chỉ, điền vào form
                        if (!string.IsNullOrEmpty(customer.Diachi))
                        {
                            // Giả sử địa chỉ được lưu theo format: "Số nhà, Phường/Xã, Quận/Huyện, Tỉnh/TP"
                            var addressParts = customer.Diachi.Split(',').Select(p => p.Trim()).ToList();
                            if (addressParts.Count >= 4)
                            {
                                viewModel.StreetAddress = addressParts[0];
                                viewModel.Ward = addressParts[1];
                                viewModel.District = addressParts[2];
                                viewModel.City = addressParts[3];
                            }
                        }
                    }
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CheckoutViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                // Khởi tạo Items nếu null
                model.Items ??= new List<CheckoutItemViewModel>();

                if (!model.Items.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrEmpty(model.ReceiverName) || 
                    string.IsNullOrEmpty(model.ReceiverPhone) || 
                    string.IsNullOrEmpty(model.Email))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin người nhận" });
                }

                Console.WriteLine($"Processing order for {model.ReceiverName}");
                Console.WriteLine($"Items count: {model.Items.Count}");

                // Kiểm tra và tạo khách hàng mới nếu chưa tồn tại
                var customer = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                string customerId;
                if (customer != null)
                {
                    customerId = customer.IdKh;
                    // Cập nhật thông tin khách hàng
                    customer.Hoten = model.ReceiverName;
                    customer.Sodienthoai = model.ReceiverPhone;
                    customer.Email = model.Email;
                    customer.Diachi = model.DeliveryMethod == DeliveryMethod.StorePickup
                        ? STORE_ADDRESS 
                        : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}";
                    
                    _context.Khachhangs.Update(customer);
                }
                else
                {
                    // Tạo khách hàng mới
                    var lastCustomerId = await _context.Khachhangs
                        .OrderByDescending(k => k.IdKh)
                        .Select(k => k.IdKh)
                        .FirstOrDefaultAsync();

                    int nextId = 1;
                    if (lastCustomerId != null)
                    {
                        // Lấy số cuối cùng và tăng lên 1
                        if (int.TryParse(lastCustomerId.Substring(2), out int currentId))
                        {
                            nextId = currentId + 1;
                        }
                        else
                        {
                            // Log lỗi nếu không parse được ID
                            Console.WriteLine($"Không thể parse ID khách hàng: {lastCustomerId}");
                            return Json(new { success = false, message = "Lỗi khi tạo mã khách hàng" });
                        }
                    }

                    // Kiểm tra xem ID mới đã tồn tại chưa
                    string newCustomerId;
                    bool idExists;
                    do
                    {
                        newCustomerId = $"KH{nextId:D6}";
                        idExists = await _context.Khachhangs.AnyAsync(k => k.IdKh == newCustomerId);
                        if (idExists)
                        {
                            nextId++;
                        }
                    } while (idExists);

                    customer = new Khachhang
                    {
                        IdKh = newCustomerId,
                        Hoten = model.ReceiverName,
                        Email = model.Email,
                        Sodienthoai = model.ReceiverPhone,
                        Diachi = model.DeliveryMethod == DeliveryMethod.StorePickup
                            ? STORE_ADDRESS 
                            : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                        IdXephangvip = "THANTHIET",
                        Diemtichluy = 0,
                        
                    };

                    _context.Khachhangs.Add(customer);

                    try
                    {
                        await _context.SaveChangesAsync();
                        customerId = newCustomerId;
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Lỗi khi lưu khách hàng: {ex.Message}");
                        return Json(new { success = false, message = "Lỗi khi tạo khách hàng mới" });
                    }
                }

                // Tạo đơn hàng mới
                var lastOrderId = await _context.Donhangs
                    .OrderByDescending(d => d.IdDh)
                    .Select(d => d.IdDh)
                    .FirstOrDefaultAsync();

                int nextOrderId = 1;
                if (lastOrderId != null)
                {
                    nextOrderId = int.Parse(lastOrderId.Substring(2)) + 1;
                }
                string orderId = $"DH{nextOrderId:D6}";

                var order = new Donhang
                {
                    IdDh = orderId,
                    IdKh = customerId,
                    Ngaydathang = DateTime.Now,
                    Diachigiaohang = model.DeliveryMethod == DeliveryMethod.StorePickup
                        ? STORE_ADDRESS 
                        : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                    Phuongthucthanhtoan = "COD",
                    Trangthai = "Chờ xác nhận",
                    Ghichu = model.Note ?? "",
                    Tongtien = model.Items.Sum(i => i.Price * i.Quantity)
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Thêm đơn hàng trước
                        _context.Donhangs.Add(order);
                        await _context.SaveChangesAsync();

                        // Tạo chi tiết đơn hàng
                        var lastDetailId = await _context.Chitietdonhangs
                            .OrderByDescending(c => c.Idchitietdonhang)
                            .Select(c => c.Idchitietdonhang)
                            .FirstOrDefaultAsync() ?? "CTDH00000";

                        int detailStartId = int.Parse(lastDetailId.Substring(4)) + 1;

                        foreach (var item in model.Items)
                        {
                            if (string.IsNullOrEmpty(item.ProductId))
                            {
                                continue;
                            }

                            var orderDetail = new Chitietdonhang
                            {
                                Idchitietdonhang = $"CTDH{detailStartId:D5}",
                                IdDh = orderId,
                                IdSp = item.ProductId,
                                Soluongsanpham = item.Quantity,
                                Dongia = item.Price
                            };

                            _context.Chitietdonhangs.Add(orderDetail);
                            detailStartId++;
                            
                            Console.WriteLine($"Adding order detail - ID: {orderDetail.Idchitietdonhang}, " +
                                             $"Order ID: {orderDetail.IdDh}, " +
                                             $"Product ID: {orderDetail.IdSp}, " +
                                             $"Quantity: {orderDetail.Soluongsanpham}, " +
                                             $"Price: {orderDetail.Dongia}");
                        }

                        // Lưu chi tiết đơn hàng
                        await _context.SaveChangesAsync();
                        
                        // Kiểm tra xác nhận sau khi lưu
                        var savedDetails = await _context.Chitietdonhangs
                            .Where(c => c.IdDh == orderId)
                            .Select(c => new { // Chỉ select các trường cần thiết
                                c.Idchitietdonhang,
                                c.IdDh,
                                c.IdSp,
                                c.Soluongsanpham,
                                c.Dongia
                            })
                            .ToListAsync();

                        Console.WriteLine($"Saved order details count: {savedDetails.Count}");
                        foreach (var detail in savedDetails)
                        {
                            Console.WriteLine($"Saved detail - ID: {detail.Idchitietdonhang}, " +
                                             $"Order ID: {detail.IdDh}, " +
                                             $"Product ID: {detail.IdSp}, " +
                                             $"Quantity: {detail.Soluongsanpham}, " +
                                             $"Price: {detail.Dongia}");
                        }

                        // Commit transaction
                        await transaction.CommitAsync();
                        Console.WriteLine($"Transaction committed successfully for order {orderId}");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"Error in transaction: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        throw;
                    }
                }

                // Lưu mã đơn hàng vào TempData
                TempData["OrderId"] = orderId;

                return Json(new { success = true, redirectUrl = Url.Action("PaymentSuccess", "PaymentResult") });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi đặt hàng: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            try
            {
                // Đảm bảo StoreAddress luôn có giá trị
                model.StoreAddress = STORE_ADDRESS;

                // Kiểm tra ModelState
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("StoreAddress");
                    
                    if (!ModelState.IsValid)
                    {
                        return View("Index", model);
                    }
                }

                // Kiểm tra thông tin cần thiết
                if (string.IsNullOrEmpty(model.ReceiverName) || 
                    string.IsNullOrEmpty(model.ReceiverPhone) || 
                    string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin người nhận");
                    return View("Index", model);
                }

                // Kiểm tra giỏ hàng
                if (model.Items == null || !model.Items.Any())
                {
                    ModelState.AddModelError("", "Giỏ hàng trống");
                    return View("Index", model);
                }

                // Xử lý khách vãng lai
                var existingCustomer = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                string customerId;
                if (existingCustomer != null)
                {
                    customerId = existingCustomer.IdKh;
                }
                else
                {
                    // Tạo khách hàng mới
                    var lastCustomerId = await _context.Khachhangs
                        .OrderByDescending(k => k.IdKh)
                        .Select(k => k.IdKh)
                        .FirstOrDefaultAsync();

                    int nextId = 1;
                    if (lastCustomerId != null)
                    {
                        // Lấy số cuối cùng và tăng lên 1
                        if (int.TryParse(lastCustomerId.Substring(2), out int currentId))
                        {
                            nextId = currentId + 1;
                        }
                        else
                        {
                            // Log lỗi nếu không parse được ID
                            Console.WriteLine($"Không thể parse ID khách hàng: {lastCustomerId}");
                            return Json(new { success = false, message = "Lỗi khi tạo mã khách hàng" });
                        }
                    }

                    // Kiểm tra xem ID mới đã tồn tại chưa
                    string newCustomerId;
                    bool idExists;
                    do
                    {
                        newCustomerId = $"KH{nextId:D6}";
                        idExists = await _context.Khachhangs.AnyAsync(k => k.IdKh == newCustomerId);
                        if (idExists)
                        {
                            nextId++;
                        }
                    } while (idExists);

                    var newCustomer = new Khachhang
                    {
                        IdKh = newCustomerId,
                        Hoten = model.ReceiverName,
                        Email = model.Email,
                        Sodienthoai = model.ReceiverPhone,
                        Diachi = model.DeliveryMethod == DeliveryMethod.StorePickup
                            ? STORE_ADDRESS
                            : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                        IdXephangvip = "THANTHIET",
                        Diemtichluy = 0,
                        Loaikhachhang = 0
                    };

                    _context.Khachhangs.Add(newCustomer);
                    await _context.SaveChangesAsync();
                    customerId = newCustomerId;
                }

                // Tạo đơn hàng mới
                var lastOrderId = await _context.Donhangs
                    .OrderByDescending(d => d.IdDh)
                    .Select(d => d.IdDh)
                    .FirstOrDefaultAsync();

                int nextOrderId = 1;
                if (lastOrderId != null)
                {
                    nextOrderId = int.Parse(lastOrderId.Substring(2)) + 1;
                }
                string orderId = $"DH{nextOrderId:D6}";

                var order = new Donhang
                {
                    IdDh = orderId,
                    IdKh = customerId,
                    Ngaydathang = DateTime.Now,
                    Diachigiaohang = model.DeliveryMethod == DeliveryMethod.StorePickup 
                        ? STORE_ADDRESS 
                        : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                    Phuongthucthanhtoan = "COD",
                    Trangthai = "Chờ xác nhận",
                    Ghichu = model.Note ?? "",
                    Tongtien = model.Items.Sum(i => i.Price * i.Quantity)
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Thêm đơn hàng trước
                        _context.Donhangs.Add(order);
                        await _context.SaveChangesAsync();

                        // Tạo chi tiết đơn hàng
                        var lastDetailId = await _context.Chitietdonhangs
                            .OrderByDescending(c => c.Idchitietdonhang)
                            .Select(c => c.Idchitietdonhang)
                            .FirstOrDefaultAsync() ?? "CTDH0000000";

                        int detailStartId = int.Parse(lastDetailId.Substring(6)) + 1;

                        foreach (var item in model.Items)
                        {
                            if (string.IsNullOrEmpty(item.ProductId))
                            {
                                continue;
                            }

                            var orderDetail = new Chitietdonhang
                            {
                                Idchitietdonhang = $"CTDH{detailStartId:D5}",
                                IdDh = orderId,
                                IdSp = item.ProductId,
                                Soluongsanpham = item.Quantity,
                                Dongia = item.Price
                            };

                            _context.Chitietdonhangs.Add(orderDetail);
                            detailStartId++;
                            
                            Console.WriteLine($"Adding order detail - ID: {orderDetail.Idchitietdonhang}, " +
                                             $"Order ID: {orderDetail.IdDh}, " +
                                             $"Product ID: {orderDetail.IdSp}, " +
                                             $"Quantity: {orderDetail.Soluongsanpham}, " +
                                             $"Price: {orderDetail.Dongia}");
                        }

                        // Lưu chi tiết đơn hàng
                        await _context.SaveChangesAsync();
                        
                        // Kiểm tra xác nhận sau khi lưu
                        var savedDetails = await _context.Chitietdonhangs
                            .Where(c => c.IdDh == orderId)
                            .Select(c => new { // Chỉ select các trường cần thiết
                                c.Idchitietdonhang,
                                c.IdDh,
                                c.IdSp,
                                c.Soluongsanpham,
                                c.Dongia
                            })
                            .ToListAsync();

                        Console.WriteLine($"Saved order details count: {savedDetails.Count}");
                        foreach (var detail in savedDetails)
                        {
                            Console.WriteLine($"Saved detail - ID: {detail.Idchitietdonhang}, " +
                                             $"Order ID: {detail.IdDh}, " +
                                             $"Product ID: {detail.IdSp}, " +
                                             $"Quantity: {detail.Soluongsanpham}, " +
                                             $"Price: {detail.Dongia}");
                        }

                        // Commit transaction
                        await transaction.CommitAsync();
                        Console.WriteLine($"Transaction committed successfully for order {orderId}");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"Error in transaction: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        throw;
                    }
                }

                return RedirectToAction("PaymentSuccess", "PaymentResult");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi đặt hàng: " + ex.Message);
                return View("Index", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderConfirmation()
        {
            var customerId = User.FindFirstValue("CustomerId");
            var viewModel = new OrderConfirmationViewModel();
            
            if (customerId != null)
            {
                var customer = await _context.Khachhangs
                    .Include(k => k.IdXephangvipNavigation)
                    .FirstOrDefaultAsync(k => k.IdKh == customerId);

                if (customer?.IdXephangvipNavigation != null)
                {
                    viewModel.VipDiscountPercentage = customer.IdXephangvipNavigation.Phantramgiamgia;
                    viewModel.VipRank = customer.IdXephangvipNavigation.Tenhang;
                }
            }

            return Json(viewModel);
        }
    }
}
