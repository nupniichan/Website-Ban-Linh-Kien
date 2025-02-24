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

            // Load valid discount codes (using DateOnly.FromDateTime for comparison)
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

                Console.WriteLine($"Processing order for {model.ReceiverName}");
                Console.WriteLine($"Items count: {model.Items.Count}");

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

                if (customer != null)
                {
                    customer.Hoten = model.ReceiverName;
                    customer.Sodienthoai = model.ReceiverPhone;
                    customer.Email = model.Email;
                    
                    _context.Khachhangs.Update(customer);
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

                    customer = new Khachhang
                    {
                        IdKh = newCustomerId,
                        Hoten = model.ReceiverName,
                        Email = model.Email,
                        Sodienthoai = model.ReceiverPhone,
                        Diachi = "",
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

                decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                decimal vipDiscountPercentage = 0;
                if (loggedInCustomer != null && loggedInCustomer.IdXephangvipNavigation != null)
                {
                    vipDiscountPercentage = loggedInCustomer.IdXephangvipNavigation.Phantramgiamgia;
                }
                decimal afterVipPrice = originalTotal * (1 - vipDiscountPercentage / 100);

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
                decimal afterDiscountCodePrice = afterVipPrice * (1 - discountCodePercentage / 100);
                decimal finalPrice = afterDiscountCodePrice;

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
                    Tongtien = finalPrice
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
                            .FirstOrDefaultAsync() ?? "CTDH0000000";

                        int detailStartId = int.Parse(lastDetailId.Substring(6)) + 1;

                        foreach (var item in model.Items)
                        {
                            if (string.IsNullOrEmpty(item.ProductId))
                            {
                                continue;
                            }

                            var product = await _context.Sanphams.FindAsync(item.ProductId);
                            if (product == null)
                            {
                                throw new Exception($"Không tìm thấy sản phẩm có ID: {item.ProductId}");
                            }

                            if (product.Soluongton < item.Quantity)
                            {
                                throw new Exception($"Sản phẩm {product.Tensanpham} không đủ số lượng trong kho");
                            }

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

                            Console.WriteLine($"Updated product {product.IdSp}: New stock: {product.Soluongton}, Total purchased: {product.Damuahang}");
                        }

                        await _context.SaveChangesAsync();
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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi đặt hàng: " + ex.Message });
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
