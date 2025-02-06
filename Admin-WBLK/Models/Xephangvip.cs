using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Xephangvip
{
    public string Id { get; set; } = null!;

    public string Tenhang { get; set; } = null!;

    public int Diemtoithieu { get; set; }

    public int Diemtoida { get; set; }

    public decimal Phantramgiamgia { get; set; }

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();
}
