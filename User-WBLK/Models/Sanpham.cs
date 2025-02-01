using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin_WBLK.Models;

public partial class Sanpham
{
    public string IdSp { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    [MinLength(3, ErrorMessage = "Tên sản phẩm phải có ít nhất 3 ký tự")]
    public string TenSp { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
    [Range(50000, double.MaxValue, ErrorMessage = "Giá sản phẩm phải từ 50.000đ trở lên")]
    public decimal Gia { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng tồn")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
    public int SoLuongTon { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập thương hiệu")]
    public string ThuongHieu { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập mô tả sản phẩm")]
    [MinLength(10, ErrorMessage = "Mô tả phải có ít nhất 10 ký tự")]
    public string? MoTa { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập thông số kỹ thuật")]
    public string? ThongSoKyThuat { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
    public string LoaiSp { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng chọn hình ảnh sản phẩm")]

    public string hinh_anh { get; set; } = null!;
    public int soluotxem { get; set; }
    public int damuahang { get; set; }
    public string IdNv { get; set; } = null!;
    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Chitietgiohang> Chitietgiohangs { get; set; } = new List<Chitietgiohang>();

    public virtual ICollection<Danhgia> Danhgia { get; set; } = new List<Danhgia>();

    public virtual Nhanvien IdNvNavigation { get; set; } = null!;
}
