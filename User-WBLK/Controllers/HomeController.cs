using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
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

        public IActionResult TraCuuDonHang()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TraCuuDonHang(string searchTerm, string searchType, int page = 1)
        {
            var query = _context.Khachhangs.AsQueryable();
            const int pageSize = 5;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                switch (searchType)
                {
                    case "phone":
                        query = query.Where(k => k.Sodienthoai.Contains(searchTerm));
                        break;
                    case "email":
                        query = query.Where(k => k.Email.Contains(searchTerm));
                        break;
                    case "name":
                        query = query.Where(k => k.Hoten.Contains(searchTerm));
                        break;
                }
            }

            var khachHang = query.FirstOrDefault();
            
            if (khachHang != null)
            {
                // Lấy tổng số đơn hàng
                var totalOrders = _context.Donhangs
                    .Count(d => d.IdKh == khachHang.IdKh);

                // Lấy danh sách đơn hàng có phân trang
                var donHangs = _context.Donhangs
                    .Where(d => d.IdKh == khachHang.IdKh)
                    .OrderByDescending(d => d.Ngaydathang)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new
                {
                    KhachHang = khachHang,
                    DonHang = donHangs.Select(d => new
                    {
                        DonHang = d,
                        ChiTiet = _context.Chitietdonhangs
                            .Where(c => c.IdDh == d.IdDh)
                            .ToList()
                    }).ToList(),
                    HasOrders = totalOrders > 0,
                    TotalOrders = totalOrders
                };

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SearchType = searchType;

                return View(result);
            }

            ViewBag.SearchTerm = searchTerm;
            return View(null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
