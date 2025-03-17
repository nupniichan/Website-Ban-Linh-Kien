using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Visitors
{
    // Interface cho visitor
    public interface IOrderVisitor
    {
        void Visit(Donhang order);
        void Visit(Chitietdonhang orderDetail);
        void Visit(Thanhtoan payment);
    }
    
    // Interface cho các đối tượng có thể được "visit"
    public interface IOrderElement
    {
        void Accept(IOrderVisitor visitor);
    }
    
    // Các phương thức mở rộng để áp dụng visitor mà không cần sửa đổi các lớp gốc
    public static class OrderExtensions
    {
        public static void Accept(this Donhang order, IOrderVisitor visitor)
        {
            visitor.Visit(order);
            
            if (order.Chitietdonhangs != null)
            {
                foreach (var detail in order.Chitietdonhangs)
                {
                    detail.Accept(visitor);
                }
            }
            
            if (order.Thanhtoans != null)
            {
                foreach (var payment in order.Thanhtoans)
                {
                    payment.Accept(visitor);
                }
            }
        }
        
        public static void Accept(this Chitietdonhang detail, IOrderVisitor visitor)
        {
            visitor.Visit(detail);
        }
        
        public static void Accept(this Thanhtoan payment, IOrderVisitor visitor)
        {
            visitor.Visit(payment);
        }
    }
} 