using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Factories
{
    public class ProductFactory : IProductFactory
    {
        private readonly DatabaseContext _context;

        public ProductFactory(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Sanpham> CreateProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            string category, 
            IFormFile imageFile)
        {
            var product = new Sanpham
            {
                IdSp = await GenerateProductId(),
                Tensanpham = name,
                Gia = price,
                Soluongton = quantity,
                Thuonghieu = brand,
                Mota = description,
                Thongsokythuat = specifications,
                Loaisanpham = category,
                Soluotxem = 0,
                Damuahang = 0
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                product.Hinhanh = await ProcessImage(imageFile);
            }

            return product;
        }

        public async Task<string> GenerateProductId()
        {
            string newId = "SP00001";
            var lastProduct = await _context.Sanphams
                .OrderByDescending(p => p.IdSp)
                .Select(p => new { p.IdSp })
                .FirstOrDefaultAsync();

            if (lastProduct != null && !string.IsNullOrEmpty(lastProduct.IdSp))
            {
                int lastNumber = int.Parse(lastProduct.IdSp.Substring(2));
                newId = $"SP{(lastNumber + 1):D5}";
            }

            return newId;
        }

        public async Task<string> ProcessImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductImage");
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await imageFile.CopyToAsync(stream);

            return "/Images/ProductImage/" + fileName;
        }
    }
} 