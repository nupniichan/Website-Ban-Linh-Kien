using Admin_WBLK.Models;

namespace Admin_WBLK.Models.States
{
    /// <summary>
    /// Lớp InStockState triển khai interface IProductState cho trạng thái còn hàng
    /// </summary>
    public class InStockState : IProductState
    {
        /// <summary>
        /// Lấy tên trạng thái
        /// </summary>
        public string GetStateName()
        {
            return "Còn hàng";
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được cập nhật không
        /// </summary>
        public bool CanUpdate(Sanpham product)
        {
            return true;
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được xóa không
        /// </summary>
        public bool CanDelete(Sanpham product)
        {
            return true;
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được bán không
        /// </summary>
        public bool CanSell(Sanpham product)
        {
            return product.Soluongton > 0;
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được cập nhật
        /// </summary>
        public void HandleUpdate(ProductContext context, Sanpham product)
        {
            // Kiểm tra số lượng tồn kho sau khi cập nhật
            if (product.Soluongton <= 0)
            {
                context.ChangeState(new OutOfStockState());
            }
            else if (product.Soluongton < 10)
            {
                context.ChangeState(new LowStockState());
            }
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được xóa
        /// </summary>
        public void HandleDelete(ProductContext context, Sanpham product)
        {
            // Không cần xử lý đặc biệt
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được bán
        /// </summary>
        public void HandleSell(ProductContext context, Sanpham product, int quantity)
        {
            // Giảm số lượng tồn kho
            product.Soluongton -= quantity;
            
            // Tăng số lượng đã mua
            product.Damuahang = (product.Damuahang ?? 0) + quantity;
            
            // Kiểm tra số lượng tồn kho sau khi bán
            if (product.Soluongton <= 0)
            {
                context.ChangeState(new OutOfStockState());
            }
            else if (product.Soluongton < 10)
            {
                context.ChangeState(new LowStockState());
            }
        }
    }
} 