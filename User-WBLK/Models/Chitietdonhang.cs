using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Chitietdonhang
{
    public string Idchitietdonhang { get; set; } = null!;

    public string IdDh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public string? IdDg { get; set; }

    public int Soluongsanpham { get; set; }

    public decimal Dongia { get; set; }

    public virtual Danhgia? IdDgNavigation { get; set; }

    public virtual Donhang IdDhNavigation { get; set; } = null!;

    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
