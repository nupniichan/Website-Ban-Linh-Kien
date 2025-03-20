using Admin_WBLK.Models;

namespace Admin_WBLK.Models.States
{
    /// <summary>
    /// Lớp ProductContext để quản lý trạng thái sản phẩm
    /// </summary>
    public class ProductContext
    {
        private IProductState _state;
        private readonly Sanpham _product;
        
        public ProductContext(Sanpham product)
        {
            _product = product;
            
            // Thiết lập trạng thái ban đầu dựa trên số lượng tồn kho
            if (_product.Soluongton <= 0)
            {
                _state = new OutOfStockState();
            }
            else if (_product.Soluongton < 10)
            {
                _state = new LowStockState();
            }
            else
            {
                _state = new InStockState();
            }
        }
        
        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        public void ChangeState(IProductState state)
        {
            _state = state;
        }
        
        /// <summary>
        /// Lấy trạng thái hiện tại
        /// </summary>
        public IProductState GetState()
        {
            return _state;
        }
        
        /// <summary>
        /// Lấy tên trạng thái hiện tại
        /// </summary>
        public string GetStateName()
        {
            return _state.GetStateName();
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được cập nhật không
        /// </summary>
        public bool CanUpdate()
        {
            return _state.CanUpdate(_product);
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được xóa không
        /// </summary>
        public bool CanDelete()
        {
            return _state.CanDelete(_product);
        }
        
        /// <summary>
        /// Kiểm tra xem sản phẩm có thể được bán không
        /// </summary>
        public bool CanSell()
        {
            return _state.CanSell(_product);
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được cập nhật
        /// </summary>
        public void Update()
        {
            _state.HandleUpdate(this, _product);
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được xóa
        /// </summary>
        public void Delete()
        {
            _state.HandleDelete(this, _product);
        }
        
        /// <summary>
        /// Xử lý khi sản phẩm được bán
        /// </summary>
        public void Sell(int quantity)
        {
            _state.HandleSell(this, _product, quantity);
        }
    }
} 