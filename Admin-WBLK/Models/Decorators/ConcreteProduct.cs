using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Decorators
{
    /// <summary>
    /// Lớp ConcreteProduct triển khai interface IProductComponent
    /// </summary>
    public class ConcreteProduct : IProductComponent
    {
        private readonly Sanpham _product;
        
        public ConcreteProduct(Sanpham product)
        {
            _product = product;
        }
        
        /// <summary>
        /// Lấy mã sản phẩm
        /// </summary>
        public string GetId()
        {
            return _product.IdSp;
        }
        
        /// <summary>
        /// Lấy tên sản phẩm
        /// </summary>
        public string GetName()
        {
            return _product.Tensanpham;
        }
        
        /// <summary>
        /// Lấy giá sản phẩm
        /// </summary>
        public decimal GetPrice()
        {
            return _product.Gia;
        }
        
        /// <summary>
        /// Lấy mô tả sản phẩm
        /// </summary>
        public string GetDescription()
        {
            return _product.Mota ?? "";
        }
    }
} 