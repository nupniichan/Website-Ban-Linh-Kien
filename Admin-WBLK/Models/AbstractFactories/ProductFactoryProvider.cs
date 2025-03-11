using Admin_WBLK.Models;

namespace Admin_WBLK.Models.AbstractFactories
{
    /// <summary>
    /// Lớp cung cấp factory phù hợp dựa trên loại sản phẩm
    /// </summary>
    public class ProductFactoryProvider
    {
        private readonly DatabaseContext _context;
        
        public ProductFactoryProvider(DatabaseContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Lấy factory phù hợp dựa trên loại sản phẩm
        /// </summary>
        public IProductAbstractFactory GetFactory(string productType)
        {
            // Hiện tại chỉ có một loại factory, nhưng trong tương lai có thể mở rộng thêm
            // Ví dụ: ElectronicProductFactory, AccessoryProductFactory, ComponentProductFactory
            return new ConcreteProductFactory(_context);
        }
    }
} 