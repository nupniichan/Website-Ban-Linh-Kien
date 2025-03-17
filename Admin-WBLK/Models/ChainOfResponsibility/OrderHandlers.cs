using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.ChainOfResponsibility
{
    // Xử lý kiểm tra đơn hàng
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
    
    // Xử lý thanh toán
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
    
    // Xử lý tồn kho
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
    
    // Xử lý thông báo
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