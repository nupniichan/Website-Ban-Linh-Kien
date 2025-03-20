namespace Admin_WBLK.Models.Decorators
{
    /// <summary>
    /// Lớp DiscountedProduct kế thừa từ ProductDecorator để thêm tính năng giảm giá
    /// </summary>
    public class DiscountedProduct : ProductDecorator
    {
        private readonly decimal _discountPercentage;
        
        public DiscountedProduct(IProductComponent component, decimal discountPercentage)
            : base(component)
        {
            _discountPercentage = discountPercentage;
        }
        
        /// <summary>
        /// Lấy tên sản phẩm với thông tin giảm giá
        /// </summary>
        public override string GetName()
        {
            return $"{base.GetName()} (Giảm {_discountPercentage}%)";
        }
        
        /// <summary>
        /// Lấy giá sản phẩm đã giảm
        /// </summary>
        public override decimal GetPrice()
        {
            decimal originalPrice = base.GetPrice();
            decimal discount = originalPrice * _discountPercentage / 100;
            return originalPrice - discount;
        }
        
        /// <summary>
        /// Lấy mô tả sản phẩm với thông tin giảm giá
        /// </summary>
        public override string GetDescription()
        {
            return $"{base.GetDescription()}\nSản phẩm đang được giảm giá {_discountPercentage}%.";
        }
    }
} 