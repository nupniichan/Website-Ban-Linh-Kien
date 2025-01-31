﻿using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Donhang
{
    public string IdDh { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public decimal Tongtien { get; set; }

    public string Diachigiaohang { get; set; } = null!;

    public DateOnly Ngaydathang { get; set; }

    public string Phuongthucthanhtoan { get; set; } = null!;

    public string IdKh { get; set; } = null!;

	public string ghichu {get;set;}
    public string? IdMgg { get; set; }

    public string IdNv { get; set; } = null!;

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Doitradh> Doitradhs { get; set; } = new List<Doitradh>();

    public virtual Khachhang IdKhNavigation { get; set; } = null!;

    public virtual Magiamgium? IdMggNavigation { get; set; }

    public virtual Nhanvien IdNvNavigation { get; set; } = null!;

    public virtual ICollection<Thanhtoan> Thanhtoans { get; set; } = new List<Thanhtoan>();
}
