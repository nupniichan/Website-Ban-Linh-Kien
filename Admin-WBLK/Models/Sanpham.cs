﻿using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

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

    public int Soluotxem { get; set; } = 0;

    public int Damuahang { get; set; } = 0;

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();
}
