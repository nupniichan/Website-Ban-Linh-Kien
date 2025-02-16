using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Danhgia
{
    public string IdDg { get; set; } = null!;

    public int Sosao { get; set; }

    public string? Noidung { get; set; }

    public DateTime? Ngaydanhgia { get; set; }

    public string IdKh { get; set; } = null!;

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual Khachhang IdKhNavigation { get; set; } = null!;
}
