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
            string cores = null,
            string memory = null,
            string socket = null,
            string formFactor = null,
            string ram = null,
            string capacity = null,    // Dung lượng storage
            string wattage = null,     // Công suất PSU
            string size = null,        // Kích thước case
            string storageType = null, // Thêm loại storage (SSD/HDD)
            int page = 1)
        {
            // Only consider products with Loaisanpham "Components"
            var query = _context.Sanphams.Where(p => p.Loaisanpham == "Components");

            // Filter by sub-category (e.g. "vga")
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"danh mục\": \"{category.ToLower()}\""));
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
            if (!string.IsNullOrEmpty(memory))
                additionalFilters.Add("memory", memory);
            if (!string.IsNullOrEmpty(socket))
                additionalFilters.Add("socket", socket);
            if (!string.IsNullOrEmpty(formFactor))
                additionalFilters.Add("formFactor", formFactor);
            if (!string.IsNullOrEmpty(capacity))
                additionalFilters.Add("capacity", capacity);
            if (!string.IsNullOrEmpty(wattage))
                additionalFilters.Add("wattage", wattage);
            if (!string.IsNullOrEmpty(size))
                additionalFilters.Add("size", size);
            if (!string.IsNullOrEmpty(storageType))
                additionalFilters.Add("storageType", storageType);

            // Apply additional filters based on the sub-category
            if (additionalFilters.Any())
            {
                switch (category?.ToLower())
                {
                    case "cpu":
                        if (additionalFilters.ContainsKey("cpuSeries"))
                        {
                            // If the user selected "Intel core", 
                            // let's remove the trailing quote so we match "intel core i9", "intel core i7", etc.
                            var searchString = $"\"dòng cpu\": \"{additionalFilters["cpuSeries"].ToLower()}";
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains(searchString));
                        }
                        else if (additionalFilters.ContainsKey("cores"))
                        {
                            var searchString = $"\"số nhân\": \"{additionalFilters["cores"].ToLower()}\"";
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains(searchString));
                        }
                        break;

                    case "vga":
                        if (additionalFilters.ContainsKey("memory"))
                        {
                            // Tìm kiếm chỉ với số dung lượng
                            var searchString = $"\"bộ nhớ\": \"{additionalFilters["memory"]}gb";
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains(searchString.ToLower()));
                        }
                        break;

                    case "mainboard":
                        if (additionalFilters.ContainsKey("socket"))
                        {
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"socket hỗ trợ\": \"{additionalFilters["socket"].ToLower()}\""));
                        }
                        if (additionalFilters.ContainsKey("formFactor"))
                        {
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"kích thước\": \"{additionalFilters["formFactor"]}\""));
                        }
                        if (additionalFilters.ContainsKey("ramSlots"))
                        {
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"số khe ram\": \"{additionalFilters["ramSlots"]}\""));
                        }
                        break;

                    case "ram":
                        if (additionalFilters.ContainsKey("capacity"))
                        {
                            // Tìm kiếm dung lượng RAM trong thông số kỹ thuật
                            var searchValue = additionalFilters["capacity"] + "GB";
                            query = query.Where(p => 
                                p.Thongsokythuat.ToLower().Contains($"\"dung lượng\": \"{searchValue}\"".ToLower()));
                        }
                        break;

                    case "psu":
                        if (additionalFilters.ContainsKey("wattage"))
                        {
                            var wattageValue = additionalFilters["wattage"];
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"công suất\": \"{wattageValue}w\""));
                        }
                        break;

                    case "case":
                        if (additionalFilters.ContainsKey("brand"))
                        {
                            query = query.Where(p => p.Thuonghieu.ToLower() == additionalFilters["brand"].ToLower());
                        }
                        if (additionalFilters.ContainsKey("size"))
                        {
                            var sizeValue = additionalFilters["size"].ToLower();
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"kích thước\": \"{sizeValue}\""));
                        }
                        break;

                    case "storage":
                        if (additionalFilters.ContainsKey("brand"))
                        {
                            query = query.Where(p => p.Thuonghieu.ToLower() == additionalFilters["brand"].ToLower());
                        }
                        if (additionalFilters.ContainsKey("storageType"))
                        {
                            var typeValue = additionalFilters["storageType"].ToLower();
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"loại\": \"{typeValue}\""));
                        }
                        if (additionalFilters.ContainsKey("capacity"))
                        {
                            var capacityValue = additionalFilters["capacity"];
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"dung lượng\": \"{capacityValue}gb\"") || 
                                                   p.Thongsokythuat.ToLower().Contains($"\"dung lượng\": \"{capacityValue}tb\""));
                        }
                        break;
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

            // Thêm phân trang
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
            string sampleBitrate = null,
            int page = 1)
        {
            // Start with products whose Loaisanpham is "audio"
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "audio");

            // Filter by sub-category (e.g. "speaker" or "microphone")
            if (!string.IsNullOrEmpty(category))
            {
                // Expecting the JSON to have a key "danh mục" with a value like "speaker" or "microphone"
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"danh mục\": \"{category.ToLower()}\""));
            }

            // Filter by brand (exact match, case-insensitive)
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Filter by type – if category is "speaker", check for key "loại loa"
            // If category is "microphone", check for key "loại micro"
            if (!string.IsNullOrEmpty(type))
            {
                string searchType = type.ToLower();
                
                if (!string.IsNullOrEmpty(category))
                {
                    if (category.ToLower() == "speaker")
                    {
                        // If the search term is "surround", just look for that substring.
                        if (searchType != null)
                        {
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"{searchType}"));
                        }
                        
                    }
                }
            }
            if (!string.IsNullOrEmpty(sampleBitrate))
                {
                    string searchSample = sampleBitrate.ToLower();

                    if (!string.IsNullOrEmpty(category))
                    {
                        if (category.ToLower() == "microphone")
                        {
                            // For microphones, filter based on "Sample / bit rate"
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"sample / bit rate\": \"{searchSample}"));
                        }
                        else if (category.ToLower() == "speaker")
                        {
                            // If a speaker category is selected, sample/bit rate may not apply.
                            // You can either ignore the sampleBitrate filter or handle it differently.
                            // For now, we'll not apply the sampleBitrate filter for speakers.
                        }
                        else
                        {
                            // If category is not recognized, default to checking the sample/bit rate key.
                            query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"sample / bit rate\": \"{searchSample}"));
                        }
                    }
                    else
                    {
                        // When no category is set, check for the sample/bit rate in the JSON.
                        query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"sample / bit rate\": \"{searchSample}"));
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

        [Route("productslist/network")]
        [Route("productslist/network/{category}/{brand}/{type}/{connection}/{priceRange}")]
        public async Task<IActionResult> Network(
            string category = null,
            string brand = null,
            string type = null,
            string connection = null,
            string priceRange = null,
            int page = 1)
        {
            // 1. Query all "Network" products
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "network");

            // 2. Keep track of how many total items before filters
            int totalBeforeFilters = await query.CountAsync();

            // BRAND
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }
            int afterBrand = await query.CountAsync();

            // TYPE
            if (!string.IsNullOrEmpty(type))
            {
                    query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"công nghệ ax\": \"{type}\""));
            }
            int afterType = await query.CountAsync();


            int afterConnection = await query.CountAsync();

            // PRICE
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
            int afterPrice = await query.CountAsync();

            // 5. Execute the final query (with paging)
            var products = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            int finalCount = products.Count;

            // 6. Prepare debug objects
            ViewBag.NetworkCounts = new {
                Total = totalBeforeFilters,
                AfterBrand = afterBrand,
                AfterType = afterType,
                AfterConnection = afterConnection,
                AfterPrice = afterPrice
            };

            ViewBag.NetworkFilters = new {
                Brand = brand,
                Type = type,
                Connection = connection,
                PriceRange = priceRange
            };

            ViewBag.NetworkTotalFound = finalCount;

            // Optionally, build a debug list
            var debugList = products.Select(p => new {
                p.IdSp,
                p.Tensanpham,
                LowerSpecs = p.Thongsokythuat?.ToLower()
            });
            ViewBag.NetworkDebug = debugList;

            // 7. Return the view with your normal ProductListViewModel
            var viewModel = new ProductListViewModel {
                Products = products,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string> {
                    { "type", type },
                    { "connection", connection }
                }
            };

            return View(viewModel);
        }



        // Peripherals Routes
        [Route("productslist/peripherals")]
        [Route("productslist/peripherals/{category}/{brand}/{dpi}/{switches}/{driver}/{connection}/{framerate}/{priceRange}")]
        public async Task<IActionResult> Peripherals(
            string category = null,
            string brand = null,
            string dpi = null,          
            string switches = null,     
            string driver = null,       
            string connection = null,   
            string framerate = null,    // for webcam framerates
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "peripherals");

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"danh mục\": \"{category.ToLower()}\""));
            }

            // Filter by brand
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Apply category-specific filters
            if (category?.ToLower() == "webcam" && !string.IsNullOrEmpty(framerate))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"frame rate\": \"{framerate}fps\""));
            }
            // ... existing filters for other categories ...

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
                    { "dpi", dpi },
                    { "switches", switches },
                    { "driver", driver },
                    { "connection", connection },
                    { "framerate", framerate }  // Add framerate to additional filters
                }
            };

            return View(viewModel);
        }


        // Storage Routes
        [Route("productslist/storage")]
        [Route("productslist/storage/{category}")]
        public async Task<IActionResult> Storage(
            string category = "ssd",    // Đặt giá trị mặc định là "ssd"
            string brand = null,
            string capacity = null,    // dung lượng
            string type = null,        // chuẩn kết nối cho ssd (sata/pcie nvme)
            string priceRange = null,
            int page = 1)
        {
            var query = _context.Sanphams.Where(p => p.Loaisanpham.ToLower() == "storage");

            // Filter by storage type (SSD/HDD)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"loại ổ cứng\": \"{category.ToLower()}\""));
            }

            // Filter by brand
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Thuonghieu.ToLower() == brand.ToLower());
            }

            // Filter by capacity
            if (!string.IsNullOrEmpty(capacity))
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"dung lượng\": \"{capacity.ToLower()}\""));
            }

            // Filter by connection type for SSDs
            if (!string.IsNullOrEmpty(type) && category?.ToLower() == "ssd")
            {
                query = query.Where(p => p.Thongsokythuat.ToLower().Contains($"\"chuẩn kết nối\": \"{type.ToLower()}\""));
            }

            // Price range filter
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
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "capacity", capacity },
                    { "type", type }
                }
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
