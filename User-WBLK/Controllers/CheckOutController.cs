﻿    using Microsoft.AspNetCore.Mvc;
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
                    VipDiscountPercentage = 0,  // default for non-VIP or guest
                    Items = new List<CheckoutItemViewModel>()
                };

                // Load valid discount codes
                viewModel.AvailableDiscounts = await _context.Magiamgia
                    .Where(m => m.Ngaysudung <= DateOnly.FromDateTime(DateTime.Now) &&
                                m.Ngayhethan >= DateOnly.FromDateTime(DateTime.Now) &&
                                m.Soluong > 0)
                    .ToListAsync();

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

                            if (customer.IdXephangvipNavigation != null)
                            {
                                viewModel.VipDiscountPercentage = customer.IdXephangvipNavigation.Phantramgiamgia;
                            }

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

                    model.Items ??= new List<CheckoutItemViewModel>();

                    if (!model.Items.Any())
                    {
                        return Json(new { success = false, message = "Giỏ hàng trống" });
                    }

                    if (string.IsNullOrEmpty(model.ReceiverName) ||
                        string.IsNullOrEmpty(model.ReceiverPhone) ||
                        string.IsNullOrEmpty(model.Email))
                    {
                        return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin người nhận" });
                    }

                    string customerId;
                    Khachhang? loggedInCustomer = null;

                    if (User.Identity?.IsAuthenticated == true)
                    {
                        customerId = User.FindFirstValue("CustomerId");
                        if (string.IsNullOrEmpty(customerId))
                        {
                            return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                        }

                        loggedInCustomer = await _context.Khachhangs
                            .Include(k => k.Giohangs)
                                .ThenInclude(g => g.Chitietgiohangs)
                                    .ThenInclude(c => c.IdSpNavigation)
                            .Include(k => k.IdXephangvipNavigation)
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
                        var lastCustomerId = await _context.Khachhangs
                            .OrderByDescending(k => k.IdKh)
                            .Select(k => k.IdKh)
                            .FirstOrDefaultAsync();

                        int nextId = 1;
                        if (lastCustomerId != null)
                        {
                            if (int.TryParse(lastCustomerId.Substring(2), out int currentId))
                            {
                                nextId = currentId + 1;
                            }
                            else
                            {
                                Console.WriteLine($"Không thể parse ID khách hàng: {lastCustomerId}");
                                return Json(new { success = false, message = "Lỗi khi tạo mã khách hàng" });
                            }
                        }

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

                    // Check if customer already exists by email or phone
                    var customer = await _context.Khachhangs
                        .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                    if (customer == null)
                    {
                        // Create new customer
                        var lastCustomerId = await _context.Khachhangs
                            .OrderByDescending(k => k.IdKh)
                            .Select(k => k.IdKh)
                            .FirstOrDefaultAsync();

                        int nextId = 1;
                        if (lastCustomerId != null && int.TryParse(lastCustomerId.Substring(2), out int currentId))
                        {
                            nextId = currentId + 1;
                        }

                        string newCustomerId;
                        do {
                            newCustomerId = $"KH{nextId:D6}";
                            var idExists = await _context.Khachhangs.AnyAsync(k => k.IdKh == newCustomerId);
                            if (idExists) nextId++;
                        } while (await _context.Khachhangs.AnyAsync(k => k.IdKh == $"KH{nextId:D6}"));

                        customer = new Khachhang
                        {
                            IdKh = newCustomerId,
                            Hoten = model.ReceiverName,
                            Email = model.Email,
                            Sodienthoai = model.ReceiverPhone,
                            Diachi = model.DeliveryMethod == DeliveryMethod.HomeDelivery 
                                ? $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}"
                                : "",
                            IdXephangvip = "THANTHIET", // Default for new customers
                            Diemtichluy = 0
                        };

                        _context.Khachhangs.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    // Create new order ID
                    var lastOrderId = await _context.Donhangs
                        .OrderByDescending(d => d.IdDh)
                        .Select(d => d.IdDh)
                        .FirstOrDefaultAsync();

                    int nextOrderId = 1;
                    if (lastOrderId != null && int.TryParse(lastOrderId.Substring(2), out int currentOrderId))
                    {
                        nextOrderId = currentOrderId + 1;
                    }

                    string orderId = $"DH{nextOrderId:D6}";

                    // Calculate pricing: original total, VIP discount, and discount code
                    decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                    decimal vipDiscountPercentage = 0;
                    if (User.Identity?.IsAuthenticated == true && loggedInCustomer?.IdXephangvipNavigation != null)
                    {
                        vipDiscountPercentage = loggedInCustomer.IdXephangvipNavigation.Phantramgiamgia;
                    }
                    decimal priceAfterVip = originalTotal * (1 - vipDiscountPercentage / 100);

                    // Log the discount code received
                    Console.WriteLine($"Discount Code Received: {model.DiscountCode}");

                    decimal discountCodePercentage = 0;
                    if (!string.IsNullOrEmpty(model.DiscountCode))
                    {
                        var discountRecord = await _context.Magiamgia.FirstOrDefaultAsync(m =>
                            m.IdMgg == model.DiscountCode &&
                            m.Ngaysudung <= DateOnly.FromDateTime(DateTime.Now) &&
                            m.Ngayhethan >= DateOnly.FromDateTime(DateTime.Now) &&
                            m.Soluong > 0);

                        if (discountRecord != null)
                        {
                            discountCodePercentage = discountRecord.Tilechietkhau;
                            discountRecord.Soluong--;
                            _context.Magiamgia.Update(discountRecord);
                            Console.WriteLine($"Found discount record. Percentage: {discountCodePercentage}%");
                        }
                        else
                        {
                            Console.WriteLine("No valid discount record found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No discount code provided.");
                    }
                    decimal finalPrice = priceAfterVip * (1 - discountCodePercentage / 100);

                    // Log computed values for debugging
                    Console.WriteLine($"Original Total: {originalTotal}");
                    Console.WriteLine($"VIP Discount Percentage: {vipDiscountPercentage}%");
                    Console.WriteLine($"Price After VIP Discount: {priceAfterVip}");
                    Console.WriteLine($"Discount Code Percentage: {discountCodePercentage}%");
                    Console.WriteLine($"Final Price (to be saved): {finalPrice}");

                    if (string.IsNullOrEmpty(model.DiscountCode)){
                        model.DiscountCode = null;
                    }
                    var order = new Donhang
                    {
                        IdDh = orderId,
                        IdKh = customer.IdKh,
                        Ngaydathang = DateTime.Now,
                        Diachigiaohang = model.DeliveryMethod == DeliveryMethod.StorePickup
                                            ? "123 Nguyễn Văn A, Quận 1, TP.HCM"
                                            : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                        Phuongthucthanhtoan = "COD",
                        Trangthai = "Chờ xác nhận",
                        Ghichu = model.Note,
                        Tongtien = finalPrice,  // Final price after discounts
                        IdMgg = model.DiscountCode // Save the discount code used
                    };


                    // Use a transaction for consistency
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            _context.Donhangs.Add(order);
                            await _context.SaveChangesAsync();

                            // Create order details
                            var lastDetailId = await _context.Chitietdonhangs
                                .OrderByDescending(c => c.Idchitietdonhang)
                                .Select(c => c.Idchitietdonhang)
                                .FirstOrDefaultAsync() ?? "CTDH00000";

                            int detailStartId = int.Parse(lastDetailId.Substring(4)) + 1;

                            foreach (var item in model.Items)
                            {
                                var product = await _context.Sanphams.FindAsync(item.ProductId);
                                if (product == null || product.Soluongton < item.Quantity)
                                {
                                    throw new Exception($"Sản phẩm {item.ProductName} không đủ số lượng trong kho");
                                }

                                product.Soluongton -= item.Quantity;
                                product.Damuahang += item.Quantity;

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
                            }

                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return Json(new { success = false, message = ex.Message });
                        }
                    }

                    // Clear the cart for logged in users
                    if (User.Identity?.IsAuthenticated == true && loggedInCustomer != null)
                    {
                        var currentCart = loggedInCustomer.Giohangs
                            .OrderByDescending(g => g.Thoigiancapnhat)
                            .FirstOrDefault();

                        if (currentCart != null)
                        {
                            _context.Chitietgiohangs.RemoveRange(currentCart.Chitietgiohangs);
                            _context.Giohangs.Remove(currentCart);
                            await _context.SaveChangesAsync();
                        }
                    }

                    TempData["OrderId"] = orderId;
                    return Json(new { success = true, redirectUrl = Url.Action("PaymentSuccess", "PaymentResult") });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
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

            [HttpPost]
            public async Task<IActionResult> CheckExistingCustomer([FromBody] CustomerCheckRequest model)
            {
                if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
                {
                    return Json(new { exists = false });
                }

                var customer = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => 
                        (!string.IsNullOrEmpty(model.Email) && k.Email == model.Email) || 
                        (!string.IsNullOrEmpty(model.Phone) && k.Sodienthoai == model.Phone));

                if (customer != null)
                {
                    string matchType = !string.IsNullOrEmpty(model.Email) && customer.Email == model.Email 
                        ? "email" 
                        : "số điện thoại";

                    return Json(new { 
                        exists = true, 
                        customer = new { 
                            name = customer.Hoten,
                            email = customer.Email,
                            phone = customer.Sodienthoai,
                            address = customer.Diachi,
                            matchType = matchType
                        } 
                    });
                }

                return Json(new { exists = false });
            }

            [HttpPost]
            public async Task<IActionResult> ProcessPayPalPayment([FromBody] CheckoutViewModel model)
            {
                try
                {
                    if (model == null)
                    {
                        return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
                    }

                    model.Items ??= new List<CheckoutItemViewModel>();

                    if (!model.Items.Any())
                    {
                        return Json(new { success = false, message = "Giỏ hàng trống" });
                    }

                    if (string.IsNullOrEmpty(model.ReceiverName) ||
                        string.IsNullOrEmpty(model.ReceiverPhone) ||
                        string.IsNullOrEmpty(model.Email))
                    {
                        return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin người nhận" });
                    }

                    string customerId;
                    Khachhang? loggedInCustomer = null;

                    if (User.Identity?.IsAuthenticated == true)
                    {
                        customerId = User.FindFirstValue("CustomerId");
                        if (string.IsNullOrEmpty(customerId))
                        {
                            return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                        }

                        loggedInCustomer = await _context.Khachhangs
                            .Include(k => k.Giohangs)
                                .ThenInclude(g => g.Chitietgiohangs)
                                    .ThenInclude(c => c.IdSpNavigation)
                            .Include(k => k.IdXephangvipNavigation)
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
                        var lastCustomerId = await _context.Khachhangs
                            .OrderByDescending(k => k.IdKh)
                            .Select(k => k.IdKh)
                            .FirstOrDefaultAsync();

                        int nextId = 1;
                        if (lastCustomerId != null)
                        {
                            if (int.TryParse(lastCustomerId.Substring(2), out int currentId))
                            {
                                nextId = currentId + 1;
                            }
                            else
                            {
                                Console.WriteLine($"Không thể parse ID khách hàng: {lastCustomerId}");
                                return Json(new { success = false, message = "Lỗi khi tạo mã khách hàng" });
                            }
                        }

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

                    var customer = await _context.Khachhangs
                        .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                    if (customer == null)
                    {
                        var lastCustomerId = await _context.Khachhangs
                            .OrderByDescending(k => k.IdKh)
                            .Select(k => k.IdKh)
                            .FirstOrDefaultAsync();

                        int nextId = 1;
                        if (lastCustomerId != null && int.TryParse(lastCustomerId.Substring(2), out int currentId))
                        {
                            nextId = currentId + 1;
                        }

                        string newCustomerId;
                        do {
                            newCustomerId = $"KH{nextId:D6}";
                            var idExists = await _context.Khachhangs.AnyAsync(k => k.IdKh == newCustomerId);
                            if (idExists) nextId++;
                        } while (await _context.Khachhangs.AnyAsync(k => k.IdKh == $"KH{nextId:D6}"));

                        customer = new Khachhang
                        {
                            IdKh = newCustomerId,
                            Hoten = model.ReceiverName,
                            Email = model.Email,
                            Sodienthoai = model.ReceiverPhone,
                            Diachi = model.DeliveryMethod == DeliveryMethod.HomeDelivery 
                                ? $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}"
                                : "",
                            IdXephangvip = "THANTHIET", // Default for new customers
                            Diemtichluy = 0
                        };

                        _context.Khachhangs.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    var lastOrderId = await _context.Donhangs
                        .OrderByDescending(d => d.IdDh)
                        .Select(d => d.IdDh)
                        .FirstOrDefaultAsync();

                    int nextOrderId = 1;
                    if (lastOrderId != null && int.TryParse(lastOrderId.Substring(2), out int currentOrderId))
                    {
                        nextOrderId = currentOrderId + 1;
                    }

                    string orderId = $"DH{nextOrderId:D6}";

                    decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                    decimal vipDiscountPercentage = 0;
                    if (User.Identity?.IsAuthenticated == true && loggedInCustomer?.IdXephangvipNavigation != null)
                    {
                        vipDiscountPercentage = loggedInCustomer.IdXephangvipNavigation.Phantramgiamgia;
                    }
                    decimal priceAfterVip = originalTotal * (1 - vipDiscountPercentage / 100);

                    decimal discountCodePercentage = 0;
                    if (!string.IsNullOrEmpty(model.DiscountCode))
                    {
                        var discountRecord = await _context.Magiamgia.FirstOrDefaultAsync(m =>
                            m.IdMgg == model.DiscountCode &&
                            m.Ngaysudung <= DateOnly.FromDateTime(DateTime.Now) &&
                            m.Ngayhethan >= DateOnly.FromDateTime(DateTime.Now) &&
                            m.Soluong > 0);

                        if (discountRecord != null)
                        {
                            discountCodePercentage = discountRecord.Tilechietkhau;
                            discountRecord.Soluong--;
                            _context.Magiamgia.Update(discountRecord);
                        }
                    }
                    decimal finalPrice = priceAfterVip * (1 - discountCodePercentage / 100);

                    if (string.IsNullOrEmpty(model.DiscountCode)){
                        model.DiscountCode = null;
                    }
                    
                    var order = new Donhang
                    {
                        IdDh = orderId,
                        IdKh = customer.IdKh,
                        Ngaydathang = DateTime.Now,
                        Diachigiaohang = model.DeliveryMethod == DeliveryMethod.StorePickup
                                            ? "123 Nguyễn Văn A, Quận 1, TP.HCM"
                                            : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                        Phuongthucthanhtoan = "PayPal",
                        Trangthai = "Chờ thanh toán",
                        Ghichu = model.Note,
                        Tongtien = finalPrice,
                        IdMgg = model.DiscountCode
                    };

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            _context.Donhangs.Add(order);
                            await _context.SaveChangesAsync();

                            var lastDetailId = await _context.Chitietdonhangs
                                .OrderByDescending(c => c.Idchitietdonhang)
                                .Select(c => c.Idchitietdonhang)
                                .FirstOrDefaultAsync() ?? "CTDH00000";

                            int detailStartId = int.Parse(lastDetailId.Substring(4)) + 1;

                            foreach (var item in model.Items)
                            {
                                var product = await _context.Sanphams.FindAsync(item.ProductId);
                                if (product == null || product.Soluongton < item.Quantity)
                                {
                                    throw new Exception($"Sản phẩm {item.ProductName} không đủ số lượng trong kho");
                                }

                                product.Soluongton -= item.Quantity;
                                product.Damuahang += item.Quantity;

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
                            }

                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return Json(new { success = false, message = ex.Message });
                        }
                    }

                    var paypalRequest = new PaypalPaymentRequest
                    {
                        OrderId = orderId,
                        Amount = finalPrice,
                        OrderInfo = $"Thanh toán đơn hàng {orderId}",
                        ShippingAddress = order.Diachigiaohang,
                        CartItems = JsonSerializer.Serialize(model.Items.Select(i => new { 
                            ProductId = i.ProductId, 
                            ProductName = i.ProductName, 
                            Quantity = i.Quantity, 
                            Price = i.Price 
                        }))
                    };

                    TempData["PayPalOrderData"] = JsonSerializer.Serialize(paypalRequest);

                    return Json(new { 
                        success = true, 
                        orderId = orderId,
                        paypalData = paypalRequest
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }

        public class CustomerCheckRequest
        {
            public string? Email { get; set; }
            public string? Phone { get; set; }
        }
    }