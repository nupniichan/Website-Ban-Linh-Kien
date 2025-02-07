using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Sanpham
{
    public string IdSp { get; set; } = null!;

    public string Tensanpham { get; set; } = null!;

    public decimal Gia { get; set; }

    public int Soluongton { get; set; }

    public string Thuonghieu { get; set; } = null!;

    public string? Mota { get; set; }

    public string? Thongsokythuat { get; set; }

    public string Loaisanpham { get; set; } = null!;

    public string? Hinhanh { get; set; }

    public int? Soluotxem { get; set; }

    public int? Damuahang { get; set; }

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual ICollection<Danhgia> Danhgia { get; set; } = new List<Danhgia>();
}
