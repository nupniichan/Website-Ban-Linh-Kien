using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Models
{
    public class ProductCardViewModel
    {
        public string IdSp { get; set; }
        public string TenSp { get; set; }
        public decimal Gia { get; set; }
        public string ImageUrl { get; set; }
        public string LoaiSp { get; set; }
        public string DanhMuc { get; set; }
        public int SoLuongTon { get; set; } 
    }
}