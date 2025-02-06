using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Donhang
{
    public string IdDh { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public decimal Tongtien { get; set; }

    public string Diachigiaohang { get; set; } = null!;

    public DateTime? Ngaydathang { get; set; }

    public string Phuongthucthanhtoan { get; set; } = null!;

    public string? Ghichu { get; set; }

    public string? LydoHuy { get; set; }

    public string IdKh { get; set; } = null!;

    public string? IdMgg { get; set; }

    public virtual Khachhang IdKhNavigation { get; set; } = null!;

    public virtual Magiamgia? IdMggNavigation { get; set; }

    public virtual ICollection<Thanhtoan> Thanhtoans { get; set; } = new List<Thanhtoan>();

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();
}
