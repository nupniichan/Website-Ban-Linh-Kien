using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Website_Ban_Linh_Kien.Models;
using System.Linq;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class CustomBuildController : Controller
    {
        private readonly DatabaseContext _context;

        public CustomBuildController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách các loại linh kiện cần thiết cho việc build PC
            var componentTypes = new List<string>
            {
                "CPU", "Mainboard", "VGA", "RAM", "SSD", "HDD", "PSU", "Case", "Cooling", "Monitor"
            };

            ViewBag.ComponentTypes = componentTypes;
            return View();
        }

        [HttpGet]
        public IActionResult GetComponents(string componentType)
        {
            List<Sanpham> components = new List<Sanpham>();

            // Xử lý các trường hợp đặc biệt
            if (componentType == "CPU")
            {
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "CPU" || 
                           (p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"cpu\""))) &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "GPU" || componentType == "VGA")
            {
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "GPU" || 
                           (p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"gpu\"") || 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"vga\""))) &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "Mainboard" || componentType == "Bo mạch chủ")
            {
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "Mainboard" || 
                           (p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"mainboard\""))) &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "RAM")
            {
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "RAM" || 
                           (p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"ram\""))) &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "SSD")
            {
                components = _context.Sanphams
                    .Where(p => p.Loaisanpham == "Storage" && 
                           p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"loại ổ cứng\": \"ssd\"") &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "HDD")
            {
                components = _context.Sanphams
                    .Where(p => p.Loaisanpham == "Storage" && 
                           p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"loại ổ cứng\": \"hdd\"") &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else if (componentType == "Monitor")
            {
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "Monitor" || 
                           (p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains("\"danh mục\": \"monitor\""))) &&
                           p.Soluongton > 0)
                    .ToList();
            }
            else
            {
                // Đối với các loại khác, tìm trong Components
                components = _context.Sanphams
                    .Where(p => (p.Loaisanpham == "Components" || p.Loaisanpham == componentType) && 
                           p.Thongsokythuat != null && 
                           p.Thongsokythuat.ToLower().Contains($"\"danh mục\": \"{componentType.ToLower()}\"") &&
                           p.Soluongton > 0)
                    .ToList();
            }

            var result = components.Select(p => new
            {
                p.IdSp,
                p.Tensanpham,
                p.Gia,
                p.Thuonghieu,
                p.Hinhanh,
                p.Thongsokythuat,
                p.Soluongton
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetComponentDetails(string id)
        {
            // Lấy chi tiết của một linh kiện cụ thể
            var component = _context.Sanphams
                .Where(p => p.IdSp == id && p.Soluongton > 0)
                .Select(p => new
                {
                    p.IdSp,
                    p.Tensanpham,
                    p.Gia,
                    p.Thuonghieu,
                    p.Hinhanh,
                    p.Thongsokythuat,
                    p.Mota,
                    p.Soluongton
                })
                .FirstOrDefault();

            if (component == null)
            {
                return NotFound();
            }

            return Json(component);
        }
    }
}
