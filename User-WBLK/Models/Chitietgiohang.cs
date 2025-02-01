using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Chitietgiohang
{
    public string IdGh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public int Soluong { get; set; }

    public virtual Giohang IdGhNavigation { get; set; } = null!;

    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
