using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Nhanvien
{
    public string IdNv { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Sodienthoai { get; set; } = null!;

    public DateOnly Ngaybatdaulam { get; set; }

    public string Gioitinh { get; set; } = null!;

    public string IdTk { get; set; } = null!;

    public virtual ICollection<Doitradh> Doitradhs { get; set; } = new List<Doitradh>();

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Taikhoan IdTkNavigation { get; set; } = null!;

    public virtual ICollection<Magiamgium> Magiamgia { get; set; } = new List<Magiamgium>();

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
