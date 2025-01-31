using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Danhgia
{
    public string IdDg { get; set; } = null!;

    public int Sosao { get; set; }

    public string? Noidung { get; set; }

    public DateTime Ngaydanhgia { get; set; }

    public string IdKh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public virtual Khachhang IdKhNavigation { get; set; } = null!;

    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
