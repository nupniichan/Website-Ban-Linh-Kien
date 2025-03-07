using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Microsoft.Extensions.Configuration;

namespace Website_Ban_Linh_Kien.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietdonhang> Chitietdonhangs { get; set; }

    public virtual DbSet<Chitietgiohang> Chitietgiohangs { get; set; }

    public virtual DbSet<Danhgia> Danhgia { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Giohang> Giohangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Magiamgia> Magiamgia { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Thanhtoan> Thanhtoans { get; set; }

    public virtual DbSet<Xephangvip> Xephangvips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Chitietdonhang>(entity =>
        {
            entity.HasKey(e => e.Idchitietdonhang).HasName("PRIMARY");

            entity.ToTable("chitietdonhang");

            entity.HasIndex(e => e.IdDh, "chitietdonhang_fk_1");

            entity.HasIndex(e => e.IdDg, "fk_chitietdonhang_danhgia");

            entity.HasIndex(e => e.IdSp, "fk_chitietdonhang_sanpham");

            entity.Property(e => e.Idchitietdonhang).HasMaxLength(10);
            entity.Property(e => e.Dongia)
                .HasPrecision(15, 2)
                .HasColumnName("dongia");
            entity.Property(e => e.IdDg).HasMaxLength(10);
            entity.Property(e => e.IdDh).HasMaxLength(10);
            entity.Property(e => e.IdSp).HasMaxLength(10);
            entity.Property(e => e.Soluongsanpham)
                .HasColumnType("int(11)")
                .HasColumnName("soluongsanpham");

            entity.HasOne(d => d.IdDgNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdDg)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_chitietdonhang_danhgia");

            entity.HasOne(d => d.IdDhNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdDh)
                .HasConstraintName("fk_chitietdonhang_donhang");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdSp)
                .HasConstraintName("fk_chitietdonhang_sanpham");
        });

        modelBuilder.Entity<Chitietgiohang>(entity =>
        {
            entity.HasKey(e => new { e.IdGh, e.IdSp })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("chitietgiohang");

            entity.HasIndex(e => e.IdSp, "IdSp");

            entity.Property(e => e.IdGh).HasMaxLength(10);
            entity.Property(e => e.IdSp).HasMaxLength(10);
            entity.Property(e => e.Soluongsanpham)
                .HasColumnType("int(11)")
                .HasColumnName("soluongsanpham");
            entity.Property(e => e.Thoigiancapnhat)
                .HasColumnType("datetime")
                .HasColumnName("thoigiancapnhat");

            entity.HasOne(d => d.IdGhNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdGh)
                .HasConstraintName("chitietgiohang_fk_1");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdSp)
                .HasConstraintName("chitietgiohang_fk_2");
        });

        modelBuilder.Entity<Danhgia>(entity =>
        {
            entity.HasKey(e => e.IdDg).HasName("PRIMARY");

            entity.ToTable("danhgia");

            entity.HasIndex(e => e.IdKh, "fk_danhgia_khachhang");

            entity.HasIndex(e => e.Ngaydanhgia, "idx_danhgia_ngaydanhgia");

            entity.HasIndex(e => e.Sosao, "idx_danhgia_sosao");

            entity.Property(e => e.IdDg).HasMaxLength(10);
            entity.Property(e => e.IdKh).HasMaxLength(10);
            entity.Property(e => e.Ngaydanhgia)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("ngaydanhgia");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");
            entity.Property(e => e.Sosao)
                .HasColumnType("int(5)")
                .HasColumnName("sosao");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("fk_danhgia_khachhang");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.IdDh).HasName("PRIMARY");

            entity.ToTable("donhang");

            entity.HasIndex(e => e.IdKh, "IdKh");

            entity.HasIndex(e => e.IdMgg, "IdMgg");

            entity.HasIndex(e => e.Ngaydathang, "idx_donhang_ngaydathang");

            entity.HasIndex(e => e.Trangthai, "idx_donhang_trangthai");

            entity.Property(e => e.IdDh).HasMaxLength(10);
            entity.Property(e => e.Diachigiaohang)
                .HasMaxLength(200)
                .HasColumnName("diachigiaohang");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(500)
                .HasColumnName("ghichu");
            entity.Property(e => e.IdKh).HasMaxLength(10);
            entity.Property(e => e.IdMgg).HasMaxLength(10);
            entity.Property(e => e.LydoHuy)
                .HasColumnType("text")
                .HasColumnName("lydo_huy");
            entity.Property(e => e.Ngaydathang)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("ngaydathang");
            entity.Property(e => e.Phuongthucthanhtoan)
                .HasMaxLength(50)
                .HasColumnName("phuongthucthanhtoan");
            entity.Property(e => e.Tongtien)
                .HasPrecision(18, 2)
                .HasColumnName("tongtien");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Đặt hàng thành công'")
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("donhang_ibfk_1");

            entity.HasOne(d => d.IdMggNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdMgg)
                .HasConstraintName("donhang_ibfk_2");
        });

        modelBuilder.Entity<Giohang>(entity =>
        {
            entity.HasKey(e => e.IdGh).HasName("PRIMARY");

            entity.ToTable("giohang");

            entity.HasIndex(e => e.IdKh, "IdKh");

            entity.Property(e => e.IdGh).HasMaxLength(10);
            entity.Property(e => e.IdKh).HasMaxLength(10);
            entity.Property(e => e.Thoigiancapnhat)
                .HasColumnType("datetime")
                .HasColumnName("thoigiancapnhat");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Giohangs)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("giohang_fk_1");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.IdKh).HasName("PRIMARY");

            entity.ToTable("khachhang");

            entity.HasIndex(e => e.IdTk, "IdTk");

            entity.HasIndex(e => e.IdXephangvip, "id_xephangvip");

            entity.HasIndex(e => e.Email, "idx_khachhang_email");

            entity.HasIndex(e => e.Sodienthoai, "idx_khachhang_sodienthoai");

            entity.Property(e => e.IdKh).HasMaxLength(10);
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("diachi");
            entity.Property(e => e.Diemtichluy)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("diemtichluy");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(5)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("hoten");
            entity.Property(e => e.IdTk).HasMaxLength(10);
            entity.Property(e => e.IdXephangvip)
                .HasMaxLength(10)
                .HasColumnName("id_xephangvip");
            entity.Property(e => e.Loaikhachhang)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("loaikhachhang");
            entity.Property(e => e.Ngaysinh).HasColumnName("ngaysinh");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(11)
                .HasColumnName("sodienthoai")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            entity.HasOne(d => d.IdTkNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.IdTk)
                .HasConstraintName("khachhang_ibfk_1");

            entity.HasOne(d => d.IdXephangvipNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.IdXephangvip)
                .HasConstraintName("khachhang_ibfk_2");
        });

        modelBuilder.Entity<Magiamgia>(entity =>
        {
            entity.HasKey(e => e.IdMgg).HasName("PRIMARY");

            entity.ToTable("magiamgia");

            entity.Property(e => e.IdMgg).HasMaxLength(10);
            entity.Property(e => e.Ngayhethan).HasColumnName("ngayhethan");
            entity.Property(e => e.Ngaysudung).HasColumnName("ngaysudung");
            entity.Property(e => e.Soluong)
                .HasColumnType("int(11)")
                .HasColumnName("soluong");
            entity.Property(e => e.Ten)
                .HasMaxLength(100)
                .HasColumnName("ten");
            entity.Property(e => e.Tilechietkhau)
                .HasPrecision(5, 2)
                .HasColumnName("tilechietkhau");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PRIMARY");

            entity.ToTable("nhanvien");

            entity.HasIndex(e => e.Idtk, "fk_nhanvien_taikhoan");

            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Chucvu)
                .HasMaxLength(50)
                .HasColumnName("chucvu")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("diachi")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(5)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("hoten")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Idtk)
                .HasMaxLength(10)
                .HasColumnName("idtk");
            entity.Property(e => e.Luong)
                .HasPrecision(18, 2)
                .HasColumnName("luong");
            entity.Property(e => e.Ngayvaolam).HasColumnName("ngayvaolam");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(11)
                .HasColumnName("sodienthoai")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            entity.HasOne(d => d.IdtkNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.Idtk)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_nhanvien_taikhoan");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.IdSp).HasName("PRIMARY");

            entity.ToTable("sanpham");

            entity.HasIndex(e => e.Gia, "idx_sanpham_gia");

            entity.HasIndex(e => e.Tensanpham, "idx_sanpham_tensanpham");

            entity.HasIndex(e => e.Thuonghieu, "idx_sanpham_thuonghieu");

            entity.Property(e => e.IdSp).HasMaxLength(10);
            entity.Property(e => e.Damuahang)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("damuahang");
            entity.Property(e => e.Gia)
                .HasPrecision(18, 2)
                .HasColumnName("gia");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(255)
                .HasColumnName("hinhanh");
            entity.Property(e => e.Loaisanpham)
                .HasMaxLength(50)
                .HasColumnName("loaisanpham");
            entity.Property(e => e.Mota)
                .HasColumnType("text")
                .HasColumnName("mota");
            entity.Property(e => e.Soluongton)
                .HasColumnType("int(11)")
                .HasColumnName("soluongton");
            entity.Property(e => e.Soluotxem)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("soluotxem");
            entity.Property(e => e.Tensanpham)
                .HasMaxLength(200)
                .HasColumnName("tensanpham");
            entity.Property(e => e.Thongsokythuat)
                .HasColumnType("text")
                .HasColumnName("thongsokythuat");
            entity.Property(e => e.Thuonghieu)
                .HasMaxLength(100)
                .HasColumnName("thuonghieu");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.IdTk).HasName("PRIMARY");

            entity.ToTable("taikhoan");

            entity.HasIndex(e => e.Quyentruycap, "idx_taikhoan_quyentruycap");

            entity.HasIndex(e => e.Tentaikhoan, "idx_taikhoan_tentaikhoan");

            entity.Property(e => e.IdTk).HasMaxLength(10);
            entity.Property(e => e.Matkhau)
                .HasMaxLength(255)
                .HasColumnName("matkhau");
            entity.Property(e => e.Ngaysuadoi).HasColumnName("ngaysuadoi");
            entity.Property(e => e.Ngaytaotk).HasColumnName("ngaytaotk");
            entity.Property(e => e.Quyentruycap)
                .HasMaxLength(50)
                .HasColumnName("quyentruycap");
            entity.Property(e => e.Tentaikhoan)
                .HasMaxLength(50)
                .HasColumnName("tentaikhoan");
        });

        modelBuilder.Entity<Thanhtoan>(entity =>
        {
            entity.HasKey(e => e.IdTt).HasName("PRIMARY");

            entity.ToTable("thanhtoan");

            entity.HasIndex(e => e.IdDh, "IdDh");

            entity.HasIndex(e => e.Ngaythanhtoan, "idx_thanhtoan_ngaythanhtoan");

            entity.HasIndex(e => e.Trangthai, "idx_thanhtoan_trangthai");

            entity.Property(e => e.IdTt).HasMaxLength(10);
            entity.Property(e => e.IdDh).HasMaxLength(10);
            entity.Property(e => e.Mathanhtoan)
                .HasMaxLength(50)
                .HasColumnName("mathanhtoan");
            entity.Property(e => e.Ngaythanhtoan)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("ngaythanhtoan");
            entity.Property(e => e.Noidungthanhtoan)
                .HasMaxLength(200)
                .HasColumnName("noidungthanhtoan");
            entity.Property(e => e.Tienthanhtoan)
                .HasPrecision(18, 2)
                .HasColumnName("tienthanhtoan");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdDhNavigation).WithMany(p => p.Thanhtoans)
                .HasForeignKey(d => d.IdDh)
                .HasConstraintName("thanhtoan_ibfk_1");
        });

        modelBuilder.Entity<Xephangvip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("xephangvip");

            entity.HasIndex(e => e.Diemtoida, "idx_xephangvip_diemtoida");

            entity.HasIndex(e => e.Diemtoithieu, "idx_xephangvip_diemtoithieu");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("id");
            entity.Property(e => e.Diemtoida)
                .HasColumnType("int(11)")
                .HasColumnName("diemtoida");
            entity.Property(e => e.Diemtoithieu)
                .HasColumnType("int(11)")
                .HasColumnName("diemtoithieu");
            entity.Property(e => e.Phantramgiamgia)
                .HasPrecision(5, 2)
                .HasColumnName("phantramgiamgia");
            entity.Property(e => e.Tenhang)
                .HasMaxLength(50)
                .HasColumnName("tenhang");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
