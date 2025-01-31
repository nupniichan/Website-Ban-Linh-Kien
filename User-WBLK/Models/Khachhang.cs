﻿using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Khachhang
{
    public string IdKh { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Diachi { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Gioitinh { get; set; } = null!;

    public DateOnly Ngaysinh { get; set; }

    public string Sodienthoai { get; set; } = null!;

    public string IdTk { get; set; } = null!;

    public virtual ICollection<Danhgia> Danhgia { get; set; } = new List<Danhgia>();

    public virtual ICollection<Doitradh> Doitradhs { get; set; } = new List<Doitradh>();

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<Giohang> Giohangs { get; set; } = new List<Giohang>();

    public virtual Taikhoan IdTkNavigation { get; set; } = null!;
}
