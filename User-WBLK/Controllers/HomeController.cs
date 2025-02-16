using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy top sản phẩm có lượt xem cao nhất
            var hotProducts = _context.Sanphams
                .OrderByDescending(p => p.Soluotxem)  // Sắp xếp theo số lượt xem giảm dần
                .Take(10)  // Lấy 10 sản phẩm
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham,
                    SoLuongTon = p.Soluongton,
                    SoLuotXem = p.Soluotxem,
                    DaMuaHang = p.Damuahang
                })
                .ToList();

            // Lấy sản phẩm theo từng section
            var pcProducts = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "pc")
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham
                })
                .ToList();

            var laptopProducts = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "laptop")
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham
                })
                .ToList();

            // Lấy sản phẩm components theo Danh mục trong thongsokythuat
            var cpuProducts = _context.Sanphams
                .Where(p => p.Loaisanpham == "Components" && 
                       p.Thongsokythuat.Contains("\"Danh mục\": \"CPU\""))
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = "Components",
                    DanhMuc = "CPU",
                    SoLuongTon = p.Soluongton
                })
                .ToList();

            // Debug để kiểm tra CPU products
            Console.WriteLine($"Found {cpuProducts.Count} CPU products:");
            foreach (var product in cpuProducts)
            {
                Console.WriteLine($"CPU Product: {product.TenSp}");
            }

            var vgaProducts = _context.Sanphams
                .Where(p => p.Loaisanpham == "Components" && 
                       p.Thongsokythuat.Contains("\"Danh mục\": \"VGA\""))
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = "Components",
                    DanhMuc = "VGA",
                    SoLuongTon = p.Soluongton
                })
                .ToList();

            // Debug để kiểm tra VGA products
            Console.WriteLine($"Found {vgaProducts.Count} VGA products:");
            foreach (var product in vgaProducts)
            {
                Console.WriteLine($"VGA Product: {product.TenSp}");
            }

            var ramProducts = _context.Sanphams
                .Where(p => p.Loaisanpham == "Components" && 
                       p.Thongsokythuat.Contains("\"Danh mục\": \"RAM\""))
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham,
                    DanhMuc = "RAM",
                    SoLuongTon = p.Soluongton
                })
                .ToList();

            // Debug để kiểm tra RAM products
            Console.WriteLine($"Found {ramProducts.Count} RAM products:");
            foreach (var product in ramProducts)
            {
                Console.WriteLine($"RAM Product: {product.TenSp}");
            }

            var mainboardProducts = _context.Sanphams
                .Where(p => p.Loaisanpham == "Components" && 
                       p.Thongsokythuat.Contains("\"Danh mục\": \"Mainboard\""))
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = "Components",
                    DanhMuc = "Mainboard",
                    SoLuongTon = p.Soluongton
                })
                .ToList();

            // Debug để kiểm tra Mainboard products
            Console.WriteLine($"Found {mainboardProducts.Count} Mainboard products:");
            foreach (var product in mainboardProducts)
            {
                Console.WriteLine($"Mainboard Product: {product.TenSp}");
            }

            var monitorProducts = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "monitor")
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham
                })
                .ToList();

            var storageProducts = _context.Sanphams
                .Where(p => p.Loaisanpham.ToLower() == "storage")
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham
                })
                .ToList();

            // Gán sản phẩm vào từng section
            ViewBag.PCProducts = pcProducts;
            ViewBag.LaptopProducts = laptopProducts;
            ViewBag.CPUProducts = cpuProducts;
            ViewBag.VGAProducts = vgaProducts;
            ViewBag.RAMProducts = ramProducts;
            ViewBag.MainboardProducts = mainboardProducts;
            ViewBag.MonitorProducts = monitorProducts;
            ViewBag.StorageProducts = storageProducts;

            // Truyền danh sách sản phẩm hot vào Model và return View ở cuối cùng
            return View(hotProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
