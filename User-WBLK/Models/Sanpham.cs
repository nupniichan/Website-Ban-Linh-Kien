using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Sanpham
{
    public string IdSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public decimal Gia { get; set; }

    public int SoLuongTon { get; set; }

    public string ThuongHieu { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? ThongSoKyThuat { get; set; }

    public string LoaiSp { get; set; } = null!;

    public string IdNv { get; set; } = null!;

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual ICollection<Danhgium> Danhgia { get; set; } = new List<Danhgium>();

    public virtual Nhanvien IdNvNavigation { get; set; } = null!;
}
