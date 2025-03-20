using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Admin_WBLK.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.AbstractFactories
{
    /// <summary>
    /// Lớp cụ thể triển khai Abstract Factory cho việc tạo các loại sản phẩm
    /// </summary>
    public class ConcreteProductFactory : IProductAbstractFactory
    {
        private readonly DatabaseContext _context;
        
        public ConcreteProductFactory(DatabaseContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Tạo sản phẩm điện tử
        /// </summary>
        public async Task<Sanpham> CreateElectronicProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile)
        {
            // Tạo ID sản phẩm mới
            string productId = await GenerateProductId("DT");
            
            // Tạo đối tượng sản phẩm
            var product = new Sanpham
            {
                IdSp = productId,
                Tensanpham = name,
                Gia = price,
                Soluongton = quantity,
                Thuonghieu = brand,
                Mota = description,
                Thongsokythuat = specifications,
                Loaisanpham = "Điện tử",
                Hinhanh = await SaveImage(imageFile, productId),
                Soluotxem = 0,
                Damuahang = 0
            };
            
            return product;
        }
        
        /// <summary>
        /// Tạo sản phẩm phụ kiện
        /// </summary>
        public async Task<Sanpham> CreateAccessoryProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile)
        {
            // Tạo ID sản phẩm mới
            string productId = await GenerateProductId("PK");
            
            // Tạo đối tượng sản phẩm
            var product = new Sanpham
            {
                IdSp = productId,
                Tensanpham = name,
                Gia = price,
                Soluongton = quantity,
                Thuonghieu = brand,
                Mota = description,
                Thongsokythuat = specifications,
                Loaisanpham = "Phụ kiện",
                Hinhanh = await SaveImage(imageFile, productId),
                Soluotxem = 0,
                Damuahang = 0
            };
            
            return product;
        }
        
        /// <summary>
        /// Tạo sản phẩm linh kiện
        /// </summary>
        public async Task<Sanpham> CreateComponentProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile)
        {
            // Tạo ID sản phẩm mới
            string productId = await GenerateProductId("LK");
            
            // Tạo đối tượng sản phẩm
            var product = new Sanpham
            {
                IdSp = productId,
                Tensanpham = name,
                Gia = price,
                Soluongton = quantity,
                Thuonghieu = brand,
                Mota = description,
                Thongsokythuat = specifications,
                Loaisanpham = "Linh kiện",
                Hinhanh = await SaveImage(imageFile, productId),
                Soluotxem = 0,
                Damuahang = 0
            };
            
            return product;
        }
        
        /// <summary>
        /// Tạo ID sản phẩm mới
        /// </summary>
        private async Task<string> GenerateProductId(string prefix)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string newId;
            int counter = 1;
            
            do
            {
                newId = $"{prefix}{date}{counter:D4}";
                counter++;
            } while (await _context.Sanphams.AnyAsync(p => p.IdSp == newId));
            
            return newId;
        }
        
        /// <summary>
        /// Lưu hình ảnh sản phẩm
        /// </summary>
        private async Task<string> SaveImage(IFormFile imageFile, string productId)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "default.jpg";
            }
            
            string fileName = $"{productId}_{Path.GetFileName(imageFile.FileName)}";
            string directoryPath = Path.Combine("wwwroot/images/products");
            string filePath = Path.Combine(directoryPath, fileName);
            
            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            
            return fileName;
        }
    }
} 