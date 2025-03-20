namespace Admin_WBLK.Models.Decorators
{
    /// <summary>
    /// Lớp FeaturedProduct kế thừa từ ProductDecorator để thêm tính năng nổi bật
    /// </summary>
    public class FeaturedProduct : ProductDecorator
    {
        public FeaturedProduct(IProductComponent component)
            : base(component)
        {
        }
        
        /// <summary>
        /// Lấy tên sản phẩm với thông tin nổi bật
        /// </summary>
        public override string GetName()
        {
            return $"⭐ {base.GetName()} ⭐";
        }
        
        /// <summary>
        /// Lấy mô tả sản phẩm với thông tin nổi bật
        /// </summary>
        public override string GetDescription()
        {
            return $"{base.GetDescription()}\nSản phẩm nổi bật được đề xuất bởi cửa hàng.";
        }
    }
} 