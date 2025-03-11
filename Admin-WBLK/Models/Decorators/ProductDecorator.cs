namespace Admin_WBLK.Models.Decorators
{
    /// <summary>
    /// Lớp trừu tượng ProductDecorator triển khai interface IProductComponent
    /// </summary>
    public abstract class ProductDecorator : IProductComponent
    {
        protected readonly IProductComponent _component;
        
        public ProductDecorator(IProductComponent component)
        {
            _component = component;
        }
        
        /// <summary>
        /// Lấy mã sản phẩm
        /// </summary>
        public virtual string GetId()
        {
            return _component.GetId();
        }
        
        /// <summary>
        /// Lấy tên sản phẩm
        /// </summary>
        public virtual string GetName()
        {
            return _component.GetName();
        }
        
        /// <summary>
        /// Lấy giá sản phẩm
        /// </summary>
        public virtual decimal GetPrice()
        {
            return _component.GetPrice();
        }
        
        /// <summary>
        /// Lấy mô tả sản phẩm
        /// </summary>
        public virtual string GetDescription()
        {
            return _component.GetDescription();
        }
    }
} 