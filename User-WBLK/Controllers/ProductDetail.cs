using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProductDetail : BaseController
    {
        private ProductDetailViewModel GetMockProduct(string id, string category)
        {
            return new ProductDetailViewModel
            {
                Id = id,
                Category = category,
                Name = $"{category.ToUpper()} Gaming Pro {id}",
                Price = 15990000M,
                ImageUrl = "https://via.placeholder.com/600x400",
                Description = "AMD Ryzen 5 5600, phiên bản nâng cấp của Ryzen 5 5600G sẽ mang đến hiệu năng mạnh mẽ. Hướng đến các công việc đòi hỏi khả năng xử lý cao, Ryzen 5 5600 sẽ không làm bạn thất vọng. Hãy cùng BPT tìm hiểu về chiếc CPU từ AMD ngay sau đây nhé!",
                
                Specifications = new Dictionary<string, string>
                {
                    { "Số nhân xử lý", "6" },
                    { "Số luồng xử lý", "12" },
                    { "Tốc độ xử lý", "Xung cơ bản 3.5GHz, xung tối đa 4.4GHz" },
                    { "Total L2 Cache", "3MB" },
                    { "Total L3 Cache", "32MB" },
                    { "Mã khóa để ép xung", "Có" },
                    { "CMOS", "TSMC 7nm FinFET" },
                    { "Kiến trúc", "AM4" },
                    { "PCI Express Version", "PCIe® 4.0" },
                    { "Giải pháp tản nhiệt (PIB)", "Wraith Stealth" },
                    { "TDP / TDP mặc định", "65W" },
                    { "Bộ nhớ hỗ trợ", "DDR4 Up to 3200MHz | Memory channel: 2" },
                    { "Product Family", "AMD Ryzen™ Processors" }
                },
                
                AdditionalImages = new List<string>
                {
                    "https://via.placeholder.com/150",
                    "https://via.placeholder.com/150",
                    "https://via.placeholder.com/150",
                    "https://via.placeholder.com/150"
                },
                
                Brand = "Gaming Pro",
                InStock = true,
                Warranty = 24,
                ProductCode = $"SP{id.PadLeft(5, '0')}",
                ViewCount = 357,
                PurchaseCount = 25,
                Rating = 5.0,
                ReviewCount = 4,
                
                Reviews = new List<ProductReview>
                {
                    new ProductReview 
                    { 
                        UserName = "Quốc Bảo", 
                        Rating = 5,
                        Comment = "AMD nài đỉnh, intel không có cửa :D"
                    },
                    new ProductReview 
                    { 
                        UserName = "Thanh", 
                        Rating = 5,
                        Comment = "⭐"
                    },
                    new ProductReview 
                    { 
                        UserName = "Bình luận", 
                        Rating = 5,
                        Comment = "Xài con này chiến game 3A + làm việc đồ họa và shop giao hàng nhanh nên ok"
                    },
                    new ProductReview 
                    { 
                        UserName = "Phát", 
                        Rating = 5,
                        Comment = "Đánh tính băng cpu này bao ngon và shop bán đúng điểm"
                    },
                    new ProductReview 
                    { 
                        UserName = "Hoàng", 
                        Rating = 4,
                        Comment = "Sản phẩm tốt, đóng gói cẩn thận"
                    },
                    new ProductReview 
                    { 
                        UserName = "Minh", 
                        Rating = 5,
                        Comment = "Giao hàng nhanh, hàng chính hãng"
                    },
                    new ProductReview 
                    { 
                        UserName = "Tuấn", 
                        Rating = 5,
                        Comment = "CPU chạy mát, xử lý nhanh"
                    },
                    new ProductReview 
                    { 
                        UserName = "Nam", 
                        Rating = 4,
                        Comment = "Giá hơi cao nhưng chất lượng tốt"
                    }
                }
            };
        }

        // PC Routes
        [Route("pc/{id}")]
        public ActionResult PC(string id)
        {
            var product = GetMockProduct(id, "pc");
            SetBreadcrumb(
                ("PC", "/productslist/pc"),
                ("Chi tiết PC", null)
            );
            return View("Index", product);
        }

        // Laptop Routes
        [Route("laptop/{id}")]
        public ActionResult Laptop(string id)
        {
            var product = GetMockProduct(id, "laptop");
            SetBreadcrumb(
                ("Laptop", "/productslist/laptop"),
                ("Chi tiết Laptop", null)
            );
            return View("Index", product);
        }

        // Components Routes
        [Route("components/{category}/{id}")]
        public ActionResult Components(string category, string id)
        {
            var product = GetMockProduct(id, category);
            SetBreadcrumb(
                ("Components", "/productslist/components"),
                (category?.ToUpper() ?? "Components", $"/productslist/components/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", product);
        }

        // Storage Routes
        [Route("storage/{id}")]
        public ActionResult Storage(string id)
        {
            var product = GetMockProduct(id, "storage");
            SetBreadcrumb(
                ("Storage", "/productslist/storage"),
                ("Chi tiết Storage", null)
            );
            return View("Index", product);
        }

        // Monitor Routes
        [Route("monitor/{id}")]
        public ActionResult Monitor(string id)
        {
            var product = GetMockProduct(id, "monitor");
            SetBreadcrumb(
                ("Monitor", "/productslist/monitor"),
                ("Chi tiết Monitor", null)
            );
            return View("Index", product);
        }

        // Audio Routes
        [Route("audio/{category}/{id}")]
        public ActionResult Audio(string category, string id)
        {
            var product = GetMockProduct(id, category);
            SetBreadcrumb(
                ("Audio", "/productslist/audio"),
                (category?.ToUpper() ?? "Audio", $"/productslist/audio/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", product);
        }

        // Peripherals Routes
        [Route("peripherals/{category}/{id}")]
        public ActionResult Peripherals(string category, string id)
        {
            var product = GetMockProduct(id, category);
            SetBreadcrumb(
                ("Peripherals", "/productslist/peripherals"),
                (category?.ToUpper() ?? "Peripherals", $"/productslist/peripherals/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", product);
        }

        // Network Routes
        [Route("network/{id}")]
        public ActionResult Network(string id)
        {
            var product = GetMockProduct(id, "network");
            SetBreadcrumb(
                ("Network", "/productslist/network"),
                ("Chi tiết Network", null)
            );
            return View("Index", product);
        }
    }
}
