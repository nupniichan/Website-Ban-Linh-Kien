using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models;

public partial class Khachhang
{
    public string IdKh { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Diachi { get; set; } = null!;

    public string? Email { get; set; }

    public string? Gioitinh { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string Sodienthoai { get; set; } = null!;

    public int? Diemtichluy { get; set; }

    public string IdTk { get; set; } = null!;

    public string? IdXephangvip { get; set; }

    public ulong? Loaikhachhang { get; set; }

    public virtual ICollection<Danhgia> Danhgia { get; set; } = new List<Danhgia>();

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<Giohang> Giohangs { get; set; } = new List<Giohang>();

    public virtual Taikhoan IdTkNavigation { get; set; } = null!;

    public virtual Xephangvip? IdXephangvipNavigation { get; set; }
}
