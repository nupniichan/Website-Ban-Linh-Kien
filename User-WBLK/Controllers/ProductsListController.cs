﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProductsListController : Controller
    {
        private readonly DatabaseContext _context;
        private const int PageSize = 15;

        public ProductsListController(DatabaseContext context)
        {
            _context = context;
        }

        // PC Routes
        [Route("productslist/pc")]
        [Route("productslist/pc/{usage}/{brand}/{cpuType}/{ram}/{gpu}/{priceRange}")]
        public async Task<IActionResult> PC(string usage = null, string brand = null, 
            string cpuType = null, string ram = null, string gpu = null, 
            string priceRange = null, int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "pc");

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo nhu cầu sử dụng
            if (!string.IsNullOrEmpty(usage))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"usage\":\"{usage}\""));
            }

            // Lọc theo CPU
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"cpu\":\"{cpuType}\""));
            }

            // Lọc theo RAM
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"ram\":\"{ram}\""));
            }

            // Lọc theo GPU
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"gpu\":\"{gpu}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                var prices = priceRange.Split("-");
                if (prices.Length == 2)
                {
                    decimal minPrice = decimal.Parse(prices[0]) * 1000000;
                    decimal maxPrice = decimal.Parse(prices[1]) * 1000000;
                    query = query.Where(p => p.Gia >= minPrice && p.Gia <= maxPrice);
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Usage = usage,
                Brand = brand,
                CpuType = cpuType,
                Ram = ram,
                Gpu = gpu,
                PriceRange = priceRange
            };

            return View(viewModel);
        }

        // Laptop Routes
        [Route("productslist/laptop")]
        [Route("productslist/laptop/{usage}/{brand}/{cpuType}/{ram}/{gpu}/{priceRange}")]
        public async Task<IActionResult> Laptop(
            string usage = null,
            string brand = null,
            string cpuType = null,
            string ram = null,
            string gpu = null,
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "laptop");

            // Lọc theo nhu cầu sử dụng
            if (!string.IsNullOrEmpty(usage))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"usage\":\"{usage}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo CPU
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"cpu\":\"{cpuType}\""));
            }

            // Lọc theo RAM
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"ram\":\"{ram}\""));
            }

            // Lọc theo GPU
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"gpu\":\"{gpu}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-10-trieu":
                        query = query.Where(p => p.Gia < 10000000);
                        break;
                    case "10-15-trieu":
                        query = query.Where(p => p.Gia >= 10000000 && p.Gia <= 15000000);
                        break;
                    case "15-20-trieu":
                        query = query.Where(p => p.Gia >= 15000000 && p.Gia <= 20000000);
                        break;
                    case "20-25-trieu":
                        query = query.Where(p => p.Gia >= 20000000 && p.Gia <= 25000000);
                        break;
                    case "25-35-trieu":
                        query = query.Where(p => p.Gia >= 25000000 && p.Gia <= 35000000);
                        break;
                    case "35-trieu":
                        query = query.Where(p => p.Gia > 35000000);
                        break;
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Usage = usage,
                Brand = brand,
                CpuType = cpuType,
                Ram = ram,
                Gpu = gpu,
                PriceRange = priceRange
            };

            return View(viewModel);
        }

        // Components Routes
        [Route("productslist/components")]
        [Route("productslist/components/{category}")]
        public async Task<IActionResult> Components(
            string category = null, 
            string brand = null,
            string priceRange = null,
            string searchTerm = null,
            Dictionary<string, string> specifications = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "components");

            // Lọc theo danh mục con (category)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"Danh mục \"") && 
                                        p.ThongSoKyThuat.Contains($"\"value\":\"{category}\""));
            }

            // Debug: In ra câu query
            Console.WriteLine($"Category filter: {category}");
            
            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.TenSp.Contains(searchTerm));
            }

            // Lọc theo thông số kỹ thuật
            if (specifications != null && specifications.Any())
            {
                foreach (var spec in specifications)
                {
                    query = query.Where(p => p.ThongSoKyThuat.Contains($"\"key\":\"{spec.Key}\"") &&
                                           p.ThongSoKyThuat.Contains($"\"value\":\"{spec.Value}\""));
                }
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-2-trieu":
                        query = query.Where(p => p.Gia < 2000000);
                        break;
                    case "2-5-trieu":
                        query = query.Where(p => p.Gia >= 2000000 && p.Gia <= 5000000);
                        break;
                    case "5-10-trieu":
                        query = query.Where(p => p.Gia >= 5000000 && p.Gia <= 10000000);
                        break;
                    case "10-20-trieu":
                        query = query.Where(p => p.Gia >= 10000000 && p.Gia <= 20000000);
                        break;
                    case "20-50-trieu":
                        query = query.Where(p => p.Gia >= 20000000 && p.Gia <= 50000000);
                        break;
                    case "tren-50-trieu":
                        query = query.Where(p => p.Gia > 50000000);
                        break;
                }
            }

            // Debug: In ra số lượng sản phẩm tìm thấy trước khi phân trang
            var totalCount = await query.CountAsync();
            Console.WriteLine($"Tổng số sản phẩm tìm thấy: {totalCount}");

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = specifications ?? new Dictionary<string, string>()
            };

            return View(viewModel);
        }

        // Monitor Routes
        [Route("productslist/monitor")]
        public async Task<IActionResult> Monitor(string brand = null, string priceRange = null, int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "monitor");

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());

            if (!string.IsNullOrEmpty(priceRange))
            {
                var prices = priceRange.Split("-");
                if (prices.Length == 2)
                {
                    decimal minPrice = decimal.Parse(prices[0]) * 1000000;
                    decimal maxPrice = decimal.Parse(prices[1]) * 1000000;
                    query = query.Where(p => p.Gia >= minPrice && p.Gia <= maxPrice);
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Brand = brand,
                PriceRange = priceRange
            };

            return View(viewModel);
        }

        // Audio Routes
        [Route("productslist/audio")]
        [Route("productslist/audio/{category}/{brand}/{type}/{priceRange}")]
        public async Task<IActionResult> Audio(
            string category = null,
            string brand = null, 
            string type = null,
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "audio");

            // Lọc theo danh mục con (category)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"Danh mục \"") && 
                                        p.ThongSoKyThuat.Contains($"\"value\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-2-trieu":
                        query = query.Where(p => p.Gia < 2000000);
                        break;
                    case "2-5-trieu":
                        query = query.Where(p => p.Gia >= 2000000 && p.Gia <= 5000000);
                        break;
                    case "5-10-trieu":
                        query = query.Where(p => p.Gia >= 5000000 && p.Gia <= 10000000);
                        break;
                    case "10-20-trieu":
                        query = query.Where(p => p.Gia >= 10000000 && p.Gia <= 20000000);
                        break;
                    case "tren-20-trieu":
                        query = query.Where(p => p.Gia > 20000000);
                        break;
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "type", type }
                }
            };

            return View(viewModel);
        }

        // Network Routes
        [Route("productslist/network")]
        [Route("productslist/network/{category}")]
        public async Task<IActionResult> Network(
            string category = null,
            string brand = null,
            string type = null,
            string connection = null,
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "network");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo kết nối
            if (!string.IsNullOrEmpty(connection))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"connection\":\"{connection}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-500-nghin":
                        query = query.Where(p => p.Gia < 500000);
                        break;
                    case "500-1-trieu":
                        query = query.Where(p => p.Gia >= 500000 && p.Gia <= 1000000);
                        break;
                    case "1-2-trieu":
                        query = query.Where(p => p.Gia >= 1000000 && p.Gia <= 2000000);
                        break;
                    case "2-5-trieu":
                        query = query.Where(p => p.Gia >= 2000000 && p.Gia <= 5000000);
                        break;
                    case "tren-5-trieu":
                        query = query.Where(p => p.Gia > 5000000);
                        break;
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "type", type },
                    { "connection", connection }
                }
            };

            return View(viewModel);
        }

        // Peripherals Routes
        [Route("productslist/peripherals")]
        [Route("productslist/peripherals/{category}")]
        public async Task<IActionResult> Peripherals(
            string category = null,
            string brand = null,
            string type = null,
            string connection = null,
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "peripherals");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo kết nối
            if (!string.IsNullOrEmpty(connection))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"connection\":\"{connection}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-500-nghin":
                        query = query.Where(p => p.Gia < 500000);
                        break;
                    case "500-1-trieu":
                        query = query.Where(p => p.Gia >= 500000 && p.Gia <= 1000000);
                        break;
                    case "1-2-trieu":
                        query = query.Where(p => p.Gia >= 1000000 && p.Gia <= 2000000);
                        break;
                    case "2-5-trieu":
                        query = query.Where(p => p.Gia >= 2000000 && p.Gia <= 5000000);
                        break;
                    case "tren-5-trieu":
                        query = query.Where(p => p.Gia > 5000000);
                        break;
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "type", type },
                    { "connection", connection }
                }
            };

            return View(viewModel);
        }

        // Storage Routes
        [Route("productslist/storage")]
        [Route("productslist/storage/{category}")]
        public async Task<IActionResult> Storage(
            string category = null,
            string brand = null,
            string capacity = null,
            string type = null,
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.LoaiSp.ToLower() == "storage");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.ThuongHieu.ToLower() == brand.ToLower());
            }

            // Lọc theo dung lượng
            if (!string.IsNullOrEmpty(capacity))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"capacity\":\"{capacity}\""));
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.ThongSoKyThuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "duoi-1-trieu":
                        query = query.Where(p => p.Gia < 1000000);
                        break;
                    case "1-2-trieu":
                        query = query.Where(p => p.Gia >= 1000000 && p.Gia <= 2000000);
                        break;
                    case "2-5-trieu":
                        query = query.Where(p => p.Gia >= 2000000 && p.Gia <= 5000000);
                        break;
                    case "5-10-trieu":
                        query = query.Where(p => p.Gia >= 5000000 && p.Gia <= 10000000);
                        break;
                    case "tren-10-trieu":
                        query = query.Where(p => p.Gia > 10000000);
                        break;
                }
            }

            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Category = category,
                Brand = brand,
                Capacity = capacity,
                Type = type,
                PriceRange = priceRange
            };

            return View(viewModel);
        }

        // Helper Methods
        private async Task<List<Sanpham>> GetPagedProductsAsync(IQueryable<Sanpham> query, int page)
        {
            return await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        private async Task<int> GetTotalPagesAsync(IQueryable<Sanpham> query)
        {
            var totalItems = await query.CountAsync();
            return (int)Math.Ceiling(totalItems / (double)PageSize);
        }
    }
}
