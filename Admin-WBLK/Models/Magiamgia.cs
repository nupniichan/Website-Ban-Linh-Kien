using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Magiamgia
{
    public string IdMgg { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public DateOnly Ngaysudung { get; set; }

    public DateOnly Ngayhethan { get; set; }

    public decimal Tilechietkhau { get; set; }

    public int Soluong { get; set; }

    public bool? Trangthai { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}
