using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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

    public virtual DbSet<Danhgium> Danhgia { get; set; }

    public virtual DbSet<Doitradh> Doitradhs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Giohang> Giohangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Magiamgium> Magiamgia { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Thanhtoan> Thanhtoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=WebBanLinhKien;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Chitietdonhang>(entity =>
        {
            entity.HasKey(e => new { e.IdDh, e.IdSp })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("chitietdonhang");

            entity.HasIndex(e => e.IdSp, "idSP");

            entity.Property(e => e.IdDh)
                .HasMaxLength(10)
                .HasColumnName("idDH");
            entity.Property(e => e.IdSp)
                .HasMaxLength(10)
                .HasColumnName("idSP");
            entity.Property(e => e.Dongia)
                .HasPrecision(10, 2)
                .HasColumnName("dongia");
            entity.Property(e => e.Soluong)
                .HasColumnType("int(11)")
                .HasColumnName("soluong");

            entity.HasOne(d => d.IdDhNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietdonhang_ibfk_1");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.IdSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietdonhang_ibfk_2");
        });

        modelBuilder.Entity<Chitietgiohang>(entity =>
        {
            entity.HasKey(e => new { e.IdGh, e.IdSp })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("chitietgiohang");

            entity.HasIndex(e => e.IdSp, "idSP");

            entity.Property(e => e.IdGh)
                .HasMaxLength(10)
                .HasColumnName("idGH");
            entity.Property(e => e.IdSp)
                .HasMaxLength(10)
                .HasColumnName("idSP");
            entity.Property(e => e.Soluong)
                .HasColumnType("int(11)")
                .HasColumnName("soluong");

            entity.HasOne(d => d.IdGhNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdGh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietgiohang_ibfk_1");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.Chitietgiohangs)
                .HasForeignKey(d => d.IdSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietgiohang_ibfk_2");
        });

        modelBuilder.Entity<Danhgium>(entity =>
        {
            entity.HasKey(e => e.IdDg).HasName("PRIMARY");

            entity.ToTable("danhgia");

            entity.HasIndex(e => new { e.IdKh, e.IdSp }, "idKH").IsUnique();

            entity.HasIndex(e => e.IdSp, "idSP");

            entity.Property(e => e.IdDg)
                .HasMaxLength(10)
                .HasColumnName("idDG");
            entity.Property(e => e.IdKh)
                .HasMaxLength(10)
                .HasColumnName("idKH");
            entity.Property(e => e.IdSp)
                .HasMaxLength(10)
                .HasColumnName("idSP");
            entity.Property(e => e.Ngaydanhgia).HasColumnName("ngaydanhgia");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");
            entity.Property(e => e.Sosao)
                .HasColumnType("int(11)")
                .HasColumnName("sosao");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.IdKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("danhgia_ibfk_1");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.IdSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("danhgia_ibfk_2");
        });

        modelBuilder.Entity<Doitradh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("doitradh");

            entity.HasIndex(e => e.IdDh, "idDH");

            entity.HasIndex(e => e.IdKh, "idKH");

            entity.HasIndex(e => e.IdNv, "idNV");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("id");
            entity.Property(e => e.Ghichu)
                .HasColumnType("text")
                .HasColumnName("ghichu");
            entity.Property(e => e.IdDh)
                .HasMaxLength(10)
                .HasColumnName("idDH");
            entity.Property(e => e.IdKh)
                .HasMaxLength(10)
                .HasColumnName("idKH");
            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .HasColumnName("idNV");
            entity.Property(e => e.Lydo)
                .HasMaxLength(200)
                .HasColumnName("lydo");
            entity.Property(e => e.Ngayxuly).HasColumnName("ngayxuly");
            entity.Property(e => e.Ngayyeucau).HasColumnName("ngayyeucau");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdDhNavigation).WithMany(p => p.Doitradhs)
                .HasForeignKey(d => d.IdDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doitradh_ibfk_3");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Doitradhs)
                .HasForeignKey(d => d.IdKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doitradh_ibfk_1");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Doitradhs)
                .HasForeignKey(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doitradh_ibfk_2");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.IdDh).HasName("PRIMARY");

            entity.ToTable("donhang");

            entity.HasIndex(e => e.IdKh, "idKH");

            entity.HasIndex(e => e.IdMgg, "idMGG");

            entity.HasIndex(e => e.IdNv, "idNV");

            entity.Property(e => e.IdDh)
                .HasMaxLength(10)
                .HasColumnName("idDH");
            entity.Property(e => e.Diachigiaohang)
                .HasMaxLength(200)
                .HasColumnName("diachigiaohang");
            entity.Property(e => e.IdKh)
                .HasMaxLength(10)
                .HasColumnName("idKH");
            entity.Property(e => e.IdMgg)
                .HasMaxLength(10)
                .HasColumnName("idMGG");
            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .HasColumnName("idNV");
            entity.Property(e => e.Ngaydathang).HasColumnName("ngaydathang");
            entity.Property(e => e.Phuongthucthanhtoan)
                .HasMaxLength(50)
                .HasColumnName("phuongthucthanhtoan");
            entity.Property(e => e.Tongtien)
                .HasPrecision(10, 2)
                .HasColumnName("tongtien");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("donhang_ibfk_1");

            entity.HasOne(d => d.IdMggNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdMgg)
                .HasConstraintName("donhang_ibfk_2");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("donhang_ibfk_3");
        });

        modelBuilder.Entity<Giohang>(entity =>
        {
            entity.HasKey(e => e.IdGh).HasName("PRIMARY");

            entity.ToTable("giohang");

            entity.HasIndex(e => e.IdKh, "idKH");

            entity.Property(e => e.IdGh)
                .HasMaxLength(10)
                .HasColumnName("idGH");
            entity.Property(e => e.IdKh)
                .HasMaxLength(10)
                .HasColumnName("idKH");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Giohangs)
                .HasForeignKey(d => d.IdKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("giohang_ibfk_1");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.IdKh).HasName("PRIMARY");

            entity.ToTable("khachhang");

            entity.HasIndex(e => e.IdTk, "idTK");

            entity.HasIndex(e => e.Hoten, "idx_khachhang_hoten");

            entity.Property(e => e.IdKh)
                .HasMaxLength(10)
                .HasColumnName("idKH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("diachi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(10)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("hoten");
            entity.Property(e => e.IdTk)
                .HasMaxLength(10)
                .HasColumnName("idTK");
            entity.Property(e => e.Ngaysinh).HasColumnName("ngaysinh");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(15)
                .HasColumnName("sodienthoai");

            entity.HasOne(d => d.IdTkNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.IdTk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("khachhang_ibfk_1");
        });

        modelBuilder.Entity<Magiamgium>(entity =>
        {
            entity.HasKey(e => e.IdMgg).HasName("PRIMARY");

            entity.ToTable("magiamgia");

            entity.HasIndex(e => e.IdNv, "idNV");

            entity.Property(e => e.IdMgg)
                .HasMaxLength(10)
                .HasColumnName("idMGG");
            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .HasColumnName("idNV");
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

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Magiamgia)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("magiamgia_ibfk_1");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PRIMARY");

            entity.ToTable("nhanvien");

            entity.HasIndex(e => e.IdTk, "idTK");

            entity.HasIndex(e => e.Hoten, "idx_nhanvien_hoten");

            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .HasColumnName("idNV");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(10)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("hoten");
            entity.Property(e => e.IdTk)
                .HasMaxLength(10)
                .HasColumnName("idTK");
            entity.Property(e => e.Ngaybatdaulam).HasColumnName("ngaybatdaulam");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(15)
                .HasColumnName("sodienthoai");

            entity.HasOne(d => d.IdTkNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.IdTk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nhanvien_ibfk_1");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.IdSp).HasName("PRIMARY");

            entity.ToTable("sanpham");

            entity.HasIndex(e => e.IdNv, "idNV");

            entity.HasIndex(e => e.TenSp, "idx_sanpham_tenSP");

            entity.Property(e => e.IdSp)
                .HasMaxLength(10)
                .HasColumnName("idSP");
            entity.Property(e => e.Gia)
                .HasPrecision(10, 2)
                .HasColumnName("gia");
            entity.Property(e => e.IdNv)
                .HasMaxLength(10)
                .HasColumnName("idNV");
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(50)
                .HasColumnName("loaiSP");
            entity.Property(e => e.MoTa)
                .HasColumnType("text")
                .HasColumnName("moTa");
            entity.Property(e => e.SoLuongTon)
                .HasColumnType("int(11)")
                .HasColumnName("soLuongTon");
            entity.Property(e => e.TenSp)
                .HasMaxLength(200)
                .HasColumnName("tenSP");
            entity.Property(e => e.ThongSoKyThuat)
                .HasColumnType("text")
                .HasColumnName("thongSoKyThuat");
            entity.Property(e => e.ThuongHieu)
                .HasMaxLength(100)
                .HasColumnName("thuongHieu");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.IdNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sanpham_ibfk_1");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.IdTk).HasName("PRIMARY");

            entity.ToTable("taikhoan");

            entity.Property(e => e.IdTk)
                .HasMaxLength(10)
                .HasColumnName("idTK");
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

            entity.HasIndex(e => e.IdDh, "idDH");

            entity.Property(e => e.IdTt)
                .HasMaxLength(10)
                .HasColumnName("idTT");
            entity.Property(e => e.IdDh)
                .HasMaxLength(10)
                .HasColumnName("idDH");
            entity.Property(e => e.Mathanhtoan)
                .HasMaxLength(50)
                .HasColumnName("mathanhtoan");
            entity.Property(e => e.Ngaythanhtoan).HasColumnName("ngaythanhtoan");
            entity.Property(e => e.Noidungthanhtoan)
                .HasMaxLength(200)
                .HasColumnName("noidungthanhtoan");
            entity.Property(e => e.Tienthanhtoan)
                .HasPrecision(10, 2)
                .HasColumnName("tienthanhtoan");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdDhNavigation).WithMany(p => p.Thanhtoans)
                .HasForeignKey(d => d.IdDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("thanhtoan_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
