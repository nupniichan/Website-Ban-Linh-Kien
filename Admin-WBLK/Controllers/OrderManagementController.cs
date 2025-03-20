using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using System.Text.Json;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.Observers;
using Admin_WBLK.Models.Facades;
using Admin_WBLK.Models.Mementos;
using Admin_WBLK.Models.ChainOfResponsibility;
using Admin_WBLK.Models.Visitors;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Admin_WBLK.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly OrderFacade _orderFacade;
        private readonly IOrderSubject _orderSubject;
        
        // Memento Pattern - Caretaker để quản lý lịch sử trạng thái
        private readonly OrderCaretaker _orderCaretaker;
        
        // Chain of Responsibility Pattern - Chuỗi xử lý đơn hàng
        private readonly IOrderHandler _orderProcessingChain;

        public OrderManagementController(DatabaseContext context, ILogger<OrderLogger> logger)
        {
            _context = context;
            
            // Khởi tạo Strategy Pattern
            var orderFilterStrategy = new DefaultOrderFilterStrategy(context);
            
            // Khởi tạo Observer Pattern
            _orderSubject = new OrderManager();
            _orderSubject.Attach(new OrderLogger(logger));
            
            // Khởi tạo Facade Pattern
            _orderFacade = new OrderFacade(context, orderFilterStrategy);
            
            // Khởi tạo Memento Pattern
            _orderCaretaker = new OrderCaretaker();
            
            // Khởi tạo Chain of Responsibility Pattern
            var validationHandler = new OrderValidationHandler();
            var paymentHandler = new PaymentHandler(context);
            var inventoryHandler = new InventoryHandler(context);
            var notificationHandler = new NotificationHandler();
            
            _orderProcessingChain = validationHandler;
            validationHandler.SetNext(paymentHandler)
                             .SetNext(inventoryHandler)
                             .SetNext(notificationHandler);
        }

        // GET: OrderManagement
        [HttpGet]
        public async Task<IActionResult> Index(
            string searchString, 
            string trangThaiDonHang,
            string loaiSanPham,
            DateTime? tuNgay, 
            DateTime? denNgay, 
            int? pageNumber)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentTrangThaiDonHang"] = trangThaiDonHang;
            ViewData["CurrentLoaiSanPham"] = loaiSanPham;
            ViewData["CurrentTuNgay"] = tuNgay;
            ViewData["CurrentDenNgay"] = denNgay;

            // Tạo truy vấn cơ bản
            var query = _context.Donhangs
                .Include(d => d.Chitietdonhangs)
                .ThenInclude(c => c.IdSpNavigation)
                .AsQueryable();

            // Thêm điều kiện sắp xếp mặc định theo ngày đặt hàng mới nhất
            query = query.OrderByDescending(d => d.Ngaydathang);

            // Lọc theo trạng thái đơn hàng
            if (!string.IsNullOrEmpty(trangThaiDonHang))
            {
                query = query.Where(d => d.Trangthai == trangThaiDonHang);
            }

            // Lọc theo loại sản phẩm
            if (!string.IsNullOrEmpty(loaiSanPham))
            {
                query = query.Where(d => d.Chitietdonhangs.Any(c => c.IdSpNavigation.Loaisanpham == loaiSanPham));
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(d => d.Ngaydathang >= tuNgay.Value);
            }

            if (denNgay.HasValue)
            {
                var endOfDay = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.Ngaydathang <= endOfDay);
            }

            // Xử lý tìm kiếm theo mã đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                
                // Tìm theo mã đơn hàng
                var orderIdQuery = query.Where(d => d.IdDh.ToLower().Contains(searchString));
                
                // Tìm theo tên khách hàng
                var customerNameQuery = _context.Khachhangs
                    .Where(k => k.Hoten.ToLower().Contains(searchString))
                    .Select(k => k.IdKh);
                
                var customerOrdersQuery = query.Where(d => customerNameQuery.Contains(d.IdKh));
                
                // Kết hợp hai kết quả
                query = orderIdQuery.Union(customerOrdersQuery);
            }

            // Lấy danh sách trạng thái đềElàm dropdown filter
            ViewBag.TrangThais = new List<string>
            {
                "Chờ xác nhận",
                "Đã thanh toán",
                "Thanh toán không thành công",
                "Đã duyệt đơn",
                "Đang giao",
                "Giao thành công",
                "Không nhận hàng",
                "Hủy đơn",
                "Đã kết thúc"
            };

            try
            {
                var totalItems = await query.CountAsync();
          
            return View(paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            // Sử dụng Facade Pattern để tìm kiếm gợi ý
            var results = await _orderFacade.SearchSuggestions(term);
            return Json(results);
        }

        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Include(d => d.Thanhtoans)
                .Select(d => new Donhang
                {
                    IdDh = d.IdDh ?? "",
                    IdKh = d.IdKh ?? "",
                    Trangthai = d.Trangthai ?? "",
                    Tongtien = d.Tongtien,
                    Diachigiaohang = d.Diachigiaohang ?? "",
                    Ngaydathang = d.Ngaydathang,
                    Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "",
                    IdMgg = d.IdMgg,
                    Ghichu = d.Ghichu ?? "",
                    LydoHuy = d.LydoHuy ?? "",
                    IdKhNavigation = d.IdKhNavigation == null ? null : new Khachhang
                    {
                        IdKh = d.IdKhNavigation.IdKh ?? "",
                        Hoten = d.IdKhNavigation.Hoten ?? "",
                        Sodienthoai = d.IdKhNavigation.Sodienthoai ?? "",
                        Email = d.IdKhNavigation.Email,
                        Diachi = d.IdKhNavigation.Diachi ?? ""
                    },
                    IdMggNavigation = d.IdMggNavigation,
                    Chitietdonhangs = d.Chitietdonhangs.Select(c => new Chitietdonhang
                    {
                        IdDh = c.IdDh ?? "",
                        IdSp = c.IdSp ?? "",
                        Soluongsanpham = c.Soluongsanpham,
                        Dongia = c.Dongia,
                        IdSpNavigation = c.IdSpNavigation == null ? null : new Sanpham
                        {
                            IdSp = c.IdSpNavigation.IdSp ?? "",
                            Tensanpham = c.IdSpNavigation.Tensanpham ?? ""
                        }
                    }).ToList(),
                    Thanhtoans = d.Thanhtoans.Select(t => new Thanhtoan
                    {
                        IdTt = t.IdTt ?? "",
                        Trangthai = t.Trangthai ?? "",
                        Tienthanhtoan = t.Tienthanhtoan,
                        Ngaythanhtoan = t.Ngaythanhtoan,
                        Noidungthanhtoan = t.Noidungthanhtoan,
                        Mathanhtoan = t.Mathanhtoan,
                        IdDh = t.IdDh ?? ""
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(donhang);
        }

        // GET: OrderManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdKh,Diachigiaohang,Phuongthucthanhtoan,IdMgg,Ghichu,LydoHuy,Tongtien,Trangthai")]
            Donhang donhang,
            string chitietdonhangs,
            string? Mathanhtoan,
            string? NoiDungThanhToan)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Check payment info if online payment is used.
                if (donhang.Phuongthucthanhtoan != "COD")
                {
                    if (string.IsNullOrEmpty(Mathanhtoan))
                    {
                        throw new Exception("Vui lòng nhập mã thanh toán cho phương thức thanh toán online!");
                    }

                    var existingPayment = await _context.Thanhtoans
                        .FirstOrDefaultAsync(t => t.Mathanhtoan == Mathanhtoan);
                    if (existingPayment != null)
                    {
                        throw new Exception("Mã thanh toán này đã tồn tại trong hềEthống!");
                    }
                }

                // Generate a new order ID and set the order date.
                donhang.IdDh = await GenerateOrderId();
                donhang.Ngaydathang = DateTime.Now;

                // Add the order to the context.
                _context.Donhangs.Add(donhang);
                
                // Process order details.
                decimal originalTotal = 0;
                if (!string.IsNullOrEmpty(chitietdonhangs))
                {
                    var chitietList = JsonSerializer.Deserialize<List<Chitietdonhang>>(chitietdonhangs);
                    
                    // Tạo danh sách chi tiết đơn hàng mới đềEtránh trùng lặp
                    var newChitietList = new List<Chitietdonhang>();
                    
                    foreach (var chitiet in chitietList)
                    {
                        // Tạo ID mới cho mỗi chi tiết đơn hàng sử dụng phương thức an toàn
                        var newChitiet = new Chitietdonhang
                        {
                            Idchitietdonhang = await GenerateOrderDetailId(),
                            IdDh = donhang.IdDh,
                            IdSp = chitiet.IdSp,
                            Soluongsanpham = chitiet.Soluongsanpham,
                            Dongia = chitiet.Dongia
                        };

                        var product = await _context.Sanphams.FindAsync(chitiet.IdSp);
                        if (product == null)
                        {
                            throw new Exception($"Không tìm thấy sản phẩm {chitiet.IdSp}!");
                        }
                        if (product.Soluongton < chitiet.Soluongsanpham)
                        {
                            throw new Exception($"Sản phẩm {product.Tensanpham} còn {product.Soluongton} sản phẩm!");
                        }

                        // **Always use the product's price from the database**:
                        newChitiet.Dongia = product.Gia;
                        originalTotal += newChitiet.Dongia * newChitiet.Soluongsanpham;

                        // Update product inventory
                        product.Soluongton -= chitiet.Soluongsanpham;
                        product.Damuahang = product.Damuahang + chitiet.Soluongsanpham;
                        _context.Update(product);

                        // Add to the new list
                        newChitietList.Add(newChitiet);
                    }
                    
                    // Add all new order details to the context
                    await _context.Chitietdonhangs.AddRangeAsync(newChitietList);
                }

                // Save so we have order + details in DB before applying discounts.
                await _context.SaveChangesAsync();

                // Add payment info if needed.
                if (!string.IsNullOrEmpty(Mathanhtoan) && donhang.Phuongthucthanhtoan != "COD")
                {
                    var thanhtoan = new Thanhtoan
                    {
                        IdTt = await GeneratePaymentId(),
                        Mathanhtoan = Mathanhtoan,
                        Trangthai = "ChềExác nhận",
                        Tienthanhtoan = donhang.Tongtien, // Will be updated after discount
                        Ngaythanhtoan = DateTime.Now,
                        Noidungthanhtoan = NoiDungThanhToan ?? "",
                        IdDh = donhang.IdDh
                    };
                    _context.Thanhtoans.Add(thanhtoan);
                    await _context.SaveChangesAsync();
                }

                // --- Recalculate final price with VIP discount applied BEFORE discount code ---
                decimal vipDiscountPercentage = 0;
                if (!string.IsNullOrEmpty(donhang.IdKh))
                {
                    var customer = await _context.Khachhangs
                        .Include(k => k.IdXephangvipNavigation)
                        .FirstOrDefaultAsync(k => k.IdKh == donhang.IdKh);
                    if (customer?.IdXephangvipNavigation != null)
                    {
                        vipDiscountPercentage = customer.IdXephangvipNavigation.Phantramgiamgia;
                    }
                }
                decimal priceAfterVip = originalTotal * (1 - vipDiscountPercentage / 100);

                // Check for a discount code (stored in donhang.IdMgg).
                decimal discountCodePercentage = 0;
                if (!string.IsNullOrEmpty(donhang.IdMgg))
                {
                    var discountRecord = await _context.Magiamgia.FirstOrDefaultAsync(m =>
                        m.IdMgg == donhang.IdMgg &&
                        m.Ngaysudung <= DateOnly.FromDateTime(DateTime.Now) &&
                        m.Ngayhethan >= DateOnly.FromDateTime(DateTime.Now) &&
                        m.Soluong > 0);
                    if (discountRecord != null)
                    {
                        discountCodePercentage = discountRecord.Tilechietkhau;
                        // Reduce the available uses.
                        discountRecord.Soluong--;
                        _context.Magiamgia.Update(discountRecord);
                        await _context.SaveChangesAsync(); // Save discount usage
                    }
                }
                decimal finalPrice = priceAfterVip * (1 - discountCodePercentage / 100);

                // Update order's final total
                donhang.Tongtien = finalPrice;
                _context.Update(donhang);

                // IMPORTANT: Save again so finalPrice is persisted.
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["Success"] = "Tạo đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(donhang);
            }
        }

        private async Task<string> GenerateOrderId()
        {
            var lastOrder = await _context.Donhangs
                .OrderByDescending(d => Convert.ToInt32(d.IdDh.Substring(2)))
                .FirstOrDefaultAsync();

            if (lastOrder == null)
            {
                return "DH000001";
            }

            int lastNumber = Convert.ToInt32(lastOrder.IdDh.Substring(2));
            return $"DH{(lastNumber + 1):D6}";
        }

        private async Task<string> GeneratePaymentId()
        {
            string newId = "TT000001";
            var lastPayment = await _context.Thanhtoans
                .OrderByDescending(t => t.IdTt)
                .Select(t => t.IdTt)
                .FirstOrDefaultAsync();

            if (lastPayment != null)
            {
                int lastNumber = int.Parse(lastPayment.Substring(2));
                newId = $"TT{(lastNumber + 1):D6}";
            }

            return newId;
        }

        // API endpoints for AJAX calls
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    Console.WriteLine("GetCustomerInfo: ID khách hàng trống");
                    return Json(null);
                }

                Console.WriteLine($"Searching for customer with ID: {id}");

                var customer = await _context.Khachhangs
                    .Where(k => k.IdKh == id)
                    .Select(k => new { k.IdKh, k.Hoten, vipDiscount = k.Diemtichluy >= 1000 ? 10 : (k.Diemtichluy >= 500 ? 5 : 0), vipRankName = k.Diemtichluy >= 1000 ? "VIP Gold" : (k.Diemtichluy >= 500 ? "VIP Silver" : "") })
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    Console.WriteLine($"Customer not found: {id}");
                    return Json(null);
                }

                Console.WriteLine($"Customer found: {customer.Hoten}");
                return Json(customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomerInfo: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Json(new { success = false, message = "Mã sản phẩm không được đềEtrống" });
                }

                Console.WriteLine($"Searching for product with ID: {id}");

                var product = await _context.Sanphams
                    .Where(s => s.IdSp == id)
                    .Select(s => new
                    {
                        s.IdSp,
                        s.Tensanpham,
                        s.Gia,
                        s.Soluongton,
                        success = true
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    Console.WriteLine($"Product not found: {id}");
                    return Json(new { success = false, message = $"Không tìm thấy sản phẩm với mã {id}" });
                }

                Console.WriteLine($"Product found: {product.Tensanpham}");
                return Json(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProductInfo: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi tìm sản phẩm" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscountInfo(string id, bool isEdit = false)
        {
            try
            {
                var discount = await _context.Magiamgia
                    .Where(m => m.IdMgg == id)
                    .FirstOrDefaultAsync();

                if (discount == null)
                {
                    return Json(new { success = false, message = "Mã giảm giá không tồn tại" });
                }

                if (isEdit)
                {
                    return Json(new
                    {
                        success = true,
                        tilechietkhau = discount.Tilechietkhau,
                        message = $"Áp dụng giảm giá {discount.Tilechietkhau}%"
                    });
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                if (today < discount.Ngaysudung)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Mã giảm giá chưa đến ngày sử dụng (Bắt đầu từ: {discount.Ngaysudung:dd/MM/yyyy})"
                    });
                }

                if (today > discount.Ngayhethan)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Mã giảm giá đã hết hạn (Hết hạn ngày: {discount.Ngayhethan:dd/MM/yyyy})"
                    });
                }

                if (discount.Soluong <= 0)
                {
                    return Json(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });
                }

                return Json(new
                {
                    success = true,
                    tilechietkhau = discount.Tilechietkhau,
                    message = $"Áp dụng giảm giá {discount.Tilechietkhau}% (Có hiệu lực đến {discount.Ngayhethan:dd/MM/yyyy})"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDiscountInfo: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi kiểm tra mã giảm giá" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffInfo(string id)
        {
            var staff = await _context.Nhanviens
                .Where(n => n.IdNv == id)
                .Select(n => new { n.IdNv, n.Hoten })
                .FirstOrDefaultAsync();
            return Json(staff);
        }

        // GET: OrderManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Include(d => d.Thanhtoans)
                .Select(d => new Donhang
                {
                    IdDh = d.IdDh ?? "",
                    IdKh = d.IdKh ?? "",
                    Trangthai = d.Trangthai ?? "",
                    Tongtien = d.Tongtien,
                    Diachigiaohang = d.Diachigiaohang ?? "",
                    Ngaydathang = d.Ngaydathang,
                    Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "",
                    IdMgg = d.IdMgg,
                    Ghichu = d.Ghichu ?? "",
                    LydoHuy = d.LydoHuy ?? "",
                    IdKhNavigation = d.IdKhNavigation == null ? null : new Khachhang
                    {
                        IdKh = d.IdKhNavigation.IdKh ?? "",
                        Hoten = d.IdKhNavigation.Hoten ?? "",
                        Sodienthoai = d.IdKhNavigation.Sodienthoai ?? "",
                        Email = d.IdKhNavigation.Email,
                        Diachi = d.IdKhNavigation.Diachi ?? ""
                    },
                    IdMggNavigation = d.IdMggNavigation,
                    Chitietdonhangs = d.Chitietdonhangs.Select(c => new Chitietdonhang
                    {
                        IdDh = c.IdDh ?? "",
                        IdSp = c.IdSp ?? "",
                        Soluongsanpham = c.Soluongsanpham,
                        Dongia = c.Dongia,
                        IdSpNavigation = c.IdSpNavigation == null ? null : new Sanpham
                        {
                            IdSp = c.IdSpNavigation.IdSp ?? "",
                            Tensanpham = c.IdSpNavigation.Tensanpham ?? ""
                        }
                    }).ToList(),
                    Thanhtoans = d.Thanhtoans.Select(t => new Thanhtoan
                    {
                        IdTt = t.IdTt ?? "",
                        Trangthai = t.Trangthai ?? "",
                        Tienthanhtoan = t.Tienthanhtoan,
                        Ngaythanhtoan = t.Ngaythanhtoan,
                        Noidungthanhtoan = t.Noidungthanhtoan,
                        Mathanhtoan = t.Mathanhtoan,
                        IdDh = t.IdDh ?? ""
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: OrderManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Sử dụng Facade Pattern để xóa đơn hàng
                if (_orderFacade != null)
                {
                    var success = await _orderFacade.DeleteOrder(id);
                    
                    if (success)
                    {
                        // Sử dụng Observer Pattern để thông báo xóa
                        if (_orderSubject != null)
                        {
                            var order = new Donhang { IdDh = id };
                            await _orderSubject.NotifyObservers(order, "Deleted");
                        }
                        
                        TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không thể xóa đơn hàng!";
                    }
                }
                else
                {
                    // Fallback nếu _orderFacade là null
                    var donhang = await _context.Donhangs.FindAsync(id);
                    if (donhang != null)
                    {
                        _context.Donhangs.Remove(donhang);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa đơn hàng: {ex.Message}";
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool DonhangExists(string id)
        {
            return _context.Donhangs.Any(e => e.IdDh == id);
        }

        // GET: OrderManagement/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                    .ThenInclude(k => k.IdXephangvipNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Select(d => new Donhang
                {
                    IdDh = d.IdDh ?? "",
                    IdKh = d.IdKh ?? "",
                    Trangthai = d.Trangthai ?? "",
                    Tongtien = d.Tongtien,
                    Diachigiaohang = d.Diachigiaohang ?? "",
                    Ngaydathang = d.Ngaydathang,
                    Phuongthucthanhtoan = d.Phuongthucthanhtoan ?? "",
                    IdMgg = d.IdMgg,
                    Ghichu = d.Ghichu ?? "",
                    LydoHuy = d.LydoHuy ?? "",
                    IdKhNavigation = d.IdKhNavigation == null ? null : new Khachhang
                    {
                        IdKh = d.IdKhNavigation.IdKh ?? "",
                        Hoten = d.IdKhNavigation.Hoten ?? "",
                        Diemtichluy = d.IdKhNavigation.Diemtichluy,
                        IdXephangvipNavigation = d.IdKhNavigation.IdXephangvipNavigation == null ? null : new Xephangvip
                        {
                            Id = d.IdKhNavigation.IdXephangvipNavigation.Id,
                            Tenhang = d.IdKhNavigation.IdXephangvipNavigation.Tenhang ?? "",
                            Phantramgiamgia = d.IdKhNavigation.IdXephangvipNavigation.Phantramgiamgia,
                            Diemtoithieu = d.IdKhNavigation.IdXephangvipNavigation.Diemtoithieu,
                            Diemtoida = d.IdKhNavigation.IdXephangvipNavigation.Diemtoida
                        }
                    },
                    IdMggNavigation = d.IdMggNavigation,
                    Chitietdonhangs = d.Chitietdonhangs.Select(c => new Chitietdonhang
                    {
                        IdDh = c.IdDh ?? "",
                        IdSp = c.IdSp ?? "",
                        Soluongsanpham = c.Soluongsanpham,
                        Dongia = c.Dongia,
                        IdSpNavigation = c.IdSpNavigation == null ? null : new Sanpham
                        {
                            IdSp = c.IdSpNavigation.IdSp ?? "",
                            Tensanpham = c.IdSpNavigation.Tensanpham ?? ""
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.IdDh == id);

            if (donhang == null)
            {
                return NotFound();
            }

            // Load payment information if available
            if (donhang.Phuongthucthanhtoan != "COD")
            {
                var thanhtoan = await _context.Thanhtoans
                    .Where(t => t.IdDh == id)
                    .OrderByDescending(t => t.Ngaythanhtoan)
                    .FirstOrDefaultAsync();

                if (thanhtoan != null)
                {
                    ViewBag.Mathanhtoan = thanhtoan.Mathanhtoan;
                    ViewBag.TrangthaiThanhtoan = thanhtoan.Trangthai;
                    ViewBag.Noidungthanhtoan = thanhtoan.Noidungthanhtoan;
                    ViewBag.NgayThanhToan = thanhtoan.Ngaythanhtoan;
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(donhang);
        }

        // POST: OrderManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            [Bind("IdDh,IdKh,Trangthai,Tongtien,Diachigiaohang,Ngaydathang,Phuongthucthanhtoan,IdMgg,Ghichu,LydoHuy")]
            Donhang donhang,
            string? Mathanhtoan,
            string? TrangthaiThanhtoan,
            string? NoiDungThanhToan,
            string returnUrl)
        {
            if (id != donhang.IdDh)
            {
                return NotFound();
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Remove validation for navigation properties.
                ModelState.Remove("IdKhNavigation");
                ModelState.Remove("IdMggNavigation");
                ModelState.Remove("Chitietdonhangs");

                // Retrieve the existing order and its details.
                var existingOrder = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (existingOrder == null)
                {
                    return NotFound();
                }
                
                // Memento Pattern - Lưu trạng thái cũ trước khi cập nhật
                _orderCaretaker.SaveState(id, existingOrder.Trangthai, User.Identity?.Name ?? "Admin");

                // If the new status is not "Hủy đơn", clear the cancellation reason.
                if (donhang.Trangthai != "Hủy đơn")
                {
                    donhang.LydoHuy = null;
                }

                // Update order info.
                donhang.Chitietdonhangs = existingOrder.Chitietdonhangs;
                _context.Entry(existingOrder).State = EntityState.Detached;
                _context.Update(donhang);

                // Process payment info.
                if (donhang.Phuongthucthanhtoan != "COD" && !string.IsNullOrEmpty(Mathanhtoan))
                {
                    var thanhtoan = await _context.Thanhtoans
                        .FirstOrDefaultAsync(t => t.IdDh == id);

                    if (thanhtoan == null)
                    {
                        thanhtoan = new Thanhtoan
                        {
                            IdTt = await GeneratePaymentId(),
                            Mathanhtoan = Mathanhtoan,
                            IdDh = id,
                            Trangthai = TrangthaiThanhtoan ?? "ChềEthanh toán",
                            Tienthanhtoan = donhang.Tongtien,
                            Ngaythanhtoan = DateTime.Now,
                            Noidungthanhtoan = NoiDungThanhToan ?? ""
                        };
                        _context.Thanhtoans.Add(thanhtoan);
                    }
                    else
                    {
                        thanhtoan.Mathanhtoan = Mathanhtoan;
                        thanhtoan.Trangthai = TrangthaiThanhtoan ?? thanhtoan.Trangthai;
                        thanhtoan.Tienthanhtoan = donhang.Tongtien;
                        thanhtoan.Noidungthanhtoan = NoiDungThanhToan ?? thanhtoan.Noidungthanhtoan;
                        _context.Update(thanhtoan);
                    }
                }

                // --- Recalculate final price with VIP discount applied BEFORE discount code in Edit ---
                decimal originalTotal = donhang.Chitietdonhangs.Sum(item => item.Dongia * item.Soluongsanpham);

                // Lookup customer for VIP discount.
                decimal vipDiscountPercentage = 0;
                if (!string.IsNullOrEmpty(donhang.IdKh))
                {
                    var customer = await _context.Khachhangs
                        .Include(k => k.IdXephangvipNavigation)
                        .FirstOrDefaultAsync(k => k.IdKh == donhang.IdKh);
                    if (customer?.IdXephangvipNavigation != null)
                    {
                        vipDiscountPercentage = customer.IdXephangvipNavigation.Phantramgiamgia;
                    }
                }
                decimal priceAfterVip = originalTotal * (1 - vipDiscountPercentage / 100);

                // Check if a discount code was applied.
                decimal discountCodePercentage = 0;
                if (!string.IsNullOrEmpty(donhang.IdMgg))
                {
                    var discountRecord = await _context.Magiamgia.FirstOrDefaultAsync(m =>
                        m.IdMgg == donhang.IdMgg &&
                        m.Ngaysudung <= DateOnly.FromDateTime(DateTime.Now) &&
                        m.Ngayhethan >= DateOnly.FromDateTime(DateTime.Now));
                    if (discountRecord != null)
                    {
                        discountCodePercentage = discountRecord.Tilechietkhau;
                    }
                }
                decimal finalPrice = priceAfterVip * (1 - discountCodePercentage / 100);
                donhang.Tongtien = finalPrice;
                _context.Update(donhang);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(donhang);
            }
        }


        public class ChitietdonhangDTO
        {
            public string IdSp { get; set; }
            public int Soluong { get; set; }
            public decimal Dongia { get; set; }
        }

        // API endpoint đềElấy chi tiết đơn hàng
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(string id)
        {
            var details = await _context.Chitietdonhangs
                .Include(c => c.IdSpNavigation)
                .Where(c => c.IdDh == id)
                .Select(c => new
                {
                    idSp = c.IdSp,
                    tenSp = c.IdSpNavigation.Tensanpham,
                    soluong = c.Soluongsanpham,
                    dongia = c.Dongia
                })
                .ToListAsync();

            return Json(details);
        }

        // GET: OrderManagement/UpdateStatus/5
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(string id, string newStatus, string returnUrl)
        {
            if (id == null || newStatus == null)
            {
                return NotFound();
            }

            var order = await _context.Donhangs.FindAsync(id);
            if (order == null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var donhang = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (donhang == null)
                {
                    return NotFound();
                }

                // Dictionary các trạng thái hợp lềE
                var validTransitions = new Dictionary<string, string[]>
                {
                    { "ChềExác nhận", new[] { "Đã duyệt đơn", "Hủy đơn" } },
                    { "Thanh toán không thành công", new[] { "Hủy đơn" } },
                    { "Đã thanh toán", new[] { "Đang giao" } },
                    { "Đã duyệt đơn", new[] { "Đang giao" } },
                    { "Đang giao", new[] { "Giao thành công", "Không nhận hàng" } },
                    { "Giao thành công", Array.Empty<string>() },
                    { "Không nhận hàng", Array.Empty<string>() },
                    { "Hủy đơn", Array.Empty<string>() },
                    { "Đã kết thúc", Array.Empty<string>() }
                };

                if (!validTransitions.ContainsKey(donhang.Trangthai) ||
                    !validTransitions[donhang.Trangthai].Contains(newStatus))
                {
                    TempData["Error"] = $"Không thềEchuyển trạng thái từ '{donhang.Trangthai}' sang '{newStatus}'!";
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(Index));
                }

                // Xử lý logic bềEsung
                switch (newStatus)
                {
                    await _context.SaveChangesAsync();
                    await _orderSubject.NotifyObservers(order, "StatusUpdated");
                    TempData["SuccessMessage"] = "Đơn hàng đã được duyệt thành công!";
                }
                else
                {
                    await _context.SaveChangesAsync();
                    await _orderSubject.NotifyObservers(order, "StatusUpdated");
                    TempData["WarningMessage"] = $"Đơn hàng chuyển sang trạng thái {order.Trangthai}!";
                }
            }
            else
            {
                // Xử lý bình thường cho các trạng thái khác
                order.Trangthai = newStatus;
                _context.Update(order);
                await _context.SaveChangesAsync();
                await _orderSubject.NotifyObservers(order, "StatusUpdated");
                TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Add this helper method to generate a new order detail ID.
        private static readonly object _orderDetailLock = new object();
        private async Task<string> GenerateOrderDetailId()
        {
            lock (_orderDetailLock)
            {
                try
                {
                    // Lấy ID lớn nhất hiện tại từ cơ sềEdữ liệu
                    var lastDetail = _context.Chitietdonhangs
                        .OrderByDescending(c => c.Idchitietdonhang)
                        .Select(c => c.Idchitietdonhang)
                        .FirstOrDefault();

                    int lastNumber = 1;
                    if (lastDetail != null)
                    {
                        // Trích xuất sềEtừ ID cuối cùng và tăng lên 1
                        if (int.TryParse(lastDetail.Substring(4), out int num))
                        {
                            lastNumber = num + 1;
                        }
                    }

                    string newId = $"CTDH{lastNumber:D6}";
                    
                    // Kiểm tra xem ID mới có tồn tại không
                    bool exists = _context.Chitietdonhangs.Any(c => c.Idchitietdonhang == newId);
                    if (exists)
                    {
                        // Nếu ID đã tồn tại, tăng sềElên cho đến khi tìm được ID chưa sử dụng
                        do
                        {
                            lastNumber++;
                            newId = $"CTDH{lastNumber:D6}";
                            exists = _context.Chitietdonhangs.Any(c => c.Idchitietdonhang == newId);
                        } while (exists);
                    }

                    return newId;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tạo ID chi tiết đơn hàng: {ex.Message}", ex);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscountSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            var today = DateOnly.FromDateTime(DateTime.Now);
            var suggestions = await _context.Magiamgia
                .Where(m => (m.IdMgg.Contains(term) || m.Ten.Contains(term)) &&
                            m.Ngaysudung <= today &&
                            m.Ngayhethan >= today &&
                            m.Soluong > 0)
                .Take(5)
                .Select(m => new
                {
                    id = m.IdMgg,
                    ten = m.Ten,
                    tilechietkhau = m.Tilechietkhau,
                    ngayhethan = m.Ngayhethan.ToString("dd/MM/yyyy"),
                    soluong = m.Soluong
                })
                .ToListAsync();

            return Json(suggestions);
        }
        
        // Memento Pattern - API endpoint để lấy lịch sử trạng thái
        [HttpGet]
        public IActionResult GetOrderStateHistory(string id)
        {
            var history = _orderCaretaker.GetHistory(id);
            return Json(history);
        }

        // Visitor Pattern - Tạo hóa đơn
        [HttpGet]
        public async Task<IActionResult> GenerateInvoice(string id)
        {
            var order = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Include(d => d.Thanhtoans)
                .FirstOrDefaultAsync(m => m.IdDh == id);
                
            if (order == null)
            {
                return NotFound();
            }
            
            // Sử dụng Visitor Pattern
            var invoiceVisitor = new InvoiceGeneratorVisitor();
            OrderExtensions.Accept(order, invoiceVisitor);
            
            string invoiceContent = invoiceVisitor.GetInvoice(order);
            
            return Json(new { success = true, invoice = invoiceContent });
        }
        
        // Visitor Pattern - Tạo báo cáo
        [HttpGet]
        public async Task<IActionResult> GenerateReport(string id)
        {
            var order = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Include(d => d.Thanhtoans)
                .FirstOrDefaultAsync(m => m.IdDh == id);
                
            if (order == null)
            {
                return NotFound();
            }
            
            // Sử dụng Visitor Pattern
            var reportVisitor = new OrderReportVisitor();
            OrderExtensions.Accept(order, reportVisitor);
            
            string reportContent = reportVisitor.GetReport(order);
            
            return Json(new { success = true, report = reportContent });
        }
        
        // Visitor Pattern - Xuất JSON
        [HttpGet]
        public async Task<IActionResult> ExportJson(string id)
        {
            var order = await _context.Donhangs
                .Include(d => d.IdKhNavigation)
                .Include(d => d.IdMggNavigation)
                .Include(d => d.Chitietdonhangs)
                    .ThenInclude(c => c.IdSpNavigation)
                .Include(d => d.Thanhtoans)
                .FirstOrDefaultAsync(m => m.IdDh == id);
                
            if (order == null)
            {
                return NotFound();
            }
            
            // Sử dụng Visitor Pattern
            var jsonVisitor = new OrderJsonExportVisitor();
            OrderExtensions.Accept(order, jsonVisitor);
            
            string jsonData = jsonVisitor.GetJsonData();
            
            return Json(new { success = true, data = jsonData });
        }
    }
    
    // Chain of Responsibility Pattern - Interface xử lý đơn hàng
    public interface IOrderHandler
    {
        Task<bool> Handle(Donhang order);
        IOrderHandler SetNext(IOrderHandler handler);
    }
    
    // Chain of Responsibility Pattern - Lớp cơ sở xử lý đơn hàng
    public abstract class OrderHandler : IOrderHandler
    {
        protected IOrderHandler _nextHandler;
        
        public IOrderHandler SetNext(IOrderHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        
        public abstract Task<bool> Handle(Donhang order);
    }
    
    // Chain of Responsibility Pattern - Xử lý kiểm tra đơn hàng
    public class OrderValidationHandler : OrderHandler
    {
        public override async Task<bool> Handle(Donhang order)
        {
            // Kiểm tra tính hợp lệ của đơn hàng
            if (order == null || string.IsNullOrEmpty(order.IdDh))
            {
                return false;
            }
            
            return _nextHandler != null ? await _nextHandler.Handle(order) : true;
        }
    }
    
    // Chain of Responsibility Pattern - Xử lý thanh toán
    public class PaymentHandler : OrderHandler
    {
        private readonly DatabaseContext _context;
        
        public PaymentHandler(DatabaseContext context)
        {
            _context = context;
        }
        
        public override async Task<bool> Handle(Donhang order)
        {
            // Xử lý thanh toán
            if (order.Phuongthucthanhtoan == "COD")
            {
                // Đơn hàng COD không cần xử lý thanh toán trước
                return _nextHandler != null ? await _nextHandler.Handle(order) : true;
            }
            
            // Kiểm tra thanh toán
            var payment = await _context.Thanhtoans
                .FirstOrDefaultAsync(t => t.IdDh == order.IdDh);
                
            if (payment == null || payment.Trangthai != "Đã thanh toán")
            {
                // Chưa thanh toán, không thể tiếp tục
                order.Trangthai = "Chờ thanh toán";
                return false;
            }
            
            return _nextHandler != null ? await _nextHandler.Handle(order) : true;
        }
    }
    
    // Chain of Responsibility Pattern - Xử lý tồn kho
    public class InventoryHandler : OrderHandler
    {
        private readonly DatabaseContext _context;
        
        public InventoryHandler(DatabaseContext context)
        {
            _context = context;
        }
        
        public override async Task<bool> Handle(Donhang order)
        {
            // Kiểm tra tồn kho
            var orderDetails = await _context.Chitietdonhangs
                .Where(c => c.IdDh == order.IdDh)
                .ToListAsync();
                
            foreach (var detail in orderDetails)
            {
                var product = await _context.Sanphams.FindAsync(detail.IdSp);
                if (product == null || product.Soluongton < detail.Soluongsanpham)
                {
                    // Không đủ hàng
                    order.Trangthai = "Chờ hàng";
                    return false;
                }
            }
            
            // Đủ hàng, cập nhật trạng thái
            order.Trangthai = "Đã duyệt đơn";
            
            return _nextHandler != null ? await _nextHandler.Handle(order) : true;
        }
    }
    
    // Chain of Responsibility Pattern - Xử lý thông báo
    public class NotificationHandler : OrderHandler
    {
        public override async Task<bool> Handle(Donhang order)
        {
            // Xử lý thông báo (có thể gửi email, SMS, v.v.)
            Console.WriteLine($"Đã gửi thông báo cho đơn hàng {order.IdDh}");
            
            return _nextHandler != null ? await _nextHandler.Handle(order) : true;
        }
    }
}
