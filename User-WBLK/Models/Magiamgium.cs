using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Magiamgium
{
    public string IdMgg { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public DateOnly Ngaysudung { get; set; }

    public DateOnly Ngayhethan { get; set; }

    public decimal Tilechietkhau { get; set; }

    public int Soluong { get; set; }

    public string? IdNv { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Nhanvien? IdNvNavigation { get; set; }
}
