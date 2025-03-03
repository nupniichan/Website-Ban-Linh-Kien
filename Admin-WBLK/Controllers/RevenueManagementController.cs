using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Controllers
{
    public class RevenueManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public RevenueManagementController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue(DateTime? fromDate, DateTime? toDate, string? paymentMethod)
        {
            var query = _context.Donhangs.AsQueryable();

            if (fromDate.HasValue)
            {
                // Đảm bảo lấy từ đầu ngày
                var fromDateStart = fromDate.Value.Date;
                query = query.Where(d => d.Ngaydathang >= fromDateStart);
            }
            
            if (toDate.HasValue)
            {
                // Đảm bảo lấy đến cuối ngày
                var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.Ngaydathang <= toDateEnd);
            }

            if (!string.IsNullOrEmpty(paymentMethod))
                query = query.Where(d => d.Phuongthucthanhtoan == paymentMethod);

            var result = await query
                .GroupBy(d => d.Phuongthucthanhtoan)
                .Select(g => new
                {
                    paymentMethod = g.Key,
                    totalAmount = g.Sum(d => d.Tongtien),
                    orderCount = g.Count(),
                    successOrderCount = g.Count(d => d.Trangthai == "Giao thành công")
                })
                .ToListAsync();

            // Tính tổng số đơn hàng thành công
            int totalSuccessOrders = result.Sum(r => r.successOrderCount);

            return Json(new { 
                revenueData = result,
                totalSuccessOrders = totalSuccessOrders
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 10, string? paymentMethod = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Log các tham số để debug
            Console.WriteLine($"GetOrders - paymentMethod: {paymentMethod}, fromDate: {fromDate}, toDate: {toDate}");
            
            var query = _context.Donhangs.AsQueryable();

            // Áp dụng bộ lọc
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query = query.Where(o => o.Phuongthucthanhtoan == paymentMethod);
                Console.WriteLine($"Filtering by payment method: {paymentMethod}");
            }

            if (fromDate.HasValue)
            {
                // Đảm bảo lấy từ đầu ngày
                var fromDateStart = fromDate.Value.Date;
                query = query.Where(o => o.Ngaydathang >= fromDateStart);
                Console.WriteLine($"Filtering from date: {fromDateStart}");
            }

            if (toDate.HasValue)
            {
                // Đảm bảo lấy đến cuối ngày
                var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.Ngaydathang <= toDateEnd);
                Console.WriteLine($"Filtering to date: {toDateEnd}");
            }

            // Đếm tổng số đơn hàng trước khi phân trang
            var totalCount = await query.CountAsync();
            Console.WriteLine($"Total count after filtering: {totalCount}");

            // Lấy dữ liệu phân trang
            var orders = await query
                .OrderByDescending(o => o.Ngaydathang)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new
                {
                    Id = o.IdDh,
                    CustomerName = o.IdKhNavigation.Hoten,
                    OrderDate = o.Ngaydathang,
                    PaymentMethod = o.Phuongthucthanhtoan,
                    TotalAmount = o.Tongtien,
                    Status = o.Trangthai
                })
                .ToListAsync();

            Console.WriteLine($"Returned orders count: {orders.Count}");
            
            return Json(new { orders, totalCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetail(string id)
        {
            try
            {
                // Log để debug
                Console.WriteLine($"GetOrderDetail - Order ID: {id}");
                
                var order = await _context.Donhangs
                    .Include(o => o.IdKhNavigation)
                    .Include(o => o.Chitietdonhangs)
                    .ThenInclude(od => od.IdSpNavigation)
                    .Include(o => o.Thanhtoans)
                    .Where(o => o.IdDh == id)
                    .Select(o => new
                    {
                        Id = o.IdDh,
                        OrderDate = o.Ngaydathang,
                        PaymentMethod = o.Phuongthucthanhtoan,
                        Status = o.Trangthai,
                        CustomerName = o.IdKhNavigation.Hoten,
                        CustomerEmail = o.IdKhNavigation.Email,
                        CustomerPhone = o.IdKhNavigation.Sodienthoai,
                        CustomerAddress = o.Diachigiaohang,
                        TotalAmount = o.Tongtien,
                        Items = o.Chitietdonhangs.Select(od => new
                        {
                            ProductName = od.IdSpNavigation.Tensanpham,
                            Price = od.Dongia,
                            Quantity = od.Soluongsanpham
                        }).ToList(),
                        Payment = o.Thanhtoans.Select(p => new
                        {
                            Id = p.IdTt,
                            Status = p.Trangthai,
                            Amount = p.Tienthanhtoan,
                            PaymentDate = p.Ngaythanhtoan,
                            Content = p.Noidungthanhtoan,
                            Code = p.Mathanhtoan
                        }).FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    Console.WriteLine($"GetOrderDetail - Order not found: {id}");
                    return NotFound();
                }

                // Log thông tin thanh toán để debug
                Console.WriteLine($"GetOrderDetail - Payment Method: {order.PaymentMethod}");
                Console.WriteLine($"GetOrderDetail - Has Payment: {order.Payment != null}");
                if (order.Payment != null)
                {
                    Console.WriteLine($"GetOrderDetail - Payment ID: {order.Payment.Id}");
                    Console.WriteLine($"GetOrderDetail - Payment Status: {order.Payment.Status}");
                }

                return Json(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderDetail - Exception: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
