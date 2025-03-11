namespace Admin_WBLK.Models.Decorators
{
    /// <summary>
    /// Interface định nghĩa các phương thức cơ bản của sản phẩm
    /// </summary>
    public interface IProductComponent
    {
        /// <summary>
        /// Lấy mã sản phẩm
        /// </summary>
        string GetId();
        
        /// <summary>
        /// Lấy tên sản phẩm
        /// </summary>
        string GetName();
        
        /// <summary>
        /// Lấy giá sản phẩm
        /// </summary>
        decimal GetPrice();
        
        /// <summary>
        /// Lấy mô tả sản phẩm
        /// </summary>
        string GetDescription();
    }
} 