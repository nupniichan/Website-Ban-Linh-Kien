using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Chitietdonhang
{
    // New primary key property
    public string Idchitietdonhang { get; set; } = null!;

    public string IdDh { get; set; } = null!;
    public string IdSp { get; set; } = null!;
    
    // New property for rating foreign key (nullable)
    public string? IdDg { get; set; }
    
    // Renamed property to match SQL column "soluongsanpham"
    public int Soluongsanpham { get; set; }
    
    public decimal Dongia { get; set; }

    public virtual Donhang IdDhNavigation { get; set; } = null!;
    public virtual Sanpham IdSpNavigation { get; set; } = null!;
    
    // New navigation property for Danhgia
    public virtual Danhgia? IdDgNavigation { get; set; }
}
