-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 26, 2025 at 03:19 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `webbanlinhkien`
--

-- --------------------------------------------------------

--
-- Table structure for table `chitietdonhang`
--

CREATE TABLE `chitietdonhang` (
  `idDH` varchar(10) NOT NULL,
  `idSP` varchar(10) NOT NULL,
  `soluong` int(11) NOT NULL,
  `dongia` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `chitietdonhang`
--

INSERT INTO `chitietdonhang` (`idDH`, `idSP`, `soluong`, `dongia`) VALUES
('DH001', 'SP001', 1, 5500000.00),
('DH001', 'SP002', 2, 1200000.00);

-- --------------------------------------------------------

--
-- Table structure for table `chitietgiohang`
--

CREATE TABLE `chitietgiohang` (
  `idGH` varchar(10) NOT NULL,
  `idSP` varchar(10) NOT NULL,
  `soluong` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `chitietgiohang`
--

INSERT INTO `chitietgiohang` (`idGH`, `idSP`, `soluong`) VALUES
('GH001', 'SP001', 1),
('GH001', 'SP002', 2);

-- --------------------------------------------------------

--
-- Table structure for table `danhgia`
--

CREATE TABLE `danhgia` (
  `idDG` varchar(10) NOT NULL,
  `sosao` int(11) NOT NULL CHECK (`sosao` between 1 and 5),
  `noidung` text DEFAULT NULL,
  `ngaydanhgia` date NOT NULL,
  `idKH` varchar(10) NOT NULL,
  `idSP` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `danhgia`
--

INSERT INTO `danhgia` (`idDG`, `sosao`, `noidung`, `ngaydanhgia`, `idKH`, `idSP`) VALUES
('DG001', 4, 'Sản phẩm tốt, giao hàng nhanh', '2024-01-25', 'KH001', 'SP001');

-- --------------------------------------------------------

--
-- Table structure for table `doitradh`
--

CREATE TABLE `doitradh` (
  `id` varchar(10) NOT NULL,
  `trangthai` varchar(50) NOT NULL,
  `lydo` varchar(200) NOT NULL,
  `ngayyeucau` date NOT NULL,
  `ngayxuly` date DEFAULT NULL,
  `ghichu` text DEFAULT NULL,
  `idKH` varchar(10) NOT NULL,
  `idNV` varchar(10) NOT NULL,
  `idDH` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `doitradh`
--

INSERT INTO `doitradh` (`id`, `trangthai`, `lydo`, `ngayyeucau`, `ngayxuly`, `ghichu`, `idKH`, `idNV`, `idDH`) VALUES
('DTR001', 'Chờ Xử Lý', 'Sản phẩm bị lỗi', '2024-01-22', NULL, NULL, 'KH001', 'NV001', 'DH001');

-- --------------------------------------------------------

--
-- Table structure for table `donhang`
--

CREATE TABLE `donhang` (
  `idDH` varchar(10) NOT NULL,
  `trangthai` varchar(50) NOT NULL,
  `tongtien` decimal(10,2) NOT NULL,
  `diachigiaohang` varchar(200) NOT NULL,
  `ngaydathang` date NOT NULL,
  `phuongthucthanhtoan` varchar(50) NOT NULL,
  `idKH` varchar(10) NOT NULL,
  `idMGG` varchar(10) DEFAULT NULL,
  `idNV` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `donhang`
--

INSERT INTO `donhang` (`idDH`, `trangthai`, `tongtien`, `diachigiaohang`, `ngaydathang`, `phuongthucthanhtoan`, `idKH`, `idMGG`, `idNV`) VALUES
('DH001', 'Đang Xử Lý', 6700000.00, 'Hà Nội', '2024-01-20', 'Chuyển Khoản', 'KH001', 'MGG001', 'NV001');

-- --------------------------------------------------------

--
-- Table structure for table `giohang`
--

CREATE TABLE `giohang` (
  `idGH` varchar(10) NOT NULL,
  `idKH` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `giohang`
--

INSERT INTO `giohang` (`idGH`, `idKH`) VALUES
('GH001', 'KH001');

-- --------------------------------------------------------

--
-- Table structure for table `khachhang`
--

CREATE TABLE `khachhang` (
  `idKH` varchar(10) NOT NULL,
  `hoten` varchar(100) NOT NULL,
  `diachi` varchar(200) NOT NULL,
  `email` varchar(100) NOT NULL,
  `gioitinh` varchar(10) NOT NULL,
  `ngaysinh` date NOT NULL,
  `sodienthoai` varchar(15) NOT NULL,
  `idTK` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `khachhang`
--

INSERT INTO `khachhang` (`idKH`, `hoten`, `diachi`, `email`, `gioitinh`, `ngaysinh`, `sodienthoai`, `idTK`) VALUES
('KH001', 'Lê Văn Cường', 'Hà Nội', 'cuong.le@email.com', 'Nam', '1990-05-15', '0923456789', 'TK003');

-- --------------------------------------------------------

--
-- Table structure for table `magiamgia`
--

CREATE TABLE `magiamgia` (
  `idMGG` varchar(10) NOT NULL,
  `ten` varchar(100) NOT NULL,
  `ngaysudung` date NOT NULL,
  `ngayhethan` date NOT NULL,
  `tilechietkhau` decimal(5,2) NOT NULL,
  `soluong` int(11) NOT NULL,
  `idNV` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `magiamgia`
--

INSERT INTO `magiamgia` (`idMGG`, `ten`, `ngaysudung`, `ngayhethan`, `tilechietkhau`, `soluong`, `idNV`) VALUES
('MGG001', 'Khuyến mãi Tết', '2024-01-20', '2024-02-15', 0.10, 100, 'NV001');

-- --------------------------------------------------------

--
-- Table structure for table `nhanvien`
--

CREATE TABLE `nhanvien` (
  `idNV` varchar(10) NOT NULL,
  `hoten` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `sodienthoai` varchar(15) NOT NULL,
  `ngaybatdaulam` date NOT NULL,
  `gioitinh` varchar(10) NOT NULL,
  `idTK` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `nhanvien`
--

INSERT INTO `nhanvien` (`idNV`, `hoten`, `email`, `sodienthoai`, `ngaybatdaulam`, `gioitinh`, `idTK`) VALUES
('NV001', 'Nguyễn Văn An', 'an.nguyen@company.com', '0901234567', '2024-01-01', 'Nam', 'TK001'),
('NV002', 'Trần Thị Bích', 'bich.tran@company.com', '0912345678', '2024-01-02', 'Nữ', 'TK002');

-- --------------------------------------------------------

--
-- Table structure for table `sanpham`
--

CREATE TABLE `sanpham` (
  `idSP` varchar(10) NOT NULL,
  `tenSP` varchar(200) NOT NULL,
  `gia` decimal(10,2) NOT NULL,
  `soLuongTon` int(11) NOT NULL,
  `thuongHieu` varchar(100) NOT NULL,
  `moTa` text DEFAULT NULL,
  `thongSoKyThuat` text DEFAULT NULL,
  `loaiSP` varchar(50) NOT NULL,
  `idNV` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sanpham`
--

INSERT INTO `sanpham` (`idSP`, `tenSP`, `gia`, `soLuongTon`, `thuongHieu`, `moTa`, `thongSoKyThuat`, `loaiSP`, `idNV`) VALUES
('SP001', 'Mainboard ASUS ROG', 5500000.00, 50, 'ASUS', 'Mainboard chơi game cao cấp', 'CPU Socket: LGA 1200, Chipset: Intel Z490', 'Linh Kiện Máy Tính', 'NV001'),
('SP002', 'RAM Corsair 16GB', 1200000.00, 100, 'Corsair', 'RAM dung lượng cao', 'DDR4, 3200MHz, 16GB', 'Linh Kiện Máy Tính', 'NV001');

-- --------------------------------------------------------

--
-- Table structure for table `taikhoan`
--

CREATE TABLE `taikhoan` (
  `idTK` varchar(10) NOT NULL,
  `matkhau` varchar(255) NOT NULL,
  `tentaikhoan` varchar(50) NOT NULL,
  `ngaytaotk` date NOT NULL,
  `ngaysuadoi` date DEFAULT NULL,
  `quyentruycap` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `taikhoan`
--

INSERT INTO `taikhoan` (`idTK`, `matkhau`, `tentaikhoan`, `ngaytaotk`, `ngaysuadoi`, `quyentruycap`) VALUES
('TK001', 'hashed_password1', 'admin_user', '2024-01-15', NULL, 'ADMIN'),
('TK002', 'hashed_password2', 'staff_user', '2024-01-16', NULL, 'STAFF'),
('TK003', 'hashed_password3', 'customer1', '2024-01-17', NULL, 'CUSTOMER');

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `idTT` varchar(10) NOT NULL,
  `mathanhtoan` varchar(50) NOT NULL,
  `trangthai` varchar(50) NOT NULL,
  `tienthanhtoan` decimal(10,2) NOT NULL,
  `ngaythanhtoan` date NOT NULL,
  `noidungthanhtoan` varchar(200) DEFAULT NULL,
  `idDH` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `thanhtoan`
--

INSERT INTO `thanhtoan` (`idTT`, `mathanhtoan`, `trangthai`, `tienthanhtoan`, `ngaythanhtoan`, `noidungthanhtoan`, `idDH`) VALUES
('TT001', 'BANK_TRANSFER_001', 'Thành Công', 6700000.00, '2024-01-20', 'Thanh toán đơn hàng DH001', 'DH001');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD PRIMARY KEY (`idDH`,`idSP`),
  ADD KEY `idSP` (`idSP`);

--
-- Indexes for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD PRIMARY KEY (`idGH`,`idSP`),
  ADD KEY `idSP` (`idSP`);

--
-- Indexes for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD PRIMARY KEY (`idDG`),
  ADD UNIQUE KEY `idKH` (`idKH`,`idSP`),
  ADD KEY `idSP` (`idSP`);

--
-- Indexes for table `doitradh`
--
ALTER TABLE `doitradh`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idKH` (`idKH`),
  ADD KEY `idNV` (`idNV`),
  ADD KEY `idDH` (`idDH`);

--
-- Indexes for table `donhang`
--
ALTER TABLE `donhang`
  ADD PRIMARY KEY (`idDH`),
  ADD KEY `idKH` (`idKH`),
  ADD KEY `idMGG` (`idMGG`),
  ADD KEY `idNV` (`idNV`);

--
-- Indexes for table `giohang`
--
ALTER TABLE `giohang`
  ADD PRIMARY KEY (`idGH`),
  ADD KEY `idKH` (`idKH`);

--
-- Indexes for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`idKH`),
  ADD KEY `idTK` (`idTK`),
  ADD KEY `idx_khachhang_hoten` (`hoten`);

--
-- Indexes for table `magiamgia`
--
ALTER TABLE `magiamgia`
  ADD PRIMARY KEY (`idMGG`),
  ADD KEY `idNV` (`idNV`);

--
-- Indexes for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`idNV`),
  ADD KEY `idTK` (`idTK`),
  ADD KEY `idx_nhanvien_hoten` (`hoten`);

--
-- Indexes for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`idSP`),
  ADD KEY `idNV` (`idNV`),
  ADD KEY `idx_sanpham_tenSP` (`tenSP`);

--
-- Indexes for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`idTK`);

--
-- Indexes for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`idTT`),
  ADD KEY `idDH` (`idDH`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD CONSTRAINT `chitietdonhang_ibfk_1` FOREIGN KEY (`idDH`) REFERENCES `donhang` (`idDH`),
  ADD CONSTRAINT `chitietdonhang_ibfk_2` FOREIGN KEY (`idSP`) REFERENCES `sanpham` (`idSP`);

--
-- Constraints for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD CONSTRAINT `chitietgiohang_ibfk_1` FOREIGN KEY (`idGH`) REFERENCES `giohang` (`idGH`),
  ADD CONSTRAINT `chitietgiohang_ibfk_2` FOREIGN KEY (`idSP`) REFERENCES `sanpham` (`idSP`);

--
-- Constraints for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD CONSTRAINT `danhgia_ibfk_1` FOREIGN KEY (`idKH`) REFERENCES `khachhang` (`idKH`),
  ADD CONSTRAINT `danhgia_ibfk_2` FOREIGN KEY (`idSP`) REFERENCES `sanpham` (`idSP`);

--
-- Constraints for table `doitradh`
--
ALTER TABLE `doitradh`
  ADD CONSTRAINT `doitradh_ibfk_1` FOREIGN KEY (`idKH`) REFERENCES `khachhang` (`idKH`),
  ADD CONSTRAINT `doitradh_ibfk_2` FOREIGN KEY (`idNV`) REFERENCES `nhanvien` (`idNV`),
  ADD CONSTRAINT `doitradh_ibfk_3` FOREIGN KEY (`idDH`) REFERENCES `donhang` (`idDH`);

--
-- Constraints for table `donhang`
--
ALTER TABLE `donhang`
  ADD CONSTRAINT `donhang_ibfk_1` FOREIGN KEY (`idKH`) REFERENCES `khachhang` (`idKH`),
  ADD CONSTRAINT `donhang_ibfk_2` FOREIGN KEY (`idMGG`) REFERENCES `magiamgia` (`idMGG`),
  ADD CONSTRAINT `donhang_ibfk_3` FOREIGN KEY (`idNV`) REFERENCES `nhanvien` (`idNV`);

--
-- Constraints for table `giohang`
--
ALTER TABLE `giohang`
  ADD CONSTRAINT `giohang_ibfk_1` FOREIGN KEY (`idKH`) REFERENCES `khachhang` (`idKH`);

--
-- Constraints for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD CONSTRAINT `khachhang_ibfk_1` FOREIGN KEY (`idTK`) REFERENCES `taikhoan` (`idTK`);

--
-- Constraints for table `magiamgia`
--
ALTER TABLE `magiamgia`
  ADD CONSTRAINT `magiamgia_ibfk_1` FOREIGN KEY (`idNV`) REFERENCES `nhanvien` (`idNV`);

--
-- Constraints for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD CONSTRAINT `nhanvien_ibfk_1` FOREIGN KEY (`idTK`) REFERENCES `taikhoan` (`idTK`);

--
-- Constraints for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `sanpham_ibfk_1` FOREIGN KEY (`idNV`) REFERENCES `nhanvien` (`idNV`);

--
-- Constraints for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD CONSTRAINT `thanhtoan_ibfk_1` FOREIGN KEY (`idDH`) REFERENCES `donhang` (`idDH`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
