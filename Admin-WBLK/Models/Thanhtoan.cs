using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Thanhtoan
{
    public string IdTt { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public decimal Tienthanhtoan { get; set; }

    public DateTime? Ngaythanhtoan { get; set; }

    public string? Noidungthanhtoan { get; set; }

    public string? Mathanhtoan { get; set; }

    public string IdDh { get; set; } = null!;

    public virtual Donhang IdDhNavigation { get; set; } = null!;
}
