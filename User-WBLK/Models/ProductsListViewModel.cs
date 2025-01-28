using System.Collections.Generic;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Models
{
    public class ProductListViewModel
    {
        // Thuộc tính cơ bản
        public List<Sanpham> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // Thuộc tính lọc chung
        public string Category { get; set; }
        public string Brand { get; set; }
        public string PriceRange { get; set; }
        public string Price { get; set; }

        // Thuộc tính lọc cho PC và Laptop
        public string Usage { get; set; }
        public string CpuType { get; set; }
        public string Ram { get; set; }
        public string Gpu { get; set; }

        // Thuộc tính lọc khác
        public string Size { get; set; }
        public string Resolution { get; set; }
        public string RefreshRate { get; set; }
        public string Capacity { get; set; }
        public string Type { get; set; }
        public string Connection { get; set; }
        public string Features { get; set; }

        // Dictionary cho các bộ lọc bổ sung
        public Dictionary<string, string> AdditionalFilters { get; set; }

        public ProductListViewModel()
        {
            Products = new List<Sanpham>();
            AdditionalFilters = new Dictionary<string, string>();
        }
    }
}