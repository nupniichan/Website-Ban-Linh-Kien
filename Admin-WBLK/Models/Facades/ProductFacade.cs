using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Commands;
using Admin_WBLK.Models.Factories;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.AbstractFactories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Admin_WBLK.Models.Facades
{
    public class ProductFacade
    {
        private readonly DatabaseContext _context;
        private readonly IProductFactory _productFactory;
        private readonly IProductSearchStrategy _searchStrategy;
        private readonly IProductFilterStrategy _filterStrategy;
        private readonly IProductSortStrategy _sortStrategy;
        private readonly ProductFactoryProvider _factoryProvider;

        public ProductFacade(
            DatabaseContext context,
            IProductFactory productFactory,
            IProductSearchStrategy searchStrategy,
            IProductFilterStrategy filterStrategy,
            IProductSortStrategy sortStrategy,
            ProductFactoryProvider factoryProvider)
        {
            _context = context;
            _productFactory = productFactory;
            _searchStrategy = searchStrategy;
            _filterStrategy = filterStrategy;
            _sortStrategy = sortStrategy;
            _factoryProvider = factoryProvider;
        }

        public async Task<PaginatedList<Sanpham>> GetProducts(
            string searchString, 
            string category, 
            string brand, 
            string sortOrder, 
            int pageNumber, 
            int pageSize)
        {
            // Tạo truy vấn cơ bản
            var query = _context.Sanphams.AsQueryable();
            
            // Áp dụng Strategy Pattern cho tìm kiếm
            query = _searchStrategy.Search(query, searchString);
            
            // Áp dụng Strategy Pattern cho sắp xếp
            query = _sortStrategy.Sort(query, sortOrder);

            // Lấy danh sách sản phẩm
            var items = await query.ToListAsync();
            
            // Áp dụng Strategy Pattern cho lọc
            var filteredItems = _filterStrategy.Filter(items, category, brand);
            
            // Phân trang
            var totalItems = filteredItems.Count();
            var pagedItems = filteredItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedList<Sanpham>(pagedItems, totalItems, pageNumber, pageSize);
        }

        public async Task<Sanpham> GetProductById(string id)
        {
            return await _context.Sanphams.FirstOrDefaultAsync(s => s.IdSp == id);
        }

        public async Task<IActionResult> CreateProduct(
            Sanpham product, 
            IFormFile imageFile, 
            string thongSoKyThuat,
            Controller controller, 
            ITempDataDictionary tempData)
        {
            try
            {
                // Sử dụng Abstract Factory Pattern để tạo sản phẩm dựa trên loại sản phẩm
                var factory = _factoryProvider.GetFactory(product.Loaisanpham);
                Sanpham createdProduct;
                
                switch (product.Loaisanpham)
                {
                    case "Điện tử":
                        createdProduct = await factory.CreateElectronicProduct(
                            product.Tensanpham,
                            product.Gia,
                            product.Soluongton,
                            product.Thuonghieu,
                            product.Mota,
                            thongSoKyThuat,
                            imageFile);
                        break;
                    case "Phụ kiện":
                        createdProduct = await factory.CreateAccessoryProduct(
                            product.Tensanpham,
                            product.Gia,
                            product.Soluongton,
                            product.Thuonghieu,
                            product.Mota,
                            thongSoKyThuat,
                            imageFile);
                        break;
                    case "Linh kiện":
                        createdProduct = await factory.CreateComponentProduct(
                            product.Tensanpham,
                            product.Gia,
                            product.Soluongton,
                            product.Thuonghieu,
                            product.Mota,
                            thongSoKyThuat,
                            imageFile);
                        break;
                    default:
                        // Sử dụng Command Pattern cho các loại sản phẩm khác
                        var command = new CreateProductCommand(_productFactory);
                        createdProduct = await command.Execute(
                            product.Tensanpham,
                            product.Gia,
                            product.Soluongton,
                            product.Thuonghieu,
                            product.Mota,
                            thongSoKyThuat,
                            product.Loaisanpham,
                            imageFile);
                        break;
                }
                
                // Lưu sản phẩm vào cơ sở dữ liệu
                _context.Sanphams.Add(createdProduct);
                await _context.SaveChangesAsync();
                
                tempData["Success"] = "Thêm sản phẩm thành công!";
                return controller.RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                tempData["Error"] = $"Lỗi: {ex.Message}";
                return controller.View(product);
            }
        }

        public async Task<IActionResult> UpdateProduct(
            string id,
            Sanpham product, 
            IFormFile imageFile, 
            string thongSoKyThuat,
            Controller controller, 
            ITempDataDictionary tempData)
        {
            try
            {
                // Tìm sản phẩm cần cập nhật
                var existingProduct = await _context.Sanphams.FindAsync(id);
                if (existingProduct == null)
                {
                    tempData["Error"] = "Không tìm thấy sản phẩm!";
                    return controller.NotFound();
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Tensanpham = product.Tensanpham;
                existingProduct.Gia = product.Gia;
                existingProduct.Soluongton = product.Soluongton;
                existingProduct.Thuonghieu = product.Thuonghieu;
                existingProduct.Mota = product.Mota;
                existingProduct.Thongsokythuat = thongSoKyThuat;
                existingProduct.Loaisanpham = product.Loaisanpham;

                // Xử lý hình ảnh nếu có
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Sử dụng factory để xử lý hình ảnh
                    var factory = _factoryProvider.GetFactory(product.Loaisanpham);
                    string fileName = await SaveImage(imageFile, existingProduct.IdSp);
                    existingProduct.Hinhanh = fileName;
                }

                // Lưu thay đổi
                _context.Update(existingProduct);
                await _context.SaveChangesAsync();

                tempData["Success"] = "Cập nhật sản phẩm thành công!";
                return controller.RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                tempData["Error"] = $"Lỗi: {ex.Message}";
                return controller.View(product);
            }
        }

        // Phương thức hỗ trợ để lưu hình ảnh
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

        public async Task<IActionResult> DeleteProduct(
            string id,
            Controller controller, 
            ITempDataDictionary tempData)
        {
            var command = new DeleteProductCommand(
                _context, 
                id, 
                controller, 
                tempData);
                
            return await command.Execute();
        }

        public async Task<List<string>> GetCategories()
        {
            var allProducts = await _context.Sanphams.ToListAsync();
            var categories = allProducts
                .Select(s =>
                {
                    if (s.Loaisanpham == "PC" || s.Loaisanpham == "Laptop" || s.Loaisanpham == "Monitor")
                        return s.Loaisanpham;

                    try
                    {
                        var specs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(s.Thongsokythuat);
                        if (specs != null)
                        {
                            if (specs.ContainsKey("Danh mục")) return specs["Danh mục"];
                            if (specs.ContainsKey("Loại ổ cứng")) return specs["Loại ổ cứng"];
                        }
                    }
                    catch { }
                    
                    return null;
                })
                .Where(c => c != null)
                .Distinct()
                .ToList();

            return categories;
        }

        public async Task<List<string>> GetBrands()
        {
            return await _context.Sanphams
                .Select(s => s.Thuonghieu)
                .Distinct()
                .ToListAsync();
        }
    }
} 