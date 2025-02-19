using Microsoft.AspNetCore.Mvc;
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
            var query = _context.Sanphams.Where(p => p.Loaisanpham == "PC");

            // Lọc theo nhu cầu sử dụng
            if (!string.IsNullOrEmpty(usage))
            {
                var usageValue = usage.Trim();
                query = query.Where(p => p.Thongsokythuat.Contains(usageValue));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.Contains(brand));
            }

            // Lọc theo CPU
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.Thongsokythuat.Contains(cpuType));
            }

            // Lọc theo RAM
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.Thongsokythuat.Contains(ram));
            }

            // Lọc theo GPU
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.Thongsokythuat.Contains(gpu));
            }

            // Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange.ToLower())
                {
                    case "5-15-trieu":
                        query = query.Where(p => p.Gia >= 5000000 && p.Gia <= 15000000);
                        break;
                    case "15-20-trieu":
                        query = query.Where(p => p.Gia >= 15000000 && p.Gia <= 20000000);
                        break;
                    case "20-30-trieu":
                        query = query.Where(p => p.Gia >= 20000000 && p.Gia <= 30000000);
                        break;
                    case "30-50-trieu":
                        query = query.Where(p => p.Gia >= 30000000 && p.Gia <= 50000000);
                        break;
                    case "50-100-trieu":
                        query = query.Where(p => p.Gia >= 50000000 && p.Gia <= 100000000);
                        break;
                    case "tren-100-trieu":
                        query = query.Where(p => p.Gia > 100000000);
                        break;
                }
            }
            var totalCount = await query.CountAsync();

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
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "laptop");

            // Lọc theo nhu cầu sử dụng
            if (!string.IsNullOrEmpty(usage))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"usage\":\"{usage}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo CPU
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"cpu\":\"{cpuType}\""));
            }

            // Lọc theo RAM
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"ram\":\"{ram}\""));
            }

            // Lọc theo GPU
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"gpu\":\"{gpu}\""));
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
            Dictionary<string, string> additionalFilters = null)
        {
            var query = _context.Sanphams.Where(p => p.Loaisanpham == "Components");

            // Lọc theo danh mục con (category)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo các bộ lọc bổ sung
            if (additionalFilters != null)
            {
                foreach (var filter in additionalFilters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        // Xử lý các bộ lọc theo category
                        switch (category?.ToLower())
                        {
                            case "cpu":
                                if (filter.Key == "cpuSeries")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"CPU Series\":\"{filter.Value}\""));
                                else if (filter.Key == "cores")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Cores\":\"{filter.Value}\""));
                                break;

                            case "vga":
                                if (filter.Key == "memory")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Memory\":\"{filter.Value}\""));
                                break;

                            case "mainboard":
                                if (filter.Key == "socket")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Socket\":\"{filter.Value}\""));
                                else if (filter.Key == "formFactor")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Form Factor\":\"{filter.Value}\""));
                                break;

                            case "ram":
                                if (filter.Key == "capacity")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Capacity\":\"{filter.Value}\""));
                                else if (filter.Key == "ramType")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"RAM Type\":\"{filter.Value}\""));
                                break;

                            case "psu":
                                if (filter.Key == "wattage")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Wattage\":\"{filter.Value}\""));
                                else if (filter.Key == "efficiency")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Efficiency\":\"{filter.Value}\""));
                                break;

                            case "case":
                                if (filter.Key == "caseSize")
                                    query = query.Where(p => p.Thongsokythuat.Contains($"\"Case Size\":\"{filter.Value}\""));
                                break;
                        }
                    }
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

            var products = await query.ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = additionalFilters ?? new Dictionary<string, string>()
            };

            return View(viewModel);
        }

        // Monitor Routes
        [Route("productslist/monitor")]
        public async Task<IActionResult> Monitor(string brand = null, string priceRange = null, int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "monitor");

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());

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
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "audio");

            // Lọc theo danh mục con (category)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"Danh mục \"") && 
                                        p.Thongsokythuat.Contains($"\"value\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"type\":\"{type}\""));
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
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "network");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo kết nối
            if (!string.IsNullOrEmpty(connection))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"connection\":\"{connection}\""));
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
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "peripherals");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"type\":\"{type}\""));
            }

            // Lọc theo kết nối
            if (!string.IsNullOrEmpty(connection))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"connection\":\"{connection}\""));
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
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "storage");

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"Danh mục\":\"{category}\""));
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Lọc theo dung lượng
            if (!string.IsNullOrEmpty(capacity))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"capacity\":\"{capacity}\""));
            }

            // Lọc theo loại
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Thongsokythuat.Contains($"\"type\":\"{type}\""));
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
