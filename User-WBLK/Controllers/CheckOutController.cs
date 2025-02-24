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
                if (model == null || !model.Items.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                if (string.IsNullOrEmpty(model.ReceiverName) || 
                    string.IsNullOrEmpty(model.ReceiverPhone) || 
                    string.IsNullOrEmpty(model.Email))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin người nhận" });
                }

                // Tạo hoặc lấy thông tin khách hàng
                var customer = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => k.Email == model.Email || k.Sodienthoai == model.ReceiverPhone);

                if (customer == null)
                {
                    // Tạo mã khách hàng mới
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
                        IdXephangvip = "THANTHIET", // Mặc định cho khách mới
                        Diemtichluy = 0
                    };

                    _context.Khachhangs.Add(customer);
                    await _context.SaveChangesAsync();
                }

                // Tạo mã đơn hàng mới
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

                // Tính toán giá tiền
                decimal originalTotal = model.Items.Sum(i => i.Price * i.Quantity);
                decimal finalPrice = originalTotal; // Không áp dụng giảm giá VIP cho guest

                // Tạo đơn hàng
                var order = new Donhang
                {
                    IdDh = orderId,
                    IdKh = customer.IdKh,
                    Ngaydathang = DateTime.Now,
                    Diachigiaohang = model.DeliveryMethod == DeliveryMethod.StorePickup 
                        ? STORE_ADDRESS 
                        : $"{model.StreetAddress}, {model.Ward}, {model.District}, {model.City}",
                    Phuongthucthanhtoan = "COD",
                    Trangthai = "Chờ xác nhận",
                    Ghichu = model.Note,
                    Tongtien = finalPrice
                };

                // Sử dụng transaction để đảm bảo tính nhất quán
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
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

                        TempData["OrderId"] = orderId;
                        return Json(new { success = true, redirectUrl = Url.Action("PaymentSuccess", "PaymentResult") });
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = ex.Message });
                    }
                }
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
    }
}
