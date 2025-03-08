using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Strategis
{
    public class DefaultProductFilterStrategy : IProductFilterStrategy
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public IEnumerable<Sanpham> Filter(IEnumerable<Sanpham> products, string category, string brand)
        {
            var result = products;

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                result = result.Where(s => s.Thuonghieu == brand);
            }

            // Lọc theo loại sản phẩm
            if (!string.IsNullOrEmpty(category))
            {
                if (category == "PC" || category == "Laptop" || category == "Monitor")
                {
                    result = result.Where(s => s.Loaisanpham == category);
                }
                else
                {
                    result = result.Where(s =>
                    {
                        try
                        {
                            var specs = JsonSerializer.Deserialize<Dictionary<string, string>>(s.Thongsokythuat, _jsonOptions);
                            return specs != null &&
                                   ((specs.ContainsKey("Danh mục") && specs["Danh mục"] == category) ||
                                    (specs.ContainsKey("Loại ổ cứng") && specs["Loại ổ cứng"] == category));
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }
            }

            return result;
        }
    }
} 