using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Doitradh
{
    public string Id { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public string Lydo { get; set; } = null!;

    public DateOnly Ngayyeucau { get; set; }

    public DateOnly? Ngayxuly { get; set; }

    public string? Ghichu { get; set; }

    public string IdKh { get; set; } = null!;

    public string IdNv { get; set; } = null!;

    public string IdDh { get; set; } = null!;

    public virtual Donhang IdDhNavigation { get; set; } = null!;

    public virtual Khachhang IdKhNavigation { get; set; } = null!;

    public virtual Nhanvien IdNvNavigation { get; set; } = null!;
}
