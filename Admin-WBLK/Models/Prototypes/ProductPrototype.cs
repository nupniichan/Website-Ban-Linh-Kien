using System;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Prototypes
{
    /// <summary>
    /// Lớp ProductPrototype triển khai interface IProductPrototype
    /// </summary>
    public class ProductPrototype : IProductPrototype
    {
        private readonly Sanpham _product;
        
        public ProductPrototype(Sanpham product)
        {
            _product = product;
        }
        
        /// <summary>
        /// Tạo một bản sao của sản phẩm
        /// </summary>
        public object Clone()
        {
            // Tạo một bản sao sâu của sản phẩm
            var clone = new Sanpham
            {
                IdSp = $"COPY_{_product.IdSp}",
                Tensanpham = $"Copy of {_product.Tensanpham}",
                Gia = _product.Gia,
                Soluongton = _product.Soluongton,
                Thuonghieu = _product.Thuonghieu,
                Mota = _product.Mota,
                Thongsokythuat = _product.Thongsokythuat,
                Loaisanpham = _product.Loaisanpham,
                Hinhanh = _product.Hinhanh,
                Soluotxem = 0,
                Damuahang = 0
            };
            
            return clone;
        }
        
        /// <summary>
        /// Tạo một bản sao của sản phẩm với giá mới
        /// </summary>
        public Sanpham CloneWithNewPrice(decimal newPrice)
        {
            var clone = (Sanpham)Clone();
            clone.Gia = newPrice;
            return clone;
        }
        
        /// <summary>
        /// Tạo một bản sao của sản phẩm với số lượng mới
        /// </summary>
        public Sanpham CloneWithNewQuantity(int newQuantity)
        {
            var clone = (Sanpham)Clone();
            clone.Soluongton = newQuantity;
            return clone;
        }
        
        /// <summary>
        /// Tạo một bản sao của sản phẩm với tên mới
        /// </summary>
        public Sanpham CloneWithNewName(string newName)
        {
            var clone = (Sanpham)Clone();
            clone.Tensanpham = newName;
            return clone;
        }
    }
} 