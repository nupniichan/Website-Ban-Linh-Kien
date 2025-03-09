using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Commands;
using Admin_WBLK.Models.Factories;
using Admin_WBLK.Models.Strategis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Facades
{
    public class ProductFacade
    {
        private readonly DatabaseContext _context;
        private readonly IProductFactory _productFactory;
        private readonly IProductSearchStrategy _searchStrategy;
        private readonly IProductFilterStrategy _filterStrategy;
        private readonly IProductSortStrategy _sortStrategy;

        public ProductFacade(
            DatabaseContext context,
            IProductFactory productFactory,
            IProductSearchStrategy searchStrategy,
            IProductFilterStrategy filterStrategy,
            IProductSortStrategy sortStrategy)
        {
            _context = context;
            _productFactory = productFactory;
            _searchStrategy = searchStrategy;
            _filterStrategy = filterStrategy;
            _sortStrategy = sortStrategy;
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
            var command = new CreateProductCommand(
                _context, 
                product, 
                imageFile, 
                thongSoKyThuat, 
                _productFactory, 
                controller, 
                tempData);
                
            return await command.Execute();
        }

        public async Task<IActionResult> UpdateProduct(
            string id,
            Sanpham product, 
            IFormFile imageFile, 
            string thongSoKyThuat,
            Controller controller, 
            ITempDataDictionary tempData)
        {
            var command = new UpdateProductCommand(
                _context, 
                product, 
                id, 
                imageFile, 
                thongSoKyThuat, 
                _productFactory, 
                controller, 
                tempData);
                
            return await command.Execute();
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