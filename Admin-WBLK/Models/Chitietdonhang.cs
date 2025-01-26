using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Chitietdonhang
{
    public string IdDh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public int Soluong { get; set; }

    public decimal Dongia { get; set; }

    public virtual Donhang IdDhNavigation { get; set; } = null!;

    public virtual Sanpham IdSpNavigation { get; set; } = null!;
}
