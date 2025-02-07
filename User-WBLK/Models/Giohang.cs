using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Giohang
{
    public string IdGh { get; set; } = null!;

    public string IdKh { get; set; } = null!;

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual Khachhang IdKhNavigation { get; set; } = null!;
}
