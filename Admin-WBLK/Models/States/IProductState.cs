using Admin_WBLK.Models;

namespace Admin_WBLK.Models.States
{
    /// <summary>
    /// Interface định nghĩa các phương thức xử lý trạng thái sản phẩm
    /// </summary>
    public interface IProductState
    {
        /// <summary>
        /// Lấy tên trạng thái
        /// </summary>
        string GetStateName();
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được cập nhật không
        /// </summary>
        bool CanUpdate(Sanpham product);
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được xóa không
        /// </summary>
        bool CanDelete(Sanpham product);
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được bán không
        /// </summary>
        bool CanSell(Sanpham product);
        
        /// <summary>
        /// Xử lý khi sản phẩm được cập nhật
        /// </summary>
        void HandleUpdate(ProductContext context, Sanpham product);
        
        /// <summary>
        /// Xử lý khi sản phẩm được xóa
        /// </summary>
        void HandleDelete(ProductContext context, Sanpham product);
        
        /// <summary>
        /// Xử lý khi sản phẩm được bán
        /// </summary>
        void HandleSell(ProductContext context, Sanpham product, int quantity);
    }
} 