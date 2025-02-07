using System;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Models;

public partial class Chitietdonhang
{
    public string IdDh { get; set; } = null!;

    public string IdSp { get; set; } = null!;

    public int Soluong { get; set; }

    public decimal Dongia { get; set; }
}
