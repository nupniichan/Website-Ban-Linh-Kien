using System;
using System.Text;
using System.Collections.Generic;

namespace Admin_WBLK.Models.Visitors
{
    // Visitor để tạo hóa đơn
    public class InvoiceGeneratorVisitor : IOrderVisitor
    {
        private StringBuilder _invoice = new StringBuilder();
        private decimal _totalAmount = 0;
        private decimal _totalDiscount = 0;
        
        public void Visit(Donhang order)
        {
            _invoice.AppendLine("=================================================");
            _invoice.AppendLine("                  HÓA ĐƠN BÁN HÀNG                ");
            _invoice.AppendLine("=================================================");
            _invoice.AppendLine($"Mã đơn hàng: {order.IdDh}");
            _invoice.AppendLine($"Ngày đặt hàng: {order.Ngaydathang?.ToString("dd/MM/yyyy HH:mm:ss")}");
            _invoice.AppendLine($"Khách hàng: {order.IdKhNavigation?.Hoten}");
            _invoice.AppendLine($"Địa chỉ giao hàng: {order.Diachigiaohang}");
            _invoice.AppendLine($"Phương thức thanh toán: {order.Phuongthucthanhtoan}");
            _invoice.AppendLine("=================================================");
            _invoice.AppendLine("STT | Sản phẩm                | SL | Đơn giá     | Thành tiền");
            _invoice.AppendLine("-------------------------------------------------");
        }
        
        public void Visit(Chitietdonhang orderDetail)
        {
            string productName = orderDetail.IdSpNavigation?.Tensanpham ?? orderDetail.IdSp;
            decimal amount = orderDetail.Dongia * orderDetail.Soluongsanpham;
            _totalAmount += amount;
            
            _invoice.AppendLine($"{orderDetail.IdSp.PadRight(4)} | {productName.PadRight(24)} | {orderDetail.Soluongsanpham.ToString().PadRight(2)} | {orderDetail.Dongia.ToString("#,##0").PadRight(11)} | {amount.ToString("#,##0")}");
        }
        
        public void Visit(Thanhtoan payment)
        {
            _invoice.AppendLine("-------------------------------------------------");
            if (payment != null)
            {
                _invoice.AppendLine($"Mã thanh toán: {payment.Mathanhtoan}");
                _invoice.AppendLine($"Trạng thái thanh toán: {payment.Trangthai}");
                if (!string.IsNullOrEmpty(payment.Noidungthanhtoan))
                {
                    _invoice.AppendLine($"Nội dung thanh toán: {payment.Noidungthanhtoan}");
                }
            }
        }
        
        public string GetInvoice(Donhang order)
        {
            // Tính toán giảm giá
            decimal originalTotal = 0;
            foreach (var item in order.Chitietdonhangs)
            {
                originalTotal += item.Dongia * item.Soluongsanpham;
            }
            
            _totalDiscount = originalTotal - order.Tongtien;
            
            _invoice.AppendLine("=================================================");
            _invoice.AppendLine($"Tổng tiền hàng:                         {originalTotal.ToString("#,##0")} VNĐ");
            
            if (_totalDiscount > 0)
            {
                _invoice.AppendLine($"Giảm giá:                              {_totalDiscount.ToString("#,##0")} VNĐ");
            }
            
            _invoice.AppendLine($"Thành tiền:                            {order.Tongtien.ToString("#,##0")} VNĐ");
            _invoice.AppendLine("=================================================");
            _invoice.AppendLine("               Cảm ơn quý khách!                ");
            _invoice.AppendLine("=================================================");
            
            return _invoice.ToString();
        }
    }
    
    // Visitor để tạo báo cáo thống kê đơn hàng
    public class OrderReportVisitor : IOrderVisitor
    {
        private StringBuilder _report = new StringBuilder();
        private Dictionary<string, int> _productQuantities = new Dictionary<string, int>();
        private decimal _totalRevenue = 0;
        
        public void Visit(Donhang order)
        {
            _report.AppendLine("=================================================");
            _report.AppendLine("              BÁO CÁO CHI TIẾT ĐƠN HÀNG           ");
            _report.AppendLine("=================================================");
            _report.AppendLine($"Mã đơn hàng: {order.IdDh}");
            _report.AppendLine($"Ngày đặt hàng: {order.Ngaydathang?.ToString("dd/MM/yyyy HH:mm:ss")}");
            _report.AppendLine($"Trạng thái: {order.Trangthai}");
            _report.AppendLine($"Khách hàng: {order.IdKhNavigation?.Hoten} (Mã: {order.IdKh})");
            
            if (!string.IsNullOrEmpty(order.IdMgg))
            {
                _report.AppendLine($"Mã giảm giá: {order.IdMgg}");
            }
            
            _report.AppendLine("=================================================");
            _report.AppendLine("THÔNG TIN SẢN PHẨM:");
            _report.AppendLine("-------------------------------------------------");
        }
        
        public void Visit(Chitietdonhang orderDetail)
        {
            string productName = orderDetail.IdSpNavigation?.Tensanpham ?? orderDetail.IdSp;
            decimal amount = orderDetail.Dongia * orderDetail.Soluongsanpham;
            _totalRevenue += amount;
            
            _report.AppendLine($"- {productName} (Mã: {orderDetail.IdSp})");
            _report.AppendLine($"  Số lượng: {orderDetail.Soluongsanpham}");
            _report.AppendLine($"  Đơn giá: {orderDetail.Dongia.ToString("#,##0")} VNĐ");
            _report.AppendLine($"  Thành tiền: {amount.ToString("#,##0")} VNĐ");
            
            // Thống kê số lượng sản phẩm
            if (!_productQuantities.ContainsKey(orderDetail.IdSp))
            {
                _productQuantities[orderDetail.IdSp] = 0;
            }
            _productQuantities[orderDetail.IdSp] += orderDetail.Soluongsanpham;
        }
        
        public void Visit(Thanhtoan payment)
        {
            if (payment != null)
            {
                _report.AppendLine("=================================================");
                _report.AppendLine("THÔNG TIN THANH TOÁN:");
                _report.AppendLine("-------------------------------------------------");
                _report.AppendLine($"Mã thanh toán: {payment.Mathanhtoan}");
                _report.AppendLine($"Trạng thái thanh toán: {payment.Trangthai}");
                _report.AppendLine($"Ngày thanh toán: {payment.Ngaythanhtoan?.ToString("dd/MM/yyyy HH:mm:ss")}");
                if (!string.IsNullOrEmpty(payment.Noidungthanhtoan))
                {
                    _report.AppendLine($"Nội dung thanh toán: {payment.Noidungthanhtoan}");
                }
            }
        }
        
        public string GetReport(Donhang order)
        {
            _report.AppendLine("=================================================");
            _report.AppendLine("THỐNG KÊ:");
            _report.AppendLine("-------------------------------------------------");
            _report.AppendLine($"Tổng số sản phẩm: {order.Chitietdonhangs.Count}");
            _report.AppendLine($"Tổng doanh thu: {_totalRevenue.ToString("#,##0")} VNĐ");
            _report.AppendLine($"Tổng tiền sau giảm giá: {order.Tongtien.ToString("#,##0")} VNĐ");
            
            if (_totalRevenue > order.Tongtien)
            {
                decimal discountAmount = _totalRevenue - order.Tongtien;
                decimal discountPercentage = (discountAmount / _totalRevenue) * 100;
                _report.AppendLine($"Giảm giá: {discountAmount.ToString("#,##0")} VNĐ ({discountPercentage.ToString("0.##")}%)");
            }
            
            _report.AppendLine("=================================================");
            
            return _report.ToString();
        }
    }
    
    // Visitor để xuất dữ liệu đơn hàng dưới dạng JSON
    public class OrderJsonExportVisitor : IOrderVisitor
    {
        private Dictionary<string, object> _orderData = new Dictionary<string, object>();
        private List<Dictionary<string, object>> _orderDetails = new List<Dictionary<string, object>>();
        private Dictionary<string, object> _paymentData = new Dictionary<string, object>();
        
        public void Visit(Donhang order)
        {
            _orderData["id"] = order.IdDh;
            _orderData["customer_id"] = order.IdKh;
            _orderData["customer_name"] = order.IdKhNavigation?.Hoten;
            _orderData["status"] = order.Trangthai;
            _orderData["total_amount"] = order.Tongtien;
            _orderData["shipping_address"] = order.Diachigiaohang;
            _orderData["order_date"] = order.Ngaydathang?.ToString("yyyy-MM-dd HH:mm:ss");
            _orderData["payment_method"] = order.Phuongthucthanhtoan;
            _orderData["discount_code"] = order.IdMgg;
            _orderData["notes"] = order.Ghichu;
            _orderData["cancellation_reason"] = order.LydoHuy;
        }
        
        public void Visit(Chitietdonhang orderDetail)
        {
            var detailData = new Dictionary<string, object>
            {
                ["product_id"] = orderDetail.IdSp,
                ["product_name"] = orderDetail.IdSpNavigation?.Tensanpham,
                ["quantity"] = orderDetail.Soluongsanpham,
                ["price"] = orderDetail.Dongia,
                ["amount"] = orderDetail.Dongia * orderDetail.Soluongsanpham
            };
            
            _orderDetails.Add(detailData);
        }
        
        public void Visit(Thanhtoan payment)
        {
            if (payment != null)
            {
                _paymentData["payment_id"] = payment.IdTt;
                _paymentData["payment_code"] = payment.Mathanhtoan;
                _paymentData["status"] = payment.Trangthai;
                _paymentData["amount"] = payment.Tienthanhtoan;
                _paymentData["payment_date"] = payment.Ngaythanhtoan?.ToString("yyyy-MM-dd HH:mm:ss");
                _paymentData["description"] = payment.Noidungthanhtoan;
            }
        }
        
        public string GetJsonData()
        {
            var result = new Dictionary<string, object>(_orderData);
            result["order_details"] = _orderDetails;
            
            if (_paymentData.Count > 0)
            {
                result["payment"] = _paymentData;
            }
            
            return System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
} 