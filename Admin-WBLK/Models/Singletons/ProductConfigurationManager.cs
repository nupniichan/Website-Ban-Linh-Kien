using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Admin_WBLK.Models.Singletons
{
    /// <summary>
    /// Lớp quản lý cấu hình sản phẩm sử dụng mẫu Singleton
    /// </summary>
    public class ProductConfigurationManager
    {
        private static ProductConfigurationManager _instance;
        private static readonly object _lock = new object();
        
        private Dictionary<string, List<string>> _productSpecifications;
        private List<string> _productCategories;
        private List<string> _productBrands;
        
        // Ngăn chặn việc tạo thể hiện từ bên ngoài
        private ProductConfigurationManager()
        {
            LoadConfiguration();
        }
        
        /// <summary>
        /// Lấy thể hiện duy nhất của ProductConfigurationManager
        /// </summary>
        public static ProductConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ProductConfigurationManager();
                        }
                    }
                }
                return _instance;
            }
        }
        
        /// <summary>
        /// Lấy danh sách các loại sản phẩm
        /// </summary>
        public List<string> GetProductCategories()
        {
            return _productCategories;
        }
        
        /// <summary>
        /// Lấy danh sách các thương hiệu
        /// </summary>
        public List<string> GetProductBrands()
        {
            return _productBrands;
        }
        
        /// <summary>
        /// Lấy danh sách các thông số kỹ thuật cho một loại sản phẩm
        /// </summary>
        public List<string> GetSpecificationsForCategory(string category)
        {
            if (_productSpecifications.ContainsKey(category))
            {
                return _productSpecifications[category];
            }
            return new List<string>();
        }
        
        /// <summary>
        /// Tải cấu hình từ file
        /// </summary>
        private void LoadConfiguration()
        {
            // Khởi tạo các giá trị mặc định
            _productCategories = new List<string>
            {
                "Điện tử",
                "Phụ kiện",
                "Linh kiện"
            };
            
            _productBrands = new List<string>
            {
                "Apple",
                "Samsung",
                "Xiaomi",
                "Dell",
                "HP",
                "Asus",
                "Acer",
                "Lenovo"
            };
            
            _productSpecifications = new Dictionary<string, List<string>>
            {
                ["Điện tử"] = new List<string> { "CPU", "RAM", "Ổ cứng", "Màn hình", "Card đồ họa" },
                ["Phụ kiện"] = new List<string> { "Kết nối", "Tương thích", "Chất liệu" },
                ["Linh kiện"] = new List<string> { "Socket", "Tốc độ", "Công suất", "Kích thước" }
            };
            
            // Trong thực tế, có thể tải cấu hình từ file JSON hoặc cơ sở dữ liệu
            try
            {
                string configPath = Path.Combine("wwwroot", "config", "product_config.json");
                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<ProductConfiguration>(jsonContent);
                    
                    if (config != null)
                    {
                        _productCategories = config.Categories ?? _productCategories;
                        _productBrands = config.Brands ?? _productBrands;
                        _productSpecifications = config.Specifications ?? _productSpecifications;
                    }
                }
            }
            catch
            {
                // Sử dụng giá trị mặc định nếu có lỗi
            }
        }
        
        /// <summary>
        /// Lớp cấu hình sản phẩm
        /// </summary>
        private class ProductConfiguration
        {
            public List<string> Categories { get; set; }
            public List<string> Brands { get; set; }
            public Dictionary<string, List<string>> Specifications { get; set; }
        }
    }
} 