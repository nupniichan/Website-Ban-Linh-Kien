using System.Collections.Generic;
using Website_Ban_Linh_Kien.Models;

public class MockProductService
{
    public ProductDetailViewModel GetProductDetail(string id, string category)
    {
        // Tạo dữ liệu mẫu
        return new ProductDetailViewModel
        {
            Id = id,
            Category = category,
            Name = "Laptop " + category.ToUpper(),
            Price = 9999000M,
            ImageUrl = "imgs/products/cpu-amd.jpg",
            Description = "Đây là mô tả chi tiết về sản phẩm...",
            Specifications = new Dictionary<string, string>
            {
                { "Thương hiệu", "Brand XYZ" },
                { "Model", "XYZ-123" },
                { "Bảo hành", "24 tháng" }
            },
            AdditionalImages = new List<string>
            {
                "https://via.placeholder.com/300x200",
                "https://via.placeholder.com/300x200",
                "https://via.placeholder.com/300x200"
            },
            Brand = "Brand XYZ",
            InStock = true,
            Warranty = 24
        };
    }
} 