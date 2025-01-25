using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;
namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProductsListController : BaseController
    {
        private const int PageSize = 15;

        // PC Routes
        [Route("productslist/pc")]
        [Route("productslist/pc/{category}")]
        public IActionResult PC(string category = null, string priceRange = null, string price = null, 
            string usage = null, string cpuType = null, string ram = null, string gpu = null, int page = 1)
        {
            SetBreadcrumb(
                ("PC", "/pc"),
                (category ?? "Tất cả", null)
            );

            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                PriceRange = priceRange,
                Price = price,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "usage", usage },
                    { "cpuType", cpuType },
                    { "ram", ram },
                    { "gpu", gpu }
                }
            };

            return View(viewModel);
        }

        // Laptop Routes
        [Route("productslist/laptop")]
        [Route("productslist/laptop/{category}")]
        public IActionResult Laptop(string category = null, string priceRange = null, string price = null, 
            string usage = null, string brand = null, string cpuType = null, string ram = null, string gpu = null, int page = 1)
        {
            SetBreadcrumb(
                ("Laptop", "/laptop"),
                (category ?? "Tất cả", null)
            );

            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                PriceRange = priceRange,
                Price = price,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "usage", usage },
                    { "brand", brand },
                    { "cpuType", cpuType },
                    { "ram", ram },
                    { "gpu", gpu }
                }
            };

            return View(viewModel);
        }

        // Components Routes
        [Route("productslist/components")]
        [Route("productslist/components/{category}")]
        [Route("productslist/components/{category}/{brand}")]
        public IActionResult Components(string category = null, string brand = null,
            string priceRange = null, int page = 1,
            // CPU filters
            string cpuSeries = null,
            string socket = null,
            string cores = null,
            string threads = null,
            // VGA filters
            string memory = null,
            string manufacturer = null,
            // Mainboard filters
            string formFactor = null,
            string mbSocket = null,
            // RAM filters
            string capacity = null,
            string ramType = null,
            // PSU filters
            string wattage = null,
            string efficiency = null,
            // Case filters
            string caseSize = null)
        {
            // Nếu không có category, mặc định chuyển đến CPU
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Components", new { category = "cpu" });
            }

            SetBreadcrumb(
                ("Components", "/productslist/components"),
                (category ?? "Tất cả", null)
            );

            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                Brand = brand
            };

            switch (category?.ToLower())
            {
                case "cpu":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "cpuSeries", cpuSeries },
                        { "socket", socket },
                        { "cores", cores },
                        { "threads", threads },
                        { "priceRange", priceRange }
                    };
                    break;

                case "vga":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "memory", memory },
                        { "manufacturer", manufacturer },
                        { "priceRange", priceRange }
                    };
                    break;

                case "mainboard":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "formFactor", formFactor },
                        { "socket", mbSocket },
                        { "priceRange", priceRange }
                    };
                    break;

                case "ram":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "capacity", capacity },
                        { "ramType", ramType },
                        { "priceRange", priceRange }
                    };
                    break;

                case "psu":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "wattage", wattage },
                        { "efficiency", efficiency },
                        { "priceRange", priceRange }
                    };
                    break;

                case "case":
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "caseSize", caseSize },
                        { "priceRange", priceRange }
                    };
                    break;

                default:
                    viewModel.AdditionalFilters = new Dictionary<string, string>
                    {
                        { "priceRange", priceRange }
                    };
                    break;
            }

            return View(viewModel);
        }

        // Storage Routes
        [Route("productslist/storage")]
        [Route("productslist/storage-{category}")]
        [Route("productslist/storage-{category}-{brand}")]
        public IActionResult Storage(
            string category = null, 
            string brand = null,
            string capacity = null,
            string priceRange = null,
            int page = 1)
        {
            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                Brand = brand,
                Capacity = capacity,
                PriceRange = priceRange
            };
            SetBreadcrumb(
                ("Storage", "/productslist/storage"),
                (category ?? "Tất cả", null)
            );
            return View(viewModel);
        }

        // Monitor Routes
        [Route("productslist/monitor")]
        [Route("productslist/monitor-{brand}")]
        [Route("productslist/monitor-{size}inch")]
        [Route("productslist/monitor-{resolution}")]
        [Route("productslist/monitor-{refreshRate}hz")]
        [Route("productslist/monitor-duoi-{price}-trieu")]
        [Route("productslist/monitor-{priceRange}-trieu")]
        [Route("productslist/monitor-tren-{price}-trieu")]
        public IActionResult Monitor(
            string brand = null,
            string size = null,
            string resolution = null,
            string refreshRate = null,
            string priceRange = null,
            string price = null,
            int page = 1)
        {
            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Brand = brand,
                Size = size,
                Resolution = resolution,
                RefreshRate = refreshRate,
                PriceRange = priceRange,
                Price = price
            };
            SetBreadcrumb(
                ("Monitor", "/productslist/monitor")
            );
            return View(viewModel);
        }

        // Audio Routes
        [Route("productslist/audio")]
        [Route("productslist/audio-{category}")]
        [Route("productslist/speaker-{brand}")]
        [Route("productslist/microphone-{brand}")]
        [Route("productslist/webcam-{brand}")]
        public IActionResult Audio(
            string category = null, 
            string brand = null, 
            string type = null,
            string priceRange = null,
            int page = 1)
        {
            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                Brand = brand,
                PriceRange = priceRange
            };

            SetBreadcrumb(
                ("Audio", "/productslist/monitor")
            );

            viewModel.AdditionalFilters = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(type))
            {
                viewModel.AdditionalFilters.Add("type", type);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                viewModel.AdditionalFilters.Add("priceRange", priceRange);
            }

            switch (category?.ToLower())
            {
                case "speaker":
                    // Có thể thêm logic xử lý đặc biệt cho loa
                    break;

                case "microphone":
                    // Có thể thêm logic xử lý đặc biệt cho micro
                    break;

                case "webcam":
                    // Có thể thêm logic xử lý đặc biệt cho webcam
                    break;
            }

            return View(viewModel);
        }

        [Route("productslist/peripherals")]
        [Route("productslist/peripherals/{category}")]
        [Route("productslist/keyboard-{brand}")]
        [Route("productslist/mouse-{brand}")]
        [Route("productslist/headphone-{brand}")]
        public IActionResult Peripherals(
            string category = null,
            string brand = null,
            string type = null,
            string connection = null,
            string priceRange = null,
            int page = 1)
        {
            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                Brand = brand,
                PriceRange = priceRange
            };

            SetBreadcrumb(
                ("Peripherals", "/productslist/monitor")
            );

            viewModel.AdditionalFilters = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(type))
            {
                viewModel.AdditionalFilters.Add("type", type);
            }

            if (!string.IsNullOrEmpty(connection))
            {
                viewModel.AdditionalFilters.Add("connection", connection);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                viewModel.AdditionalFilters.Add("priceRange", priceRange);
            }

            switch (category?.ToLower())
            {
                case "keyboard":
                    // Có thể thêm logic xử lý đặc biệt cho bàn phím
                    break;

                case "mouse":
                    // Có thể thêm logic xử lý đặc biệt cho chuột
                    break;

                case "headphone":
                    // Có thể thêm logic xử lý đặc biệt cho tai nghe
                    break;
            }

            return View(viewModel);
        }

        // Network Routes
        [Route("productslist/network")]
        [Route("productslist/network/{category}")]
        [Route("productslist/network/{category}/{brand}")]
        public IActionResult Network(string category = null, string brand = null, string type = null, 
            string connection = null, string priceRange = null, int page = 1)
        {
            var viewModel = new ProductListViewModel
            {
                CurrentPage = page,
                TotalPages = 5,
                Category = category,
                Brand = brand,
                PriceRange = priceRange,
                AdditionalFilters = new Dictionary<string, string>
                {
                    { "type", type },
                    { "connection", connection }
                }
            };

            SetBreadcrumb(
                ("Network", "/productslist/Network"),
                (category ?? "Tất cả", null)
            );
            return View(viewModel);
        }

        // Cái này tính sau
        [Route("productslist/{product}-{priceRange}-trieu")]
        [Route("productslist/{product}-above-{price}-trieu")]
        public IActionResult ByPriceRange(string product, string priceRange, string price = null)
        {
            ViewBag.Product = product;
            ViewBag.PriceRange = priceRange;
            ViewBag.Price = price;

            string viewName = product.ToLower() switch
            {
                "pc" => "PC",
                "laptop" => "Laptop",
                "monitor" => "Monitor",
                _ => "Index"
            };

            return View(viewName);
        }
        
        public IActionResult Components(string category)
        {
            var products = new List<ProductCardViewModel>
            {
                new ProductCardViewModel 
                { 
                    Id = "intel-i9",
                    Category = "cpu",
                    Name = "Intel Core i9",
                    Price = 12990000M,
                    ImageUrl = "path/to/image"
                },
                // Other products...
            };
            
            return View(products);
        }
    }
}
