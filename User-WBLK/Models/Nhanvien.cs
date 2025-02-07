using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Nhanvien
{
    public string IdNv { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Chucvu { get; set; } = null!;

    public decimal Luong { get; set; }

    public string Gioitinh { get; set; } = null!;

    public string Sodienthoai { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Diachi { get; set; } = null!;

    public DateOnly Ngayvaolam { get; set; }

    public string? Idtk { get; set; }

    public virtual Taikhoan? IdtkNavigation { get; set; }
}
