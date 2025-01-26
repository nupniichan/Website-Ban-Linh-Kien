using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Taikhoan
{
    public string IdTk { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string Tentaikhoan { get; set; } = null!;

    public DateOnly Ngaytaotk { get; set; }

    public DateOnly? Ngaysuadoi { get; set; }

    public string Quyentruycap { get; set; } = null!;

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
