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
                DeliveryMethod = DeliveryMethod.HomeDelivery,
                VipDiscountPercentage = 0  // default for non-VIP or guest
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                var customerId = User.FindFirstValue("CustomerId");
                if (!string.IsNullOrEmpty(customerId))
                {
                    var customer = await _context.Khachhangs
                        .Include(k => k.Giohangs)
                            .ThenInclude(g => g.Chitietgiohangs)
                                .ThenInclude(c => c.IdSpNavigation)
                        .Include(k => k.IdXephangvipNavigation) // Include VIP info
                        .FirstOrDefaultAsync(k => k.IdKh == customerId);

                    if (customer != null)
                    {
                        viewModel.CustomerId = customer.IdKh;
                        viewModel.ReceiverName = customer.Hoten;
                        viewModel.Email = customer.Email;
                        viewModel.ReceiverPhone = customer.Sodienthoai;

                        // Set VIP discount percentage if available
                        if (customer.IdXephangvipNavigation != null)
                        {
                            viewModel.VipDiscountPercentage = customer.IdXephangvipNavigation.Phantramgiamgia;
                        }

                        // Populate cart items...
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

                        // Process address...
                        if (!string.IsNullOrEmpty(customer.Diachi))
                        {
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

            // Optionally calculate TotalAmount here, e.g.
            // viewModel.TotalAmount = viewModel.Items.Sum(i => i.Price * i.Quantity);

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

                string customerId;
                Khachhang? loggedInCustomer = null;

                if (User.Identity?.IsAuthenticated == true)
                {
                    // Lấy CustomerId từ Claims cho khách hàng đã đăng nhập
                    customerId = User.FindFirstValue("CustomerId");
                    if (string.IsNullOrEmpty(customerId))
                    {
                        return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                    }

                    // Cập nhật model với thông tin từ giỏ hàng trong database
                    loggedInCustomer = await _context.Khachhangs
                        .Include(k => k.Giohangs)
                            .ThenInclude(g => g.Chitietgiohangs)
                                .ThenInclude(c => c.IdSpNavigation)
                        .Include(k => k.IdXephangvipNavigation) // include VIP info here
                        .FirstOrDefaultAsync(k => k.IdKh == customerId);


                    if (loggedInCustomer?.Giohangs != null)
                    {
                        var currentCart = loggedInCustomer.Giohangs
                            .OrderByDescending(g => g.Thoigiancapnhat)
                            .FirstOrDefault();

                        if (currentCart?.Chitietgiohangs != null)
                        {
                            model.Items = currentCart.Chitietgiohangs.Select(item => new CheckoutItemViewModel
                            {
                                ProductId = item.IdSp,
                                ProductName = item.IdSpNavigation.Tensanpham,
                                ImageUrl = item.IdSpNavigation.Hinhanh,
                                Quantity = item.Soluongsanpham,
                                Price = item.IdSpNavigation.Gia
                            }).ToList();
                        }
                    }
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

                    customerId = newCustomerId;
                }

                // Kiểm tra và tạo khách hàng mới nếu chưa tồn tại
                var customer = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                if (customer != null)
                {
                    // Chỉ cập nhật thông tin cơ bản của khách hàng, không cập nhật địa chỉ
                    customer.Hoten = model.ReceiverName;
                    customer.Sodienthoai = model.ReceiverPhone;
                    customer.Email = model.Email;
                    
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
                        Diachi = "", // Để trống hoặc có thể đặt một giá trị mặc định
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

                // Calculate original total and apply VIP discount if available
                decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                decimal discountPercentage = 0;
                if (loggedInCustomer != null && loggedInCustomer.IdXephangvipNavigation != null)
                {
                    discountPercentage = loggedInCustomer.IdXephangvipNavigation.Phantramgiamgia;
                }
                decimal totalAfterDiscount = originalTotal * (1 - discountPercentage / 100);

                // Tạo đơn hàng với địa chỉ giao hàng riêng
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
                    Ghichu = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note,
                    Tongtien = totalAfterDiscount
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Thêm đơn hàng trước
                        _context.Donhangs.Add(order);
                        await _context.SaveChangesAsync();

                        // Tạo chi tiết đơn hàng và cập nhật số lượng sản phẩm
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

                            // Lấy thông tin sản phẩm
                            var product = await _context.Sanphams.FindAsync(item.ProductId);
                            if (product == null)
                            {
                                throw new Exception($"Không tìm thấy sản phẩm có ID: {item.ProductId}");
                            }

                            // Kiểm tra số lượng tồn
                            if (product.Soluongton < item.Quantity)
                            {
                                throw new Exception($"Sản phẩm {product.Tensanpham} không đủ số lượng trong kho");
                            }

                            // Cập nhật số lượng tồn và số lượng đã mua
                            product.Soluongton -= item.Quantity;
                            product.Damuahang = product.Damuahang + item.Quantity;

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
                            
                            Console.WriteLine($"Updated product {product.IdSp}: " +
                                            $"New stock: {product.Soluongton}, " +
                                            $"Total purchased: {product.Damuahang}");
                        }

                        // Lưu tất cả thay đổi
                        await _context.SaveChangesAsync();
                        
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

                // Sau khi tạo đơn hàng thành công, xóa giỏ hàng nếu là khách hàng đã đăng nhập
                if (User.Identity?.IsAuthenticated == true && loggedInCustomer != null)
                {
                    var currentCart = loggedInCustomer.Giohangs
                        .OrderByDescending(g => g.Thoigiancapnhat)
                        .FirstOrDefault();

                    if (currentCart != null)
                    {
                        // Xóa chi tiết giỏ hàng
                        _context.Chitietgiohangs.RemoveRange(currentCart.Chitietgiohangs);
                        // Xóa giỏ hàng
                        _context.Giohangs.Remove(currentCart);
                        await _context.SaveChangesAsync();
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
                        Diachi = "", // Để trống hoặc có thể đặt một giá trị mặc định
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

                // Calculate original total and apply VIP discount if available
                decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                decimal discountPercentage = 0;
                // For guest orders, no VIP discount is applied
                if (existingCustomer != null && existingCustomer.IdXephangvipNavigation != null)
                {
                    discountPercentage = existingCustomer.IdXephangvipNavigation.Phantramgiamgia;
                }
                decimal totalAfterDiscount = originalTotal * (1 - discountPercentage / 100);

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
                    Ghichu = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note,
                    Tongtien = totalAfterDiscount
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Thêm đơn hàng trước
                        _context.Donhangs.Add(order);
                        await _context.SaveChangesAsync();

                        // Tạo chi tiết đơn hàng và cập nhật số lượng sản phẩm
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

                            // Lấy thông tin sản phẩm
                            var product = await _context.Sanphams.FindAsync(item.ProductId);
                            if (product == null)
                            {
                                throw new Exception($"Không tìm thấy sản phẩm có ID: {item.ProductId}");
                            }

                            // Kiểm tra số lượng tồn
                            if (product.Soluongton < item.Quantity)
                            {
                                throw new Exception($"Sản phẩm {product.Tensanpham} không đủ số lượng trong kho");
                            }

                            // Cập nhật số lượng tồn và số lượng đã mua
                            product.Soluongton -= item.Quantity;
                            product.Damuahang = product.Damuahang + item.Quantity;

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
                            
                            Console.WriteLine($"Updated product {product.IdSp}: " +
                                            $"New stock: {product.Soluongton}, " +
                                            $"Total purchased: {product.Damuahang}");
                        }

                        // Lưu tất cả thay đổi
                        await _context.SaveChangesAsync();
                        
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
