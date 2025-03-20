using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Composites
{
    // Builder class để xây dựng cấu trúc phân cấp doanh thu
    public class RevenueBuilder
    {
        private readonly DatabaseContext _context;
        
        public RevenueBuilder(DatabaseContext context)
        {
            _context = context;
        }
        
        public async Task<IRevenueComponent> BuildRevenueHierarchy(DateTime? fromDate, DateTime? toDate)
        {
            // Tạo root composite
            var root = new RevenueComposite("Tổng doanh thu");
            
            // Tạo composite cho từng phương thức thanh toán
            var paymentMethods = await _context.Donhangs
                .Where(d => d.Trangthai == "Giao thành công" || d.Trangthai == "Đã kết thúc")
                .Where(d => (!fromDate.HasValue || d.Ngaydathang >= fromDate) && 
                           (!toDate.HasValue || d.Ngaydathang <= toDate))
                .Select(d => d.Phuongthucthanhtoan)
                .Distinct()
                .ToListAsync();
                
            foreach (var method in paymentMethods)
            {
                var paymentMethodComposite = new RevenueComposite(method ?? "Không xác định");
                
                // Tính tổng doanh thu theo phương thức thanh toán
                var paymentMethodRevenue = await _context.Donhangs
                    .Where(d => d.Phuongthucthanhtoan == method && 
                               (d.Trangthai == "Giao thành công" || d.Trangthai == "Đã kết thúc"))
                    .Where(d => (!fromDate.HasValue || d.Ngaydathang >= fromDate) && 
                               (!toDate.HasValue || d.Ngaydathang <= toDate))
                    .SumAsync(d => d.Tongtien);
                    
                // Thêm leaf cho tổng doanh thu theo phương thức thanh toán
                paymentMethodComposite.Add(new RevenueLeaf("Tổng " + method, paymentMethodRevenue, method ?? "Không xác định"));
                
                // Tạo các leaf cho từng đơn hàng
                var orders = await _context.Donhangs
                    .Where(d => d.Phuongthucthanhtoan == method && 
                               (d.Trangthai == "Giao thành công" || d.Trangthai == "Đã kết thúc"))
                    .Where(d => (!fromDate.HasValue || d.Ngaydathang >= fromDate) && 
                               (!toDate.HasValue || d.Ngaydathang <= toDate))
                    .OrderByDescending(d => d.Tongtien)
                    .Take(5) // Chỉ lấy 5 đơn hàng có giá trị cao nhất
                    .ToListAsync();
                    
                foreach (var order in orders)
                {
                    paymentMethodComposite.Add(new RevenueLeaf(
                        $"Đơn hàng {order.IdDh}", 
                        order.Tongtien, 
                        $"Đơn hàng ngày {order.Ngaydathang?.ToString("dd/MM/yyyy")}"
                    ));
                }
                
                root.Add(paymentMethodComposite);
            }
            
            return root;
        }
    }
} 