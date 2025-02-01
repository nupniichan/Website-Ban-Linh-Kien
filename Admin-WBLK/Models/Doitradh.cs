using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Admin_WBLK.Models;

public partial class Doitradh
{
    public string Id { get; set; } = null!;

    public string Trangthai { get; set; } = "Chờ xử lý";

    public string Lydo { get; set; } = null!;

    public DateTime Ngayyeucau { get; set; }

    public DateTime? Ngayxuly { get; set; }

    public string? Ghichu { get; set; }

    public string IdKh { get; set; } = null!;

    public string IdNv { get; set; } = null!;

    public string IdDh { get; set; } = null!;

    [JsonIgnore]
    [BindNever]
    public virtual Donhang? IdDhNavigation { get; set; }

    [JsonIgnore]
    [BindNever]
    public virtual Khachhang? IdKhNavigation { get; set; }

    [JsonIgnore]
    [BindNever]
    public virtual Nhanvien? IdNvNavigation { get; set; }
}
