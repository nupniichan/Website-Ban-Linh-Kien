using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    [Route("productslistdebug")]
    public class ProductsListDebugController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductsListDebugController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /productslistdebug/all
        [HttpGet("all")]
        public async Task<IActionResult> DebugAll()
        {
            //-----------------------------------------------------
            // 1) PC FILTERING DEBUG (We want SP000024 to match)
            //-----------------------------------------------------
            // Start with Loaisanpham == "pc"
            var pcQuery = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "pc");
            int pcTotal = await pcQuery.CountAsync();

            // Usage = "graphics" (exact match in JSON, key = "Nhu cầu")
            string pcUsage = "graphics";
            if (!string.IsNullOrEmpty(pcUsage))
            {
                pcQuery = pcQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"nhu cầu\": \"{pcUsage}\""));
            }
            int pcAfterUsage = await pcQuery.CountAsync();

            // CPU partial = "intel core i9" (so it matches e.g. "intel core i9-14900k")
            string pcCpu = "intel core i9";
            if (!string.IsNullOrEmpty(pcCpu))
            {
                pcQuery = pcQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"cpu\": \"{pcCpu}")); 
            }
            int pcAfterCpu = await pcQuery.CountAsync();

            // RAM partial = "64gb ddr5"
            string pcRam = "64gb ddr5";
            if (!string.IsNullOrEmpty(pcRam))
            {
                pcQuery = pcQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"ram\": \"{pcRam}"));
            }
            int pcAfterRam = await pcQuery.CountAsync();

            // GPU partial = "rtx 3060"
            string pcGpu = "rtx 3060";
            if (!string.IsNullOrEmpty(pcGpu))
            {
                pcQuery = pcQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"vga\": \"{pcGpu}"));
            }
            int pcAfterGpu = await pcQuery.CountAsync();

            // Price = "tren-30-trieu" => p.Gia > 30000000
            string pcPrice = "tren-30-trieu";
            if (!string.IsNullOrEmpty(pcPrice))
            {
                switch (pcPrice.ToLower())
                {
                    case "tren-30-trieu":
                        pcQuery = pcQuery.Where(p => p.Gia > 30000000);
                        break;
                }
            }
            int pcAfterPrice = await pcQuery.CountAsync();

            var pcProducts = await pcQuery.ToListAsync();
            var pcDebug = pcProducts.Select(p => new
            {
                p.IdSp,
                p.Tensanpham,
                p.Thuonghieu,
                OriginalSpecs = p.Thongsokythuat,
                LowerSpecs = p.Thongsokythuat?.ToLower()
            }).ToList();

            ViewBag.PCCounts = new
            {
                Total = pcTotal,
                AfterUsage = pcAfterUsage,
                AfterCpu = pcAfterCpu,
                AfterRam = pcAfterRam,
                AfterGpu = pcAfterGpu,
                AfterPrice = pcAfterPrice
            };
            ViewBag.PCFilters = new
            {
                Usage = pcUsage,
                Cpu = pcCpu,
                Ram = pcRam,
                Gpu = pcGpu,
                PriceRange = pcPrice
            };
            ViewBag.PCDebug = pcDebug;
            ViewBag.PCTotalFound = pcProducts.Count;

            //-----------------------------------------------------
            // 2) LAPTOP FILTERING DEBUG (We want SP000421 to match)
            //-----------------------------------------------------
            // Start with Loaisanpham == "laptop"
            var laptopQuery = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "laptop");
            int laptopTotal = await laptopQuery.CountAsync();

            // Usage = "gaming" (exact match in JSON, key = "Nhu cầu")
            string laptopUsage = "gaming";
            if (!string.IsNullOrEmpty(laptopUsage))
            {
                laptopQuery = laptopQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"nhu cầu\": \"{laptopUsage}\""));
            }
            int laptopAfterUsage = await laptopQuery.CountAsync();

            // Brand = "msi" (exact match in Thuonghieu)
            string laptopBrand = "msi";
            if (!string.IsNullOrEmpty(laptopBrand))
            {
                laptopQuery = laptopQuery.Where(p => p.Thuonghieu.ToLower() == laptopBrand.ToLower());
            }
            int laptopAfterBrand = await laptopQuery.CountAsync();

            // CPU partial = "intel core i7"
            string laptopCpu = "intel core i7";
            if (!string.IsNullOrEmpty(laptopCpu))
            {
                laptopQuery = laptopQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"cpu\": \"{laptopCpu}"));
            }
            int laptopAfterCpu = await laptopQuery.CountAsync();

            // RAM partial = "16gb ddr4"
            string laptopRam = "16gb ddr4";
            if (!string.IsNullOrEmpty(laptopRam))
            {
                laptopQuery = laptopQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"ram\": \"{laptopRam}"));
            }
            int laptopAfterRam = await laptopQuery.CountAsync();

            // GPU partial = "rtx 3070"
            string laptopGpu = "rtx 3070";
            if (!string.IsNullOrEmpty(laptopGpu))
            {
                laptopQuery = laptopQuery.Where(p => p.Thongsokythuat.ToLower()
                    .Contains($"\"vga\": \"{laptopGpu}"));
            }
            int laptopAfterGpu = await laptopQuery.CountAsync();

            // Price = "tren-30-trieu" => p.Gia > 30000000
            string laptopPrice = "tren-30-trieu";
            if (!string.IsNullOrEmpty(laptopPrice))
            {
                switch (laptopPrice.ToLower())
                {
                    case "tren-30-trieu":
                        laptopQuery = laptopQuery.Where(p => p.Gia > 30000000);
                        break;
                }
            }
            int laptopAfterPrice = await laptopQuery.CountAsync();

            var laptopProducts = await laptopQuery.ToListAsync();
            var laptopDebug = laptopProducts.Select(p => new
            {
                p.IdSp,
                p.Tensanpham,
                p.Thuonghieu,
                OriginalSpecs = p.Thongsokythuat,
                LowerSpecs = p.Thongsokythuat?.ToLower()
            }).ToList();

            ViewBag.LaptopCounts = new
            {
                Total = laptopTotal,
                AfterUsage = laptopAfterUsage,
                AfterBrand = laptopAfterBrand,
                AfterCpu = laptopAfterCpu,
                AfterRam = laptopAfterRam,
                AfterGpu = laptopAfterGpu,
                AfterPrice = laptopAfterPrice
            };
            ViewBag.LaptopFilters = new
            {
                Usage = laptopUsage,
                Brand = laptopBrand,
                Cpu = laptopCpu,
                Ram = laptopRam,
                Gpu = laptopGpu,
                PriceRange = laptopPrice
            };
            ViewBag.LaptopDebug = laptopDebug;
            ViewBag.LaptopTotalFound = laptopProducts.Count;

            // Return both sets in a combined object for the single view
            var combined = new
            {
                PCProducts = pcProducts,
                LaptopProducts = laptopProducts
            };

            return View("DebugAll", combined);
        }
    }
}
