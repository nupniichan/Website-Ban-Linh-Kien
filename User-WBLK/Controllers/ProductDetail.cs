using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Website_Ban_Linh_Kien.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProductDetail : BaseController
    {
        private readonly DatabaseContext _context;

        public ProductDetail(DatabaseContext context)
        {
            _context = context;
        }

        private ProductDetailViewModel MapSanphamToViewModel(Sanpham sp)
        {
            // Parse thongSoKyThuat từ JSON string sang Dictionary
            Dictionary<string, string> specifications = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(sp.ThongSoKyThuat))
            {
                try
                {
                    var specList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(sp.ThongSoKyThuat);
                    foreach (var spec in specList)
                    {
                        if (spec.ContainsKey("key") && spec.ContainsKey("value"))
                        {
                            specifications.Add(spec["key"], spec["value"]);
                        }
                    }
                }
                catch (Exception ex) 
                { 
                    // Log lỗi nếu cần
                    System.Diagnostics.Debug.WriteLine($"Error parsing specifications: {ex.Message}");
                }
            }

            // Tính toán thống kê đánh giá
            var reviews = sp.Danhgia.ToList();
            var totalReviews = reviews.Count;
            var averageRating = totalReviews > 0 ? reviews.Average(r => r.Sosao) : 0;
            
            // Tạo phân phối rating (1-5 sao)
            var ratingDistribution = new Dictionary<int, int>();
            for (int i = 1; i <= 5; i++)
            {
                ratingDistribution[i] = reviews.Count(r => r.Sosao == i);
            }

            return new ProductDetailViewModel
            {
                Id = sp.IdSp,
                Name = sp.TenSp,
                Price = sp.Gia,
                ImageUrl = sp.hinh_anh,
                Description = sp.MoTa,
                Specifications = specifications,
                Brand = sp.ThuongHieu,
                InStock = sp.SoLuongTon > 0,
                ProductCode = sp.IdSp,
                Category = sp.LoaiSp,
                TotalReviews = totalReviews,
                AverageRating = Math.Round(averageRating, 1),
                RatingDistribution = ratingDistribution,
                Reviews = reviews.Select(r => new ProductReview
                {
                    UserName = r.IdKhNavigation?.Hoten ?? "Anonymous",
                    Rating = r.Sosao,
                    Comment = r.Noidung,
                    Date = r.Ngaydanhgia
                }).ToList(),
                // Các thuộc tính khác có thể thêm theo nhu cầu
                ViewCount = sp.soluotxem,
                PurchaseCount = sp.damuahang,
                Rating = averageRating,
                Warranty = 24 // Có thể thêm trường này vào database nếu cần
            };
        }

        // PC Routes
        [Route("pc/{id}")]
        public ActionResult PC(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == "pc");
            if (product == null)
                return NotFound();

            // Tăng số lượt xem
            product.soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("PC", "/productslist/pc"),
                ("Chi tiết PC", null)
            );
            return View("Index", viewModel);
        }

        // Laptop Routes
        [Route("laptop/{id}")]
        public ActionResult Laptop(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == "laptop");
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Laptop", "/productslist/laptop"),
                ("Chi tiết Laptop", null)
            );
            return View("Index", viewModel);
        }

        // Components Routes - Add new overload
        [Route("components/{id}")]
        public ActionResult Components(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id);

            if (product == null)
                return NotFound();

            // Tăng số lượt xem
            product.soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Components", "/productslist/components"),
                ("Chi tiết sản phẩm", null)
            );

            return View("Index", viewModel);
        }

        // Existing Components route
        [Route("components/{category}/{id}")]
        public ActionResult Components(string category, string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == category.ToLower());

            if (product == null)
                return NotFound();

            // Tăng số lượt xem
            product.soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Components", "/productslist/components"),
                (category?.ToUpper() ?? "Components", $"/productslist/components/{category}"),
                ("Chi tiết sản phẩm", null)
            );

            return View("Index", viewModel);
        }

        // Storage Routes
        [Route("storage/{id}")]
        public ActionResult Storage(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == "storage");
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Storage", "/productslist/storage"),
                ("Chi tiết Storage", null)
            );
            return View("Index", viewModel);
        }

        // Monitor Routes
        [Route("monitor/{id}")]
        public ActionResult Monitor(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == "monitor");
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Monitor", "/productslist/monitor"),
                ("Chi tiết Monitor", null)
            );
            return View("Index", viewModel);
        }

        // Audio Routes
        [Route("audio/{category}/{id}")]
        public ActionResult Audio(string category, string id)
        {
            var product = _context.Sanphams.FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == category.ToLower());
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Audio", "/productslist/audio"),
                (category?.ToUpper() ?? "Audio", $"/productslist/audio/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", viewModel);
        }

        // Peripherals Routes
        [Route("peripherals/{category}/{id}")]
        public ActionResult Peripherals(string category, string id)
        {
            var product = _context.Sanphams.FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == category.ToLower());
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Peripherals", "/productslist/peripherals"),
                (category?.ToUpper() ?? "Peripherals", $"/productslist/peripherals/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", viewModel);
        }

        // Network Routes
        [Route("network/{id}")]
        public ActionResult Network(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Danhgia)
                .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.LoaiSp.ToLower() == "network");
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Network", "/productslist/network"),
                ("Chi tiết Network", null)
            );
            return View("Index", viewModel);
        }
    }
}
