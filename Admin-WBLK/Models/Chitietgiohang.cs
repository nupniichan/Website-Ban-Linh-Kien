using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Chitietgiohang
{
    public string IdGh { get; set; } = null!;
    public string IdSp { get; set; } = null!;
    
    // Renamed property to match SQL column "soluongsanpham"
    public int Soluongsanpham { get; set; }
    
    // New property for update timestamp
    public DateTime? Thoigiancapnhat { get; set; }

    // Renamed navigation property for clarity (mapping the additional FK on IdGh)
    public virtual Khachhang KhachhangNavigation { get; set; } = null!;
    
    public virtual Giohang IdGhNavigation { get; set; } = null!;
    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
