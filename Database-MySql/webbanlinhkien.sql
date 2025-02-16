-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 12, 2025 at 10:47 AM
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
  `Idchitietdonhang` varchar(10) NOT NULL,
  `IdDh` varchar(10) NOT NULL,
  `IdSp` varchar(10) NOT NULL,
  `IdDg` varchar(10) DEFAULT NULL,
  `soluongsanpham` int(11) NOT NULL,
  `dongia` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `chitietdonhang`
--

INSERT INTO `chitietdonhang` (`Idchitietdonhang`, `IdDh`, `IdSp`, `IdDg`, `soluongsanpham`, `dongia`) VALUES
('CTDH000001', 'DH000001', 'SP000001', NULL, 2, 15990000.00),
('CTDH000002', 'DH000001', 'SP000002', NULL, 1, 14990000.00),
('CTDH000003', 'DH000002', 'SP000003', NULL, 1, 7990000.00),
('CTDH000004', 'DH000002', 'SP000004', NULL, 1, 12990000.00),
('CTDH000005', 'DH000003', 'SP000001', NULL, 1, 15990000.00),
('CTDH000006', 'DH000003', 'SP000003', NULL, 2, 7990000.00),
('CTDH000007', 'DH000004', 'SP000002', NULL, 2, 14990000.00),
('CTDH000008', 'DH000004', 'SP000004', NULL, 1, 12990000.00),
('CTDH000009', 'DH000005', 'SP000018', NULL, 1, 2990000.00),
('CTDH000010', 'DH000006', 'SP000003', NULL, 1, 7990000.00),
('CTDH000011', 'DH000007', 'SP000004', NULL, 2, 12990000.00),
('CTDH000012', 'DH000008', 'SP000001', NULL, 2, 15990000.00),
('CTDH000013', 'DH000008', 'SP000002', NULL, 1, 14990000.00),
('CTDH000014', 'DH000009', 'SP000015', NULL, 1, 19990000.00),
('CTDH000015', 'DH000009', 'SP000016', NULL, 1, 59990000.00),
('CTDH000016', 'DH000010', 'SP000024', NULL, 1, 19990000.00),
('CTDH000017', 'DH000010', 'SP000025', NULL, 1, 12990000.00),
('CTDH000018', 'DH000011', 'SP000016', NULL, 1, 59990000.00),
('CTDH000019', 'DH000012', 'SP000018', NULL, 1, 2990000.00),
('CTDH000020', 'DH000013', 'SP000024', NULL, 1, 19990000.00),
('CTDH000021', 'DH000014', 'SP000025', NULL, 1, 12990000.00),
('CTDH000022', 'DH000015', 'SP000018', NULL, 1, 2990000.00),
('CTDH000023', 'DH000016', 'SP000025', NULL, 1, 12990000.00),
('CTDH000024', 'DH000016', 'SP000028', NULL, 1, 6990000.00),
('CTDH000025', 'DH000017', 'SP000003', NULL, 2, 7990000.00),
('CTDH000026', 'DH000019', 'SP000028', NULL, 1, 6990000.00),
('CTDH000027', 'DH000020', 'SP000018', NULL, 2, 2990000.00);

-- --------------------------------------------------------

--
-- Table structure for table `chitietgiohang`
--

CREATE TABLE `chitietgiohang` (
  `IdGh` varchar(10) NOT NULL,
  `IdSp` varchar(10) NOT NULL,
  `soluongsanpham` int(11) NOT NULL,
  `thoigiancapnhat` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `danhgia`
--

CREATE TABLE `danhgia` (
  `IdDg` varchar(10) NOT NULL,
  `sosao` int(5) NOT NULL CHECK (`sosao` between 1 and 5),
  `noidung` text DEFAULT NULL,
  `ngaydanhgia` datetime DEFAULT current_timestamp(),
  `IdKh` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `donhang`
--

CREATE TABLE `donhang` (
  `IdDh` varchar(10) NOT NULL,
  `trangthai` varchar(50) NOT NULL DEFAULT 'Đặt hàng thành công',
  `tongtien` decimal(10,2) NOT NULL,
  `diachigiaohang` varchar(200) NOT NULL,
  `ngaydathang` datetime DEFAULT current_timestamp(),
  `phuongthucthanhtoan` varchar(50) NOT NULL,
  `ghichu` varchar(500) DEFAULT NULL,
  `lydo_huy` text DEFAULT NULL,
  `IdKh` varchar(10) NOT NULL,
  `IdMgg` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `donhang`
--

INSERT INTO `donhang` (`IdDh`, `trangthai`, `tongtien`, `diachigiaohang`, `ngaydathang`, `phuongthucthanhtoan`, `ghichu`, `lydo_huy`, `IdKh`, `IdMgg`) VALUES
('DH000001', 'Giao thành công', 52490000.00, 'Hà Nội, Đống Đa', '2025-02-01 08:30:00', 'COD', NULL, NULL, 'KH000001', 'MG000001'),
('DH000002', 'Đang giao', 15990000.00, 'Hồ Chí Minh', '2025-02-02 09:15:00', 'VNPAY', 'Giao trong ngày', NULL, 'KH000002', 'MG000002'),
('DH000003', 'Đã duyệt đơn', 7990000.00, 'Đà Nẵng', '2025-02-03 10:45:00', 'VNPAY', NULL, NULL, 'KH000003', NULL),
('DH000004', 'Giao thành công', 49990000.00, 'Hải Phòng', '2025-02-04 13:20:00', 'Paypal', NULL, NULL, 'KH000004', 'MG000003'),
('DH000005', 'huy_don', 2990000.00, 'Hà Nội', '2025-02-05 14:30:00', 'Paypal', 'Khách đổi ý', 'Tôi không muốn đặt hàng nữa', 'KH000005', NULL),
('DH000006', 'Đặt hàng thành công', 8990000.00, 'Hồ Chí Minh', '2025-02-06 15:30:00', 'VNPAY', NULL, NULL, 'KH000006', NULL),
('DH000007', 'Đang giao', 25990000.00, 'Đà Nẵng', '2025-02-07 16:15:00', 'COD', NULL, NULL, 'KH000007', 'MG000004'),
('DH000008', 'Giao thành công', 39990000.00, 'Hải Phòng', '2025-02-08 17:45:00', 'VNPAY', NULL, NULL, 'KH000008', 'MG000005'),
('DH000009', 'Đang giao', 69990000.00, 'Hà Nội', '2025-02-09 18:20:00', 'COD', NULL, NULL, 'KH000009', NULL),
('DH000010', 'Đặt hàng thành công', 19990000.00, 'Hồ Chí Minh', '2025-02-10 19:30:00', 'Paypal', NULL, NULL, 'KH000010', 'MG000006'),
('DH000011', 'Giao thành công', 59990000.00, 'Đà Nẵng', '2025-02-11 20:15:00', 'VNPAY', NULL, NULL, 'KH000011', NULL),
('DH000012', 'Đang giao', 2990000.00, 'Hải Phòng', '2025-02-12 21:45:00', 'COD', NULL, NULL, 'KH000012', 'MG000007'),
('DH000013', 'Đã duyệt đơn', 19990000.00, 'Hà Nội', '2025-02-13 22:20:00', 'VNPAY', NULL, NULL, 'KH000013', NULL),
('DH000014', 'Giao thành công', 12990000.00, 'Hồ Chí Minh', '2025-02-14 07:30:00', 'Paypal', NULL, NULL, 'KH000014', 'MG000008'),
('DH000015', 'Đặt hàng thành công', 3990000.00, 'Đà Nẵng', '2025-02-15 08:15:00', 'COD', NULL, NULL, 'KH000015', NULL),
('DH000016', 'Đang giao', 25990000.00, 'Hải Phòng', '2025-02-16 09:45:00', 'VNPAY', NULL, NULL, 'KH000016', 'MG000009'),
('DH000017', 'Giao thành công', 14990000.00, 'Hà Nội', '2025-02-17 10:20:00', 'COD', NULL, NULL, 'KH000017', NULL),
('DH000018', 'Đặt hàng thành công', 12990000.00, 'Hồ Chí Minh', '2025-02-18 11:45:00', 'Paypal', NULL, NULL, 'KH000018', 'MG000010'),
('DH000019', 'Đang giao', 6990000.00, 'Đà Nẵng', '2025-02-19 12:20:00', 'VNPAY', NULL, NULL, 'KH000019', NULL),
('DH000020', 'Giao thành công', 4990000.00, 'Hải Phòng', '2025-02-20 13:30:00', 'COD', NULL, NULL, 'KH000001', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `giohang`
--

CREATE TABLE `giohang` (
  `IdGh` varchar(10) NOT NULL,
  `IdKh` varchar(10) NOT NULL,
  `thoigiancapnhat` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `giohang`
--

INSERT INTO `giohang` (`IdGh`, `IdKh`, `thoigiancapnhat`) VALUES
('GH000001', 'KH000001', NULL),
('GH000002', 'KH000002', NULL),
('GH000003', 'KH000003', NULL),
('GH000004', 'KH000004', NULL),
('GH000005', 'KH000005', NULL),
('GH000006', 'KH000006', NULL),
('GH000007', 'KH000007', NULL),
('GH000008', 'KH000008', NULL),
('GH000009', 'KH000009', NULL),
('GH000010', 'KH000010', NULL),
('GH000011', 'KH000011', NULL),
('GH000012', 'KH000012', NULL),
('GH000013', 'KH000013', NULL),
('GH000014', 'KH000014', NULL),
('GH000015', 'KH000015', NULL),
('GH000016', 'KH000016', NULL),
('GH000017', 'KH000017', NULL),
('GH000018', 'KH000018', NULL),
('GH000019', 'KH000019', NULL),
('GH000020', 'KH000001', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `khachhang`
--

CREATE TABLE `khachhang` (
  `IdKh` varchar(10) NOT NULL,
  `hoten` varchar(100) NOT NULL,
  `diachi` varchar(200) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `gioitinh` varchar(5) DEFAULT NULL,
  `ngaysinh` date DEFAULT NULL,
  `sodienthoai` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `diemtichluy` int(11) DEFAULT 0,
  `IdTk` varchar(10) NOT NULL,
  `id_xephangvip` varchar(10) DEFAULT NULL,
  `loaikhachhang` bit(1) DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `khachhang`
--

INSERT INTO `khachhang` (`IdKh`, `hoten`, `diachi`, `email`, `gioitinh`, `ngaysinh`, `sodienthoai`, `diemtichluy`, `IdTk`, `id_xephangvip`, `loaikhachhang`) VALUES
('KH000001', 'Nguyễn Văn A', 'Hà Nội', 'a@gmail.com', 'Nam', '1990-01-01', '0987654321', 100, 'TK000002', 'THANTHIET', 1),
('KH000002', 'Trần Thị B', 'Hồ Chí Minh', 'b@gmail.com', 'Nữ', '1995-02-15', '0912345678', 777, 'TK000003', 'BAC', 1),
('KH000003', 'Phạm Văn C', 'Đà Nẵng', 'c@gmail.com', 'Nam', '1992-03-20', '0934567890', 1002, 'TK000004', 'VANG', 1),
('KH000004', 'Kim Đăng D', 'Hải Phòng', 'd@gmail.com', 'Nữ', '1998-04-25', '0956789012', 10000, 'TK000005', 'KIMCUONG', 1),
('KH000005', 'Nguyễn Văn E', 'Hà Nội', 'e@gmail.com', 'Nam', '1993-05-30', '0978901234', 300, 'TK000006', 'THANTHIET', 1),
('KH000006', 'Phạm Hải A', 'Hồ Chí Minh', 'pha@gmail.com', 'Nữ', '2001-06-05', '0967890123', 600, 'TK000007', 'BAC', 1),
('KH000007', 'Trần Minh B', 'Đà Nẵng', 'tmb@gmail.com', 'Nam', '2000-07-10', '0945678901', 3333, 'TK000008', 'VANG', 1),
('KH000008', 'Nguyễn Hà C', 'Hải Phòng', 'nhc@gmail.com', 'Nữ', '2001-08-15', '0934567890', 30832, 'TK000009', 'KIMCUONG', 1),
('KH000009', 'Lê Văn D', 'Đà Nẵng', 'lvd@gmail.com', 'Nam', '2000-09-20', '0923456789', 450, 'TK000010', 'THANTHIET', 1),
('KH000010', 'Phạm Thị E', 'Hà Nội', 'pte@gmail.com', 'Nữ', '1996-10-25', '0912345678', 800, 'TK000011', 'BAC', 1),
('KH000011', 'Hoàng Văn F', 'Hồ Chí Minh', 'hvf@gmail.com', 'Nam', '1990-11-30', '0934567890', 2500, 'TK000012', 'VANG', 1),
('KH000012', 'Trần Thu G', 'Đà Nẵng', 'ttg@gmail.com', 'Nữ', '1994-12-05', '0945678901', 15000, 'TK000013', 'KIMCUONG', 1),
('KH000013', 'Nguyễn Nam H', 'Hà Nội', 'nnh@gmail.com', 'Nam', '1997-01-10', '0956789012', 350, 'TK000014', 'THANTHIET', 1),
('KH000014', 'Vũ Thị I', 'Hà Nội', 'vti@gmail.com', 'Nữ', '1992-02-15', '0967890123', 900, 'TK000015', 'BAC', 1),
('KH000015', 'Đặng Văn K', 'Hồ Chí Minh', 'dvk@gmail.com', 'Nam', '1995-03-20', '0978901234', 4000, 'TK000016', 'VANG', 1),
('KH000016', 'Mai Thị L', 'Đà Nẵng', 'mtl@gmail.com', 'Nữ', '1991-04-25', '0989012345', 20000, 'TK000017', 'KIMCUONG', 1),
('KH000017', 'Phan Văn M', 'Hải Phòng', 'pvm@gmail.com', 'Nam', '1998-05-30', '0990123456', 250, 'TK000018', 'THANTHIET', 1),
('KH000018', 'Trương Thị N', 'Hà Nội', 'ttn@gmail.com', 'Nữ', '2004-06-05', '0901234567', 1200, 'TK000019', 'BAC', 1),
('KH000019', 'Bùi Văn O', 'Hồ Chí Minh', 'bvo@gmail.com', 'Nam', '2002-07-10', '0912345678', 5000, 'TK000020', 'VANG', 1);

-- --------------------------------------------------------

--
-- Table structure for table `magiamgia`
--

CREATE TABLE `magiamgia` (
  `IdMgg` varchar(10) NOT NULL,
  `ten` varchar(100) NOT NULL,
  `ngaysudung` date NOT NULL,
  `ngayhethan` date NOT NULL,
  `tilechietkhau` decimal(5,2) NOT NULL,
  `soluong` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `magiamgia`
--

INSERT INTO `magiamgia` (`IdMgg`, `ten`, `ngaysudung`, `ngayhethan`, `tilechietkhau`, `soluong`) VALUES
('MG000001', 'Giảm giá đầu năm', '2025-01-01', '2025-01-31', 10.00, 100),
('MG000002', 'Mừng xuân 2025', '2025-02-01', '2025-02-15', 15.00, 50),
('MG000003', 'Khách hàng VIP', '2025-02-01', '2025-12-31', 20.00, 30),
('MG000004', 'Sinh nhật shop', '2025-03-01', '2025-03-31', 25.00, 40),
('MG000005', 'Mua hè sôi động', '2025-06-01', '2025-06-30', 12.00, 80),
('MG000006', 'Back to school', '2025-08-15', '2025-09-15', 18.00, 60),
('MG000007', 'Black Friday', '2025-11-20', '2025-11-30', 30.00, 20),
('MG000008', 'Noel 2025', '2025-12-20', '2025-12-25', 20.00, 45),
('MG000009', 'Tết 2026', '2026-01-20', '2026-02-05', 25.00, 55),
('MG000010', 'Valentine', '2025-02-10', '2025-02-14', 14.00, 70),
('MG000011', 'Mừng 8/3', '2025-03-05', '2025-03-08', 15.00, 65),
('MG000012', 'Quốc tế thiếu nhi', '2025-05-25', '2025-06-01', 10.00, 90),
('MG000013', 'Quốc khánh', '2025-09-01', '2025-09-03', 20.00, 40),
('MG000014', 'Halloween', '2025-10-25', '2025-10-31', 13.00, 75),
('MG000015', 'Cyber Monday', '2025-11-25', '2025-11-26', 28.00, 25),
('MG000016', 'Khách hàng mới', '2025-01-01', '2025-12-31', 10.00, 200),
('MG000017', 'Mua nhiều giảm nhiều', '2025-01-01', '2025-12-31', 15.00, 150),
('MG000018', 'Flash Sale', '2025-07-07', '2025-07-07', 35.00, 15),
('MG000019', 'Giờ vàng', '2025-04-15', '2025-04-15', 40.00, 10),
('MG000020', 'Cuối năm', '2025-12-26', '2025-12-31', 22.00, 35);

-- --------------------------------------------------------

--
-- Table structure for table `nhanvien`
--

CREATE TABLE `nhanvien` (
  `IdNv` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hoten` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `chucvu` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `luong` decimal(10,2) NOT NULL,
  `gioitinh` varchar(5) NOT NULL,
  `sodienthoai` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `diachi` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ngayvaolam` date NOT NULL,
  `idtk` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `nhanvien`
--

INSERT INTO `nhanvien` (`IdNv`, `hoten`, `chucvu`, `luong`, `gioitinh`, `sodienthoai`, `email`, `diachi`, `ngayvaolam`, `idtk`) VALUES
('NV0001', 'Nguyễn Văn A', 'Nhân viên bán hàng', 10000000.00, 'Nam', '0901234567', 'a@gmail.com', 'Hà Nội', '2023-01-01', NULL),
('NV0002', 'Trần Thị B', 'Nhân viên kho', 9000000.00, 'Nữ', '0912345678', 'b@gmail.com', 'Hồ Chí Minh', '2022-05-10', NULL),
('NV0003', 'Lê Văn C', 'Nhân viên kế toán', 12000000.00, 'Nam', '0923456789', 'c@gmail.com', 'Đà Nẵng', '2021-11-20', NULL),
('NV0004', 'Phạm Thị D', 'Nhân viên bán hàng', 11000000.00, 'Nữ', '0934567890', 'd@gmail.com', 'Cần Thơ', '2020-07-15', NULL),
('NV0005', 'Hoàng Minh E', 'Quản lý cửa hàng', 15000000.00, 'Nam', '0945678901', 'e@gmail.com', 'Hải Phòng', '2019-03-12', NULL),
('NV0006', 'Vũ Văn F', 'Nhân viên bảo vệ', 8000000.00, 'Nam', '0956789012', 'f@gmail.com', 'Nha Trang', '2023-06-01', NULL),
('NV0007', 'Bùi Thị G', 'Nhân viên kho', 9000000.00, 'Nữ', '0967890123', 'g@gmail.com', 'Huế', '2022-04-22', NULL),
('NV0008', 'Đặng Văn H', 'Nhân viên IT', 13000000.00, 'Nam', '0978901234', 'h@gmail.com', 'Hà Nội', '2021-09-18', NULL),
('NV0009', 'Nguyễn Thị I', 'Nhân viên bán hàng', 10000000.00, 'Nữ', '0989012345', 'i@gmail.com', 'Hải Dương', '2020-02-28', NULL),
('NV0010', 'Trương Minh J', 'Nhân viên marketing', 12500000.00, 'Nam', '0990123456', 'j@gmail.com', 'Hồ Chí Minh', '2019-12-05', NULL),
('NV0011', 'Lâm Văn K', 'Nhân viên kỹ thuật', 11000000.00, 'Nam', '0901122334', 'k@gmail.com', 'Đà Nẵng', '2023-07-17', NULL),
('NV0012', 'Tô Thị L', 'Nhân viên kho', 9500000.00, 'Nữ', '0912233445', 'l@gmail.com', 'Cần Thơ', '2022-08-23', NULL),
('NV0013', 'Ngô Văn M', 'Nhân viên bảo vệ', 8500000.00, 'Nam', '0923344556', 'm@gmail.com', 'Huế', '2021-05-30', NULL),
('NV0014', 'Hồ Minh N', 'Nhân viên kế toán', 11800000.00, 'Nam', '0934455667', 'n@gmail.com', 'Hà Nội', '2020-10-10', NULL),
('NV0015', 'Dương Thị O', 'Nhân viên bán hàng', 10200000.00, 'Nữ', '0945566778', 'o@gmail.com', 'Hải Phòng', '2019-06-08', NULL),
('NV0016', 'Đoàn Văn P', 'Nhân viên IT', 13500000.00, 'Nam', '0956677889', 'p@gmail.com', 'Nha Trang', '2023-09-14', NULL),
('NV0017', 'Trịnh Thị Q', 'Quản lý kho', 14000000.00, 'Nữ', '0967788990', 'q@gmail.com', 'Huế', '2022-03-11', NULL),
('NV0018', 'Phan Văn R', 'Nhân viên marketing', 12700000.00, 'Nam', '0978899001', 'r@gmail.com', 'Hồ Chí Minh', '2021-08-19', NULL),
('NV0019', 'Lưu Minh S', 'Nhân viên kỹ thuật', 11300000.00, 'Nam', '0989900112', 's@gmail.com', 'Đà Nẵng', '2020-04-25', NULL),
('NV0020', 'Tạ Thị T', 'Nhân viên bảo vệ', 8600000.00, 'Nam', '0990011223', 't@gmail.com', 'Hải Dương', '2019-01-30', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `sanpham`
--

CREATE TABLE `sanpham` (
  `IdSp` varchar(10) NOT NULL,
  `tensanpham` varchar(200) NOT NULL,
  `gia` decimal(10,2) NOT NULL,
  `soluongton` int(11) NOT NULL,
  `thuonghieu` varchar(100) NOT NULL,
  `mota` text DEFAULT NULL,
  `thongsokythuat` text DEFAULT NULL,
  `loaisanpham` varchar(50) NOT NULL,
  `hinhanh` varchar(255) DEFAULT NULL,
  `soluotxem` int(11) DEFAULT 0,
  `damuahang` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sanpham`
--

INSERT INTO `sanpham` (`IdSp`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000001', 'CPU Intel Core i9-13900K', 15990000.00, 50, 'Intel', 'CPU Intel Core i9-13900K mạnh mẽ, lý tưởng cho gaming và công việc sáng tạo.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.5GHz\", \"Lõi\": \"24\"}', 'Components', 'cpu_i9_13900k.jpg', 0, 0),
('SP000002', 'CPU Intel Core i7-13700K', 12500000.00, 45, 'Intel', 'CPU Intel Core i7-13700K phù hợp với nhu cầu gaming cao cấp.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.8GHz\", \"Lõi\": \"16\"}', 'Components', 'cpu_i7_13700k.jpg', 0, 0),
('SP000003', 'CPU Intel Core i5-13600K', 9900000.00, 60, 'Intel', 'CPU Intel Core i5-13600K – hiệu năng tốt, phù hợp với người dùng phổ thông.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.7GHz\", \"Lõi\": \"14\"}', 'Components', 'cpu_i5_13600k.jpg', 0, 0),
('SP000004', 'CPU AMD Ryzen 9 7900X', 14200000.00, 40, 'AMD', 'CPU AMD Ryzen 9 7900X – tối ưu hóa hiệu suất cho các tác vụ nặng.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.7GHz\", \"Lõi\": \"12\"}', 'Components', 'cpu_ryzen9_7900x.jpg', 0, 0),
('SP000005', 'CPU AMD Ryzen 7 7700X', 10600000.00, 55, 'AMD', 'AMD Ryzen 7 7700X – hiệu năng gaming xuất sắc với giá hợp lý.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.5GHz\", \"Lõi\": \"8\"}', 'Components', 'cpu_ryzen7_7700x.jpg', 0, 0),
('SP000006', 'CPU AMD Ryzen 5 7600X', 7900000.00, 65, 'AMD', 'AMD Ryzen 5 7600X – lựa chọn hàng đầu cho game thủ phổ thông.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.7GHz\", \"Lõi\": \"6\"}', 'Components', 'cpu_ryzen5_7600x.jpg', 0, 0),
('SP000007', 'CPU Intel Core i9-12900KS', 13500000.00, 38, 'Intel', 'CPU Intel Core i9-12900KS – mạnh mẽ với hiệu năng đỉnh cao.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"5.2GHz\", \"Lõi\": \"16\"}', 'Components', 'cpu_i9_12900ks.jpg', 0, 0),
('SP000008', 'CPU Intel Core i7-12700K', 9700000.00, 50, 'Intel', 'CPU Intel Core i7-12700K – giải pháp cân bằng giữa hiệu suất và giá thành.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.6GHz\", \"Lõi\": \"12\"}', 'Components', 'cpu_i7_12700k.jpg', 0, 0),
('SP000009', 'CPU Intel Core i5-12600K', 7700000.00, 55, 'Intel', 'CPU Intel Core i5-12600K – hiệu năng ổn định cho game thủ.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.7GHz\", \"Lõi\": \"10\"}', 'Components', 'cpu_i5_12600k.jpg', 0, 0),
('SP000010', 'CPU AMD Ryzen 9 5950X', 13500000.00, 30, 'AMD', 'CPU AMD Ryzen 9 5950X – lý tưởng cho xử lý đa luồng và tác vụ nặng.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.9GHz\", \"Lõi\": \"16\"}', 'Components', 'cpu_ryzen9_5950x.jpg', 0, 0),
('SP000011', 'CPU AMD Ryzen 7 5800X3D', 8900000.00, 48, 'AMD', 'AMD Ryzen 7 5800X3D – hiệu năng gaming đỉnh cao với 3D V-Cache.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.5GHz\", \"Lõi\": \"8\"}', 'Components', 'cpu_ryzen7_5800x3d.jpg', 0, 0),
('SP000012', 'CPU AMD Ryzen 5 5600X', 5400000.00, 70, 'AMD', 'AMD Ryzen 5 5600X – CPU tầm trung với hiệu năng ấn tượng.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.2GHz\", \"Lõi\": \"6\"}', 'Components', 'cpu_ryzen5_5600x.jpg', 0, 0),
('SP000013', 'CPU Intel Core i3-12100F', 2800000.00, 80, 'Intel', 'Intel Core i3-12100F – hiệu năng ổn định cho các tác vụ văn phòng.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.3GHz\", \"Lõi\": \"4\"}', 'Components', 'cpu_i3_12100f.jpg', 0, 0),
('SP000014', 'CPU AMD Ryzen 3 5300G', 3200000.00, 75, 'AMD', 'AMD Ryzen 3 5300G – tích hợp GPU mạnh mẽ, phù hợp cho HTPC.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.0GHz\", \"Lõi\": \"4\"}', 'Components', 'cpu_ryzen3_5300g.jpg', 0, 0),
('SP000015', 'CPU Intel Xeon W-1370P', 17000000.00, 25, 'Intel', 'Intel Xeon W-1370P – CPU workstation mạnh mẽ cho xử lý dữ liệu.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.5GHz\", \"Lõi\": \"8\"}', 'Components', 'cpu_xeon_w1370p.jpg', 0, 0),
('SP000016', 'CPU AMD Threadripper 5975WX', 45000000.00, 10, 'AMD', 'AMD Threadripper 5975WX – CPU cao cấp cho công việc đòi hỏi hiệu năng cao.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"4.5GHz\", \"Lõi\": \"32\"}', 'Components', 'cpu_threadripper_5975wx.jpg', 0, 0),
('SP000017', 'CPU Intel Xeon Platinum 8358P', 65000000.00, 5, 'Intel', 'Intel Xeon Platinum 8358P – dành cho các hệ thống máy chủ doanh nghiệp.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"2.6GHz\", \"Lõi\": \"32\"}', 'Components', 'cpu_xeon_8358p.jpg', 0, 0),
('SP000018', 'CPU AMD EPYC 7763', 68000000.00, 3, 'AMD', 'AMD EPYC 7763 – CPU server mạnh mẽ cho trung tâm dữ liệu.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"2.4GHz\", \"Lõi\": \"64\"}', 'Components', 'cpu_epyc_7763.jpg', 0, 0),
('SP000019', 'CPU Apple M2 Pro', 12000000.00, 35, 'Apple', 'Apple M2 Pro – CPU tối ưu cho MacBook với hiệu năng mạnh mẽ.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.2GHz\", \"Lõi\": \"10\"}', 'Components', 'cpu_m2_pro.jpg', 0, 0),
('SP000020', 'CPU Apple M2 Max', 18000000.00, 20, 'Apple', 'Apple M2 Max – hiệu năng mạnh mẽ dành cho các tác vụ chuyên nghiệp.', '{\"Danh mục\": \"CPU\", \"Tốc độ\": \"3.5GHz\", \"Lõi\": \"12\"}', 'Components', 'cpu_m2_max.jpg', 0, 0),
('SP000021', 'VGA ASUS TUF RTX 4060 Ti 8GB', 12900000.00, 20, 'Asus', 'VGA ASUS TUF RTX 4060 Ti – tối ưu hiệu năng gaming.', '{\"Danh mục\": \"VGA\", \"Memory\": \"8GB GDDR6\", \"Clock\": \"2535MHz\"}', 'Components', 'vga_asus_4060ti.jpg', 0, 0),
('SP000022', 'VGA ASUS ROG STRIX RTX 4070 12GB', 18700000.00, 15, 'Asus', 'ASUS ROG STRIX RTX 4070 – mạnh mẽ với công nghệ AI.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6X\", \"Clock\": \"2610MHz\"}', 'Components', 'vga_asus_rog_4070.jpg', 0, 0),
('SP000023', 'VGA MSI GAMING X RTX 4070 Ti 12GB', 22800000.00, 12, 'MSI', 'MSI RTX 4070 Ti – hiệu năng hàng đầu cho game thủ.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6X\", \"Clock\": \"2730MHz\"}', 'Components', 'vga_msi_4070ti.jpg', 0, 0),
('SP000024', 'VGA Gigabyte AORUS RTX 4080 16GB', 33500000.00, 10, 'Gigabyte', 'Gigabyte AORUS RTX 4080 – sức mạnh đồ họa cao cấp.', '{\"Danh mục\": \"VGA\", \"Memory\": \"16GB GDDR6X\", \"Clock\": \"2505MHz\"}', 'Components', 'vga_giga_4080.jpg', 0, 0),
('SP000025', 'VGA ASUS TUF RTX 4090 24GB', 48500000.00, 7, 'Asus', 'ASUS TUF RTX 4090 – card đồ họa đỉnh cao cho AI & gaming.', '{\"Danh mục\": \"VGA\", \"Memory\": \"24GB GDDR6X\", \"Clock\": \"2520MHz\"}', 'Components', 'vga_asus_4090.jpg', 0, 0),
('SP000026', 'VGA MSI RTX 4060 Gaming X 8GB', 10800000.00, 25, 'MSI', 'MSI RTX 4060 Gaming X – lựa chọn tuyệt vời cho gaming.', '{\"Danh mục\": \"VGA\", \"Memory\": \"8GB GDDR6\", \"Clock\": \"2670MHz\"}', 'Components', 'vga_msi_4060.jpg', 0, 0),
('SP000027', 'VGA ZOTAC RTX 4070 AMP AIRO 12GB', 17900000.00, 13, 'Zotac', 'ZOTAC RTX 4070 AMP AIRO – thiết kế tối ưu, tản nhiệt mạnh.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6X\", \"Clock\": \"2520MHz\"}', 'Components', 'vga_zotac_4070.jpg', 0, 0),
('SP000028', 'VGA Palit RTX 4080 GameRock 16GB', 32900000.00, 9, 'Palit', 'Palit RTX 4080 GameRock – thiết kế đẹp, hiệu năng khủng.', '{\"Danh mục\": \"VGA\", \"Memory\": \"16GB GDDR6X\", \"Clock\": \"2610MHz\"}', 'Components', 'vga_palit_4080.jpg', 0, 0),
('SP000029', 'VGA GALAX RTX 4060 EX 8GB', 10500000.00, 30, 'GALAX', 'GALAX RTX 4060 EX – card đồ họa phổ thông với hiệu năng tốt.', '{\"Danh mục\": \"VGA\", \"Memory\": \"8GB GDDR6\", \"Clock\": \"2670MHz\"}', 'Components', 'vga_galax_4060.jpg', 0, 0),
('SP000030', 'VGA AMD Radeon RX 6700 XT 12GB', 12900000.00, 22, 'AMD', 'AMD RX 6700 XT – Hiệu năng mạnh mẽ cho gaming 2K.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6\", \"Clock\": \"2581MHz\"}', 'Components', 'vga_amd_6700xt.jpg', 0, 0),
('SP000031', 'VGA AMD Radeon RX 7900 XTX 24GB', 28500000.00, 8, 'AMD', 'AMD RX 7900 XTX – Card đồ họa đầu bảng từ AMD.', '{\"Danh mục\": \"VGA\", \"Memory\": \"24GB GDDR6\", \"Clock\": \"2500MHz\"}', 'Components', 'vga_amd_7900xtx.jpg', 0, 0),
('SP000032', 'VGA Intel Arc A770 16GB', 10200000.00, 20, 'Intel', 'Intel Arc A770 – card đồ họa gaming mạnh mẽ từ Intel.', '{\"Danh mục\": \"VGA\", \"Memory\": \"16GB GDDR6\", \"Clock\": \"2400MHz\"}', 'Components', 'vga_intel_a770.jpg', 0, 0),
('SP000033', 'VGA Intel Arc A750 8GB', 8600000.00, 28, 'Intel', 'Intel Arc A750 – giá rẻ nhưng vẫn đủ hiệu năng cho game.', '{\"Danh mục\": \"VGA\", \"Memory\": \"8GB GDDR6\", \"Clock\": \"2050MHz\"}', 'Components', 'vga_intel_a750.jpg', 0, 0),
('SP000034', 'VGA MSI RTX 3050 Ventus 8GB', 7500000.00, 35, 'MSI', 'MSI RTX 3050 Ventus – lựa chọn tốt cho gaming phổ thông.', '{\"Danh mục\": \"VGA\", \"Memory\": \"8GB GDDR6\", \"Clock\": \"1777MHz\"}', 'Components', 'vga_msi_3050.jpg', 0, 0),
('SP000035', 'VGA Gigabyte RTX 3060 Gaming OC 12GB', 9500000.00, 27, 'Gigabyte', 'Gigabyte RTX 3060 – tối ưu cho gaming và sáng tạo nội dung.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6\", \"Clock\": \"1837MHz\"}', 'Components', 'vga_giga_3060.jpg', 0, 0),
('SP000036', 'VGA Zotac RTX 3080 Trinity 10GB', 18500000.00, 11, 'Zotac', 'Zotac RTX 3080 Trinity – hiệu suất mạnh mẽ, giá hợp lý.', '{\"Danh mục\": \"VGA\", \"Memory\": \"10GB GDDR6X\", \"Clock\": \"1710MHz\"}', 'Components', 'vga_zotac_3080.jpg', 0, 0),
('SP000037', 'VGA AMD Radeon RX 6800 XT 16GB', 16700000.00, 14, 'AMD', 'AMD RX 6800 XT – hiệu năng ấn tượng cho gaming 4K.', '{\"Danh mục\": \"VGA\", \"Memory\": \"16GB GDDR6\", \"Clock\": \"2250MHz\"}', 'Components', 'vga_amd_6800xt.jpg', 0, 0),
('SP000038', 'VGA ASUS Dual RTX 3060 12GB', 9200000.00, 26, 'Asus', 'ASUS Dual RTX 3060 – phù hợp cho gaming và đồ họa.', '{\"Danh mục\": \"VGA\", \"Memory\": \"12GB GDDR6\", \"Clock\": \"1777MHz\"}', 'Components', 'vga_asus_3060.jpg', 0, 0),
('SP000039', 'VGA MSI RTX 4090 SUPRIM X 24GB', 49800000.00, 5, 'MSI', 'MSI RTX 4090 SUPRIM X – đỉnh cao card đồ họa.', '{\"Danh mục\": \"VGA\", \"Memory\": \"24GB GDDR6X\", \"Clock\": \"2625MHz\"}', 'Components', 'vga_msi_4090.jpg', 0, 0),
('SP000040', 'VGA NVIDIA Titan RTX 24GB', 62000000.00, 3, 'NVIDIA', 'NVIDIA Titan RTX – chuyên biệt cho AI & Deep Learning.', '{\"Danh mục\": \"VGA\", \"Memory\": \"24GB GDDR6\", \"Clock\": \"1770MHz\"}', 'Components', 'vga_nvidia_titan.jpg', 0, 0),
('SP000041', 'Mainboard ASUS TUF B760M-PLUS D4', 3700000.00, 40, 'ASUS', 'ASUS TUF B760M-PLUS D4 – bo mạch chủ mạnh mẽ cho CPU Intel.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"B760\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asus_b760m.jpg', 0, 0),
('SP000042', 'Mainboard ASUS ROG STRIX Z790-E Gaming', 8990000.00, 25, 'ASUS', 'ASUS ROG STRIX Z790-E – bo mạch chủ cao cấp dành cho gaming.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"Z790\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_asus_z790.jpg', 0, 0),
('SP000043', 'Mainboard MSI MAG B760 TOMAHAWK WIFI', 4200000.00, 38, 'MSI', 'MSI MAG B760 TOMAHAWK – hỗ trợ Intel Gen 13 & 14.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"B760\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_msi_b760.jpg', 0, 0),
('SP000044', 'Mainboard MSI PRO B660M-A DDR4', 3100000.00, 50, 'MSI', 'MSI PRO B660M-A – lựa chọn tốt cho hệ thống Intel tầm trung.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"B660\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_msi_b660m.jpg', 0, 0),
('SP000045', 'Mainboard Gigabyte B760 AORUS ELITE AX', 4600000.00, 30, 'Gigabyte', 'Gigabyte B760 AORUS ELITE AX – bo mạch chủ gaming đáng tin cậy.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"B760\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_giga_b760.jpg', 0, 0),
('SP000046', 'Mainboard Gigabyte Z790 AORUS MASTER', 10990000.00, 15, 'Gigabyte', 'Gigabyte Z790 AORUS MASTER – dành cho build cao cấp.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"Z790\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_giga_z790.jpg', 0, 0),
('SP000047', 'Mainboard ASRock B550M Steel Legend', 2950000.00, 55, 'ASRock', 'ASRock B550M Steel Legend – hỗ trợ CPU Ryzen mạnh mẽ.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM4\", \"Chipset\": \"B550\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asrock_b550m.jpg', 0, 0),
('SP000048', 'Mainboard ASRock X670E Taichi', 12990000.00, 12, 'ASRock', 'ASRock X670E Taichi – bo mạch chủ cao cấp cho Ryzen 7000.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM5\", \"Chipset\": \"X670E\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_asrock_x670e.jpg', 0, 0),
('SP000049', 'Mainboard ASUS PRIME H610M-K D4', 1950000.00, 60, 'ASUS', 'ASUS PRIME H610M-K – giá rẻ, ổn định cho hệ thống phổ thông.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"H610\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asus_h610m.jpg', 0, 0),
('SP000050', 'Mainboard ASUS PRO WS WRX80E-SAGE SE WIFI', 22500000.00, 5, 'ASUS', 'ASUS PRO WS WRX80E-SAGE – bo mạch chủ workstation cao cấp.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"sWRX8\", \"Chipset\": \"WRX80\", \"RAM\": \"DDR4 ECC\"}', 'Components', 'mainboard_asus_wrx80.jpg', 0, 0),
('SP000051', 'Mainboard MSI MEG X670E ACE', 15900000.00, 10, 'MSI', 'MSI MEG X670E ACE – tối ưu cho Ryzen 7000.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM5\", \"Chipset\": \"X670E\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_msi_x670e.jpg', 0, 0),
('SP000052', 'Mainboard Gigabyte B450M DS3H', 1800000.00, 75, 'Gigabyte', 'Gigabyte B450M DS3H – bo mạch chủ giá rẻ nhưng ổn định.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM4\", \"Chipset\": \"B450\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_giga_b450m.jpg', 0, 0),
('SP000053', 'Mainboard ASRock H510M-HDV', 1750000.00, 80, 'ASRock', 'ASRock H510M-HDV – lựa chọn phù hợp cho build văn phòng.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1200\", \"Chipset\": \"H510\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asrock_h510m.jpg', 0, 0),
('SP000054', 'Mainboard ASUS ROG CROSSHAIR X670E EXTREME', 19990000.00, 6, 'ASUS', 'ASUS ROG CROSSHAIR X670E EXTREME – bo mạch chủ gaming cao cấp.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM5\", \"Chipset\": \"X670E\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_asus_x670e.jpg', 0, 0),
('SP000055', 'Mainboard MSI PRO Z690-A DDR5', 4890000.00, 35, 'MSI', 'MSI PRO Z690-A – hiệu năng cao với RAM DDR5.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"Z690\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_msi_z690.jpg', 0, 0),
('SP000056', 'Mainboard ASUS TUF Gaming B550M-PLUS', 2850000.00, 50, 'ASUS', 'ASUS TUF Gaming B550M-PLUS – mainboard gaming tầm trung.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM4\", \"Chipset\": \"B550\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asus_b550m.jpg', 0, 0),
('SP000057', 'Mainboard Gigabyte Z590 AORUS ELITE AX', 5990000.00, 28, 'Gigabyte', 'Gigabyte Z590 AORUS ELITE AX – gaming hiệu năng cao.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1200\", \"Chipset\": \"Z590\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_giga_z590.jpg', 0, 0),
('SP000058', 'Mainboard ASRock X570 Phantom Gaming 4', 3790000.00, 40, 'ASRock', 'ASRock X570 Phantom Gaming 4 – mainboard gaming bền bỉ.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM4\", \"Chipset\": \"X570\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asrock_x570.jpg', 0, 0),
('SP000059', 'Mainboard MSI MAG Z790 TOMAHAWK WIFI', 8990000.00, 18, 'MSI', 'MSI MAG Z790 TOMAHAWK – bo mạch chủ hiệu năng mạnh.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"LGA1700\", \"Chipset\": \"Z790\", \"RAM\": \"DDR5\"}', 'Components', 'mainboard_msi_z790.jpg', 0, 0),
('SP000060', 'Mainboard ASUS PRIME B550-PLUS', 2890000.00, 45, 'ASUS', 'ASUS PRIME B550-PLUS – mainboard ổn định cho Ryzen.', '{\"Danh mục\": \"Mainboard\", \"Socket\": \"AM4\", \"Chipset\": \"B550\", \"RAM\": \"DDR4\"}', 'Components', 'mainboard_asus_b550.jpg', 0, 0),
('SP000061', 'RAM Corsair Vengeance RGB 32GB DDR5 6000MHz', 3200000.00, 50, 'Corsair', 'Bộ nhớ RAM DDR5 6000MHz với LED RGB đẹp mắt.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_corsair_32gb.jpg', 0, 0),
('SP000062', 'RAM Corsair Dominator Platinum 64GB DDR5 6600MHz', 7800000.00, 40, 'Corsair', 'Dominator Platinum RGB – RAM cao cấp với tản nhiệt tối ưu.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6600MHz\"}', 'Components', 'ram_corsair_dominator_64gb.jpg', 0, 0),
('SP000063', 'RAM G.Skill Trident Z5 RGB 32GB DDR5 6400MHz', 4100000.00, 55, 'G.Skill', 'RAM DDR5 6400MHz dành cho gaming hiệu suất cao.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_gskill_trident_32gb.jpg', 0, 0),
('SP000064', 'RAM G.Skill Ripjaws S5 64GB DDR5 6000MHz', 7100000.00, 35, 'G.Skill', 'G.Skill Ripjaws S5 – RAM DDR5 tối ưu cho hiệu suất.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_gskill_ripjaws_64gb.jpg', 0, 0),
('SP000065', 'RAM Kingston Fury Beast 16GB DDR5 5200MHz', 2100000.00, 70, 'Kingston', 'RAM Kingston Fury Beast 16GB – hiệu suất ổn định.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Tốc độ\": \"5200MHz\"}', 'Components', 'ram_kingston_fury_16gb.jpg', 0, 0),
('SP000066', 'RAM Kingston Fury Renegade 32GB DDR5 7200MHz', 8200000.00, 25, 'Kingston', 'RAM DDR5 tốc độ cực cao dành cho ép xung.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"7200MHz\"}', 'Components', 'ram_kingston_renegade_32gb.jpg', 0, 0),
('SP000067', 'RAM TeamGroup T-Force Delta RGB 64GB DDR5 6200MHz', 6990000.00, 30, 'TeamGroup', 'T-Force Delta RGB – thiết kế RGB đẹp mắt.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6200MHz\"}', 'Components', 'ram_teamgroup_delta_64gb.jpg', 0, 0),
('SP000068', 'RAM TeamGroup Vulcan 32GB DDR5 6000MHz', 3600000.00, 40, 'TeamGroup', 'Vulcan 32GB DDR5 – RAM hiệu năng tốt, giá hợp lý.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_teamgroup_vulcan_32gb.jpg', 0, 0),
('SP000069', 'RAM Crucial DDR5 32GB 5600MHz', 3100000.00, 60, 'Crucial', 'RAM Crucial DDR5 – hiệu suất ổn định với tốc độ 5600MHz.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_crucial_32gb.jpg', 0, 0),
('SP000070', 'RAM Crucial Ballistix 64GB DDR5 6200MHz', 6800000.00, 30, 'Crucial', 'Crucial Ballistix DDR5 – RAM gaming chuyên dụng.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6200MHz\"}', 'Components', 'ram_crucial_ballistix_64gb.jpg', 0, 0),
('SP000071', 'RAM Patriot Viper Venom RGB 32GB DDR5 6400MHz', 4200000.00, 40, 'Patriot', 'Patriot Viper Venom RGB – RAM DDR5 gaming chất lượng.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_patriot_viper_32gb.jpg', 0, 0),
('SP000072', 'RAM Patriot Signature Line 16GB DDR5 4800MHz', 1800000.00, 75, 'Patriot', 'Patriot Signature Line – RAM DDR5 giá rẻ, hiệu năng tốt.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Tốc độ\": \"4800MHz\"}', 'Components', 'ram_patriot_signature_16gb.jpg', 0, 0),
('SP000073', 'RAM Samsung DDR5 32GB 5600MHz', 2950000.00, 50, 'Samsung', 'RAM Samsung DDR5 32GB – hiệu suất cao, giá hợp lý.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_samsung_32gb.jpg', 0, 0),
('SP000074', 'RAM Samsung DDR5 64GB 6400MHz', 6900000.00, 35, 'Samsung', 'Samsung DDR5 – tốc độ cao, phù hợp cho workstation.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_samsung_64gb.jpg', 0, 0),
('SP000075', 'RAM ADATA XPG Lancer RGB 32GB DDR5 6000MHz', 3900000.00, 45, 'ADATA', 'ADATA XPG Lancer RGB – RAM gaming với hiệu suất cao.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_adata_xpg_32gb.jpg', 0, 0),
('SP000076', 'RAM ADATA XPG Lancer RGB 64GB DDR5 6800MHz', 7500000.00, 28, 'ADATA', 'ADATA XPG Lancer – RAM mạnh mẽ với tốc độ 6800MHz.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6800MHz\"}', 'Components', 'ram_adata_xpg_64gb.jpg', 0, 0),
('SP000077', 'RAM Lexar ARES RGB 32GB DDR5 5600MHz', 3300000.00, 55, 'Lexar', 'Lexar ARES RGB – RAM gaming ổn định và đẹp mắt.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_lexar_ares_32gb.jpg', 0, 0),
('SP000078', 'RAM Lexar ARES RGB 64GB DDR5 6400MHz', 6900000.00, 30, 'Lexar', 'Lexar ARES RGB – RAM hiệu suất cao cho gaming và workstation.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_lexar_ares_64gb.jpg', 0, 0),
('SP000079', 'RAM Corsair Vengeance LPX 16GB DDR5 5200MHz', 2000000.00, 65, 'Corsair', 'Corsair Vengeance LPX – RAM giá tốt, hiệu năng ổn định.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Tốc độ\": \"5200MHz\"}', 'Components', 'ram_corsair_lpx_16gb.jpg', 0, 0),
('SP000080', 'RAM Corsair Dominator Platinum 128GB DDR5 7200MHz', 15900000.00, 10, 'Corsair', 'Dominator Platinum RGB – RAM DDR5 cao cấp nhất.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"128GB\", \"Tốc độ\": \"7200MHz\"}', 'Components', 'ram_corsair_dominator_128gb.jpg', 0, 0),
('SP000082', 'PSU Cooler Master MWE 850 GOLD V2', 2290000.00, 15, 'Cooler Master', 'Nguồn 850W Gold, hiệu năng vượt trội cho gaming.', '{\"Danh mục\": \"PSU\", \"Watts\": \"850W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_cooler_850w.jpg', 0, 0),
('SP000083', 'PSU Corsair RM750x 80 Plus Gold', 2790000.00, 18, 'Corsair', 'PSU Corsair 750W Gold, hiệu suất cao cho hệ thống mạnh.', '{\"Danh mục\": \"PSU\", \"Watts\": \"750W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_corsair_rm750x.jpg', 0, 0),
('SP000084', 'PSU Corsair RM1000x 80 Plus Gold', 3590000.00, 10, 'Corsair', 'Nguồn 1000W chuẩn Gold, tối ưu cho build cao cấp.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_corsair_rm1000x.jpg', 0, 0),
('SP000085', 'PSU EVGA SuperNOVA 850 G5 80 Plus Gold', 2890000.00, 12, 'EVGA', 'PSU EVGA 850W hiệu suất cao với chứng nhận Gold.', '{\"Danh mục\": \"PSU\", \"Watts\": \"850W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_evga_850w.jpg', 0, 0),
('SP000086', 'PSU EVGA SuperNOVA 1000 G5 80 Plus Gold', 3990000.00, 8, 'EVGA', 'Nguồn 1000W Gold từ EVGA – ổn định, bền bỉ.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_evga_1000w.jpg', 0, 0),
('SP000087', 'PSU Gigabyte P750GM 750W 80 Plus Gold', 1990000.00, 25, 'Gigabyte', 'PSU Gigabyte 750W Gold, giá tốt, hiệu suất ổn định.', '{\"Danh mục\": \"PSU\", \"Watts\": \"750W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_gigabyte_750w.jpg', 0, 0),
('SP000088', 'PSU Gigabyte AORUS P1000W 80 Plus Platinum', 4390000.00, 6, 'Gigabyte', 'Nguồn 1000W Platinum dành cho hệ thống cao cấp.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Platinum\"}', 'Components', 'psu_gigabyte_1000w.jpg', 0, 0),
('SP000089', 'PSU ASUS ROG Strix 850W 80 Plus Gold', 3190000.00, 10, 'ASUS', 'Nguồn 850W từ ASUS ROG Strix, gaming bền bỉ.', '{\"Danh mục\": \"PSU\", \"Watts\": \"850W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_asus_850w.jpg', 0, 0),
('SP000090', 'PSU ASUS ROG Thor 1200W 80 Plus Platinum', 5990000.00, 5, 'ASUS', 'Nguồn ASUS ROG Thor 1200W – dành cho build tối thượng.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1200W\", \"Efficiency\": \"Platinum\"}', 'Components', 'psu_asus_1200w.jpg', 0, 0),
('SP000091', 'PSU MSI MAG A650BN 650W 80 Plus Bronze', 1490000.00, 30, 'MSI', 'MSI 650W chuẩn Bronze, giá rẻ nhưng ổn định.', '{\"Danh mục\": \"PSU\", \"Watts\": \"650W\", \"Efficiency\": \"Bronze\"}', 'Components', 'psu_msi_650w.jpg', 0, 0),
('SP000092', 'PSU MSI MPG A850GF 850W 80 Plus Gold', 2890000.00, 15, 'MSI', 'MSI 850W Gold – gaming bền bỉ và hiệu suất cao.', '{\"Danh mục\": \"PSU\", \"Watts\": \"850W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_msi_850w.jpg', 0, 0),
('SP000093', 'PSU Thermaltake Toughpower GF1 750W 80 Plus Gold', 2590000.00, 18, 'Thermaltake', 'Nguồn 750W từ Thermaltake, gaming hiệu suất cao.', '{\"Danh mục\": \"PSU\", \"Watts\": \"750W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_thermaltake_750w.jpg', 0, 0),
('SP000094', 'PSU Thermaltake Toughpower PF1 1200W 80 Plus Platinum', 5290000.00, 7, 'Thermaltake', 'PSU 1200W Platinum dành cho hệ thống cao cấp.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1200W\", \"Efficiency\": \"Platinum\"}', 'Components', 'psu_thermaltake_1200w.jpg', 0, 0),
('SP000095', 'PSU Antec NeoECO Gold 750W 80 Plus Gold', 2290000.00, 20, 'Antec', 'Nguồn 750W từ Antec, gaming ổn định.', '{\"Danh mục\": \"PSU\", \"Watts\": \"750W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_antec_750w.jpg', 0, 0),
('SP000096', 'PSU Antec HCG 1000W 80 Plus Platinum', 4790000.00, 9, 'Antec', 'Antec HCG 1000W Platinum, gaming & workstation.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Platinum\"}', 'Components', 'psu_antec_1000w.jpg', 0, 0),
('SP000097', 'PSU Deepcool PQ1000M 1000W 80 Plus Platinum', 4590000.00, 6, 'Deepcool', 'Deepcool PQ1000M – nguồn Platinum dành cho gaming.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Platinum\"}', 'Components', 'psu_deepcool_1000w.jpg', 0, 0),
('SP000098', 'PSU Seasonic Focus GX-850 850W 80 Plus Gold', 3190000.00, 12, 'Seasonic', 'Seasonic GX-850 – thương hiệu PSU nổi tiếng, hiệu suất cao.', '{\"Danh mục\": \"PSU\", \"Watts\": \"850W\", \"Efficiency\": \"Gold\"}', 'Components', 'psu_seasonic_850w.jpg', 0, 0),
('SP000099', 'PSU Seasonic PRIME TX-1000 1000W 80 Plus Titanium', 6490000.00, 5, 'Seasonic', 'Nguồn 1000W Titanium đỉnh cao từ Seasonic.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1000W\", \"Efficiency\": \"Titanium\"}', 'Components', 'psu_seasonic_1000w.jpg', 0, 0),
('SP000100', 'PSU Be Quiet! Dark Power 12 1200W 80 Plus Titanium', 7490000.00, 4, 'Be Quiet!', 'PSU Be Quiet! 1200W – công suất lớn, hiệu suất tối đa.', '{\"Danh mục\": \"PSU\", \"Watts\": \"1200W\", \"Efficiency\": \"Titanium\"}', 'Components', 'psu_bequiet_1200w.jpg', 0, 0),
('SP000101', 'Case NZXT H510 ATX Mid Tower', 1650000.00, 15, 'NZXT', 'Case NZXT H510 thiết kế hiện đại, mặt kính cường lực.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_nzxt_h510.jpg', 0, 0),
('SP000102', 'Case NZXT H7 Flow ATX Mid Tower', 2650000.00, 10, 'NZXT', 'H7 Flow với thiết kế tối ưu hóa luồng khí, hỗ trợ tản nhiệt.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_nzxt_h7.jpg', 0, 0),
('SP000103', 'Case Corsair 4000D Airflow ATX', 1890000.00, 20, 'Corsair', '4000D Airflow – tối ưu luồng khí, phù hợp cho gaming.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_corsair_4000d.jpg', 0, 0),
('SP000104', 'Case Corsair iCUE 5000X RGB ATX', 3790000.00, 12, 'Corsair', 'Case Corsair 5000X với kính cường lực RGB tuyệt đẹp.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính, nhựa\"}', 'Components', 'case_corsair_5000x.jpg', 0, 0),
('SP000105', 'Case Cooler Master MasterBox TD500 Mesh', 2150000.00, 18, 'Cooler Master', 'Case Cooler Master TD500 với thiết kế 3D Mesh độc đáo.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_cooler_td500.jpg', 0, 0),
('SP000106', 'Case Cooler Master HAF 700 EVO E-ATX', 7990000.00, 5, 'Cooler Master', 'HAF 700 EVO – case full tower với khả năng mở rộng tối đa.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"E-ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_cooler_haf700.jpg', 0, 0),
('SP000107', 'Case Lian Li PC-O11 Dynamic ATX', 2890000.00, 10, 'Lian Li', 'PC-O11 Dynamic – case kính cường lực ấn tượng.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Nhôm, kính\"}', 'Components', 'case_lianli_o11.jpg', 0, 0),
('SP000108', 'Case Lian Li Lancool III RGB ATX', 2590000.00, 12, 'Lian Li', 'Lancool III với kính cường lực và RGB.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_lianli_lancool.jpg', 0, 0),
('SP000109', 'Case Fractal Design Meshify C ATX', 1750000.00, 14, 'Fractal Design', 'Meshify C – case nhỏ gọn với hiệu suất tản nhiệt cao.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_fractal_meshify.jpg', 0, 0),
('SP000110', 'Case Fractal Design Define 7 XL Full Tower', 4690000.00, 7, 'Fractal Design', 'Define 7 XL – case full tower siêu rộng.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"E-ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_fractal_define7.jpg', 0, 0),
('SP000111', 'Case Thermaltake View 71 RGB ATX', 3950000.00, 8, 'Thermaltake', 'View 71 RGB – kính cường lực 4 mặt, hiệu năng cao.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_tt_view71.jpg', 0, 0),
('SP000112', 'Case Thermaltake Core P3 Open Frame', 3290000.00, 9, 'Thermaltake', 'Core P3 – case thiết kế Open Frame độc đáo.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép\"}', 'Components', 'case_tt_corep3.jpg', 0, 0),
('SP000113', 'Case Be Quiet! Silent Base 802 ATX', 3190000.00, 6, 'Be Quiet!', 'Silent Base 802 – thiết kế yên tĩnh và tối ưu luồng khí.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, nhôm\"}', 'Components', 'case_bequiet_802.jpg', 0, 0),
('SP000114', 'Case Phanteks Eclipse P500A D-RGB ATX', 2590000.00, 12, 'Phanteks', 'Eclipse P500A – hiệu suất tối đa với lưới thông gió.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_phanteks_p500a.jpg', 0, 0),
('SP000115', 'Case Phanteks Enthoo Pro II Full Tower', 4290000.00, 7, 'Phanteks', 'Enthoo Pro II – case E-ATX với không gian tối ưu.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"E-ATX\", \"Chất liệu\": \"Thép\"}', 'Components', 'case_phanteks_pro2.jpg', 0, 0),
('SP000116', 'Case InWin 303 ATX Mid Tower', 2090000.00, 10, 'InWin', 'InWin 303 – case thép chắc chắn, phong cách tối giản.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_inwin_303.jpg', 0, 0),
('SP000117', 'Case InWin D-Frame Mini ITX', 4990000.00, 5, 'InWin', 'D-Frame Mini – case ITX độc đáo, thiết kế cao cấp.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"Mini-ITX\", \"Chất liệu\": \"Nhôm\"}', 'Components', 'case_inwin_dframe.jpg', 0, 0),
('SP000118', 'Case Deepcool CH510 Mesh Digital ATX', 1790000.00, 18, 'Deepcool', 'CH510 Mesh Digital – case ATX tối ưu luồng khí.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép\"}', 'Components', 'case_deepcool_ch510.jpg', 0, 0),
('SP000119', 'Case Antec NX410 RGB Mid Tower', 1290000.00, 20, 'Antec', 'NX410 RGB – case tầm trung, thiết kế đẹp mắt.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Thép, kính\"}', 'Components', 'case_antec_nx410.jpg', 0, 0),
('SP000120', 'Case Antec Torque Open-Air ATX', 6890000.00, 5, 'Antec', 'Antec Torque – thiết kế Open-Air đầy phong cách.', '{\"Danh mục\": \"Case\", \"Form Factor\": \"ATX\", \"Chất liệu\": \"Nhôm\"}', 'Components', 'case_antec_torque.jpg', 0, 0),
('SP000121', 'Laptop MSI Katana Blaze X - Intel i7, 16GB, 512GB SSD, RTX 3060', 25990000.00, 15, 'MSI', 'MSI Katana Blaze X mang đến hiệu năng mạnh mẽ với Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, lý tưởng cho game thủ chuyên nghiệp.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'msi_katana_blaze_x.jpg', 0, 0),
('SP000122', 'Laptop MSI Raider Fury - Intel i7, 16GB, 1TB SSD, RTX 3070', 32990000.00, 10, 'MSI', 'MSI Raider Fury kết hợp thiết kế sang trọng và hiệu năng cao với Intel i7, 16GB RAM, 1TB SSD NVMe và NVIDIA RTX 3070, phù hợp cho game thủ đòi hỏi cấu hình mạnh mẽ.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"1TB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3070\"}', 'Laptop', 'msi_raider_fury.jpg', 0, 0),
('SP000123', 'Laptop ASUS ROG Strix G15 - Intel i7, 16GB, 512GB SSD, RTX 3060', 26990000.00, 20, 'ASUS', 'ASUS ROG Strix G15 được thiết kế dành cho game thủ với hiệu năng ấn tượng từ Intel i7, 16GB RAM, 512GB SSD NVMe và card đồ họa NVIDIA RTX 3060.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'asus_rog_strix_g15.jpg', 0, 0),
('SP000124', 'Laptop ASUS TUF Gaming F15 - Intel i5, 8GB, 256GB SSD, GTX 1650', 21990000.00, 25, 'ASUS', 'ASUS TUF Gaming F15 mang lại sự cân bằng giữa hiệu năng và giá thành với Intel i5, 8GB RAM, 256GB SSD NVMe và NVIDIA GTX 1650, lý tưởng cho game thủ ngân sách.', '{\"CPU\": \"Intel Core i5-12400H\", \"RAM\": \"8GB DDR4\", \"Screen\": \"15.6 inch FHD 60Hz\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce GTX 1650\"}', 'Laptop', 'asus_tuf_gaming_f15.jpg', 0, 0),
('SP000125', 'Laptop Acer Predator Helios 300 - Intel i7, 16GB, 512GB SSD, RTX 3060', 27990000.00, 18, 'Acer', 'Acer Predator Helios 300 mang đến hiệu năng mạnh mẽ với Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, phù hợp cho game thủ chuyên nghiệp.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'acer_predator_helios_300.jpg', 0, 0),
('SP000126', 'Laptop Acer Nitro 5 - Intel i5, 8GB, 256GB SSD, GTX 1650', 20990000.00, 30, 'Acer', 'Acer Nitro 5 là lựa chọn kinh tế cho game thủ với Intel i5, 8GB RAM, 256GB SSD NVMe và NVIDIA GTX 1650, đảm bảo hiệu năng ổn định cho nhu cầu giải trí hàng ngày.', '{\"CPU\": \"Intel Core i5-12400H\", \"RAM\": \"8GB DDR4\", \"Screen\": \"15.6 inch FHD 60Hz\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce GTX 1650\"}', 'Laptop', 'acer_nitro_5.jpg', 0, 0),
('SP000127', 'Laptop Dell G5 15 - Intel i7, 16GB, 512GB SSD, RTX 3060', 25990000.00, 12, 'Dell', 'Dell G5 15 cung cấp hiệu năng vượt trội với Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, thích hợp cho cả chơi game lẫn công việc sáng tạo.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'dell_g5_15.jpg', 0, 0),
('SP000128', 'Laptop Dell Alienware M15 - Intel i9, 32GB, 1TB SSD, RTX 3080', 47990000.00, 5, 'Dell', 'Dell Alienware M15 là dòng cao cấp với Intel i9, 32GB RAM, 1TB SSD NVMe và NVIDIA RTX 3080, mang lại trải nghiệm gaming đỉnh cao.', '{\"CPU\": \"Intel Core i9-12900H\", \"RAM\": \"32GB DDR4\", \"Screen\": \"15.6 inch FHD 240Hz\", \"SSD\": \"1TB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3080\"}', 'Laptop', 'dell_alienware_m15.jpg', 0, 0),
('SP000129', 'Laptop HP Omen 15 - Intel i7, 16GB, 512GB SSD, RTX 3060', 26990000.00, 14, 'HP', 'HP Omen 15 thiết kế cho game thủ và chuyên gia sáng tạo với Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, đảm bảo hiệu năng mạnh mẽ.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'hp_omen_15.jpg', 0, 0),
('SP000130', 'Laptop HP Pavilion Gaming - Intel i5, 8GB, 256GB SSD, GTX 1650', 21990000.00, 20, 'HP', 'HP Pavilion Gaming cung cấp hiệu năng ổn định với Intel i5, 8GB RAM, 256GB SSD NVMe và NVIDIA GTX 1650, lý tưởng cho game thủ mới bắt đầu.', '{\"CPU\": \"Intel Core i5-12400H\", \"RAM\": \"8GB DDR4\", \"Screen\": \"15.6 inch FHD 60Hz\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce GTX 1650\"}', 'Laptop', 'hp_pavilion_gaming.jpg', 0, 0),
('SP000131', 'Laptop Lenovo Legion 5 - AMD Ryzen 7, 16GB, 512GB SSD, RTX 3060', 25990000.00, 16, 'Lenovo', 'Lenovo Legion 5 kết hợp CPU AMD Ryzen 7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, phù hợp cho game thủ và đa nhiệm chuyên nghiệp.', '{\"CPU\": \"AMD Ryzen 7 5800H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'lenovo_legion_5.jpg', 0, 0),
('SP000132', 'Laptop Lenovo IdeaPad Gaming 3 - AMD Ryzen 5, 8GB, 256GB SSD, GTX 1650', 20990000.00, 22, 'Lenovo', 'Lenovo IdeaPad Gaming 3 là lựa chọn kinh tế với CPU AMD Ryzen 5, 8GB RAM, 256GB SSD NVMe và NVIDIA GTX 1650, thích hợp cho nhu cầu học tập và giải trí.', '{\"CPU\": \"AMD Ryzen 5 5600H\", \"RAM\": \"8GB DDR4\", \"Screen\": \"15.6 inch FHD 60Hz\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce GTX 1650\"}', 'Laptop', 'lenovo_ideapad_gaming_3.jpg', 0, 0),
('SP000133', 'Laptop Razer Blade 15 - Intel i7, 16GB, 1TB SSD, RTX 3070', 39990000.00, 8, 'Razer', 'Razer Blade 15 nổi tiếng với thiết kế mỏng nhẹ và hiệu năng vượt trội, tích hợp Intel i7, 16GB RAM, 1TB SSD NVMe và NVIDIA RTX 3070, lý tưởng cho game thủ và sáng tạo nội dung.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"1TB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3070\"}', 'Laptop', 'razer_blade_15.jpg', 0, 0),
('SP000134', 'Laptop Razer Blade Stealth 13 - Intel i5, 8GB, 512GB SSD, MX450', 28990000.00, 10, 'Razer', 'Razer Blade Stealth 13 là ultrabook nhỏ gọn với hiệu năng đủ dùng cho công việc và sáng tạo, sử dụng Intel i5, 8GB RAM, 512GB SSD NVMe và NVIDIA MX450.', '{\"CPU\": \"Intel Core i5-1135G7\", \"RAM\": \"8GB DDR4\", \"Screen\": \"13.3 inch FHD\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce MX450\"}', 'Laptop', 'razer_blade_stealth_13.jpg', 0, 0),
('SP000135', 'Laptop Gigabyte Aorus 15G - Intel i7, 16GB, 512GB SSD, RTX 3060', 26990000.00, 15, 'Gigabyte', 'Gigabyte Aorus 15G được xây dựng cho game thủ với Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3060, đảm bảo hiệu năng cao và độ ổn định.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3060\"}', 'Laptop', 'gigabyte_aorus_15g.jpg', 0, 0),
('SP000136', 'Laptop Gigabyte Aero 15 - Intel i9, 32GB, 1TB SSD, RTX 3080', 48990000.00, 4, 'Gigabyte', 'Gigabyte Aero 15 hướng tới các chuyên gia sáng tạo với Intel i9, 32GB RAM, 1TB SSD NVMe và NVIDIA RTX 3080, lý tưởng cho thiết kế đồ họa và dựng phim.', '{\"CPU\": \"Intel Core i9-12900H\", \"RAM\": \"32GB DDR4\", \"Screen\": \"15.6 inch FHD 240Hz\", \"SSD\": \"1TB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3080\"}', 'Laptop', 'gigabyte_aero_15.jpg', 0, 0),
('SP000137', 'Laptop Microsoft Surface Laptop 4 - Intel i7, 16GB, 512GB SSD, Iris Xe', 27990000.00, 12, 'Microsoft', 'Microsoft Surface Laptop 4 nổi bật với thiết kế tinh tế và hiệu năng ổn định nhờ Intel i7, 16GB RAM, 512GB SSD NVMe và đồ họa Intel Iris Xe, phù hợp cho công việc văn phòng.', '{\"CPU\": \"Intel Core i7-1165G7\", \"RAM\": \"16GB DDR4\", \"Screen\": \"13.5 inch PixelSense\", \"SSD\": \"512GB NVMe\", \"GPU\": \"Intel Iris Xe\"}', 'Laptop', 'microsoft_surface_laptop_4.jpg', 0, 0),
('SP000138', 'Laptop Microsoft Surface Book 3 - Intel i7, 16GB, 256GB SSD, GTX 1660 Ti', 31990000.00, 8, 'Microsoft', 'Microsoft Surface Book 3 có thiết kế độc đáo với màn hình tách rời, tích hợp Intel i7, 16GB RAM, 256GB SSD NVMe và NVIDIA GTX 1660 Ti, tối ưu cho công việc sáng tạo.', '{\"CPU\": \"Intel Core i7-1065G7\", \"RAM\": \"16GB DDR4\", \"Screen\": \"13.5 inch PixelSense\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce GTX 1660 Ti\"}', 'Laptop', 'microsoft_surface_book_3.jpg', 0, 0),
('SP000139', 'Laptop Samsung Galaxy Book Odyssey - Intel i7, 16GB, 512GB SSD, RTX 3050', 26990000.00, 10, 'Samsung', 'Samsung Galaxy Book Odyssey kết hợp thiết kế hiện đại với hiệu năng mạnh mẽ từ Intel i7, 16GB RAM, 512GB SSD NVMe và NVIDIA RTX 3050, lý tưởng cho cả công việc lẫn giải trí.', '{\"CPU\": \"Intel Core i7-12700H\", \"RAM\": \"16GB DDR4\", \"Screen\": \"15.6 inch FHD 144Hz\", \"SSD\": \"512GB NVMe\", \"GPU\": \"NVIDIA GeForce RTX 3050\"}', 'Laptop', 'samsung_galaxy_book_odyssey.jpg', 0, 0),
('SP000140', 'Laptop Samsung Notebook 9 Pro - Intel i5, 8GB, 256GB SSD, MX250', 22990000.00, 15, 'Samsung', 'Samsung Notebook 9 Pro là ultrabook nhẹ nhàng với hiệu năng ổn định từ Intel i5, 8GB RAM, 256GB SSD NVMe và NVIDIA MX250, phù hợp cho nhu cầu công việc hàng ngày.', '{\"CPU\": \"Intel Core i5-1135G7\", \"RAM\": \"8GB DDR4\", \"Screen\": \"15.6 inch FHD\", \"SSD\": \"256GB NVMe\", \"GPU\": \"NVIDIA GeForce MX250\"}', 'Laptop', 'samsung_notebook_9_pro.jpg', 0, 0),
('SP000141', 'PC Build Thunder X - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 34930000.00, 16, 'BPT', 'PC build hiệu năng cao với CPU Intel Core i9-14900K, 32GB DDR5, card đồ họa RTX 3060 12GB và ổ cứng 1TB NVMe, Model Thunder X.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_thunder_x.jpg', 0, 0),
('SP000142', 'PC Build Thunder X Pro - Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB', 35930000.00, 16, 'BPT', 'PC build mạnh mẽ với CPU Intel Core i9-14900K, 32GB DDR5, card đồ họa RTX 3070 8GB và ổ cứng 1TB NVMe, Model Thunder X Pro.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3070 8GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_thunder_x_pro.jpg', 0, 0),
('SP000143', 'PC Build Lightning A - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 36930000.00, 16, 'BPT', 'PC build cao cấp cho sáng tạo nội dung và gaming với CPU Intel Core i9-14900K, 32GB DDR5, card RTX 3060 12GB và ổ cứng 2TB NVMe, Model Lightning A.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"2TB NVMe\"}', 'PC', 'pc_lightning_a.jpg', 0, 0),
('SP000144', 'PC Build Lightning B - Intel Core i9-14900K, 64GB DDR5, RTX 3060 12GB', 37930000.00, 16, 'BPT', 'PC build với bộ nhớ khủng 64GB DDR5, tối ưu cho đa nhiệm, Model Lightning B.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_lightning_b.jpg', 0, 0),
('SP000145', 'PC Build Storm Alpha - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 35530000.00, 16, 'BPT', 'PC build hiệu năng mạnh mẽ, lý tưởng cho game và đồ họa, Model Storm Alpha.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_storm_alpha.jpg', 0, 0),
('SP000146', 'PC Build Storm Beta - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 35730000.00, 16, 'BPT', 'PC build với giải pháp tản nhiệt tiên tiến, Model Storm Beta.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_storm_beta.jpg', 0, 0),
('SP000147', 'PC Build Vortex - Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB', 36230000.00, 16, 'BPT', 'PC build mạnh mẽ cho gaming và sáng tạo nội dung, Model Vortex.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3070 8GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_vortex.jpg', 0, 0),
('SP000148', 'PC Build Vortex Plus - Intel Core i9-14900K, 32GB DDR5, RTX 3080 10GB', 37230000.00, 16, 'BPT', 'PC build với card đồ họa đỉnh cao, Model Vortex Plus.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3080 10GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_vortex_plus.jpg', 0, 0),
('SP000149', 'PC Build Nebula - Intel Core i9-14900K, 64GB DDR5, RTX 3060 12GB', 38230000.00, 16, 'BPT', 'PC build cho các tác vụ đồ họa nặng với bộ nhớ 64GB DDR5 và ổ cứng 2TB NVMe, Model Nebula.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"2TB NVMe\"}', 'PC', 'pc_nebula.jpg', 0, 0),
('SP000150', 'PC Build Nebula Ultra - Intel Core i9-14900K, 64GB DDR5, RTX 3080 10GB', 39230000.00, 16, 'BPT', 'PC build cao cấp với hiệu năng vượt trội, Model Nebula Ultra.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3080 10GB\", \"SSD\": \"2TB NVMe\"}', 'PC', 'pc_nebula_ultra.jpg', 0, 0),
('SP000151', 'PC Build Titan A - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 35030000.00, 16, 'BPT', 'PC build mạnh mẽ, tối ưu cho game và làm việc, Model Titan A.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_titan_a.jpg', 0, 0),
('SP000152', 'PC Build Titan B - Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB', 35930000.00, 16, 'BPT', 'PC build với hiệu năng vượt trội, Model Titan B.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3070 8GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_titan_b.jpg', 0, 0),
('SP000153', 'PC Build Titan X - Intel Core i9-14900K, 64GB DDR5, RTX 3080 10GB', 39930000.00, 16, 'BPT', 'PC build cao cấp dành cho game thủ chuyên nghiệp, Model Titan X.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3080 10GB\", \"SSD\": \"2TB NVMe\"}', 'PC', 'pc_titan_x.jpg', 0, 0),
('SP000154', 'PC Build Quantum - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 35130000.00, 16, 'BPT', 'PC build hiệu năng ổn định, Model Quantum.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_quantum.jpg', 0, 0),
('SP000155', 'PC Build Quantum Pro - Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB', 36130000.00, 16, 'BPT', 'PC build cao cấp với các thành phần hàng đầu, Model Quantum Pro.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3070 8GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_quantum_pro.jpg', 0, 0),
('SP000156', 'PC Build Apex - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 35330000.00, 16, 'BPT', 'PC build đa năng, lý tưởng cho sáng tạo nội dung, Model Apex.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_apex.jpg', 0, 0),
('SP000157', 'PC Build Apex X - Intel Core i9-14900K, 64GB DDR5, RTX 3080 10GB', 36930000.00, 16, 'BPT', 'PC build với cấu hình khủng, Model Apex X.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3080 10GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_apex_x.jpg', 0, 0),
('SP000158', 'PC Build Fusion - Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB', 34530000.00, 16, 'BPT', 'PC build mạnh mẽ, thiết kế hiện đại, Model Fusion.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3060 12GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_fusion.jpg', 0, 0),
('SP000159', 'PC Build Fusion Plus - Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB', 35530000.00, 16, 'BPT', 'PC build với hiệu năng vượt trội, Model Fusion Plus.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"32GB DDR5\", \"VGA\": \"RTX 3070 8GB\", \"SSD\": \"1TB NVMe\"}', 'PC', 'pc_fusion_plus.jpg', 0, 0),
('SP000160', 'PC Build Fusion Pro - Intel Core i9-14900K, 64GB DDR5, RTX 3080 10GB', 39530000.00, 16, 'BPT', 'PC build tối ưu cho công việc sáng tạo và gaming, Model Fusion Pro.', '{\"CPU\": \"Intel Core i9-14900K\", \"RAM\": \"64GB DDR5\", \"VGA\": \"RTX 3080 10GB\", \"SSD\": \"2TB NVMe\"}', 'PC', 'pc_fusion_pro.jpg', 0, 0),
('SP000161', 'Màn hình ASUS TUF Gaming VG249Q3A - Phantom 23.8\" 144Hz', 3050000.00, 34, 'Asus', 'Monitor gaming với tần số 144Hz, hình ảnh sắc nét, phiên bản Phantom của ASUS TUF Gaming.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'asus_tuf_phantom.jpg', 0, 0),
('SP000162', 'Màn hình Acer Nitro XV242Q - Predator 23.8\" 144Hz', 3100000.00, 30, 'Acer', 'Monitor gaming Acer Nitro XV242Q với thiết kế Predator, mang lại trải nghiệm mượt mà.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'acer_nitro_predator.jpg', 0, 0),
('SP000163', 'Màn hình LG UltraGear 24GN600-B 23.8\" 144Hz', 3080000.00, 28, 'LG', 'Monitor LG UltraGear 24GN600-B với thời gian phản hồi nhanh và màu sắc sống động.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'lg_ultragear_24gn600b.jpg', 0, 0),
('SP000164', 'Màn hình Dell S2421HGF 23.8\" 144Hz', 3070000.00, 26, 'Dell', 'Dell S2421HGF mang đến hình ảnh sắc nét và thiết kế hiện đại, phù hợp cho game thủ.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'dell_s2421hgf.jpg', 0, 0),
('SP000165', 'Màn hình Samsung Odyssey G3 23.8\" 144Hz', 3060000.00, 32, 'Samsung', 'Samsung Odyssey G3 với công nghệ hiển thị tiên tiến, độ tương phản cao cho trải nghiệm game.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'samsung_odyssey_g3.jpg', 0, 0),
('SP000166', 'Màn hình BenQ ZOWIE XL2411P 23.8\" 144Hz', 3090000.00, 22, 'BenQ', 'BenQ ZOWIE XL2411P mang lại trải nghiệm game competitive với thời gian phản hồi cực nhanh.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'benq_zowie_xl2411p.jpg', 0, 0),
('SP000167', 'Màn hình HP X24ih 23.8\" 144Hz', 3100000.00, 20, 'HP', 'HP X24ih cung cấp hình ảnh sống động và hiệu năng ổn định cho game thủ.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'hp_x24ih.jpg', 0, 0),
('SP000168', 'Màn hình ASUS ROG Strix XG24V 23.8\" 144Hz', 3120000.00, 18, 'Asus', 'ASUS ROG Strix XG24V với thiết kế sắc sảo và hiệu năng cao, tối ưu cho game competitive.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'asus_rog_strix_xg24v.jpg', 0, 0),
('SP000169', 'Màn hình Acer Predator XB253Q 23.8\" 144Hz', 3130000.00, 24, 'Acer', 'Acer Predator XB253Q mang lại trải nghiệm hình ảnh tuyệt vời với độ trễ thấp, lý tưởng cho game thủ.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'acer_predator_xb253q.jpg', 0, 0),
('SP000170', 'Màn hình Dell Alienware AW2521HF 23.8\" 144Hz', 3140000.00, 26, 'Dell', 'Dell Alienware AW2521HF với thiết kế độc đáo, mang lại tốc độ làm mới cực nhanh cho game competitive.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'dell_alienware_aw2521hf.jpg', 0, 0);
INSERT INTO `sanpham` (`IdSp`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000171', 'Màn hình Samsung CHG70 23.8\" 144Hz', 3150000.00, 30, 'Samsung', 'Samsung CHG70 mang đến màu sắc rực rỡ và độ tương phản cao, lý tưởng cho game và giải trí.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'samsung_chg70.jpg', 0, 0),
('SP000172', 'Màn hình BenQ EX2780Q 23.8\" 144Hz', 3160000.00, 28, 'BenQ', 'BenQ EX2780Q cung cấp hiệu năng và âm thanh sống động, phù hợp cho game và giải trí.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'benq_ex2780q.jpg', 0, 0),
('SP000173', 'Màn hình HP Omen X 23.8\" 144Hz', 3170000.00, 26, 'HP', 'HP Omen X thiết kế tối ưu cho game với thời gian phản hồi nhanh và hình ảnh sắc nét.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'hp_omen_x.jpg', 0, 0),
('SP000174', 'Màn hình ASUS TUF Gaming VG249Q3A - Stealth 23.8\" 144Hz', 3180000.00, 24, 'Asus', 'Phiên bản Stealth của ASUS TUF Gaming VG249Q3A với thiết kế mới và hiệu năng ưu việt.', '{\"Kích thước\": \"23.8 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'asus_tuf_stealth.jpg', 0, 0),
('SP000175', 'Màn hình Acer Nitro XV272U 27\" 144Hz', 3190000.00, 34, 'Acer', 'Acer Nitro XV272U mang lại trải nghiệm game mượt mà với màn hình 27 inch, tốc độ 144Hz và độ phân giải cao.', '{\"Kích thước\": \"27 inch\", \"Tần số quét\": \"144Hz\", \"Độ phân giải\": \"2560x1440\"}', 'Monitor', 'acer_nitro_xv272u.jpg', 0, 0),
('SP000176', 'Màn hình LG UltraGear 27GN750-B 27\" 240Hz', 3200000.00, 32, 'LG', 'LG UltraGear 27GN750-B mang lại tốc độ làm mới cực nhanh 240Hz, tối ưu cho game competitive.', '{\"Kích thước\": \"27 inch\", \"Tần số quét\": \"240Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'lg_ultragear_27gn750b.jpg', 0, 0),
('SP000177', 'Màn hình Dell S2721DGF 27\" 165Hz', 3210000.00, 30, 'Dell', 'Dell S2721DGF với độ phân giải 2560x1440 và tốc độ quét 165Hz, mang đến hình ảnh sống động.', '{\"Kích thước\": \"27 inch\", \"Tần số quét\": \"165Hz\", \"Độ phân giải\": \"2560x1440\"}', 'Monitor', 'dell_s2721dgf.jpg', 0, 0),
('SP000178', 'Màn hình Samsung Odyssey G7 27\" 240Hz', 3220000.00, 28, 'Samsung', 'Samsung Odyssey G7 với màn hình cong 27 inch, tốc độ 240Hz và độ phân giải 2560x1440, cho trải nghiệm gaming đỉnh cao.', '{\"Kích thước\": \"27 inch\", \"Tần số quét\": \"240Hz\", \"Độ phân giải\": \"2560x1440\"}', 'Monitor', 'samsung_odyssey_g7.jpg', 0, 0),
('SP000179', 'Màn hình BenQ ZOWIE XL2546K 24.5\" 240Hz', 3230000.00, 26, 'BenQ', 'BenQ ZOWIE XL2546K với thời gian phản hồi siêu nhanh, lý tưởng cho game competitive.', '{\"Kích thước\": \"24.5 inch\", \"Tần số quét\": \"240Hz\", \"Độ phân giải\": \"1920x1080\"}', 'Monitor', 'benq_zowie_xl2546k.jpg', 0, 0),
('SP000180', 'Màn hình HP Omen X 27\" 240Hz', 3240000.00, 34, 'HP', 'HP Omen X phiên bản 27 inch với tốc độ 240Hz, mang lại trải nghiệm hình ảnh vượt trội cho game thủ.', '{\"Kích thước\": \"27 inch\", \"Tần số quét\": \"240Hz\", \"Độ phân giải\": \"2560x1440\"}', 'Monitor', 'hp_omen_x_27.jpg', 0, 0),
('SP000181', 'SSD Samsung 980 PRO 1TB PCIe NVMe', 2500000.00, 200, 'Samsung', 'SSD NVMe 1TB với hiệu năng cực đỉnh, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\"}', 'Storage', 'ssd_samsung_980pro_a.jpg', 0, 0),
('SP000182', 'SSD Samsung 980 PRO 2TB PCIe NVMe', 4500000.00, 180, 'Samsung', 'SSD NVMe 2TB hiệu năng cao, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"2TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\"}', 'Storage', 'ssd_samsung_980pro_b.jpg', 0, 0),
('SP000183', 'SSD Crucial P5 1TB PCIe NVMe', 2200000.00, 210, 'Crucial', 'SSD NVMe 1TB với hiệu năng ổn định, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3400MB/s\"}', 'Storage', 'ssd_crucial_p5_a.jpg', 0, 0),
('SP000184', 'SSD Crucial MX500 1TB SATA', 1800000.00, 250, 'Crucial', 'SSD SATA 1TB với hiệu năng đáng tin cậy, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\"}', 'Storage', 'ssd_crucial_mx500_a.jpg', 0, 0),
('SP000185', 'SSD WD Blue 3D NAND 1TB SATA', 1900000.00, 230, 'WD', 'SSD SATA 1TB mang lại hiệu năng ổn định cho mọi hệ thống, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\"}', 'Storage', 'ssd_wd_blue_3dnand_a.jpg', 0, 0),
('SP000186', 'SSD WD Black SN750 1TB PCIe NVMe', 2600000.00, 220, 'WD', 'SSD NVMe 1TB hiệu năng mạnh mẽ, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3470MB/s\"}', 'Storage', 'ssd_wd_black_sn750_a.jpg', 0, 0),
('SP000187', 'SSD Kingston KC3000 1TB PCIe NVMe', 2700000.00, 210, 'Kingston', 'SSD NVMe 1TB với tốc độ siêu nhanh, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\"}', 'Storage', 'ssd_kingston_kc3000_a.jpg', 0, 0),
('SP000188', 'SSD Kingston A400 480GB SATA', 1200000.00, 250, 'Kingston', 'SSD SATA 480GB giá rẻ, hiệu năng ổn định, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"480GB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"500MB/s\"}', 'Storage', 'ssd_kingston_a400_a.jpg', 0, 0),
('SP000189', 'SSD Crucial MX500 500GB SATA', 1300000.00, 240, 'Crucial', 'SSD SATA 500GB với độ bền cao, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"500GB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\"}', 'Storage', 'ssd_crucial_mx500_b.jpg', 0, 0),
('SP000190', 'SSD SanDisk Ultra 3D 1TB SATA', 1850000.00, 230, 'SanDisk', 'SSD SATA 1TB với hiệu năng ổn định, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\"}', 'Storage', 'ssd_sandisk_ultra3d_a.jpg', 0, 0),
('SP000191', 'SSD Seagate FireCuda 520 1TB PCIe NVMe', 2800000.00, 220, 'Seagate', 'SSD NVMe 1TB với tốc độ đọc 5000MB/s, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"5000MB/s\"}', 'Storage', 'ssd_seagate_firecuda520_a.jpg', 0, 0),
('SP000192', 'SSD Seagate FireCuda 520 2TB PCIe NVMe', 4500000.00, 210, 'Seagate', 'SSD NVMe 2TB cao cấp cho game và đồ họa, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"2TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"5000MB/s\"}', 'Storage', 'ssd_seagate_firecuda520_b.jpg', 0, 0),
('SP000193', 'SSD PNY CS900 480GB SATA', 1100000.00, 240, 'PNY', 'SSD SATA 480GB với giá phải chăng, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"480GB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"535MB/s\"}', 'Storage', 'ssd_pny_cs900_a.jpg', 0, 0),
('SP000194', 'SSD Intel 660p 1TB PCIe NVMe', 2000000.00, 230, 'Intel', 'SSD NVMe 1TB Intel 660p hiệu năng tốt, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"1800MB/s\"}', 'Storage', 'ssd_intel_660p_a.jpg', 0, 0),
('SP000195', 'SSD Intel 660p 2TB PCIe NVMe', 3500000.00, 220, 'Intel', 'SSD NVMe 2TB Intel 660p với dung lượng lớn, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"2TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"1800MB/s\"}', 'Storage', 'ssd_intel_660p_b.jpg', 0, 0),
('SP000196', 'SSD ADATA XPG SX8200 Pro 1TB PCIe NVMe', 2300000.00, 230, 'ADATA', 'SSD NVMe 1TB với hiệu năng cao, Model A của ADATA XPG SX8200 Pro.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3500MB/s\"}', 'Storage', 'ssd_adata_xpg_a.jpg', 0, 0),
('SP000197', 'SSD ADATA XPG SX8200 Pro 512GB PCIe NVMe', 1500000.00, 240, 'ADATA', 'SSD NVMe 512GB ADATA XPG SX8200 Pro hiệu năng ấn tượng, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"512GB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3500MB/s\"}', 'Storage', 'ssd_adata_xpg_b.jpg', 0, 0),
('SP000198', 'SSD Corsair MP600 1TB PCIe NVMe', 2600000.00, 230, 'Corsair', 'SSD NVMe 1TB Corsair MP600 với hiệu năng vượt trội, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"4950MB/s\"}', 'Storage', 'ssd_corsair_mp600_a.jpg', 0, 0),
('SP000199', 'SSD Corsair MP600 2TB PCIe NVMe', 4000000.00, 220, 'Corsair', 'SSD NVMe 2TB Corsair MP600 cho các ứng dụng nặng, Model B.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"2TB\", \"Interface\": \"PCIe NVMe\", \"Tốc độ đọc\": \"4950MB/s\"}', 'Storage', 'ssd_corsair_mp600_b.jpg', 0, 0),
('SP000200', 'SSD Crucial BX500 1TB SATA', 1700000.00, 240, 'Crucial', 'SSD SATA 1TB Crucial BX500 cho hiệu năng ổn định, Model A.', '{\"Danh mục\": \"SSD\", \"Dung lượng\": \"1TB\", \"Interface\": \"SATA\", \"Tốc độ đọc\": \"540MB/s\"}', 'Storage', 'ssd_crucial_bx500_a.jpg', 0, 0),
('SP000201', 'HDD Western Caviar Blue 1TB 7200rpm - Model A', 1350000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model A: Đáng tin cậy, phù hợp cho lưu trữ dữ liệu cá nhân.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelA.jpg', 0, 0),
('SP000202', 'HDD Western Caviar Blue 1TB 7200rpm - Model B', 1355000.00, 20, 'Western', 'HDD 1TB 7200rpm – Model B: Hiệu năng cao cho môi trường văn phòng, hoạt động mát mẻ.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelB.jpg', 0, 0),
('SP000203', 'HDD Western Caviar Blue 1TB 7200rpm - Model C', 1360000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model C: Thiết kế chắc chắn, truyền dữ liệu nhanh, đảm bảo độ bền cao.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelC.jpg', 0, 0),
('SP000204', 'HDD Western Caviar Blue 1TB 7200rpm - Model D', 1365000.00, 20, 'Western', 'HDD 1TB 7200rpm – Model D: Lựa chọn phổ thông, hoạt động êm ái và ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelD.jpg', 0, 0),
('SP000205', 'HDD Western Caviar Blue 1TB 7200rpm - Model E', 1370000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model E: Thiết kế bền bỉ, thích hợp cho môi trường làm việc khắc nghiệt.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelE.jpg', 0, 0),
('SP000206', 'HDD Seagate Barracuda 1TB 7200rpm - Model A', 1300000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model A: Hiệu năng ổn định cho lưu trữ dữ liệu cá nhân.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelA.jpg', 0, 0),
('SP000207', 'HDD Seagate Barracuda 1TB 7200rpm - Model B', 1305000.00, 21, 'Seagate', 'HDD 1TB 7200rpm – Model B: Thiết kế mạnh mẽ, đáng tin cậy cho doanh nghiệp nhỏ.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelB.jpg', 0, 0),
('SP000208', 'HDD Seagate Barracuda 1TB 7200rpm - Model C', 1310000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model C: Truyền tải dữ liệu nhanh và độ bền cao.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelC.jpg', 0, 0),
('SP000209', 'HDD Seagate Barracuda 1TB 7200rpm - Model D', 1315000.00, 21, 'Seagate', 'HDD 1TB 7200rpm – Model D: Lựa chọn phổ thông cho văn phòng với hiệu năng ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelD.jpg', 0, 0),
('SP000210', 'HDD Seagate Barracuda 1TB 7200rpm - Model E', 1320000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model E: Hiệu năng vượt trội, phù hợp cho lưu trữ dữ liệu lớn.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelE.jpg', 0, 0),
('SP000211', 'HDD Toshiba P300 2TB 7200rpm - Model A', 1280000.00, 23, 'Toshiba', 'HDD 2TB 7200rpm – Model A: Lưu trữ ổn định cho hệ thống cá nhân.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"2TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelA.jpg', 0, 0),
('SP000212', 'HDD Toshiba P300 10TB 7200rpm - Model B', 1285000.00, 22, 'Toshiba', 'HDD 10TB 7200rpm – Model B: Thiết kế bền bỉ, hoạt động êm ái cho môi trường văn phòng.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"10TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelB.jpg', 0, 0),
('SP000213', 'HDD Toshiba P300 5TB 7200rpm - Model C', 1290000.00, 23, 'Toshiba', 'HDD 5TB 7200rpm – Model C: Hiệu năng cao, phù hợp cho các tác vụ lưu trữ chuyên sâu.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"5TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelC.jpg', 0, 0),
('SP000214', 'HDD Toshiba P300 1TB 7200rpm - Model D', 1295000.00, 22, 'Toshiba', 'HDD 1TB 7200rpm – Model D: Truyền dữ liệu nhanh, tiết kiệm điện năng.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelD.jpg', 0, 0),
('SP000215', 'HDD Toshiba P300 500GB 7200rpm - Model E', 1300000.00, 23, 'Toshiba', 'HDD 500GB 7200rpm – Model E: Lựa chọn chuyên nghiệp cho doanh nghiệp với hiệu năng ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"500GB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelE.jpg', 0, 0),
('SP000216', 'HDD HGST Ultrastar 1TB 7200rpm - Model A', 1400000.00, 20, 'HGST', 'HDD 1TB 7200rpm – Model A: Hiệu năng lưu trữ cao với độ tin cậy tuyệt đối.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelA.jpg', 0, 0),
('SP000217', 'HDD HGST Ultrastar 10TB 7200rpm - Model B', 1405000.00, 19, 'HGST', 'HDD 10TB 7200rpm – Model B: Lưu trữ hiệu quả cho dữ liệu lớn, phù hợp với doanh nghiệp.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"10TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelB.jpg', 0, 0),
('SP000218', 'HDD HGST Ultrastar 4TB 7200rpm - Model C', 1410000.00, 20, 'HGST', 'HDD 4TB 7200rpm – Model C: Thiết kế chuyên nghiệp, hoạt động ổn định trong môi trường khắt khe.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"4TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelC.jpg', 0, 0),
('SP000219', 'HDD HGST Ultrastar 2TB 7200rpm - Model D', 1415000.00, 19, 'HGST', 'HDD 2TB 7200rpm – Model D: Tốc độ truy xuất nhanh, lý tưởng cho các hệ thống doanh nghiệp.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"2TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelD.jpg', 0, 0),
('SP000220', 'HDD HGST Ultrastar 500GB 7200rpm - Model E', 1420000.00, 20, 'HGST', 'HDD 500GB 7200rpm – Model E: Sự lựa chọn hoàn hảo cho hệ thống lưu trữ cao cấp với hiệu năng vượt trội.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"500GB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelE.jpg', 0, 0),
('SP000221', 'Keyboard E-DRA EK506 Mechanical RGB Model A', 160000.00, 21, 'E-DRA', 'Bàn phím cơ E-DRA EK506 Model A với switch chất lượng, đèn RGB sáng rực và thiết kế full-size (104 keys).', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Connection\": \"Wired\"}', 'Peripherals', 'keyboard_edra_modelA.jpg', 0, 0),
('SP000222', 'Keyboard Logitech G513 Mechanical RGB', 180000.00, 25, 'Logitech', 'Bàn phím cơ Logitech G513 với switch Romer-G, thiết kế hiện đại và đèn nền RGB có thể tùy chỉnh.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Connection\": \"Wired\"}', 'Peripherals', 'keyboard_logitech_g513.jpg', 0, 0),
('SP000223', 'Keyboard Razer BlackWidow V3 Mechanical RGB', 220000.00, 18, 'Razer', 'Bàn phím cơ Razer BlackWidow V3 với switch Razer Green, mang lại phản hồi gõ nhanh và ánh sáng RGB cá nhân hóa.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Razer Green\"}', 'Peripherals', 'keyboard_razer_bw_v3.jpg', 0, 0),
('SP000224', 'Keyboard Corsair K70 RGB MK.2 Mechanical', 250000.00, 20, 'Corsair', 'Bàn phím cơ Corsair K70 RGB MK.2 với switch Cherry MX Speed, thiết kế bền bỉ và đèn nền RGB tuyệt đẹp.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Cherry MX Speed\"}', 'Peripherals', 'keyboard_corsair_k70.jpg', 0, 0),
('SP000225', 'Keyboard HyperX Alloy FPS Pro Mechanical', 150000.00, 30, 'HyperX', 'Bàn phím tenkeyless HyperX Alloy FPS Pro với thiết kế tối giản, switch Cherry MX và ánh sáng LED đỏ tinh tế.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"87 keys\", \"RGB\": \"Yes\", \"Switch\": \"Cherry MX\"}', 'Peripherals', 'keyboard_hyperx_alloy_fps_pro.jpg', 0, 0),
('SP000226', 'Keyboard SteelSeries Apex Pro Mechanical', 300000.00, 15, 'SteelSeries', 'Bàn phím cơ SteelSeries Apex Pro với công nghệ OmniPoint cho khả năng điều chỉnh độ nhạy từng phím và đèn RGB đầy cá nhân hóa.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"OmniPoint\"}', 'Peripherals', 'keyboard_steelseries_apex_pro.jpg', 0, 0),
('SP000227', 'Keyboard Ducky One 2 Mini Mechanical RGB', 190000.00, 25, 'Ducky', 'Bàn phím 60% Ducky One 2 Mini với thiết kế nhỏ gọn, đèn RGB rực rỡ và keycaps chất lượng cao.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"60% keys\", \"RGB\": \"Yes\"}', 'Peripherals', 'keyboard_ducky_one2_mini.jpg', 0, 0),
('SP000228', 'Keyboard ASUS ROG Strix Scope Mechanical', 210000.00, 22, 'ASUS', 'Bàn phím ASUS ROG Strix Scope với layout đặc biệt cho game FPS, mang đến phản hồi nhanh và đèn RGB ấn tượng.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Special\": \"FPS layout\"}', 'Peripherals', 'keyboard_asus_rog_strix_scope.jpg', 0, 0),
('SP000229', 'Keyboard Cooler Master CK552 Mechanical RGB', 170000.00, 28, 'Cooler Master', 'Bàn phím cơ Cooler Master CK552 với switch Outemu, thiết kế tối ưu và đèn RGB tùy chỉnh cho hiệu suất gõ mượt mà.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Outemu\"}', 'Peripherals', 'keyboard_cooler_master_ck552.jpg', 0, 0),
('SP000230', 'Keyboard MSI Vigor GK70 Mechanical RGB', 165000.00, 24, 'MSI', 'Bàn phím MSI Vigor GK70 với thiết kế độc đáo, độ bền cao và đèn nền RGB sặc sỡ, phù hợp cho game thủ.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Custom\"}', 'Peripherals', 'keyboard_msi_vigor_gk70.jpg', 0, 0),
('SP000231', 'Keyboard E-DRA EK506 Wireless Mechanical RGB', 162000.00, 21, 'E-DRA', 'Phiên bản không dây của E-DRA EK506 với kết nối ổn định, đèn RGB cá nhân hóa và thiết kế full-size.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Wireless\": \"Yes\"}', 'Peripherals', 'keyboard_edra_wireless.jpg', 0, 0),
('SP000232', 'Keyboard Logitech G Pro X Mechanical RGB', 175000.00, 23, 'Logitech', 'Bàn phím Logitech G Pro X với khả năng thay switch hot-swappable, thiết kế gọn nhẹ cho game thủ chuyên nghiệp.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"87 keys\", \"RGB\": \"Yes\", \"Switch\": \"Hot-swappable\"}', 'Peripherals', 'keyboard_logitech_gprox.jpg', 0, 0),
('SP000233', 'Keyboard Razer Huntsman Elite Mechanical RGB', 240000.00, 20, 'Razer', 'Bàn phím Razer Huntsman Elite với switch opto-mechanical, thiết kế sang trọng và đèn RGB đa hiệu ứng.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Opto-mechanical\"}', 'Peripherals', 'keyboard_razer_huntsman_elite.jpg', 0, 0),
('SP000234', 'Keyboard Corsair K95 RGB Platinum XT Mechanical', 280000.00, 18, 'Corsair', 'Bàn phím Corsair K95 RGB Platinum XT với khung hợp kim, hỗ trợ macro và switch Cherry MX Speed cho trải nghiệm tối ưu.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Cherry MX Speed\"}', 'Peripherals', 'keyboard_corsair_k95_rgb_platinum_xt.jpg', 0, 0),
('SP000235', 'Keyboard HyperX Alloy Elite 2 Mechanical RGB', 195000.00, 22, 'HyperX', 'Bàn phím HyperX Alloy Elite 2 với khung kim loại chắc chắn, phản hồi gõ nhanh và đèn RGB sống động.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Cherry MX\"}', 'Peripherals', 'keyboard_hyperx_alloy_elite2.jpg', 0, 0),
('SP000236', 'Keyboard SteelSeries Apex 7 Mechanical', 210000.00, 19, 'SteelSeries', 'Bàn phím SteelSeries Apex 7 với khả năng tùy chỉnh độ nhạy từng phím nhờ switch OmniPoint, mang lại trải nghiệm chơi game chuyên nghiệp.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"OmniPoint\"}', 'Peripherals', 'keyboard_steelseries_apex7.jpg', 0, 0),
('SP000237', 'Keyboard Ducky Shine 7 Mechanical RGB', 230000.00, 20, 'Ducky', 'Bàn phím Ducky Shine 7 với thiết kế tinh tế, keycaps PBT chất lượng và hệ thống đèn RGB đồng bộ.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\"}', 'Peripherals', 'keyboard_ducky_shine7.jpg', 0, 0),
('SP000238', 'Keyboard ASUS ROG Falchion Wireless Mechanical', 190000.00, 23, 'ASUS', 'Bàn phím ASUS ROG Falchion không dây với kích thước tenkeyless, kết nối ổn định và đèn RGB tích hợp.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"87 keys\", \"RGB\": \"Yes\", \"Wireless\": \"Yes\"}', 'Peripherals', 'keyboard_asus_rog_falchion.jpg', 0, 0),
('SP000239', 'Keyboard Cooler Master CK550 Mechanical', 175000.00, 25, 'Cooler Master', 'Bàn phím Cooler Master CK550 với switch Outemu, thiết kế chắc chắn và đèn nền RGB tùy chỉnh cho hiệu suất gõ tốt.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Outemu\"}', 'Peripherals', 'keyboard_cooler_master_ck550.jpg', 0, 0),
('SP000240', 'Keyboard MSI Vigor GK72 Mechanical RGB', 180000.00, 22, 'MSI', 'Bàn phím MSI Vigor GK72 với thiết kế độc đáo, độ bền cao và hệ thống đèn RGB sặc sỡ, phù hợp cho game thủ đòi hỏi hiệu năng.', '{\"Danh mục\": \"Keyboard\", \"Layout\": \"104 keys\", \"RGB\": \"Yes\", \"Switch\": \"Custom\"}', 'Peripherals', 'keyboard_msi_vigor_gk72.jpg', 0, 0),
('SP000241', 'Mouse ASUS TUF Gaming M3 Wired', 400000.00, 21, 'Asus', 'Chuột gaming ASUS TUF Gaming M3 Wired với cảm biến 16000 DPI, thiết kế công thái học và độ bền cao.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\"}', 'Peripherals', 'mouse_asus_tuf_m3.jpg', 0, 0),
('SP000242', 'Mouse Logitech G502 HERO Wired', 450000.00, 30, 'Logitech', 'Chuột Logitech G502 HERO với cảm biến HERO 25K, 11 nút lập trình và đèn RGB tùy chỉnh, phù hợp cho game thủ chuyên nghiệp.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"No\", \"Buttons\": \"11\"}', 'Peripherals', 'mouse_logitech_g502.jpg', 0, 0),
('SP000243', 'Mouse Razer DeathAdder V2 Wired', 420000.00, 25, 'Razer', 'Chuột Razer DeathAdder V2 với cảm biến Focus+ đạt 20000 DPI và thiết kế công thái học giúp giảm mỏi tay khi chơi game.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"No\", \"Buttons\": \"8\"}', 'Peripherals', 'mouse_razer_deathadder_v2.jpg', 0, 0),
('SP000244', 'Mouse Corsair Harpoon RGB Wireless', 500000.00, 18, 'Corsair', 'Chuột Corsair Harpoon RGB Wireless với kết nối không dây ổn định, đèn LED RGB đa sắc và cảm biến 18000 DPI.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"18000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\"}', 'Peripherals', 'mouse_corsair_harpoon.jpg', 0, 0),
('SP000245', 'Mouse SteelSeries Rival 3 Wireless', 470000.00, 20, 'SteelSeries', 'Chuột SteelSeries Rival 3 Wireless với thời gian phản hồi nhanh và pin lâu dài, lý tưởng cho game thủ di động.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\"}', 'Peripherals', 'mouse_steelseries_rival3.jpg', 0, 0),
('SP000246', 'Mouse HyperX Pulsefire Surge Wired', 430000.00, 22, 'HyperX', 'Chuột HyperX Pulsefire Surge với đèn RGB 360° và cảm biến 16000 DPI, mang lại độ chính xác cao trong mọi tình huống.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"7\"}', 'Peripherals', 'mouse_hyperx_pulsefire_surge.jpg', 0, 0),
('SP000247', 'Mouse Logitech G Pro Wireless', 550000.00, 15, 'Logitech', 'Chuột Logitech G Pro Wireless nhẹ và linh hoạt, sử dụng cảm biến HERO 25K cho hiệu suất tối ưu.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"5\"}', 'Peripherals', 'mouse_logitech_gpro_wireless.jpg', 0, 0),
('SP000248', 'Mouse Razer Viper Ultimate Wireless', 600000.00, 14, 'Razer', 'Chuột Razer Viper Ultimate Wireless với cảm biến 20000 DPI, thiết kế siêu nhẹ và kết nối không dây mượt mà.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"Yes\", \"Buttons\": \"8\"}', 'Peripherals', 'mouse_razer_viper_ultimate.jpg', 0, 0),
('SP000249', 'Mouse ASUS ROG Strix Impact II Wireless', 520000.00, 19, 'Asus', 'Chuột ASUS ROG Strix Impact II Wireless với kích thước nhỏ gọn, độ chính xác cao và kết nối không dây ổn định.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\"}', 'Peripherals', 'mouse_asus_rog_strix_impact_ii.jpg', 0, 0),
('SP000250', 'Mouse Cooler Master MM710 Wired Lightweight', 390000.00, 24, 'Cooler Master', 'Chuột Cooler Master MM710 với thiết kế dạng lưới siêu nhẹ, cảm biến 16000 DPI và phản hồi cực nhanh, phù hợp cho di chuyển trong game.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"53g\"}', 'Peripherals', 'mouse_cooler_master_mm710.jpg', 0, 0),
('SP000251', 'Mouse Glorious Model O Wired', 410000.00, 23, 'Glorious', 'Chuột Glorious Model O với thiết kế dạng lưới, siêu nhẹ và cảm biến 16000 DPI, đem lại trải nghiệm chơi game mượt mà.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"67g\"}', 'Peripherals', 'mouse_glorious_model_o.jpg', 0, 0),
('SP000252', 'Mouse Razer Basilisk V3 Wired', 430000.00, 21, 'Razer', 'Chuột Razer Basilisk V3 với tính năng tùy chỉnh nút bấm, cảm biến 20000 DPI và thiết kế công thái học chuyên dụng cho game thủ.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"No\", \"Buttons\": \"11\"}', 'Peripherals', 'mouse_razer_basilisk_v3.jpg', 0, 0),
('SP000253', 'Mouse Logitech G304 LIGHTSPEED Wireless', 450000.00, 22, 'Logitech', 'Chuột Logitech G304 LIGHTSPEED không dây với cảm biến HERO 25000, thời gian phản hồi nhanh và pin sử dụng lâu dài.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\"}', 'Peripherals', 'mouse_logitech_g304.jpg', 0, 0),
('SP000254', 'Mouse Corsair Ironclaw RGB Wireless', 480000.00, 20, 'Corsair', 'Chuột Corsair Ironclaw RGB Wireless được thiết kế dành riêng cho tay lớn với cảm biến 16000 DPI và đèn RGB cá nhân hóa.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\", \"Ergonomic\": \"Yes\"}', 'Peripherals', 'mouse_corsair_ironclaw.jpg', 0, 0),
('SP000255', 'Mouse SteelSeries Rival 650 Wireless', 530000.00, 18, 'SteelSeries', 'Chuột SteelSeries Rival 650 Wireless với tính năng sạc không dây và cảm biến TrueMove3+ cho độ chính xác tuyệt đối.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"12000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\", \"Charging\": \"Wireless\"}', 'Peripherals', 'mouse_steelseries_rival650.jpg', 0, 0),
('SP000256', 'Mouse HyperX Pulsefire Dart Wireless', 510000.00, 17, 'HyperX', 'Chuột HyperX Pulsefire Dart Wireless với thiết kế êm tay, kết nối không dây tốc độ cao và cảm biến 18000 DPI.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"18000\", \"Wireless\": \"Yes\", \"Buttons\": \"8\"}', 'Peripherals', 'mouse_hyperx_pulsefire_dart.jpg', 0, 0),
('SP000257', 'Mouse Logitech G305 LIGHTSPEED Wireless', 420000.00, 25, 'Logitech', 'Chuột Logitech G305 LIGHTSPEED không dây với cảm biến HERO 25000 và thiết kế nhỏ gọn, thích hợp cho di chuyển.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\"}', 'Peripherals', 'mouse_logitech_g305.jpg', 0, 0),
('SP000258', 'Mouse Razer Viper Mini Wired', 380000.00, 26, 'Razer', 'Chuột Razer Viper Mini với thiết kế siêu nhẹ, cảm biến 8500 DPI, lý tưởng cho game thủ cần tốc độ di chuyển nhanh.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"8500\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"62g\"}', 'Peripherals', 'mouse_razer_viper_mini.jpg', 0, 0),
('SP000259', 'Mouse Cooler Master MM710 Pro Wired', 397000.00, 23, 'Cooler Master', 'Chuột Cooler Master MM710 Pro với thiết kế siêu nhẹ, cảm biến 16000 DPI và độ bền cao, tối ưu cho game thủ chuyên nghiệp.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"55g\"}', 'Peripherals', 'mouse_cooler_master_mm710_pro.jpg', 0, 0),
('SP000260', 'Mouse Glorious Model O Wireless', 560000.00, 20, 'Glorious', 'Chuột Glorious Model O Wireless với thiết kế dạng lưới siêu nhẹ, cảm biến 19000 DPI và kết nối không dây ổn định, mang lại trải nghiệm chơi game đỉnh cao.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"19000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"66g\"}', 'Peripherals', 'mouse_glorious_model_o_wireless.jpg', 0, 0),
('SP000261', 'Headphone HyperX Cloud III Over-Ear Model A', 2250000.00, 40, 'HyperX', 'Tai nghe HyperX Cloud III Model A với công nghệ khử tiếng ồn tiên tiến và âm thanh vòm sống động, lý tưởng cho game thủ.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Mic\": \"Yes\", \"Color\": \"Black\"}', 'Peripherals', 'headphone_modelA.jpg', 0, 0),
('SP000262', 'Headphone Razer Kraken X Over-Ear Model B', 2100000.00, 35, 'Razer', 'Model B của Razer Kraken X có thiết kế siêu nhẹ cùng âm thanh sống động, tối ưu cho những giờ chơi game dài.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Mic\": \"Yes\", \"Weight\": \"250g\"}', 'Peripherals', 'headphone_modelB.jpg', 0, 0),
('SP000263', 'Headphone Logitech G Pro X Over-Ear Model C', 2400000.00, 38, 'Logitech', 'Logitech G Pro X Model C nổi bật với microphone Blue VO!CE cho chất lượng thu âm đỉnh cao, phù hợp cho streamers.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Mic\": \"Blue VO!CE\", \"Color\": \"Gray\"}', 'Peripherals', 'headphone_modelC.jpg', 0, 0),
('SP000264', 'Headphone Sennheiser HD 660 S Over-Ear Model D', 2600000.00, 30, 'Sennheiser', 'Sennheiser HD 660 S Model D mang đến âm thanh tự nhiên, chi tiết, lý tưởng cho những người đam mê âm nhạc.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Impedance\": \"150 Ohm\"}', 'Peripherals', 'headphone_modelD.jpg', 0, 0),
('SP000265', 'Headphone Audio-Technica ATH-M50x Over-Ear Model E', 2300000.00, 50, 'Audio-Technica', 'Audio-Technica ATH-M50x Model E cung cấp âm bass mạnh mẽ và dải tần số rộng, phù hợp cho cả thu âm và nghe nhạc.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Frequency Response\": \"15-28000 Hz\"}', 'Peripherals', 'headphone_modelE.jpg', 0, 0),
('SP000266', 'Headphone SteelSeries Arctis 7 Over-Ear Model F', 2350000.00, 33, 'SteelSeries', 'SteelSeries Arctis 7 Model F đem lại trải nghiệm không dây ổn định với âm thanh sống động và thiết kế thoải mái.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Battery Life\": \"20h\"}', 'Peripherals', 'headphone_modelF.jpg', 0, 0),
('SP000267', 'Headphone Beyerdynamic DT 990 Pro Over-Ear Model G', 2450000.00, 42, 'Beyerdynamic', 'Beyerdynamic DT 990 Pro Model G mang lại chất âm mở và âm thanh chất lượng cao cho cả phòng thu và giải trí.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Impedance\": \"250 Ohm\"}', 'Peripherals', 'headphone_modelG.jpg', 0, 0),
('SP000268', 'Headphone Bose QuietComfort 35 II Over-Ear Model H', 2800000.00, 37, 'Bose', 'Bose QuietComfort 35 II Model H nổi bật với công nghệ khử tiếng ồn chủ động, đem lại sự yên tĩnh tuyệt đối trong môi trường ồn ào.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Noise Cancelling\": \"Active\"}', 'Peripherals', 'headphone_modelH.jpg', 0, 0),
('SP000269', 'Headphone Sony WH-1000XM4 Over-Ear Model I', 2900000.00, 29, 'Sony', 'Sony WH-1000XM4 Model I cung cấp âm thanh chất lượng cao cùng công nghệ chống ồn hàng đầu và kết nối không dây mạnh mẽ.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Noise Cancelling\": \"Advanced\"}', 'Peripherals', 'headphone_modelI.jpg', 0, 0),
('SP000270', 'Headphone Jabra Elite 85h Over-Ear Model J', 2750000.00, 47, 'Jabra', 'Jabra Elite 85h Model J mang đến hiệu suất âm thanh vượt trội với khả năng điều chỉnh thông minh và pin lâu dài.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Battery Life\": \"36h\"}', 'Peripherals', 'headphone_modelJ.jpg', 0, 0),
('SP000271', 'Headphone Plantronics RIG 800LX Over-Ear Model K', 2200000.00, 55, 'Plantronics', 'Plantronics RIG 800LX Model K với thiết kế tiên tiến, mang lại âm thanh sống động và tích hợp microphone hiệu quả.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Mic\": \"Yes\"}', 'Peripherals', 'headphone_modelK.jpg', 0, 0),
('SP000272', 'Headphone Corsair HS70 Pro Over-Ear Model L', 2255000.00, 40, 'Corsair', 'Corsair HS70 Pro Model L kết hợp giữa âm thanh ấn tượng và kết nối không dây ổn định, thích hợp cho game và giải trí.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Battery Life\": \"16h\"}', 'Peripherals', 'headphone_modelL.jpg', 0, 0),
('SP000273', 'Headphone Microsoft Surface Headphones 2 Over-Ear Model M', 2650000.00, 35, 'Microsoft', 'Microsoft Surface Headphones 2 Model M đem đến trải nghiệm âm thanh tinh tế với công nghệ khử tiếng ồn chủ động và thiết kế sang trọng.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Noise Cancelling\": \"Active\"}', 'Peripherals', 'headphone_modelM.jpg', 0, 0),
('SP000274', 'Headphone Turtle Beach Elite Pro Over-Ear Model N', 2150000.00, 60, 'Turtle Beach', 'Turtle Beach Elite Pro Model N được thiết kế riêng cho game thủ với âm thanh mạnh mẽ và microphone rõ nét.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Mic\": \"Yes\", \"Color\": \"Blue\"}', 'Peripherals', 'headphone_modelN.jpg', 0, 0),
('SP000275', 'Headphone AKG K702 Over-Ear Model O', 2550000.00, 42, 'AKG', 'AKG K702 Model O nổi bật với âm thanh mở rộng và chi tiết, là lựa chọn tuyệt vời cho các chuyên gia âm thanh.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Impedance\": \"62 Ohm\"}', 'Peripherals', 'headphone_modelO.jpg', 0, 0),
('SP000276', 'Headphone JBL Quantum 800 Over-Ear Model P', 2405000.00, 44, 'JBL', 'JBL Quantum 800 Model P cung cấp âm thanh sống động và kết nối không dây ổn định, tối ưu cho trải nghiệm chơi game.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Battery Life\": \"14h\"}', 'Peripherals', 'headphone_modelP.jpg', 0, 0),
('SP000277', 'Headphone Beats Studio3 Over-Ear Model Q', 2500000.00, 39, 'Beats', 'Beats Studio3 Model Q mang đến âm thanh mạnh mẽ với thiết kế hiện đại, phù hợp với phong cách sống năng động.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Noise Cancelling\": \"Yes\"}', 'Peripherals', 'headphone_modelQ.jpg', 0, 0),
('SP000278', 'Headphone Shure SRH1540 Over-Ear Model R', 2655000.00, 31, 'Shure', 'Shure SRH1540 Model R được thiết kế cho âm thanh chính xác và độ bền cao, lý tưởng cho các phòng thu chuyên nghiệp.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"Impedance\": \"46 Ohm\"}', 'Peripherals', 'headphone_modelR.jpg', 0, 0),
('SP000279', 'Headphone Razer Nari Ultimate Over-Ear Model S', 2755000.00, 28, 'Razer', 'Razer Nari Ultimate Model S tích hợp công nghệ rung cảm ứng âm thanh và sạc nhanh, đem lại trải nghiệm game độc đáo.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wireless\", \"Haptic Feedback\": \"Yes\", \"Battery Life\": \"15h\"}', 'Peripherals', 'headphone_modelS.jpg', 0, 0),
('SP000280', 'Headphone HyperX Cloud Orbit S Over-Ear Model T', 2950000.00, 36, 'HyperX', 'HyperX Cloud Orbit S Model T tích hợp công nghệ âm thanh 3D cá nhân hóa, đem lại trải nghiệm game sống động và chính xác.', '{\"Danh mục\": \"Headphone\", \"Type\": \"Over-Ear\", \"Connectivity\": \"Wired\", \"3D Audio\": \"Yes\", \"Mic\": \"Yes\"}', 'Peripherals', 'headphone_modelT.jpg', 0, 0),
('SP000281', 'Router ASUS RT-AX1800HP Dual-Band Model A', 1390000.00, 5, 'ASUS', 'Router ASUS RT-AX1800HP với công nghệ WiFi 6, hiệu suất ổn định và 4 cổng LAN, Model A.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Ports\": \"4\"}', 'Network', 'router_modelA.jpg', 0, 0),
('SP000282', 'Router Netgear Nighthawk RAX20 Dual-Band Model B', 1490000.00, 7, 'Netgear', 'Router Netgear Nighthawk RAX20 tích hợp WiFi 6, mang đến hiệu năng mạnh mẽ và thiết kế hiện đại, Model B.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Ports\": \"4\"}', 'Network', 'router_modelB.jpg', 0, 0),
('SP000283', 'Router TP-Link Archer AX21 Dual-Band Model C', 1290000.00, 8, 'TP-Link', 'Router TP-Link Archer AX21 với công nghệ WiFi 6, đảm bảo kết nối ổn định và bốn ăng-ten tối ưu, Model C.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Antennas\": \"4\"}', 'Network', 'router_modelC.jpg', 0, 0),
('SP000284', 'Router Linksys EA7500 Dual-Band Model D', 1350000.00, 6, 'Linksys', 'Router Linksys EA7500 với hiệu suất cao và thiết kế hiện đại, hỗ trợ WiFi 5 cho độ tin cậy tối đa, Model D.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\"}', 'Network', 'router_modelD.jpg', 0, 0),
('SP000285', 'Router D-Link DIR-3060 Dual-Band Model E', 1285000.00, 9, 'D-Link', 'Router D-Link DIR-3060 mang đến kết nối nhanh chóng và ổn định với hỗ trợ USB chia sẻ dữ liệu, Model E.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"USB\": \"Yes\"}', 'Network', 'router_modelE.jpg', 0, 0),
('SP000286', 'Router Huawei WiFi Q2 Pro Tri-Band Model F', 1590000.00, 4, 'Huawei', 'Router Huawei WiFi Q2 Pro tích hợp công nghệ Tri-Band, cho tín hiệu mạnh mẽ và khả năng mở rộng Mesh, Model F.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Tri-Band\", \"WiFi\": \"6\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelF.jpg', 0, 0),
('SP000287', 'Router Ubiquiti UniFi Dream Machine Model G', 1790000.00, 3, 'Ubiquiti', 'Router Ubiquiti UniFi Dream Machine tích hợp bộ điều khiển mạng tiên tiến, phù hợp cho doanh nghiệp nhỏ, Model G.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Controller\": \"Built-in\"}', 'Network', 'router_modelG.jpg', 0, 0),
('SP000288', 'Router Google Nest Wifi Dual-Band Model H', 1690000.00, 5, 'Google', 'Router Google Nest Wifi với hệ thống Mesh giúp phủ sóng toàn diện cho ngôi nhà, Model H.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelH.jpg', 0, 0),
('SP000289', 'Router MikroTik hAP ac2 Dual-Band Model I', 1190000.00, 10, 'MikroTik', 'Router MikroTik hAP ac2 với hiệu suất mạnh mẽ, thiết kế bền bỉ và 5 cổng LAN, Model I.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Ports\": \"5\"}', 'Network', 'router_modelI.jpg', 0, 0),
('SP000290', 'Router Synology RT2600ac Dual-Band Model J', 1890000.00, 4, 'Synology', 'Router Synology RT2600ac hỗ trợ quản lý mạng chuyên sâu cùng các tính năng bảo mật tiên tiến, Model J.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Security\": \"Advanced\"}', 'Network', 'router_modelJ.jpg', 0, 0),
('SP000291', 'Router ASUS RT-AX88U Dual-Band Model K', 1990000.00, 2, 'ASUS', 'Router ASUS RT-AX88U tích hợp WiFi 6 với tốc độ cực cao và 8 cổng LAN, Model K.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Ports\": \"8\"}', 'Network', 'router_modelK.jpg', 0, 0),
('SP000292', 'Router Netgear Orbi RBK752 Tri-Band Model L', 2090000.00, 3, 'Netgear', 'Router Netgear Orbi RBK752 với hệ thống Mesh tiên tiến, giúp phủ sóng liên tục và hiệu quả, Model L.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Tri-Band\", \"WiFi\": \"6\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelL.jpg', 0, 0),
('SP000293', 'Router TP-Link Deco X60 Mesh WiFi Tri-Band Model M', 1995000.00, 6, 'TP-Link', 'Router TP-Link Deco X60 với hệ thống Mesh thông minh, mang lại kết nối liền mạch cho gia đình, Model M.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Tri-Band\", \"WiFi\": \"6\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelM.jpg', 0, 0),
('SP000294', 'Router Linksys Velop MX10600 Mesh WiFi Tri-Band Model N', 2290000.00, 2, 'Linksys', 'Router Linksys Velop MX10600 với thiết kế Mesh tối ưu, đảm bảo kết nối mạnh mẽ trong toàn bộ ngôi nhà, Model N.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Tri-Band\", \"WiFi\": \"6\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelN.jpg', 0, 0),
('SP000295', 'Router D-Link COVR-2202 Mesh WiFi Dual-Band Model O', 1595000.00, 4, 'D-Link', 'Router D-Link COVR-2202 tích hợp công nghệ Mesh, mang đến tốc độ và sự ổn định trong kết nối, Model O.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelO.jpg', 0, 0),
('SP000296', 'Router Huawei 5G CPE Pro 2 Model P', 2590000.00, 3, 'Huawei', 'Router Huawei 5G CPE Pro 2 hỗ trợ kết nối 5G tốc độ cao, lý tưởng cho ứng dụng công nghệ mới, Model P.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"5G\": \"Yes\"}', 'Network', 'router_modelP.jpg', 0, 0),
('SP000297', 'Router Ubiquiti EdgeRouter 4 Gigabit Model Q', 1895000.00, 5, 'Ubiquiti', 'Router Ubiquiti EdgeRouter 4 với khả năng định tuyến chuyên nghiệp và 4 cổng Gigabit, Model Q.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Wired\", \"WiFi\": \"None\", \"Ports\": \"4\"}', 'Network', 'router_modelQ.jpg', 0, 0),
('SP000298', 'Router Google Wifi Mesh Model R', 1695000.00, 7, 'Google', 'Router Google Wifi với hệ thống Mesh giúp phủ sóng liên tục, Model R, phù hợp cho ngôi nhà thông minh.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelR.jpg', 0, 0),
('SP000299', 'Router MikroTik RB2011UiAS-2HnD-IN Dual-Band Model S', 1090000.00, 8, 'MikroTik', 'Router MikroTik RB2011UiAS-2HnD-IN với khả năng định tuyến linh hoạt, 10 cổng LAN và thiết kế chắc chắn, Model S.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Ports\": \"10\"}', 'Network', 'router_modelS.jpg', 0, 0),
('SP000300', 'Router Synology MR2200ac Mesh WiFi Dual-Band Model T', 2190000.00, 3, 'Synology', 'Router Synology MR2200ac với công nghệ Mesh và hệ thống quản lý thông minh, mang đến hiệu suất vượt trội, Model T.', '{\"Danh mục\": \"Router\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Mesh\": \"Yes\"}', 'Network', 'router_modelT.jpg', 0, 0),
('SP000301', 'Wifi Card ASUS PCE-AX3000 Dual-Band Model A', 690000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AX3000 hỗ trợ WiFi 6, thiết kế tích hợp anten kép cho hiệu suất tối ưu, Model A.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelA.jpg', 0, 0),
('SP000302', 'Wifi Card TP-Link Archer T6E Dual-Band Model B', 695000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer T6E hỗ trợ chuẩn WiFi 5 với tốc độ truyền tải ổn định, Model B.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelB.jpg', 0, 0),
('SP000303', 'Wifi Card Intel Wi-Fi 6 AX200 Dual-Band Model C', 700000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6 AX200 cung cấp tốc độ cao và độ ổn định tuyệt vời, Model C.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelC.jpg', 0, 0),
('SP000304', 'Wifi Card Gigabyte GC-WBAX200 Dual-Band Model D', 705000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WBAX200 hỗ trợ WiFi 6 và Bluetooth 5.0, Model D.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Bluetooth\": \"5.0\"}', 'Network', 'wifi_modelD.jpg', 0, 0),
('SP000305', 'Wifi Card MSI MW65 Dual-Band Model E', 710000.00, 21, 'MSI', 'Wifi Card MSI MW65 với khả năng tăng tốc tốc độ và kết nối ổn định, Model E.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelE.jpg', 0, 0),
('SP000306', 'Wifi Card ASUS PCE-AX3000 Dual-Band Model F', 711000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AX3000 Model F tích hợp công nghệ WiFi 6 cho tốc độ truyền tải vượt trội, Model F.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelF.jpg', 0, 0),
('SP000307', 'Wifi Card TP-Link Archer TX3000E Dual-Band Model G', 712000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer TX3000E hỗ trợ WiFi 6 và công nghệ MU-MIMO, Model G.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"MU-MIMO\": \"Yes\"}', 'Network', 'wifi_modelG.jpg', 0, 0),
('SP000308', 'Wifi Card Intel Wi-Fi 6E AX210 Dual-Band Model H', 713000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6E AX210 hỗ trợ băng tần mở rộng cho kết nối tốc độ cao, Model H.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6E\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelH.jpg', 0, 0),
('SP000309', 'Wifi Card Gigabyte GC-WB1735D Dual-Band Model I', 714000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WB1735D hỗ trợ WiFi 6 với hiệu suất kết nối ổn định, Model I.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Bluetooth\": \"5.1\"}', 'Network', 'wifi_modelI.jpg', 0, 0),
('SP000310', 'Wifi Card ASUS PCE-AC88 Dual-Band Model J', 715000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AC88 hỗ trợ WiFi 5 với thiết kế hiệu quả cho game thủ, Model J.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"4\"}', 'Network', 'wifi_modelJ.jpg', 0, 0),
('SP000311', 'Wifi Card MSI MPG AC1300 Dual-Band Model K', 716000.00, 21, 'MSI', 'Wifi Card MSI MPG AC1300 mang lại hiệu suất ổn định cho kết nối không dây, Model K.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelK.jpg', 0, 0),
('SP000312', 'Wifi Card TP-Link Archer T3U Plus Dual-Band Model L', 717000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer T3U Plus với khả năng tăng cường tín hiệu mạnh mẽ, Model L.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"USB\": \"Yes\"}', 'Network', 'wifi_modelL.jpg', 0, 0),
('SP000313', 'Wifi Card Intel Wireless-AC 9560 Dual-Band Model M', 718000.00, 21, 'Intel', 'Wifi Card Intel Wireless-AC 9560 hỗ trợ kết nối không dây nhanh chóng và an toàn, Model M.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelM.jpg', 0, 0),
('SP000314', 'Wifi Card ASUS PCE-AX58BT Dual-Band Model N', 719000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AX58BT tích hợp Bluetooth, hỗ trợ WiFi 6 cho kết nối nhanh, Model N.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Bluetooth\": \"Yes\"}', 'Network', 'wifi_modelN.jpg', 0, 0),
('SP000315', 'Wifi Card Gigabyte GC-WB1732 Dual-Band Model O', 720000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WB1732 hỗ trợ WiFi 5 với hiệu suất truyền tải ổn định, Model O.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Bluetooth\": \"4.2\"}', 'Network', 'wifi_modelO.jpg', 0, 0),
('SP000316', 'Wifi Card TP-Link Archer TX50E Dual-Band Model P', 721000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer TX50E với công nghệ WiFi 6 và thiết kế tiên tiến, Model P.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"MU-MIMO\": \"Yes\"}', 'Network', 'wifi_modelP.jpg', 0, 0),
('SP000317', 'Wifi Card MSI AC1300 PCIe Dual-Band Model Q', 722000.00, 21, 'MSI', 'Wifi Card MSI AC1300 PCIe mang lại kết nối nhanh chóng và ổn định cho máy tính, Model Q.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"PCIe\": \"Yes\"}', 'Network', 'wifi_modelQ.jpg', 0, 0),
('SP000318', 'Wifi Card Intel Wi-Fi 6E AX210NGW Dual-Band Model R', 723000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6E AX210NGW hỗ trợ băng tần mở rộng cho kết nối hiệu suất cao, Model R.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6E\", \"Antennas\": \"2\"}', 'Network', 'wifi_modelR.jpg', 0, 0),
('SP000319', 'Wifi Card ASUS PCE-AC88 Dual-Band Model S', 724000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AC88 với thiết kế 4 anten, tối ưu cho game và giải trí, Model S.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"5\", \"Antennas\": \"4\"}', 'Network', 'wifi_modelS.jpg', 0, 0),
('SP000320', 'Wifi Card Gigabyte GC-WB1733 Dual-Band Model T', 725000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WB1733 hỗ trợ WiFi 6 với hiệu suất kết nối ổn định và tích hợp Bluetooth, Model T.', '{\"Danh mục\": \"Wifi Card\", \"Băng tần\": \"Dual-Band\", \"WiFi\": \"6\", \"Bluetooth\": \"5.0\"}', 'Network', 'wifi_modelT.jpg', 0, 0),
('SP000321', 'Speaker Razer Nommo V2 Pro 2.1 Model A', 10450000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro với âm thanh vòm sống động, Model A.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"500W\"}', 'Audio', 'speaker_modelA.jpg', 0, 0),
('SP000322', 'Speaker Logitech Z623 2.1 Model B', 8500000.00, 40, 'Logitech', 'Speaker Logitech Z623 mang đến âm thanh mạnh mẽ và bass sâu, Model B.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"400W\"}', 'Audio', 'speaker_modelB.jpg', 0, 0);
INSERT INTO `sanpham` (`IdSp`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000323', 'Speaker Bose Companion 2 Series III Model C', 9500000.00, 30, 'Bose', 'Speaker Bose Companion 2 Series III với âm thanh rõ ràng và chi tiết, Model C.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.0\", \"Power\": \"150W\"}', 'Audio', 'speaker_modelC.jpg', 0, 0),
('SP000324', 'Speaker JBL Professional 305P MkII Model D', 6700000.00, 35, 'JBL', 'Speaker JBL Professional 305P MkII mang đến âm thanh sống động và sắc nét, Model D.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.0\", \"Power\": \"200W\"}', 'Audio', 'speaker_modelD.jpg', 0, 0),
('SP000325', 'Speaker Creative Pebble 2.0 Model E', 2500000.00, 60, 'Creative', 'Speaker Creative Pebble 2.0 với thiết kế nhỏ gọn và âm thanh sống động, Model E.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.0\", \"Power\": \"20W\"}', 'Audio', 'speaker_modelE.jpg', 0, 0),
('SP000326', 'Speaker Edifier R1280T Model F', 3200000.00, 45, 'Edifier', 'Speaker Edifier R1280T cho âm thanh ấm áp và trung thực, Model F.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.0\", \"Power\": \"42W\"}', 'Audio', 'speaker_modelF.jpg', 0, 0),
('SP000327', 'Speaker Klipsch ProMedia 2.1 Model G', 7200000.00, 38, 'Klipsch', 'Speaker Klipsch ProMedia 2.1 mang đến âm bass mạnh mẽ và âm thanh sống động, Model G.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"150W\"}', 'Audio', 'speaker_modelG.jpg', 0, 0),
('SP000328', 'Speaker Logitech Z906 5.1 Model H', 14500000.00, 25, 'Logitech', 'Speaker Logitech Z906 với hệ thống âm thanh vòm 5.1, Model H.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"5.1\", \"Power\": \"500W\"}', 'Audio', 'speaker_modelH.jpg', 0, 0),
('SP000329', 'Speaker Razer Nommo V2 Pro 2.1 Model I', 10530000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro mang lại trải nghiệm âm thanh chân thực, Model I.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"500W\"}', 'Audio', 'speaker_modelI.jpg', 0, 0),
('SP000330', 'Speaker Bose Companion 50 Model J', 11000000.00, 30, 'Bose', 'Speaker Bose Companion 50 với âm thanh vòm mạnh mẽ, Model J.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"250W\"}', 'Audio', 'speaker_modelJ.jpg', 0, 0),
('SP000331', 'Speaker JBL 308BT Model K', 9800000.00, 40, 'JBL', 'Speaker JBL 308BT mang đến trải nghiệm âm thanh không dây mạnh mẽ, Model K.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"300W\"}', 'Audio', 'speaker_modelK.jpg', 0, 0),
('SP000332', 'Speaker Creative Sound BlasterX Katana Model L', 8600000.00, 35, 'Creative', 'Speaker Creative Sound BlasterX Katana với hiệu suất âm thanh tuyệt vời, Model L.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"250W\"}', 'Audio', 'speaker_modelL.jpg', 0, 0),
('SP000333', 'Speaker Logitech Z337 Model M', 4500000.00, 55, 'Logitech', 'Speaker Logitech Z337 cho âm bass sâu lắng và âm thanh cân bằng, Model M.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"100W\"}', 'Audio', 'speaker_modelM.jpg', 0, 0),
('SP000334', 'Speaker Razer Nommo V2 Pro 2.1 Model N', 10580000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro cho âm thanh rực rỡ, Model N.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"500W\"}', 'Audio', 'speaker_modelN.jpg', 0, 0),
('SP000335', 'Speaker Edifier S350DB Model O', 6400000.00, 47, 'Edifier', 'Speaker Edifier S350DB cho âm bass mạnh mẽ và chi tiết, Model O.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"150W\"}', 'Audio', 'speaker_modelO.jpg', 0, 0),
('SP000336', 'Speaker Bose Companion 20 Multimedia Speaker Model P', 7600000.00, 42, 'Bose', 'Speaker Bose Companion 20 với thiết kế compact và âm thanh trung thực, Model P.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.0\", \"Power\": \"120W\"}', 'Audio', 'speaker_modelP.jpg', 0, 0),
('SP000337', 'Speaker JBL Bar 2.1 Model Q', 11500000.00, 28, 'JBL', 'Speaker JBL Bar 2.1 mang đến trải nghiệm âm thanh vòm mạnh mẽ, Model Q.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"400W\"}', 'Audio', 'speaker_modelQ.jpg', 0, 0),
('SP000338', 'Speaker Logitech Z407 Model R', 5200000.00, 50, 'Logitech', 'Speaker Logitech Z407 cho âm thanh rõ ràng và bass sống động, Model R.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"90W\"}', 'Audio', 'speaker_modelR.jpg', 0, 0),
('SP000339', 'Speaker Creative Pebble Plus 2.1 Model S', 3300000.00, 65, 'Creative', 'Speaker Creative Pebble Plus 2.1 với thiết kế hiện đại và âm thanh sống động, Model S.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"60W\"}', 'Audio', 'speaker_modelS.jpg', 0, 0),
('SP000340', 'Speaker Razer Nommo V2 Pro 2.1 Model T', 10640000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro cao cấp, Model T.', '{\"Danh mục\": \"Speaker\", \"Channels\": \"2.1\", \"Power\": \"500W\"}', 'Audio', 'speaker_modelT.jpg', 0, 0),
('SP000341', 'Microphone ASUS ROG Carnyx USB Condenser Model A', 5000000.00, 44, 'ASUS', 'Microphone ASUS ROG Carnyx USB cho thu âm chuyên nghiệp, Model A.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\"}', 'Audio', 'mic_modelA.jpg', 0, 0),
('SP000342', 'Microphone Blue Yeti X USB Condenser Model B', 5250000.00, 40, 'Blue Yeti', 'Microphone Blue Yeti X USB mang đến chất lượng thu âm cao cấp và đa dạng chế độ ghi âm, Model B.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Features\": \"Multi-pattern\"}', 'Audio', 'mic_modelB.jpg', 0, 0),
('SP000343', 'Microphone Rode NT-USB Condenser Model C', 5100000.00, 38, 'Rode', 'Microphone Rode NT-USB cho thu âm mượt mà và chi tiết, Model C.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Pickup Pattern\": \"Cardioid\"}', 'Audio', 'mic_modelC.jpg', 0, 0),
('SP000344', 'Microphone Shure MV7 USB Dynamic Model D', 5200000.00, 42, 'Shure', 'Microphone Shure MV7 với công nghệ thu âm hiện đại và cảm ứng điều khiển, Model D.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Dynamic\", \"Connectivity\": \"USB\", \"Features\": \"Touch Panel\"}', 'Audio', 'mic_modelD.jpg', 0, 0),
('SP000345', 'Microphone Audio-Technica AT2020USB+ Condenser Model E', 5300000.00, 45, 'Audio-Technica', 'Microphone Audio-Technica AT2020USB+ cho chất lượng thu âm trung thực và dải tần số rộng, Model E.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Frequency Response\": \"20Hz-20kHz\"}', 'Audio', 'mic_modelE.jpg', 0, 0),
('SP000346', 'Microphone Samson Meteor Mic USB Condenser Model F', 5400000.00, 47, 'Samson', 'Microphone Samson Meteor Mic USB với thiết kế cổ điển và âm thanh ấm áp, Model F.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Design\": \"Retro\"}', 'Audio', 'mic_modelF.jpg', 0, 0),
('SP000347', 'Microphone AKG Lyra USB Condenser Model G', 5500000.00, 40, 'AKG', 'Microphone AKG Lyra USB cho thu âm chuyên nghiệp với 5 chế độ ghi âm linh hoạt, Model G.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Modes\": \"5\"}', 'Audio', 'mic_modelG.jpg', 0, 0),
('SP000348', 'Microphone HyperX SoloCast USB Condenser Model H', 5050000.00, 50, 'HyperX', 'Microphone HyperX SoloCast USB mang đến âm thanh rõ ràng và cài đặt nhanh gọn, Model H.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Pickup Pattern\": \"Cardioid\"}', 'Audio', 'mic_modelH.jpg', 0, 0),
('SP000349', 'Microphone Elgato Wave:3 USB Condenser Model I', 5600000.00, 39, 'Elgato', 'Microphone Elgato Wave:3 USB tối ưu cho livestream và podcast với công nghệ Wave Link, Model I.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Features\": \"Wave Link\"}', 'Audio', 'mic_modelI.jpg', 0, 0),
('SP000350', 'Microphone Razer Seiren X USB Condenser Model J', 5150000.00, 44, 'Razer', 'Microphone Razer Seiren X USB với thiết kế nhỏ gọn, phù hợp cho gaming và streaming, Model J.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Design\": \"Compact\"}', 'Audio', 'mic_modelJ.jpg', 0, 0),
('SP000351', 'Microphone Blue Yeti Nano USB Condenser Model K', 5255000.00, 42, 'Blue Yeti', 'Microphone Blue Yeti Nano USB với kích thước nhỏ gọn nhưng vẫn đảm bảo chất lượng âm thanh vượt trội, Model K.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Features\": \"Compact\"}', 'Audio', 'mic_modelK.jpg', 0, 0),
('SP000352', 'Microphone Rode Podcaster USB Dynamic Model L', 5350000.00, 41, 'Rode', 'Microphone Rode Podcaster USB cho thu âm podcast chuyên nghiệp với mô hình pickup cardioid, Model L.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Dynamic\", \"Connectivity\": \"USB\", \"Pickup Pattern\": \"Cardioid\"}', 'Audio', 'mic_modelL.jpg', 0, 0),
('SP000353', 'Microphone Shure MV5 USB Condenser Model M', 5405000.00, 43, 'Shure', 'Microphone Shure MV5 USB với thiết kế hiện đại và trọng lượng siêu nhẹ, Model M.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Features\": \"Lightweight\"}', 'Audio', 'mic_modelM.jpg', 0, 0),
('SP000354', 'Microphone Audio-Technica AT2005USB Dynamic Model N', 5450000.00, 44, 'Audio-Technica', 'Microphone Audio-Technica AT2005USB với khả năng kết nối kép USB/XLR, Model N.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Dynamic\", \"Connectivity\": \"USB/XLR\", \"Frequency Response\": \"45Hz-16kHz\"}', 'Audio', 'mic_modelN.jpg', 0, 0),
('SP000355', 'Microphone Samson C01U Pro USB Condenser Model O', 5505000.00, 42, 'Samson', 'Microphone Samson C01U Pro USB mang đến âm thanh sắc nét và trung thực, Model O.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Frequency Response\": \"20Hz-20kHz\"}', 'Audio', 'mic_modelO.jpg', 0, 0),
('SP000356', 'Microphone AKG P120 USB Condenser Model P', 5550000.00, 40, 'AKG', 'Microphone AKG P120 USB với khả năng thu âm chi tiết và độ nhạy cao, Model P.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Sensitivity\": \"-34 dB\"}', 'Audio', 'mic_modelP.jpg', 0, 0),
('SP000357', 'Microphone HyperX SoloCast USB Condenser Model Q', 5605000.00, 44, 'HyperX', 'Microphone HyperX SoloCast USB mang đến chất lượng thu âm cao cấp và dễ sử dụng, Model Q.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Pickup Pattern\": \"Cardioid\"}', 'Audio', 'mic_modelQ.jpg', 0, 0),
('SP000358', 'Microphone Elgato Wave:1 USB Condenser Model R', 5650000.00, 43, 'Elgato', 'Microphone Elgato Wave:1 USB cho trải nghiệm thu âm mượt mà và âm thanh sống động, Model R.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Features\": \"Studio Quality\"}', 'Audio', 'mic_modelR.jpg', 0, 0),
('SP000359', 'Microphone Sennheiser e965 USB Condenser Model S', 5700000.00, 42, 'Sennheiser', 'Microphone Sennheiser e965 USB mang đến âm thanh chuyên nghiệp với dải tần số rộng, Model S.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Frequency Response\": \"20Hz-20kHz\"}', 'Audio', 'mic_modelS.jpg', 0, 0),
('SP000360', 'Microphone Razer Seiren X USB Condenser Model T', 5750000.00, 44, 'Razer', 'Microphone Razer Seiren X USB với hiệu suất thu âm vượt trội và thiết kế độc đáo, Model T.', '{\"Danh mục\": \"Microphone\", \"Type\": \"Condenser\", \"Connectivity\": \"USB\", \"Design\": \"Sleek\"}', 'Audio', 'mic_modelT.jpg', 0, 0),
('SP000361', 'Webcam Logitech C310 HD 720p Model A', 690000.00, 85, 'Logitech', 'Webcam Logitech C310 HD 720p cho video call chất lượng, Model A.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelA.jpg', 0, 0),
('SP000362', 'Webcam Logitech C920 HD Pro 1080p Model B', 990000.00, 80, 'Logitech', 'Webcam Logitech C920 HD Pro 1080p cho video call chuyên nghiệp, Model B.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelB.jpg', 0, 0),
('SP000363', 'Webcam Microsoft LifeCam HD-3000 Model C', 750000.00, 90, 'Microsoft', 'Webcam Microsoft LifeCam HD-3000 với chất lượng hình ảnh HD, Model C.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelC.jpg', 0, 0),
('SP000364', 'Webcam Razer Kiyo Model D', 850000.00, 70, 'Razer', 'Webcam Razer Kiyo tích hợp đèn LED điều chỉnh, Model D.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"LED\": \"Adjustable\"}', 'Audio', 'webcam_modelD.jpg', 0, 0),
('SP000365', 'Webcam Logitech Brio 4K Model E', 2500000.00, 60, 'Logitech', 'Webcam Logitech Brio 4K cho chất lượng hình ảnh vượt trội, Model E.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelE.jpg', 0, 0),
('SP000366', 'Webcam Dell UltraSharp WB702 Model F', 1500000.00, 75, 'Dell', 'Webcam Dell UltraSharp WB702 cho video call với chất lượng HD, Model F.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelF.jpg', 0, 0),
('SP000367', 'Webcam Logitech C270 HD 720p Model G', 650000.00, 100, 'Logitech', 'Webcam Logitech C270 HD 720p cho video call chất lượng, Model G.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelG.jpg', 0, 0),
('SP000368', 'Webcam A4Tech PK-910H Model H', 500000.00, 95, 'A4Tech', 'Webcam A4Tech PK-910H với thiết kế nhỏ gọn, Model H.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelH.jpg', 0, 0),
('SP000369', 'Webcam Creative Live! Cam Chat HD Model I', 700000.00, 88, 'Creative', 'Webcam Creative Live! Cam Chat HD cho cuộc gọi trực tuyến, Model I.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelI.jpg', 0, 0),
('SP000370', 'Webcam Logitech C922 Pro Stream Model J', 1200000.00, 82, 'Logitech', 'Webcam Logitech C922 Pro Stream cho trải nghiệm livestream chuyên nghiệp, Model J.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelJ.jpg', 0, 0),
('SP000371', 'Webcam Microsoft LifeCam Studio Model K', 1300000.00, 80, 'Microsoft', 'Webcam Microsoft LifeCam Studio cho video call chất lượng cao, Model K.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelK.jpg', 0, 0),
('SP000372', 'Webcam Logitech C615 HD 720p Model L', 800000.00, 85, 'Logitech', 'Webcam Logitech C615 HD 720p với khả năng xoay 360 độ, Model L.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Pan/Tilt\": \"360/90\"}', 'Audio', 'webcam_modelL.jpg', 0, 0),
('SP000373', 'Webcam Razer Kiyo Pro Model M', 1800000.00, 75, 'Razer', 'Webcam Razer Kiyo Pro cho chất lượng hình ảnh xuất sắc, Model M.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"LED\": \"Adjustable\"}', 'Audio', 'webcam_modelM.jpg', 0, 0),
('SP000374', 'Webcam Logitech StreamCam Model N', 1500000.00, 70, 'Logitech', 'Webcam Logitech StreamCam cho livestream chuyên nghiệp, Model N.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelN.jpg', 0, 0),
('SP000375', 'Webcam AUSDOM AF640 Full HD Model O', 1100000.00, 65, 'AUSDOM', 'Webcam AUSDOM AF640 Full HD với hình ảnh sắc nét, Model O.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelO.jpg', 0, 0),
('SP000376', 'Webcam Logitech Brio 4K Model P', 3000000.00, 55, 'Logitech', 'Webcam Logitech Brio 4K cho hình ảnh chuyên nghiệp, Model P.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelP.jpg', 0, 0),
('SP000377', 'Webcam Creative Live! Cam IP HD Model Q', 850000.00, 90, 'Creative', 'Webcam Creative Live! Cam IP HD cho cuộc gọi trực tuyến rõ nét, Model Q.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\"}', 'Audio', 'webcam_modelQ.jpg', 0, 0),
('SP000378', 'Webcam Dell UltraSharp WB702 Model R', 1400000.00, 78, 'Dell', 'Webcam Dell UltraSharp WB702 cho video call chuyên nghiệp, Model R.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\"}', 'Audio', 'webcam_modelR.jpg', 0, 0),
('SP000379', 'Webcam Logitech C930e Model S', 1300000.00, 82, 'Logitech', 'Webcam Logitech C930e với góc quay rộng, Model S.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Field of View\": \"90°\"}', 'Audio', 'webcam_modelS.jpg', 0, 0),
('SP000380', 'Webcam Razer Kiyo Ultra Model T', 2200000.00, 80, 'Razer', 'Webcam Razer Kiyo Ultra cho hình ảnh siêu nét, Model T.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"LED\": \"Yes\"}', 'Audio', 'webcam_modelT.jpg', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `taikhoan`
--

CREATE TABLE `taikhoan` (
  `IdTk` varchar(10) NOT NULL,
  `matkhau` varchar(255) NOT NULL,
  `tentaikhoan` varchar(50) NOT NULL,
  `ngaytaotk` date NOT NULL,
  `ngaysuadoi` date DEFAULT NULL,
  `quyentruycap` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `taikhoan`
--

INSERT INTO `taikhoan` (`IdTk`, `matkhau`, `tentaikhoan`, `ngaytaotk`, `ngaysuadoi`, `quyentruycap`) VALUES
('TK000001', 'admin', 'admin', '2025-02-10', NULL, 'quantrivien'),
('TK000002', 'user1', 'user1', '2025-02-11', NULL, 'khachhang'),
('TK000003', 'user2', 'user2', '2025-02-12', NULL, 'khachhang'),
('TK000004', 'user3', 'user3', '2025-02-13', NULL, 'khachhang'),
('TK000005', 'user4', 'user4', '2025-02-14', NULL, 'khachhang'),
('TK000006', 'user5', 'user5', '2025-02-15', NULL, 'khachhang'),
('TK000007', 'user6', 'user6', '2025-02-16', NULL, 'khachhang'),
('TK000008', 'user7', 'user7', '2025-02-17', NULL, 'khachhang'),
('TK000009', 'user8', 'user8', '2025-02-18', NULL, 'khachhang'),
('TK000010', 'user9', 'user9', '2025-02-19', NULL, 'khachhang'),
('TK000011', 'user10', 'user10', '2025-02-20', NULL, 'khachhang'),
('TK000012', 'user11', 'user11', '2025-02-21', NULL, 'khachhang'),
('TK000013', 'user12', 'user12', '2025-02-22', NULL, 'khachhang'),
('TK000014', 'user13', 'user13', '2025-02-23', NULL, 'khachhang'),
('TK000015', 'user14', 'user14', '2025-02-24', NULL, 'khachhang'),
('TK000016', 'user15', 'user15', '2025-02-25', NULL, 'khachhang'),
('TK000017', 'user16', 'user16', '2025-02-26', NULL, 'khachhang'),
('TK000018', 'user17', 'user17', '2025-02-27', NULL, 'khachhang'),
('TK000019', 'user18', 'user18', '2025-02-28', NULL, 'khachhang'),
('TK000020', 'user19', 'user19', '2025-01-30', NULL, 'khachhang');

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `IdTt` varchar(10) NOT NULL,
  `trangthai` varchar(50) NOT NULL,
  `tienthanhtoan` decimal(10,2) NOT NULL,
  `ngaythanhtoan` datetime DEFAULT current_timestamp(),
  `noidungthanhtoan` varchar(200) DEFAULT NULL,
  `mathanhtoan` varchar(50) DEFAULT NULL,
  `IdDh` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `thanhtoan`
--

INSERT INTO `thanhtoan` (`IdTt`, `trangthai`, `tienthanhtoan`, `ngaythanhtoan`, `noidungthanhtoan`, `mathanhtoan`, `IdDh`) VALUES
('TT000001', 'Thành công', 52490000.00, '2025-02-01 09:30:00', 'Thanh toán đơn hàng DH000001', 'PAY001', 'DH000001'),
('TT000002', 'Thành công', 15990000.00, '2025-02-02 10:15:00', 'Thanh toán đơn hàng DH000002', 'PAY002', 'DH000002'),
('TT000003', 'Thành công', 7990000.00, '2025-02-03 11:45:00', 'Thanh toán đơn hàng DH000003', 'PAY003', 'DH000003'),
('TT000004', 'Thành công', 49990000.00, '2025-02-04 14:20:00', 'Thanh toán đơn hàng DH000004', 'PAY004', 'DH000004'),
('TT000005', 'Thành công', 2990000.00, '2025-02-05 15:30:00', 'Thanh toán đơn hàng DH000005', 'PAY005', 'DH000005'),
('TT000006', 'Thanh toán thất bại', 8990000.00, '2025-02-06 16:30:00', 'Thanh toán đơn hàng DH000006', 'PAY006', 'DH000006'),
('TT000007', 'Thành công', 25990000.00, '2025-02-07 17:15:00', 'Thanh toán đơn hàng DH000007', 'PAY007', 'DH000007'),
('TT000008', 'Thành công', 39990000.00, '2025-02-08 18:45:00', 'Thanh toán đơn hàng DH000008', 'PAY008', 'DH000008'),
('TT000009', 'Thành công', 69990000.00, '2025-02-09 19:20:00', 'Thanh toán đơn hàng DH000009', 'PAY009', 'DH000009'),
('TT000010', 'Đang thanh toán', 19990000.00, '2025-02-10 20:30:00', 'Thanh toán đơn hàng DH000010', 'PAY010', 'DH000010'),
('TT000011', 'Thành công', 59990000.00, '2025-02-11 21:15:00', 'Thanh toán đơn hàng DH000011', 'PAY011', 'DH000011'),
('TT000012', 'Thành công', 2990000.00, '2025-02-12 22:45:00', 'Thanh toán đơn hàng DH000012', 'PAY012', 'DH000012'),
('TT000013', 'Thành công', 19990000.00, '2025-02-13 23:20:00', 'Thanh toán đơn hàng DH000013', 'PAY013', 'DH000013'),
('TT000014', 'Thành công', 12990000.00, '2025-02-14 08:30:00', 'Thanh toán đơn hàng DH000014', 'PAY014', 'DH000014'),
('TT000015', 'Thành công', 3990000.00, '2025-02-15 09:15:00', 'Thanh toán đơn hàng DH000015', 'PAY015', 'DH000015'),
('TT000016', 'Thành công', 25990000.00, '2025-02-16 10:45:00', 'Thanh toán đơn hàng DH000016', 'PAY016', 'DH000016'),
('TT000017', 'Thành công', 14990000.00, '2025-02-17 11:20:00', 'Thanh toán đơn hàng DH000017', 'PAY017', 'DH000017'),
('TT000018', 'Thành công', 12990000.00, '2025-02-18 12:30:00', 'Thanh toán đơn hàng DH000018', 'PAY018', 'DH000018'),
('TT000019', 'Thành công', 6990000.00, '2025-02-19 13:15:00', 'Thanh toán đơn hàng DH000019', 'PAY019', 'DH000019'),
('TT000020', 'Thành công', 4990000.00, '2025-02-20 14:45:00', 'Thanh toán đơn hàng DH000020', 'PAY020', 'DH000020');

-- --------------------------------------------------------

--
-- Table structure for table `xephangvip`
--

CREATE TABLE `xephangvip` (
  `id` varchar(10) NOT NULL,
  `tenhang` varchar(50) NOT NULL,
  `diemtoithieu` int(11) NOT NULL,
  `diemtoida` int(11) NOT NULL,
  `phantramgiamgia` decimal(5,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `xephangvip`
--

INSERT INTO `xephangvip` (`id`, `tenhang`, `diemtoithieu`, `diemtoida`, `phantramgiamgia`) VALUES
('BAC', 'Bạc', 500, 999, 3.00),
('KIMCUONG', 'Kim cương', 5000, 999999, 10.00),
('THANTHIET', 'Thân thiết', 0, 499, 0.00),
('VANG', 'Vàng', 1000, 4999, 7.00);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD PRIMARY KEY (`Idchitietdonhang`),
  ADD KEY `chitietdonhang_fk_1` (`IdDh`),
  ADD KEY `fk_chitietdonhang_sanpham` (`IdSp`),
  ADD KEY `fk_chitietdonhang_danhgia` (`IdDg`);

--
-- Indexes for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD PRIMARY KEY (`IdGh`,`IdSp`),
  ADD KEY `IdSp` (`IdSp`);

--
-- Indexes for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD PRIMARY KEY (`IdDg`),
  ADD KEY `idx_danhgia_sosao` (`sosao`),
  ADD KEY `idx_danhgia_ngaydanhgia` (`ngaydanhgia`),
  ADD KEY `fk_danhgia_khachhang` (`IdKh`);

--
-- Indexes for table `donhang`
--
ALTER TABLE `donhang`
  ADD PRIMARY KEY (`IdDh`),
  ADD KEY `IdKh` (`IdKh`),
  ADD KEY `IdMgg` (`IdMgg`),
  ADD KEY `idx_donhang_trangthai` (`trangthai`),
  ADD KEY `idx_donhang_ngaydathang` (`ngaydathang`);

--
-- Indexes for table `giohang`
--
ALTER TABLE `giohang`
  ADD PRIMARY KEY (`IdGh`),
  ADD KEY `IdKh` (`IdKh`);

--
-- Indexes for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`IdKh`),
  ADD KEY `IdTk` (`IdTk`),
  ADD KEY `id_xephangvip` (`id_xephangvip`),
  ADD KEY `idx_khachhang_email` (`email`),
  ADD KEY `idx_khachhang_sodienthoai` (`sodienthoai`);

--
-- Indexes for table `magiamgia`
--
ALTER TABLE `magiamgia`
  ADD PRIMARY KEY (`IdMgg`);

--
-- Indexes for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`IdNv`),
  ADD KEY `fk_nhanvien_taikhoan` (`idtk`);

--
-- Indexes for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`IdSp`),
  ADD KEY `idx_sanpham_tensanpham` (`tensanpham`),
  ADD KEY `idx_sanpham_gia` (`gia`),
  ADD KEY `idx_sanpham_thuonghieu` (`thuonghieu`);

--
-- Indexes for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`IdTk`),
  ADD KEY `idx_taikhoan_tentaikhoan` (`tentaikhoan`),
  ADD KEY `idx_taikhoan_quyentruycap` (`quyentruycap`);

--
-- Indexes for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`IdTt`),
  ADD KEY `IdDh` (`IdDh`),
  ADD KEY `idx_thanhtoan_trangthai` (`trangthai`),
  ADD KEY `idx_thanhtoan_ngaythanhtoan` (`ngaythanhtoan`);

--
-- Indexes for table `xephangvip`
--
ALTER TABLE `xephangvip`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_xephangvip_diemtoithieu` (`diemtoithieu`),
  ADD KEY `idx_xephangvip_diemtoida` (`diemtoida`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD CONSTRAINT `fk_chitietdonhang_danhgia` FOREIGN KEY (`IdDg`) REFERENCES `danhgia` (`IdDg`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_chitietdonhang_donhang` FOREIGN KEY (`IdDh`) REFERENCES `donhang` (`IdDh`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_chitietdonhang_sanpham` FOREIGN KEY (`IdSp`) REFERENCES `sanpham` (`IdSp`) ON DELETE CASCADE;

--
-- Constraints for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD CONSTRAINT `chitietgiohang_fk_1` FOREIGN KEY (`IdGh`) REFERENCES `giohang` (`IdGh`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_fk_2` FOREIGN KEY (`IdSp`) REFERENCES `sanpham` (`IdSp`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_ibfk_2` FOREIGN KEY (`IdSp`) REFERENCES `sanpham` (`IdSp`) ON DELETE CASCADE,
  ADD CONSTRAINT `giohang_ibfk_1` FOREIGN KEY (`IdGh`) REFERENCES `khachhang` (`IdKh`) ON DELETE CASCADE;

--
-- Constraints for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD CONSTRAINT `fk_danhgia_khachhang` FOREIGN KEY (`IdKh`) REFERENCES `khachhang` (`IdKh`) ON DELETE CASCADE;

--
-- Constraints for table `donhang`
--
ALTER TABLE `donhang`
  ADD CONSTRAINT `donhang_ibfk_1` FOREIGN KEY (`IdKh`) REFERENCES `khachhang` (`IdKh`) ON DELETE CASCADE,
  ADD CONSTRAINT `donhang_ibfk_2` FOREIGN KEY (`IdMgg`) REFERENCES `magiamgia` (`IdMgg`);

--
-- Constraints for table `giohang`
--
ALTER TABLE `giohang`
  ADD CONSTRAINT `giohang_fk_1` FOREIGN KEY (`IdKh`) REFERENCES `khachhang` (`IdKh`) ON DELETE CASCADE;

--
-- Constraints for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD CONSTRAINT `khachhang_ibfk_1` FOREIGN KEY (`IdTk`) REFERENCES `taikhoan` (`IdTk`) ON DELETE CASCADE,
  ADD CONSTRAINT `khachhang_ibfk_2` FOREIGN KEY (`id_xephangvip`) REFERENCES `xephangvip` (`id`);

--
-- Constraints for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD CONSTRAINT `fk_nhanvien_taikhoan` FOREIGN KEY (`idtk`) REFERENCES `taikhoan` (`IdTk`) ON DELETE CASCADE;

--
-- Constraints for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD CONSTRAINT `thanhtoan_ibfk_1` FOREIGN KEY (`IdDh`) REFERENCES `donhang` (`IdDh`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
