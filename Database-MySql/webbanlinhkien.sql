-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 04, 2025 at 08:37 AM
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
  `id_donhang` varchar(10) NOT NULL,
  `id_sanpham` varchar(10) NOT NULL,
  `soluong` int(11) NOT NULL,
  `dongia` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `chitietdonhang`
--

INSERT INTO `chitietdonhang` (`id_donhang`, `id_sanpham`, `soluong`, `dongia`) VALUES
('DH000001', 'SP000001', 2, 15990000.00),
('DH000001', 'SP000002', 1, 14990000.00),
('DH000002', 'SP000003', 1, 7990000.00),
('DH000002', 'SP000004', 1, 12990000.00),
('DH000003', 'SP000001', 1, 15990000.00),
('DH000003', 'SP000003', 2, 7990000.00),
('DH000004', 'SP000002', 2, 14990000.00),
('DH000004', 'SP000004', 1, 12990000.00),
('DH000005', 'SP000018', 1, 2990000.00),
('DH000006', 'SP000003', 1, 7990000.00),
('DH000007', 'SP000004', 2, 12990000.00),
('DH000008', 'SP000001', 2, 15990000.00),
('DH000008', 'SP000002', 1, 14990000.00),
('DH000009', 'SP000015', 1, 19990000.00),
('DH000009', 'SP000016', 1, 59990000.00),
('DH000010', 'SP000024', 1, 19990000.00),
('DH000011', 'SP000016', 1, 59990000.00),
('DH000012', 'SP000018', 1, 2990000.00),
('DH000013', 'SP000024', 1, 19990000.00),
('DH000014', 'SP000025', 1, 12990000.00),
('DH000015', 'SP000018', 1, 2990000.00),
('DH000016', 'SP000025', 1, 12990000.00),
('DH000016', 'SP000028', 1, 6990000.00),
('DH000017', 'SP000003', 2, 7990000.00),
('DH000018', 'SP000025', 1, 12990000.00),
('DH000019', 'SP000028', 1, 6990000.00),
('DH000020', 'SP000018', 2, 2990000.00);

-- --------------------------------------------------------

--
-- Table structure for table `chitietgiohang`
--

CREATE TABLE `chitietgiohang` (
  `id_giohang` varchar(10) NOT NULL,
  `id_sanpham` varchar(10) NOT NULL,
  `soluong` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `danhgia`
--

CREATE TABLE `danhgia` (
  `id` varchar(10) NOT NULL,
  `sosao` int(11) NOT NULL CHECK (`sosao` between 1 and 5),
  `noidung` text DEFAULT NULL,
  `ngaydanhgia` datetime DEFAULT current_timestamp(),
  `id_khachhang` varchar(10) NOT NULL,
  `id_sanpham` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `danhgia`
--

INSERT INTO `danhgia` (`id`, `sosao`, `noidung`, `ngaydanhgia`, `id_khachhang`, `id_sanpham`) VALUES
('DG000001', 5, 'Sản phẩm tuyệt vời, chạy mượt!', '2025-02-01 10:30:00', 'KH000001', 'SP000001'),
('DG000002', 4, 'Hàng tốt nhưng giao hơi lâu.', '2025-02-02 14:20:00', 'KH000002', 'SP000002'),
('DG000003', 5, 'CPU mạnh mẽ, giá hợp lý!', '2025-02-03 09:15:00', 'KH000003', 'SP000003'),
('DG000004', 5, 'Mainboard ổn định, chạy rất mát.', '2025-02-04 16:45:00', 'KH000004', 'SP000004'),
('DG000005', 3, 'Sản phẩm bình thường, chưa test kỹ.', '2025-02-05 11:30:00', 'KH000005', 'SP000001'),
('DG000006', 4, 'Hiệu năng tốt', '2025-02-06 13:15:00', 'KH000006', 'SP000002'),
('DG000007', 5, 'Chơi game rất mượt', '2025-02-07 14:30:00', 'KH000007', 'SP000003'),
('DG000008', 3, 'Laptop hơi nóng khi chơi game', '2025-02-08 15:45:00', 'KH000008', 'SP000015'),
('DG000009', 5, 'Bàn phím gõ rất thích', '2025-02-09 16:20:00', 'KH000009', 'SP000018'),
('DG000010', 4, 'Màn hình hiển thị sắc nét', '2025-02-10 17:30:00', 'KH000010', 'SP000024'),
('DG000011', 5, 'Router phủ sóng tốt', '2025-02-11 18:45:00', 'KH000011', 'SP000025'),
('DG000012', 4, 'SSD tốc độ nhanh', '2025-02-12 19:15:00', 'KH000012', 'SP000028'),
('DG000013', 5, 'Laptop cao cấp, đáng tiền', '2025-02-13 20:30:00', 'KH000013', 'SP000016'),
('DG000014', 3, 'Sản phẩm tạm ổn', '2025-02-14 21:45:00', 'KH000014', 'SP000004'),
('DG000015', 4, 'Đóng gói cẩn thận', '2025-02-15 22:20:00', 'KH000015', 'SP000015'),
('DG000016', 5, 'Giá cả hợp lý', '2025-02-16 23:30:00', 'KH000016', 'SP000016'),
('DG000017', 4, 'Dịch vụ tốt', '2025-02-17 08:45:00', 'KH000017', 'SP000018'),
('DG000018', 5, 'Rất hài lòng', '2025-02-18 09:15:00', 'KH000018', 'SP000024'),
('DG000019', 4, 'Sẽ ủng hộ shop dài dài', '2025-02-19 10:30:00', 'KH000019', 'SP000025'),
('DG000020', 5, 'Tuyệt vời!', '2025-02-20 11:45:00', 'KH000001', 'SP000028');

-- --------------------------------------------------------

--
-- Table structure for table `donhang`
--

CREATE TABLE `donhang` (
  `id` varchar(10) NOT NULL,
  `trangthai` enum('dat_hang_thanh_cong','da_duyet_don','dang_giao','giao_thanh_cong','huy_don') NOT NULL DEFAULT 'dat_hang_thanh_cong',
  `tongtien` decimal(10,2) NOT NULL,
  `diachigiaohang` varchar(200) NOT NULL,
  `ngaydathang` datetime DEFAULT current_timestamp(),
  `phuongthucthanhtoan` varchar(50) NOT NULL,
  `ghichu` varchar(500) DEFAULT NULL,
  `lydo_huy` text DEFAULT NULL,
  `id_khachhang` varchar(10) NOT NULL,
  `id_magiamgia` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `donhang`
--

INSERT INTO `donhang` (`id`, `trangthai`, `tongtien`, `diachigiaohang`, `ngaydathang`, `phuongthucthanhtoan`, `ghichu`, `lydo_huy`, `id_khachhang`, `id_magiamgia`) VALUES
('DH000001', 'giao_thanh_cong', 52490000.00, 'Hà Nội', '2025-02-01 08:30:00', 'COD', NULL, NULL, 'KH000001', 'MG000001'),
('DH000002', 'dang_giao', 15990000.00, 'Hồ Chí Minh', '2025-02-02 09:15:00', 'VNPAY', 'Giao trong ngày', NULL, 'KH000002', 'MG000002'),
('DH000003', 'da_duyet_don', 7990000.00, 'Đà Nẵng', '2025-02-03 10:45:00', 'VNPAY', NULL, NULL, 'KH000003', NULL),
('DH000004', 'giao_thanh_cong', 49990000.00, 'Hải Phòng', '2025-02-04 13:20:00', 'Paypal', NULL, NULL, 'KH000004', 'MG000003'),
('DH000005', 'huy_don', 2990000.00, 'Hà Nội', '2025-02-05 14:30:00', 'Paypal', 'Khách đổi ý', 'Tôi không muốn đặt hàng nữa', 'KH000005', NULL),
('DH000006', 'dat_hang_thanh_cong', 8990000.00, 'Hồ Chí Minh', '2025-02-06 15:30:00', 'VNPAY', NULL, NULL, 'KH000006', NULL),
('DH000007', 'dang_giao', 25990000.00, 'Đà Nẵng', '2025-02-07 16:15:00', 'COD', NULL, NULL, 'KH000007', 'MG000004'),
('DH000008', 'giao_thanh_cong', 39990000.00, 'Hải Phòng', '2025-02-08 17:45:00', 'VNPAY', NULL, NULL, 'KH000008', 'MG000005'),
('DH000009', 'dang_giao', 69990000.00, 'Hà Nội', '2025-02-09 18:20:00', 'COD', NULL, NULL, 'KH000009', NULL),
('DH000010', 'dat_hang_thanh_cong', 19990000.00, 'Hồ Chí Minh', '2025-02-10 19:30:00', 'Paypal', NULL, NULL, 'KH000010', 'MG000006'),
('DH000011', 'giao_thanh_cong', 59990000.00, 'Đà Nẵng', '2025-02-11 20:15:00', 'VNPAY', NULL, NULL, 'KH000011', NULL),
('DH000012', 'dang_giao', 2990000.00, 'Hải Phòng', '2025-02-12 21:45:00', 'COD', NULL, NULL, 'KH000012', 'MG000007'),
('DH000013', 'da_duyet_don', 19990000.00, 'Hà Nội', '2025-02-13 22:20:00', 'VNPAY', NULL, NULL, 'KH000013', NULL),
('DH000014', 'giao_thanh_cong', 12990000.00, 'Hồ Chí Minh', '2025-02-14 07:30:00', 'Paypal', NULL, NULL, 'KH000014', 'MG000008'),
('DH000015', 'dat_hang_thanh_cong', 3990000.00, 'Đà Nẵng', '2025-02-15 08:15:00', 'COD', NULL, NULL, 'KH000015', NULL),
('DH000016', 'dang_giao', 25990000.00, 'Hải Phòng', '2025-02-16 09:45:00', 'VNPAY', NULL, NULL, 'KH000016', 'MG000009'),
('DH000017', 'giao_thanh_cong', 14990000.00, 'Hà Nội', '2025-02-17 10:20:00', 'COD', NULL, NULL, 'KH000017', NULL),
('DH000018', 'dat_hang_thanh_cong', 12990000.00, 'Hồ Chí Minh', '2025-02-18 11:45:00', 'Paypal', NULL, NULL, 'KH000018', 'MG000010'),
('DH000019', 'dang_giao', 6990000.00, 'Đà Nẵng', '2025-02-19 12:20:00', 'VNPAY', NULL, NULL, 'KH000019', NULL),
('DH000020', 'giao_thanh_cong', 4990000.00, 'Hải Phòng', '2025-02-20 13:30:00', 'COD', NULL, NULL, 'KH000001', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `giohang`
--

CREATE TABLE `giohang` (
  `id` varchar(10) NOT NULL,
  `id_khachhang` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `giohang`
--

INSERT INTO `giohang` (`id`, `id_khachhang`) VALUES
('GH000001', 'KH000001'),
('GH000020', 'KH000001'),
('GH000002', 'KH000002'),
('GH000003', 'KH000003'),
('GH000004', 'KH000004'),
('GH000005', 'KH000005'),
('GH000006', 'KH000006'),
('GH000007', 'KH000007'),
('GH000008', 'KH000008'),
('GH000009', 'KH000009'),
('GH000010', 'KH000010'),
('GH000011', 'KH000011'),
('GH000012', 'KH000012'),
('GH000013', 'KH000013'),
('GH000014', 'KH000014'),
('GH000015', 'KH000015'),
('GH000016', 'KH000016'),
('GH000017', 'KH000017'),
('GH000018', 'KH000018'),
('GH000019', 'KH000019');

-- --------------------------------------------------------

--
-- Table structure for table `khachhang`
--

CREATE TABLE `khachhang` (
  `id` varchar(10) NOT NULL,
  `hoten` varchar(100) NOT NULL,
  `diachi` varchar(200) NOT NULL,
  `email` varchar(100) NOT NULL,
  `gioitinh` varchar(10) NOT NULL,
  `ngaysinh` date NOT NULL,
  `sodienthoai` varchar(15) NOT NULL,
  `diemtichluy` int(11) DEFAULT 0,
  `id_taikhoan` varchar(10) NOT NULL,
  `id_xephangvip` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `khachhang`
--

INSERT INTO `khachhang` (`id`, `hoten`, `diachi`, `email`, `gioitinh`, `ngaysinh`, `sodienthoai`, `diemtichluy`, `id_taikhoan`, `id_xephangvip`) VALUES
('KH000001', 'Nguyễn Văn A', 'Hà Nội', 'a@gmail.com', 'Nam', '1990-01-01', '0987654321', 100, 'TK000002', 'THANTHIET'),
('KH000002', 'Trần Thị B', 'Hồ Chí Minh', 'b@gmail.com', 'Nữ', '1995-02-15', '0912345678', 777, 'TK000003', 'BAC'),
('KH000003', 'Phạm Văn C', 'Đà Nẵng', 'c@gmail.com', 'Nam', '1992-03-20', '0934567890', 1002, 'TK000004', 'VANG'),
('KH000004', 'Kim Đăng D', 'Hải Phòng', 'd@gmail.com', 'Nữ', '1998-04-25', '0956789012', 10000, 'TK000005', 'KIMCUONG'),
('KH000005', 'Nguyễn Văn E', 'Hà Nội', 'e@gmail.com', 'Nam', '1993-05-30', '0978901234', 300, 'TK000006', 'THANTHIET'),
('KH000006', 'Phạm Hải A', 'Hồ Chí Minh', 'pha@gmail.com', 'Nữ', '2001-06-05', '0967890123', 600, 'TK000007', 'BAC'),
('KH000007', 'Trần Minh B', 'Đà Nẵng', 'tmb@gmail.com', 'Nam', '2000-07-10', '0945678901', 3333, 'TK000008', 'VANG'),
('KH000008', 'Nguyễn Hà C', 'Hải Phòng', 'nhc@gmail.com', 'Nữ', '2001-08-15', '0934567890', 30832, 'TK000009', 'KIMCUONG'),
('KH000009', 'Lê Văn D', 'Đà Nẵng', 'lvd@gmail.com', 'Nam', '2000-09-20', '0923456789', 450, 'TK000010', 'THANTHIET'),
('KH000010', 'Phạm Thị E', 'Hà Nội', 'pte@gmail.com', 'Nữ', '1996-10-25', '0912345678', 800, 'TK000011', 'BAC'),
('KH000011', 'Hoàng Văn F', 'Hồ Chí Minh', 'hvf@gmail.com', 'Nam', '1990-11-30', '0934567890', 2500, 'TK000012', 'VANG'),
('KH000012', 'Trần Thu G', 'Đà Nẵng', 'ttg@gmail.com', 'Nữ', '1994-12-05', '0945678901', 15000, 'TK000013', 'KIMCUONG'),
('KH000013', 'Nguyễn Nam H', 'Hà Nội', 'nnh@gmail.com', 'Nam', '1997-01-10', '0956789012', 350, 'TK000014', 'THANTHIET'),
('KH000014', 'Vũ Thị I', 'Hà Nội', 'vti@gmail.com', 'Nữ', '1992-02-15', '0967890123', 900, 'TK000015', 'BAC'),
('KH000015', 'Đặng Văn K', 'Hồ Chí Minh', 'dvk@gmail.com', 'Nam', '1995-03-20', '0978901234', 4000, 'TK000016', 'VANG'),
('KH000016', 'Mai Thị L', 'Đà Nẵng', 'mtl@gmail.com', 'Nữ', '1991-04-25', '0989012345', 20000, 'TK000017', 'KIMCUONG'),
('KH000017', 'Phan Văn M', 'Hải Phòng', 'pvm@gmail.com', 'Nam', '1998-05-30', '0990123456', 250, 'TK000018', 'THANTHIET'),
('KH000018', 'Trương Thị N', 'Hà Nội', 'ttn@gmail.com', 'Nữ', '2004-06-05', '0901234567', 1200, 'TK000019', 'BAC'),
('KH000019', 'Bùi Văn O', 'Hồ Chí Minh', 'bvo@gmail.com', 'Nam', '2002-07-10', '0912345678', 5000, 'TK000020', 'VANG');

-- --------------------------------------------------------

--
-- Table structure for table `magiamgia`
--

CREATE TABLE `magiamgia` (
  `id` varchar(10) NOT NULL,
  `ten` varchar(100) NOT NULL,
  `ngaysudung` date NOT NULL,
  `ngayhethan` date NOT NULL,
  `tilechietkhau` decimal(5,2) NOT NULL,
  `soluong` int(11) NOT NULL,
  `trangthai` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `magiamgia`
--

INSERT INTO `magiamgia` (`id`, `ten`, `ngaysudung`, `ngayhethan`, `tilechietkhau`, `soluong`, `trangthai`) VALUES
('MG000001', 'Giảm giá đầu năm', '2025-01-01', '2025-01-31', 10.00, 100, 1),
('MG000002', 'Mừng xuân 2025', '2025-02-01', '2025-02-15', 15.00, 50, 1),
('MG000003', 'Khách hàng VIP', '2025-02-01', '2025-12-31', 20.00, 30, 1),
('MG000004', 'Sinh nhật shop', '2025-03-01', '2025-03-31', 25.00, 40, 1),
('MG000005', 'Mua hè sôi động', '2025-06-01', '2025-06-30', 12.00, 80, 1),
('MG000006', 'Back to school', '2025-08-15', '2025-09-15', 18.00, 60, 1),
('MG000007', 'Black Friday', '2025-11-20', '2025-11-30', 30.00, 20, 1),
('MG000008', 'Noel 2025', '2025-12-20', '2025-12-25', 20.00, 45, 1),
('MG000009', 'Tết 2026', '2026-01-20', '2026-02-05', 25.00, 55, 1),
('MG000010', 'Valentine', '2025-02-10', '2025-02-14', 14.00, 70, 1),
('MG000011', 'Mừng 8/3', '2025-03-05', '2025-03-08', 15.00, 65, 1),
('MG000012', 'Quốc tế thiếu nhi', '2025-05-25', '2025-06-01', 10.00, 90, 1),
('MG000013', 'Quốc khánh', '2025-09-01', '2025-09-03', 20.00, 40, 1),
('MG000014', 'Halloween', '2025-10-25', '2025-10-31', 13.00, 75, 1),
('MG000015', 'Cyber Monday', '2025-11-25', '2025-11-26', 28.00, 25, 1),
('MG000016', 'Khách hàng mới', '2025-01-01', '2025-12-31', 10.00, 200, 1),
('MG000017', 'Mua nhiều giảm nhiều', '2025-01-01', '2025-12-31', 15.00, 150, 1),
('MG000018', 'Flash Sale', '2025-07-07', '2025-07-07', 35.00, 15, 1),
('MG000019', 'Giờ vàng', '2025-04-15', '2025-04-15', 40.00, 10, 1),
('MG000020', 'Cuối năm', '2025-12-26', '2025-12-31', 22.00, 35, 1);

-- --------------------------------------------------------

--
-- Table structure for table `nhanvien`
--

CREATE TABLE `nhanvien` (
  `id` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hoten` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `chucvu` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `luong` decimal(10,2) NOT NULL,
  `sodienthoai` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `diachi` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ngayvaolam` date NOT NULL,
  `idtk` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `nhanvien`
--

INSERT INTO `nhanvien` (`id`, `hoten`, `chucvu`, `luong`, `sodienthoai`, `email`, `diachi`, `ngayvaolam`, `idtk`) VALUES
('NV0001', 'Nguyễn Văn A', 'Nhân viên bán hàng', 10000000.00, '0901234567', 'a@gmail.com', 'Hà Nội', '2023-01-01', NULL),
('NV0002', 'Trần Thị B', 'Nhân viên kho', 9000000.00, '0912345678', 'b@gmail.com', 'Hồ Chí Minh', '2022-05-10', NULL),
('NV0003', 'Lê Văn C', 'Nhân viên kế toán', 12000000.00, '0923456789', 'c@gmail.com', 'Đà Nẵng', '2021-11-20', NULL),
('NV0004', 'Phạm Thị D', 'Nhân viên bán hàng', 11000000.00, '0934567890', 'd@gmail.com', 'Cần Thơ', '2020-07-15', NULL),
('NV0005', 'Hoàng Minh E', 'Quản lý cửa hàng', 15000000.00, '0945678901', 'e@gmail.com', 'Hải Phòng', '2019-03-12', NULL),
('NV0006', 'Vũ Văn F', 'Nhân viên bảo vệ', 8000000.00, '0956789012', 'f@gmail.com', 'Nha Trang', '2023-06-01', NULL),
('NV0007', 'Bùi Thị G', 'Nhân viên kho', 9000000.00, '0967890123', 'g@gmail.com', 'Huế', '2022-04-22', NULL),
('NV0008', 'Đặng Văn H', 'Nhân viên IT', 13000000.00, '0978901234', 'h@gmail.com', 'Hà Nội', '2021-09-18', NULL),
('NV0009', 'Nguyễn Thị I', 'Nhân viên bán hàng', 10000000.00, '0989012345', 'i@gmail.com', 'Hải Dương', '2020-02-28', NULL),
('NV0010', 'Trương Minh J', 'Nhân viên marketing', 12500000.00, '0990123456', 'j@gmail.com', 'Hồ Chí Minh', '2019-12-05', NULL),
('NV0011', 'Lâm Văn K', 'Nhân viên kỹ thuật', 11000000.00, '0901122334', 'k@gmail.com', 'Đà Nẵng', '2023-07-17', NULL),
('NV0012', 'Tô Thị L', 'Nhân viên kho', 9500000.00, '0912233445', 'l@gmail.com', 'Cần Thơ', '2022-08-23', NULL),
('NV0013', 'Ngô Văn M', 'Nhân viên bảo vệ', 8500000.00, '0923344556', 'm@gmail.com', 'Huế', '2021-05-30', NULL),
('NV0014', 'Hồ Minh N', 'Nhân viên kế toán', 11800000.00, '0934455667', 'n@gmail.com', 'Hà Nội', '2020-10-10', NULL),
('NV0015', 'Dương Thị O', 'Nhân viên bán hàng', 10200000.00, '0945566778', 'o@gmail.com', 'Hải Phòng', '2019-06-08', NULL),
('NV0016', 'Đoàn Văn P', 'Nhân viên IT', 13500000.00, '0956677889', 'p@gmail.com', 'Nha Trang', '2023-09-14', NULL),
('NV0017', 'Trịnh Thị Q', 'Quản lý kho', 14000000.00, '0967788990', 'q@gmail.com', 'Huế', '2022-03-11', NULL),
('NV0018', 'Phan Văn R', 'Nhân viên marketing', 12700000.00, '0978899001', 'r@gmail.com', 'Hồ Chí Minh', '2021-08-19', NULL),
('NV0019', 'Lưu Minh S', 'Nhân viên kỹ thuật', 11300000.00, '0989900112', 's@gmail.com', 'Đà Nẵng', '2020-04-25', NULL),
('NV0020', 'Tạ Thị T', 'Nhân viên bảo vệ', 8600000.00, '0990011223', 't@gmail.com', 'Hải Dương', '2019-01-30', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `sanpham`
--

CREATE TABLE `sanpham` (
  `id` varchar(10) NOT NULL,
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

INSERT INTO `sanpham` (`id`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000001', 'CPU Intel Core i9-13900K', 15990000.00, 50, 'Intel', 'CPU Intel thế hệ 13 cao cấp nhất', '{\r\n    \"Socket\": \"LGA 1700\",\r\n    \"Số nhân\": 24,\r\n    \"Số luồng\": 32,\r\n    \"Xung nhịp cơ bản\": \"3.0 GHz\",\r\n    \"Xung nhịp tối đa\": \"5.8 GHz\",\r\n    \"Cache\": \"36MB L3 + 32MB L2\",\r\n    \"TDP\": \"125W\",\r\n    \"Công nghệ\": \"Intel 7\",\r\n    \"Hỗ trợ RAM\": \"DDR4/DDR5\"\r\n}', 'CPU', 'cpu_i9_13900k.jpg', 0, 0),
('SP000002', 'CPU AMD Ryzen 9 7950X', 14990000.00, 40, 'AMD', 'CPU AMD thế hệ mới nhất', '{\r\n    \"Socket\": \"AM5\",\r\n    \"Số nhân\": 16,\r\n    \"Số luồng\": 32,\r\n    \"Xung nhịp cơ bản\": \"4.5 GHz\",\r\n    \"Xung nhịp tối đa\": \"5.7 GHz\",\r\n    \"Cache\": \"64MB L3\",\r\n    \"TDP\": \"170W\",\r\n    \"Công nghệ\": \"TSMC 5nm\",\r\n    \"Hỗ trợ RAM\": \"DDR5\"\r\n}', 'CPU', 'cpu_r9_7950x.jpg', 0, 0),
('SP000003', 'CPU Intel Core i5-13600K', 7990000.00, 60, 'Intel', 'CPU Intel tầm trung', '{\r\n    \"Socket\": \"LGA 1700\",\r\n    \"Số nhân\": 14,\r\n    \"Số luồng\": 20,\r\n    \"Xung nhịp cơ bản\": \"3.5 GHz\",\r\n    \"Xung nhịp tối đa\": \"5.1 GHz\",\r\n    \"Cache\": \"24MB L3\",\r\n    \"TDP\": \"125W\",\r\n    \"Công nghệ\": \"Intel 7\",\r\n    \"Hỗ trợ RAM\": \"DDR4/DDR5\"\r\n}', 'CPU', 'cpu_i5_13600k.jpg', 0, 0),
('SP000004', 'MAINBOARD ASUS ROG MAXIMUS Z790 HERO', 12990000.00, 30, 'ASUS', 'Bo mạch chủ cao cấp cho Intel', '{\r\n    \"Socket\": \"LGA 1700\",\r\n    \"Chipset\": \"Intel Z790\",\r\n    \"Kích thước\": \"ATX\",\r\n    \"Khe RAM\": \"4 khe DDR5\",\r\n    \"Hỗ trợ RAM tối đa\": \"128GB\",\r\n    \"Khe M.2\": \"4 khe\",\r\n    \"Cổng SATA\": \"6 cổng\",\r\n    \"USB\": {\r\n        \"USB 3.2 Gen 2x2\": \"2 cổng\",\r\n        \"USB 3.2 Gen 2\": \"4 cổng\",\r\n        \"USB 3.2 Gen 1\": \"6 cổng\"\r\n    }\r\n}', 'Mainboard', 'mb_asus_z790.jpg', 0, 0),
('SP000005', 'MAINBOARD MSI MEG X670E ACE', 15990000.00, 25, 'MSI', 'Bo mạch chủ cao cấp cho AMD', '{\r\n    \"Socket\": \"AM5\",\r\n    \"Chipset\": \"X670E\",\r\n    \"Kích thước\": \"E-ATX\",\r\n    \"Khe RAM\": \"4 khe DDR5\",\r\n    \"Hỗ trợ RAM tối đa\": \"128GB\",\r\n    \"Khe M.2\": \"5 khe\",\r\n    \"Cổng SATA\": \"6 cổng\"\r\n}', 'Mainboard', 'mb_msi_x670e.jpg', 0, 0),
('SP000006', 'RAM G.Skill Trident Z5 RGB 32GB', 4990000.00, 80, 'G.Skill', 'Kit RAM DDR5 hiệu năng cao', '{\r\n    \"Dung lượng\": \"32GB (2x16GB)\",\r\n    \"Thế hệ\": \"DDR5\",\r\n    \"Tốc độ\": \"6000MHz\",\r\n    \"Timing\": \"36-36-36-96\",\r\n    \"Điện áp\": \"1.35V\",\r\n    \"Tản nhiệt\": \"Nhôm\",\r\n    \"LED\": \"RGB\"\r\n}', 'RAM', 'ram_gskill_z5.jpg', 0, 0),
('SP000007', 'RAM Corsair Vengeance 16GB', 1590000.00, 100, 'Corsair', 'RAM DDR4 giá tốt', '{\r\n    \"Dung lượng\": \"16GB (2x8GB)\",\r\n    \"Thế hệ\": \"DDR4\",\r\n    \"Tốc độ\": \"3200MHz\",\r\n    \"Timing\": \"16-18-18-36\",\r\n    \"Điện áp\": \"1.35V\"\r\n}', 'RAM', 'ram_corsair_vengeance.jpg', 0, 0),
('SP000008', 'RAM Kingston Fury Beast 64GB', 7990000.00, 30, 'Kingston', 'RAM DDR5 dung lượng cao', '{\r\n    \"Dung lượng\": \"64GB (2x32GB)\",\r\n    \"Thế hệ\": \"DDR5\",\r\n    \"Tốc độ\": \"6000MHz\",\r\n    \"Timing\": \"40-40-40-80\",\r\n    \"Điện áp\": \"1.35V\"\r\n}', 'RAM', 'ram_kingston_fury.jpg', 0, 0),
('SP000009', 'VGA MSI Gaming X RTX 4070', 15990000.00, 35, 'MSI', 'Card đồ họa tầm trung', '{\r\n    \"GPU\": \"NVIDIA Ada Lovelace\",\r\n    \"VRAM\": \"12GB GDDR6X\",\r\n    \"Xung nhịp Boost\": \"2475 MHz\",\r\n    \"Ray Tracing\": \"3rd Generation\",\r\n    \"DLSS\": \"DLSS 3\",\r\n    \"Nguồn đề xuất\": \"650W\"\r\n}', 'VGA', 'vga_msi_4070.jpg', 0, 0),
('SP000010', 'VGA ASUS ROG STRIX RTX 4090 OC', 49990000.00, 15, 'ASUS', 'Card đồ họa NVIDIA flagship', '{\r\n    \"GPU\": \"NVIDIA Ada Lovelace\",\r\n    \"VRAM\": \"24GB GDDR6X\",\r\n    \"Xung nhịp Boost\": \"2640 MHz\",\r\n    \"Băng thông bộ nhớ\": \"1008 GB/s\",\r\n    \"Ray Tracing\": \"3rd Generation\",\r\n    \"DLSS\": \"DLSS 3\",\r\n    \"Nguồn đề xuất\": \"1000W\"\r\n}', 'VGA', 'vga_asus_4090.jpg', 0, 0),
('SP000011', 'VGA Gigabyte RTX 4060 Gaming OC', 8990000.00, 45, 'Gigabyte', 'Card đồ họa giá tốt', '{\r\n    \"GPU\": \"NVIDIA Ada Lovelace\",\r\n    \"VRAM\": \"8GB GDDR6\",\r\n    \"Xung nhịp Boost\": \"2460 MHz\",\r\n    \"Ray Tracing\": \"3rd Generation\",\r\n    \"DLSS\": \"DLSS 3\",\r\n    \"Nguồn đề xuất\": \"550W\"\r\n}', 'VGA', 'vga_gigabyte_4060.jpg', 0, 0),
('SP000012', 'LAPTOP gaming MSI Katana GF66', 25990000.00, 20, 'MSI', 'Laptop gaming tầm trung', '{\r\n    \"CPU\": \"Intel Core i7-12700H\",\r\n    \"GPU\": \"NVIDIA RTX 3060 Laptop\",\r\n    \"RAM\": \"16GB DDR4-3200\",\r\n    \"Màn hình\": \"15.6 inch FHD 144Hz\"\r\n}', 'Laptop', 'laptop_msi_katana.jpg', 0, 0),
('SP000013', 'LAPTOP Dell XPS 13 Plus', 39990000.00, 15, 'Dell', 'Laptop doanh nhân cao cấp', '{\r\n    \"CPU\": \"Intel Core i7-1260P\",\r\n    \"GPU\": \"Intel Iris Xe\",\r\n    \"RAM\": \"16GB LPDDR5\",\r\n    \"Màn hình\": \"13.4 inch 3.5K OLED\"\r\n}', 'Laptop', 'laptop_dell_xps.jpg', 0, 0),
('SP000014', 'LAPTOP gaming Gigabyte AERO 16', 69990000.00, 10, 'Gigabyte', 'Laptop gaming cho người sáng tạo', '{\r\n    \"CPU\": \"Intel Core i9-12900HK\",\r\n    \"GPU\": \"NVIDIA RTX 3080 Ti Laptop\",\r\n    \"RAM\": \"32GB DDR5\",\r\n    \"Màn hình\": \"16 inch 4K OLED\"\r\n}', 'Laptop', 'laptop_gigabyte_aero.jpg', 0, 0),
('SP000015', 'LAPTOP Acer Nitro 5', 19990000.00, 30, 'Acer', 'Laptop gaming giá tốt', '{\r\n    \"CPU\": \"AMD Ryzen 5 7600H\",\r\n    \"GPU\": \"NVIDIA RTX 4050 Laptop\",\r\n    \"RAM\": \"8GB DDR5\",\r\n    \"Màn hình\": \"15.6 inch FHD 144Hz\"\r\n}', 'Laptop', 'laptop_acer_nitro.jpg', 0, 0),
('SP000016', 'LAPTOP ASUS ROG Zephyrus G14', 59990000.00, 10, 'ASUS', 'Laptop gaming cao cấp nhỏ gọn', '{\r\n    \"CPU\": \"AMD Ryzen 9 7940HS\",\r\n    \"GPU\": \"NVIDIA RTX 4090 Laptop\",\r\n    \"RAM\": \"32GB DDR5-4800\",\r\n    \"Màn hình\": \"14 inch QHD+ 165Hz\"\r\n}', 'Laptop', 'laptop_asus_g14.jpg', 0, 0),
('SP000018', 'KEYBOARD Logitech G Pro X', 2990000.00, 50, 'Logitech', 'Bàn phím cơ gaming cao cấp', '{\r\n    \"Kiểu\": \"Tenkeyless\",\r\n    \"Switch\": \"GX Blue Clicky\",\r\n    \"Keycap\": \"PBT Double-shot\",\r\n    \"Kết nối\": \"USB-C có thể tháo rời\",\r\n    \"LED\": \"RGB 16.8M màu\"\r\n}', 'Keyboard', 'kb_logitech_prox.jpg', 0, 0),
('SP000024', 'MONITOR LG 27GP950-B', 19990000.00, 20, 'LG', 'Màn hình gaming 4K cao cấp', '{\r\n    \"Kích thước\": \"27 inch\",\r\n    \"Độ phân giải\": \"3840x2160\",\r\n    \"Tần số quét\": \"144Hz\",\r\n    \"Tấm nền\": \"Nano IPS\",\r\n    \"HDR\": \"VESA DisplayHDR 600\"\r\n}', 'Monitor', 'monitor_lg_27gp950.jpg', 0, 0),
('SP000025', 'ROUTER ASUS ROG Rapture GT-AX11000', 12990000.00, 15, 'ASUS', 'Router WiFi 6 cao cấp cho gaming', '{\r\n    \"Chuẩn WiFi\": \"WiFi 6 (802.11ax)\",\r\n    \"CPU\": \"1.8GHz Quad-core\",\r\n    \"RAM\": \"1GB\",\r\n    \"Cổng mạng\": \"1x 2.5G WAN, 4x 1G LAN\"\r\n}', 'Router', 'router_asus_gt11000.jpg', 0, 0),
('SP000028', 'SSD Samsung 990 PRO 2TB', 6990000.00, 25, 'Samsung', 'SSD NVMe PCIe 4.0 hiệu năng cao', '{\r\n    \"Dung lượng\": \"2TB\",\r\n    \"Giao tiếp\": \"PCIe 4.0 x4 NVMe\",\r\n    \"Tốc độ đọc\": \"7450MB/s\",\r\n    \"Tốc độ ghi\": \"6900MB/s\"\r\n}', 'Storage', 'ssd_samsung_990pro.jpg', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `taikhoan`
--

CREATE TABLE `taikhoan` (
  `id` varchar(10) NOT NULL,
  `matkhau` varchar(255) NOT NULL,
  `tentaikhoan` varchar(50) NOT NULL,
  `ngaytaotk` date NOT NULL,
  `ngaysuadoi` date DEFAULT NULL,
  `quyentruycap` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `taikhoan`
--

INSERT INTO `taikhoan` (`id`, `matkhau`, `tentaikhoan`, `ngaytaotk`, `ngaysuadoi`, `quyentruycap`) VALUES
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
('TK000020', 'user19', 'user19', '0000-00-00', NULL, 'khachhang');

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `id` varchar(10) NOT NULL,
  `trangthai` varchar(50) NOT NULL,
  `tienthanhtoan` decimal(10,2) NOT NULL,
  `ngaythanhtoan` datetime DEFAULT current_timestamp(),
  `noidungthanhtoan` varchar(200) DEFAULT NULL,
  `mathanhtoan` varchar(50) DEFAULT NULL,
  `id_donhang` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `thanhtoan`
--

INSERT INTO `thanhtoan` (`id`, `trangthai`, `tienthanhtoan`, `ngaythanhtoan`, `noidungthanhtoan`, `mathanhtoan`, `id_donhang`) VALUES
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
  ADD PRIMARY KEY (`id_donhang`,`id_sanpham`),
  ADD KEY `id_sanpham` (`id_sanpham`);

--
-- Indexes for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD PRIMARY KEY (`id_giohang`,`id_sanpham`),
  ADD KEY `id_sanpham` (`id_sanpham`);

--
-- Indexes for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id_khachhang` (`id_khachhang`,`id_sanpham`),
  ADD KEY `id_sanpham` (`id_sanpham`),
  ADD KEY `idx_danhgia_sosao` (`sosao`),
  ADD KEY `idx_danhgia_ngaydanhgia` (`ngaydanhgia`);

--
-- Indexes for table `donhang`
--
ALTER TABLE `donhang`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_khachhang` (`id_khachhang`),
  ADD KEY `id_magiamgia` (`id_magiamgia`),
  ADD KEY `idx_donhang_trangthai` (`trangthai`),
  ADD KEY `idx_donhang_ngaydathang` (`ngaydathang`);

--
-- Indexes for table `giohang`
--
ALTER TABLE `giohang`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_khachhang` (`id_khachhang`);

--
-- Indexes for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_taikhoan` (`id_taikhoan`),
  ADD KEY `id_xephangvip` (`id_xephangvip`),
  ADD KEY `idx_khachhang_email` (`email`),
  ADD KEY `idx_khachhang_sodienthoai` (`sodienthoai`);

--
-- Indexes for table `magiamgia`
--
ALTER TABLE `magiamgia`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_nhanvien_taikhoan` (`idtk`);

--
-- Indexes for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_sanpham_tensanpham` (`tensanpham`),
  ADD KEY `idx_sanpham_gia` (`gia`),
  ADD KEY `idx_sanpham_thuonghieu` (`thuonghieu`);

--
-- Indexes for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_taikhoan_tentaikhoan` (`tentaikhoan`),
  ADD KEY `idx_taikhoan_quyentruycap` (`quyentruycap`);

--
-- Indexes for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_donhang` (`id_donhang`),
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
  ADD CONSTRAINT `chitietdonhang_ibfk_1` FOREIGN KEY (`id_donhang`) REFERENCES `donhang` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietdonhang_ibfk_2` FOREIGN KEY (`id_sanpham`) REFERENCES `sanpham` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `chitietgiohang`
--
ALTER TABLE `chitietgiohang`
  ADD CONSTRAINT `chitietgiohang_ibfk_1` FOREIGN KEY (`id_giohang`) REFERENCES `giohang` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_ibfk_2` FOREIGN KEY (`id_sanpham`) REFERENCES `sanpham` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `danhgia`
--
ALTER TABLE `danhgia`
  ADD CONSTRAINT `danhgia_ibfk_1` FOREIGN KEY (`id_khachhang`) REFERENCES `khachhang` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `danhgia_ibfk_2` FOREIGN KEY (`id_sanpham`) REFERENCES `sanpham` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `donhang`
--
ALTER TABLE `donhang`
  ADD CONSTRAINT `donhang_ibfk_1` FOREIGN KEY (`id_khachhang`) REFERENCES `khachhang` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `donhang_ibfk_2` FOREIGN KEY (`id_magiamgia`) REFERENCES `magiamgia` (`id`);

--
-- Constraints for table `giohang`
--
ALTER TABLE `giohang`
  ADD CONSTRAINT `giohang_ibfk_1` FOREIGN KEY (`id_khachhang`) REFERENCES `khachhang` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD CONSTRAINT `khachhang_ibfk_1` FOREIGN KEY (`id_taikhoan`) REFERENCES `taikhoan` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `khachhang_ibfk_2` FOREIGN KEY (`id_xephangvip`) REFERENCES `xephangvip` (`id`);

--
-- Constraints for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD CONSTRAINT `fk_nhanvien_taikhoan` FOREIGN KEY (`idtk`) REFERENCES `taikhoan` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD CONSTRAINT `thanhtoan_ibfk_1` FOREIGN KEY (`id_donhang`) REFERENCES `donhang` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
