using Admin_WBLK.Models;

namespace Admin_WBLK.Models.States
{
    /// <summary>
    /// Lớp OutOfStockState triển khai interface IProductState cho trạng thái hết hàng
    /// </summary>
    public class OutOfStockState : IProductState
    {
        /// <summary>
        /// Lấy tên trạng thái
        /// </summary>
        public string GetStateName()
        {
            return "Hết hàng";
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
            return false; // Không thể bán khi hết hàng
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được cập nhật
        /// </summary>
        public void HandleUpdate(ProductContext context, Sanpham product)
        {
            // Kiểm tra số lượng tồn kho sau khi cập nhật
            if (product.Soluongton > 0 && product.Soluongton < 10)
            {
                context.ChangeState(new LowStockState());
            }
            else if (product.Soluongton >= 10)
            {
                context.ChangeState(new InStockState());
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
            // Không thể bán khi hết hàng
            throw new InvalidOperationException("Không thể bán sản phẩm đã hết hàng");
        }
    }
} 