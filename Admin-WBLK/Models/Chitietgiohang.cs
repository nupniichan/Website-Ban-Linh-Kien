using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Chitietgiohang
{
    public string IdGh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public int Soluongsanpham { get; set; }

    public DateTime? Thoigiancapnhat { get; set; }

    public virtual Khachhang IdGh1 { get; set; } = null!;

    public virtual Giohang IdGhNavigation { get; set; } = null!;

    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
