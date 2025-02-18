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
            // Start with products whose Loaisanpham is "pc" (case-insensitive)
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "pc");

            // Filter by usage – in the PC JSON, usage is stored under "Nhu cầu"
            if (!string.IsNullOrEmpty(usage))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"nhu cầu\": \"{usage.ToLower()}"));
            }

            // Filter by CPU type using partial matching.
            // Removing the closing quote in the search string allows a filter of "intel core i9" to match "intel core i9-14900k"
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"cpu\": \"{cpuType.ToLower()}"));
            }

            // Filter by RAM specification using partial matching.
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"ram\": \"{ram.ToLower()}"));
            }

            // Filter by GPU specification.
            // Note: Many PC builds store the GPU under "VGA"
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"vga\": \"{gpu.ToLower()}"));
            }

            // Filter by price range with full options
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
                    // You can add more cases as needed.
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
            // Start with products whose Loaisanpham is "laptop" (case-insensitive)
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "laptop");

            // Filter by usage – assume the JSON stores it as "Nhu cầu"
            if (!string.IsNullOrEmpty(usage))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"nhu cầu\": \"{usage.ToLower()}"));
            }

            // Filter by brand (direct column match; case-insensitive)
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Filter by CPU using partial matching.
            if (!string.IsNullOrEmpty(cpuType))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"cpu\": \"{cpuType.ToLower()}"));
            }

            // Filter by RAM using partial matching.
            if (!string.IsNullOrEmpty(ram))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"ram\": \"{ram.ToLower()}"));
            }

            // Filter by GPU using partial matching.
            // (Many laptops store GPU under "gpu" in the JSON.)
            if (!string.IsNullOrEmpty(gpu))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"vga\": \"{gpu.ToLower()}"));
            }

            // Filter by price range
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
                    // Add more cases if necessary.
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
            string cpuSeries = null,
            string cores = null)
        {
            // Only consider products with Loaisanpham "Components"
            var query = _context.Sanphams.Where(p => p.Loaisanpham == "Components");

            // Filter by sub-category (e.g. "cpu")
            if (!string.IsNullOrEmpty(category))
            {
                // Use lower-case matching for robustness
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"{category.ToLower()}\""));
            }

            // Filter by brand (if provided)
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Build a dictionary for additional filters
            var additionalFilters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(cpuSeries))
                additionalFilters.Add("cpuSeries", cpuSeries);
            if (!string.IsNullOrEmpty(cores))
                additionalFilters.Add("cores", cores);

            // Apply additional filters based on the sub-category.
            // Here we assume that when category == "cpu", the JSON contains keys "Dòng CPU" and "Số nhân".
            if (additionalFilters != null)
            {
                foreach (var filter in additionalFilters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        switch (category?.ToLower())
                        {
                            case "cpu":
                                if (filter.Key == "cpuSeries")
                                {
                                    // If the user selected "Intel core", 
                                    // let's remove the trailing quote so we match "intel core i9", "intel core i7", etc.
                                    var searchString = $"\"dòng cpu\": \"{filter.Value.ToLower()}";
                                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains(searchString));
                                }
                                else if (filter.Key == "cores")
                                {
                                    var searchString = $"\"số nhân\": \"{filter.Value.ToLower()}\"";
                                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains(searchString));
                                }

                                break;

                            // Add additional cases for other sub-categories if needed.
                        }
                    }
                }
            }

            // Filter by price range
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

            // Execute the query
            var products = await query.ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = additionalFilters
            };

            return View(viewModel);
        }


        // Monitor Routes
        [Route("productslist/monitor")]
        [Route("productslist/monitor/{brand?}/{size?}/{resolution?}/{refreshRate?}/{priceRange?}")]
        public async Task<IActionResult> Monitor(
            string brand = null,
            string size = null,
            string resolution = null,
            string refreshRate = null,
            string priceRange = null,
            int page = 1)
        {
            // Start with products whose Loaisanpham is "monitor" (case-insensitive)
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "monitor");

            // Filter by Brand
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Filter by Size (expects a value like "24", "27", etc. and looks for "XX inch" in the JSON)
            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"kích thước\": \"{size} inch"));
            }

            // Filter by Resolution. Map view options to expected JSON values.
            if (!string.IsNullOrEmpty(resolution))
            {
                if (resolution.ToLower() == "1080p")
                {
                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains("\"độ phân giải\": \"1920x1080"));
                }
                else if (resolution.ToLower() == "1440p")
                {
                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains("\"độ phân giải\": \"2560x1440"));
                }
                else if (resolution.ToLower() == "4k")
                {
                    // Some entries may include "3840x2160" or simply "4k"
                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains("\"độ phân giải\": \"3840x2160") ||
                                            p.Thongsokythuat.ToLower().Contains("4k"));
                }
            }

            // Filter by Refresh Rate – the view sends values like "60", "75", etc.
            if (!string.IsNullOrEmpty(refreshRate))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"tần số quét\": \"{refreshRate}hz"));
            }

            // Filter by Price Range using the view's keys:
            // "duoi-2-trieu": less than 2,000,000; "2-5-trieu": between 2,000,000 and 5,000,000;
            // "5-10-trieu": between 5,000,000 and 10,000,000; "10-15-trieu": between 10,000,000 and 15,000,000;
            // "tren-15-trieu": greater than 15,000,000.
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
                    case "10-15-trieu":
                        query = query.Where(p => p.Gia >= 10000000 && p.Gia <= 15000000);
                        break;
                    case "tren-15-trieu":
                        query = query.Where(p => p.Gia > 15000000);
                        break;
                }
            }

            // Paging and view model creation
            var products = await GetPagedProductsAsync(query, page);
            var totalPages = await GetTotalPagesAsync(query);

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                Brand = brand,
                Size = size,
                Resolution = resolution,
                RefreshRate = refreshRate,
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
