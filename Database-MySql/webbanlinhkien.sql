-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 21, 2025 at 05:17 AM
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
('CTDH000026', 'DH000019', 'SP000028', NULL, 1, 6990000.00),
('CTDH00027', 'DH000021', 'SP000021', NULL, 1, 34930000.00),
('CTDH00028', 'DH000022', 'SP000021', NULL, 1, 34930000.00),
('CTDH00029', 'DH000023', 'SP000021', NULL, 2, 34930000.00),
('CTDH00030', 'DH000024', 'SP000001', NULL, 1, 15990000.00),
('CTDH00031', 'DH000025', 'SP000002', NULL, 1, 13990000.00),
('CTDH00032', 'DH000026', 'SP000002', NULL, 1, 13990000.00),
('CTDH00033', 'DH000027', 'SP000001', NULL, 1, 15990000.00);

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
('DH000001', 'Giao thành công', 52490000.00, '15 Nguyễn Khuyến, Phường Láng, Quận Đống Đa, Hà Nội', '2025-02-01 08:30:00', 'COD', NULL, NULL, 'KH000001', 'MG000001'),
('DH000002', 'Đang giao', 15990000.00, '22 Lê Lợi, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', '2025-02-02 09:15:00', 'VNPAY', 'Giao trong ngày', NULL, 'KH000002', 'MG000002'),
('DH000003', 'Đã duyệt đơn', 7990000.00, '35 Trần Phú, Phường Hải Châu 1, Quận Hải Châu, Đà Nẵng', '2025-02-03 10:45:00', 'VNPAY', NULL, NULL, 'KH000003', NULL),
('DH000004', 'Giao thành công', 49990000.00, '18 Lạch Tray, Phường Hồng Bàng, Quận Ngô Quyền, Hải Phòng', '2025-02-04 13:20:00', 'Paypal', NULL, NULL, 'KH000004', 'MG000003'),
('DH000005', 'Hủy đơn', 2990000.00, '10 Phan Bội Châu, Phường Nguyễn Du, Quận Hai Bà Trưng, Hà Nội', '2025-02-05 14:30:00', 'Paypal', 'Khách đổi ý', 'Tôi không muốn đặt hàng nữa', 'KH000005', NULL),
('DH000006', 'Đặt hàng thành công', 8990000.00, '50 Nguyễn Thị Minh Khai, Phường 11, Quận 3, Thành phố Hồ Chí Minh', '2025-02-06 15:30:00', 'VNPAY', NULL, NULL, 'KH000006', NULL),
('DH000007', 'Đang giao', 25990000.00, '80 Lê Duẩn, Phường Thạch Thang, Quận Thanh Khê, Đà Nẵng', '2025-02-07 16:15:00', 'COD', NULL, NULL, 'KH000007', 'MG000004'),
('DH000008', 'Giao thành công', 39990000.00, '25 Võ Nguyên Giáp, Phường Tân Thắng, Quận Ngô Quyền, Hải Phòng', '2025-02-08 17:45:00', 'VNPAY', NULL, NULL, 'KH000008', 'MG000005'),
('DH000009', 'Đang giao', 69990000.00, '32 Trần Hưng Đạo, Phường Cầu Ông Lãnh, Quận Hoàn Kiếm, Hà Nội', '2025-02-09 18:20:00', 'COD', NULL, NULL, 'KH000009', NULL),
('DH000010', 'Đặt hàng thành công', 19990000.00, '45 Đồng Khởi, Phường Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', '2025-02-10 19:30:00', 'Paypal', NULL, NULL, 'KH000010', 'MG000006'),
('DH000011', 'Giao thành công', 59990000.00, '60 Nguyễn Văn Linh, Phường Thanh Bình, Quận Hải Châu, Đà Nẵng', '2025-02-11 20:15:00', 'VNPAY', NULL, NULL, 'KH000011', NULL),
('DH000012', 'Đang giao', 2990000.00, '12 Lý Thường Kiệt, Phường Thủy Dương, Quận Ngô Quyền, Hải Phòng', '2025-02-12 21:45:00', 'COD', NULL, NULL, 'KH000012', 'MG000007'),
('DH000013', 'Đã duyệt đơn', 19990000.00, '78 Trần Phú, Phường Phúc Tân, Quận Hoàn Kiếm, Hà Nội', '2025-02-13 22:20:00', 'VNPAY', NULL, NULL, 'KH000013', NULL),
('DH000014', 'Giao thành công', 12990000.00, '90 Hai Bà Trưng, Phường 14, Quận 3, Thành phố Hồ Chí Minh', '2025-02-14 07:30:00', 'Paypal', NULL, NULL, 'KH000014', 'MG000008'),
('DH000015', 'Đặt hàng thành công', 3990000.00, '33 Võ Văn Kiệt, Phường Hòa Thuận, Quận Ngũ Hành Sơn, Đà Nẵng', '2025-02-15 08:15:00', 'COD', NULL, NULL, 'KH000015', NULL),
('DH000016', 'Đang giao', 25990000.00, '27 Phan Châu Trinh, Phường Kênh Dương, Quận Ngô Quyền, Hải Phòng', '2025-02-16 09:45:00', 'VNPAY', NULL, NULL, 'KH000016', 'MG000009'),
('DH000017', 'Giao thành công', 14990000.00, '55 Nguyễn Công Trứ, Phường Phúc Lợi, Quận Ba Đình, Hà Nội', '2025-02-17 10:20:00', 'COD', NULL, NULL, 'KH000017', NULL),
('DH000018', 'Đặt hàng thành công', 12990000.00, '40 Lê Lợi, Phường Tân Định, Quận 1, Thành phố Hồ Chí Minh', '2025-02-18 11:45:00', 'Paypal', NULL, NULL, 'KH000018', 'MG000010'),
('DH000019', 'Đang giao', 6990000.00, '65 Võ Văn Kiệt, Phường Hòa Hiệp, Quận Cẩm Lệ, Đà Nẵng', '2025-02-19 12:20:00', 'VNPAY', NULL, NULL, 'KH000019', NULL),
('DH000020', 'Giao thành công', 4990000.00, '22 Cầu Đất, Phường Hồng Bàng, Quận Ngô Quyền, Hải Phòng', '2025-02-20 13:30:00', 'COD', NULL, NULL, 'KH000001', NULL),
('DH000021', 'Chờ xác nhận', 34930000.00, '249Đ Nguyễn Văn Luông, Phường 11, Quận 6, Hồ Chí Minh', '2025-02-20 10:54:59', 'COD', '', NULL, 'KH000021', NULL),
('DH000022', 'Chờ xác nhận', 34930000.00, '249Đ, Nguyễn Văn Luông, Phường 11, Quận 6, Thành phố Hồ Chí Minh, a, a, Tp.Hồ Chí Minh', '2025-02-20 10:55:58', 'COD', '', NULL, 'KH000022', NULL),
('DH000023', 'Chờ xác nhận', 69860000.00, 'a, a, aa, a', '2025-02-20 11:24:06', 'COD', '', NULL, 'KH00020', NULL),
('DH000024', 'Chờ xác nhận', 15990000.00, 'a, a, a, a', '2025-02-20 11:42:49', 'COD', '', NULL, 'KH00020', NULL),
('DH000025', 'Chờ xác nhận', 13990000.00, 'c, c, c, c', '2025-02-20 11:45:30', 'COD', '', NULL, 'KH00020', NULL),
('DH000026', 'Chờ xác nhận', 13990000.00, 'a, a, a, a', '2025-02-20 12:00:58', 'COD', '', NULL, 'KH000023', NULL),
('DH000027', 'Chờ xác nhận', 15990000.00, 'C17 Khu nhà ở U&I An Phú, Phường An Phú, Thuận An, Bình Dương, , , Bình Dương', '2025-02-21 11:07:27', 'COD', '', NULL, 'KH230104', NULL);

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
  `IdTk` varchar(10) DEFAULT NULL,
  `id_xephangvip` varchar(10) DEFAULT NULL,
  `loaikhachhang` bit(1) DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `khachhang`
--

INSERT INTO `khachhang` (`IdKh`, `hoten`, `diachi`, `email`, `gioitinh`, `ngaysinh`, `sodienthoai`, `diemtichluy`, `IdTk`, `id_xephangvip`, `loaikhachhang`) VALUES
('KH000001', 'a', '123 Nguyễn Văn A, Quận 1, TP.HCM', 'a@gmail.com', 'Nam', '1990-01-01', '0946575839', 100, 'TK000002', 'THANTHIET', b'1'),
('KH000002', 'Trần Thị B', '45 Lê Lợi, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 'b@gmail.com', 'Nữ', '1995-02-15', '0912345678', 777, 'TK000003', 'BAC', b'1'),
('KH000003', 'Phạm Văn C', '78 Nguyễn Văn Linh, Phường Hải Châu 1, Quận Hải Châu, Đà Nẵng', 'c@gmail.com', 'Nam', '1992-03-20', '0934567890', 1002, 'TK000004', 'VANG', b'1'),
('KH000004', 'Kim Đăng D', '90 Lạch Tray, Phường Hồng Bàng, Quận Ngô Quyền, Hải Phòng', 'd@gmail.com', 'Nữ', '1998-04-25', '0956789012', 10000, 'TK000005', 'KIMCUONG', b'1'),
('KH000005', 'Nguyễn Văn E', '34 Trần Hưng Đạo, Phường Ô Chợ Dừa, Quận Đống Đa, Hà Nội', 'e@gmail.com', 'Nam', '1993-05-30', '0978901234', 300, 'TK000006', 'THANTHIET', b'1'),
('KH000006', 'Phạm Hải A', '56 Phan Đình Phùng, Phường 14, Quận Phú Nhuận, Thành phố Hồ Chí Minh', 'pha@gmail.com', 'Nữ', '2001-06-05', '0967890123', 600, 'TK000007', 'BAC', b'1'),
('KH000007', 'Trần Minh B', '101 Nguyễn Văn Quá, Phường Thạch Thang, Quận Thanh Khê, Đà Nẵng', 'tmb@gmail.com', 'Nam', '2000-07-10', '0945678901', 3333, 'TK000008', 'VANG', b'1'),
('KH000008', 'Nguyễn Hà C', '202 Võ Nguyên Giáp, Phường Lãm Hà, Quận Ngô Quyền, Hải Phòng', 'nhc@gmail.com', 'Nữ', '2001-08-15', '0934567890', 30832, 'TK000009', 'KIMCUONG', b'1'),
('KH000009', 'Lê Văn D', '88 Nguyễn Tri Phương, Phường An Hải, Quận Sơn Trà, Đà Nẵng', 'lvd@gmail.com', 'Nam', '2000-09-20', '0923456789', 450, 'TK000010', 'THANTHIET', b'1'),
('KH000010', 'Phạm Thị E', '77 Kim Mã, Phường Liễu Giai, Quận Ba Đình, Hà Nội', 'pte@gmail.com', 'Nữ', '1996-10-25', '0912345678', 800, 'TK000011', 'BAC', b'1'),
('KH000011', 'Hoàng Văn F', '66 Pasteur, Phường 7, Quận 3, Thành phố Hồ Chí Minh', 'hvf@gmail.com', 'Nam', '1990-11-30', '0934567890', 2500, 'TK000012', 'VANG', b'1'),
('KH000012', 'Trần Thu G', '55 Trần Phú, Phường Bình Hiên, Quận Thanh Khê, Đà Nẵng', 'ttg@gmail.com', 'Nữ', '1994-12-05', '0945678901', 15000, 'TK000013', 'KIMCUONG', b'1'),
('KH000013', 'Nguyễn Nam H', '123 Phố Huế, Phường Hàng Bột, Quận Hoàn Kiếm, Hà Nội', 'nnh@gmail.com', 'Nam', '1997-01-10', '0956789012', 350, 'TK000014', 'THANTHIET', b'1'),
('KH000014', 'Vũ Thị I', '89 Cầu Giấy, Phường Dịch Vọng, Quận Cầu Giấy, Hà Nội', 'vti@gmail.com', 'Nữ', '1992-02-15', '0967890123', 900, 'TK000015', 'BAC', b'1'),
('KH000015', 'Đặng Văn K', '35 Võ Văn Tần, Phường 2, Quận Tân Bình, Thành phố Hồ Chí Minh', 'dvk@gmail.com', 'Nam', '1995-03-20', '0978901234', 4000, 'TK000016', 'VANG', b'1'),
('KH000016', 'Mai Thị L', '47 Lê Duẩn, Phường Mỹ An, Quận Ngũ Hành Sơn, Đà Nẵng', 'mtl@gmail.com', 'Nữ', '1991-04-25', '0989012345', 20000, 'TK000017', 'KIMCUONG', b'1'),
('KH000017', 'Phan Văn M', '68 Hồng Bàng, Phường Hồng Bàng, Quận Lê Chân, Hải Phòng', 'pvm@gmail.com', 'Nam', '1998-05-30', '0990123456', 250, 'TK000018', 'THANTHIET', b'1'),
('KH000018', 'Trương Thị N', '100 Phạm Hùng, Phường Thịnh Quang, Quận Thanh Xuân, Hà Nội', 'ttn@gmail.com', 'Nữ', '2004-06-05', '0901234567', 1200, 'TK000019', 'BAC', b'1'),
('KH000019', 'Bùi Văn O', '150 Đinh Tiên Hoàng, Phường Đa Kao, Quận 1, Thành phố Hồ Chí Minh', 'bvo@gmail.com', 'Nam', '2002-07-10', '0912345678', 5000, 'TK000020', 'VANG', b'1'),
('KH000021', 'asd', '249Đ Nguyễn Văn Luông, Phường 11, Quận 6, Hồ Chí Minh', 'asd@gmail.com', NULL, NULL, '0949752094', 0, NULL, 'THANTHIET', b'0'),
('KH000022', 'gas', '249Đ, Nguyễn Văn Luông, Phường 11, Quận 6, Thành phố Hồ Chí Minh, a, a, Tp.Hồ Chí Minh', 'gas@gmail.com', NULL, NULL, '0949752092', 0, NULL, 'THANTHIET', b'0'),
('KH000023', 'sadsa', '', 'kjgs@gmail.com', NULL, NULL, '095353163', 0, NULL, 'THANTHIET', b'0'),
('KH00020', 'Nguyễn Phi Quốc Bảo', 'a, a, a, a', 'nuponiibaka.com@gmail.com', 'Nam', '2004-08-02', '0949752097', 0, 'TK00021', NULL, b'0'),
('KH230104', 'Trần Hồng Phát', 'C17 Khu nhà ở U&I, Phường An Phú, Thành Phố Thuận An, Bình Dương\r\n', 'asdsaphat.com@gmail.com', 'Nam', '2004-01-23', '0948048197', 0, 'TK230104', NULL, b'0');

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
('SP000001', 'CPU Intel Core i9-13900K', 15990000.00, 48, 'Intel', 'CPU Intel Core i9-13900K mạnh mẽ, lý tưởng cho gaming và công việc sáng tạo.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i9\", \"Số nhân\": \"24\", \"Số luồng\": \"32\", \"Xung nhịp\": \"3.0GHz\", \"Xung nhịp cơ bản\": \"2.8GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"36MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i9_13900k.jpg', 14, 2),
('SP000002', 'CPU Intel Core i7-13700K', 13990000.00, 43, 'Intel', 'CPU Intel Core i7-13700K hiệu năng cao, lý tưởng cho gaming.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i7\", \"Số nhân\": \"16\", \"Số luồng\": \"24\", \"Xung nhịp\": \"3.2GHz\", \"Xung nhịp cơ bản\": \"3.0GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"30MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i7_13700k.jpg', 8, 2),
('SP000003', 'CPU Intel Core i5-13600K', 11990000.00, 40, 'Intel', 'CPU Intel Core i5-13600K cân bằng giữa hiệu năng và giá cả.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i5\", \"Số nhân\": \"10\", \"Số luồng\": \"16\", \"Xung nhịp\": \"3.5GHz\", \"Xung nhịp cơ bản\": \"3.0GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"20MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i5_13600k.jpg', 6, 0),
('SP000004', 'CPU AMD Ryzen 9 7950X', 17990000.00, 35, 'AMD', 'CPU AMD Ryzen 9 7950X hiệu năng mạnh mẽ cho các tác vụ nặng.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM5\", \"Dòng CPU\": \"AMD Ryzen 9\", \"Số nhân\": \"16\", \"Số luồng\": \"32\", \"Xung nhịp\": \"4.5GHz\", \"Xung nhịp cơ bản\": \"3.4GHz\", \"Điện năng tiêu thụ\": \"170W\", \"Bộ nhớ đệm\": \"64MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen9_7950x.jpg', 2, 0),
('SP000005', 'CPU AMD Ryzen 7 7700X', 12990000.00, 30, 'AMD', 'CPU AMD Ryzen 7 7700X mang lại hiệu năng gaming ấn tượng.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM5\", \"Dòng CPU\": \"AMD Ryzen 7\", \"Số nhân\": \"8\", \"Số luồng\": \"16\", \"Xung nhịp\": \"4.5GHz\", \"Xung nhịp cơ bản\": \"3.8GHz\", \"Điện năng tiêu thụ\": \"105W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen7_7700x.jpg', 0, 0),
('SP000006', 'CPU Intel Core i3-12100F', 5490000.00, 60, 'Intel', 'CPU Intel Core i3-12100F lý tưởng cho công việc văn phòng.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i3\", \"Số nhân\": \"4\", \"Số luồng\": \"8\", \"Xung nhịp\": \"4.3GHz\", \"Xung nhịp cơ bản\": \"3.3GHz\", \"Điện năng tiêu thụ\": \"58W\", \"Bộ nhớ đệm\": \"12MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_i3_12100f.jpg', 0, 0),
('SP000007', 'CPU AMD Ryzen 5 7600X', 7490000.00, 55, 'AMD', 'CPU AMD Ryzen 5 7600X phù hợp cho game thủ phổ thông.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM5\", \"Dòng CPU\": \"AMD Ryzen 5\", \"Số nhân\": \"6\", \"Số luồng\": \"12\", \"Xung nhịp\": \"4.7GHz\", \"Xung nhịp cơ bản\": \"4.2GHz\", \"Điện năng tiêu thụ\": \"105W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen5_7600x.jpg', 0, 0),
('SP000008', 'CPU Intel Core i7-12700K', 12990000.00, 50, 'Intel', 'CPU Intel Core i7-12700K cân bằng giữa hiệu năng và tiết kiệm điện.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i7\", \"Số nhân\": \"12\", \"Số luồng\": \"20\", \"Xung nhịp\": \"3.6GHz\", \"Xung nhịp cơ bản\": \"3.2GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"25MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i7_12700k.jpg', 0, 0),
('SP000009', 'CPU AMD Ryzen 9 5900X', 13990000.00, 40, 'AMD', 'CPU AMD Ryzen 9 5900X phù hợp cho công việc đa nhiệm.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 9\", \"Số nhân\": \"12\", \"Số luồng\": \"24\", \"Xung nhịp\": \"3.7GHz\", \"Xung nhịp cơ bản\": \"3.4GHz\", \"Điện năng tiêu thụ\": \"105W\", \"Bộ nhớ đệm\": \"70MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen9_5900x.jpg', 0, 0),
('SP000010', 'CPU Intel Core i5-12600K', 10990000.00, 45, 'Intel', 'CPU Intel Core i5-12600K cho hiệu năng ổn định.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i5\", \"Số nhân\": \"10\", \"Số luồng\": \"16\", \"Xung nhịp\": \"3.7GHz\", \"Xung nhịp cơ bản\": \"3.5GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"16MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i5_12600k.jpg', 0, 0),
('SP000011', 'CPU AMD Ryzen 7 5800X', 8990000.00, 48, 'AMD', 'CPU AMD Ryzen 7 5800X cho gaming và đa nhiệm.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 7\", \"Số nhân\": \"8\", \"Số luồng\": \"16\", \"Xung nhịp\": \"3.8GHz\", \"Xung nhịp cơ bản\": \"3.6GHz\", \"Điện năng tiêu thụ\": \"105W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen7_5800x.jpg', 0, 0),
('SP000012', 'CPU Intel Core i9-12900K', 14990000.00, 30, 'Intel', 'CPU Intel Core i9-12900K cho hiệu năng đỉnh cao.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i9\", \"Số nhân\": \"16\", \"Số luồng\": \"24\", \"Xung nhịp\": \"3.2GHz\", \"Xung nhịp cơ bản\": \"3.0GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"30MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 770\"}', 'Components', 'cpu_i9_12900k.jpg', 0, 0),
('SP000013', 'CPU AMD Ryzen 5 5600X', 7490000.00, 60, 'AMD', 'CPU AMD Ryzen 5 5600X mang lại hiệu năng ổn định.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 5\", \"Số nhân\": \"6\", \"Số luồng\": \"12\", \"Xung nhịp\": \"3.7GHz\", \"Xung nhịp cơ bản\": \"3.5GHz\", \"Điện năng tiêu thụ\": \"65W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen5_5600x.jpg', 0, 0),
('SP000014', 'CPU Intel Core i5-12400', 6490000.00, 65, 'Intel', 'CPU Intel Core i5-12400 cho công việc văn phòng và giải trí.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1700\", \"Dòng CPU\": \"Intel Core i5\", \"Số nhân\": \"6\", \"Số luồng\": \"12\", \"Xung nhịp\": \"2.5GHz\", \"Xung nhịp cơ bản\": \"2.1GHz\", \"Điện năng tiêu thụ\": \"65W\", \"Bộ nhớ đệm\": \"12MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 730\"}', 'Components', 'cpu_i5_12400.jpg', 0, 0),
('SP000015', 'CPU AMD Ryzen 9 5950X', 15990000.00, 25, 'AMD', 'CPU AMD Ryzen 9 5950X dành cho xử lý đa luồng.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 9\", \"Số nhân\": \"16\", \"Số luồng\": \"32\", \"Xung nhịp\": \"3.4GHz\", \"Xung nhịp cơ bản\": \"3.0GHz\", \"Điện năng tiêu thụ\": \"105W\", \"Bộ nhớ đệm\": \"72MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen9_5950x.jpg', 0, 0),
('SP000016', 'CPU Intel Core i3-10100', 4490000.00, 70, 'Intel', 'CPU Intel Core i3-10100 cho nhu cầu cơ bản.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1200\", \"Dòng CPU\": \"Intel Core i3\", \"Số nhân\": \"4\", \"Số luồng\": \"8\", \"Xung nhịp\": \"3.6GHz\", \"Xung nhịp cơ bản\": \"3.3GHz\", \"Điện năng tiêu thụ\": \"65W\", \"Bộ nhớ đệm\": \"6MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 630\"}', 'Components', 'cpu_i3_10100.jpg', 0, 0),
('SP000017', 'CPU AMD Ryzen 7 3700X', 7490000.00, 55, 'AMD', 'CPU AMD Ryzen 7 3700X cho hiệu năng đa nhiệm.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 7\", \"Số nhân\": \"8\", \"Số luồng\": \"16\", \"Xung nhịp\": \"3.6GHz\", \"Xung nhịp cơ bản\": \"3.4GHz\", \"Điện năng tiêu thụ\": \"65W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen7_3700x.jpg', 0, 0),
('SP000018', 'CPU Intel Core i7-10700K', 8990000.00, 50, 'Intel', 'CPU Intel Core i7-10700K cho gaming hiệu năng cao.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1200\", \"Dòng CPU\": \"Intel Core i7\", \"Số nhân\": \"8\", \"Số luồng\": \"16\", \"Xung nhịp\": \"3.8GHz\", \"Xung nhịp cơ bản\": \"3.7GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"16MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 630\"}', 'Components', 'cpu_i7_10700k.jpg', 0, 0),
('SP000019', 'CPU AMD Ryzen 5 3600', 5490000.00, 60, 'AMD', 'CPU AMD Ryzen 5 3600 cho gaming và làm việc đa nhiệm.', '{\"Danh mục\": \"CPU\", \"Socket\": \"AM4\", \"Dòng CPU\": \"AMD Ryzen 5\", \"Số nhân\": \"6\", \"Số luồng\": \"12\", \"Xung nhịp\": \"3.6GHz\", \"Xung nhịp cơ bản\": \"3.4GHz\", \"Điện năng tiêu thụ\": \"65W\", \"Bộ nhớ đệm\": \"32MB\", \"Nhân đồ họa tích hợp\": \"Không\"}', 'Components', 'cpu_ryzen5_3600.jpg', 0, 0),
('SP000020', 'CPU Intel Core i5-10600K', 6290000.00, 65, 'Intel', 'CPU Intel Core i5-10600K cho hiệu năng gaming ổn định.', '{\"Danh mục\": \"CPU\", \"Socket\": \"LGA1200\", \"Dòng CPU\": \"Intel Core i5\", \"Số nhân\": \"6\", \"Số luồng\": \"12\", \"Xung nhịp\": \"4.1GHz\", \"Xung nhịp cơ bản\": \"4.0GHz\", \"Điện năng tiêu thụ\": \"125W\", \"Bộ nhớ đệm\": \"12MB\", \"Nhân đồ họa tích hợp\": \"Intel UHD Graphics 630\"}', 'Components', 'cpu_i5_10600k.jpg', 0, 0),
('SP000021', 'PC Build Thunder X', 34930000.00, 12, 'BPT', 'PC build hiệu năng cao với Intel Core i9-14900K, 32GB DDR5, RTX 3060 12GB, Model Thunder X.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"ASUS ROG STRIX Z790\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"NZXT H710\",\r\n      \"Tản nhiệt\": \"Corsair Hydro H100i\",\r\n      \"Quạt\": \"6 x 120mm\"\r\n  }', 'PC', 'pc_thunder_x.jpg', 11, 4),
('SP000022', 'PC Build Thunder X Pro', 35930000.00, 16, 'BPT', 'PC build mạnh mẽ với Intel Core i9-14900K, 32GB DDR5, RTX 3070 8GB, Model Thunder X Pro.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"MSI MEG X670E\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3070 8GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"Cooler Master MasterBox\",\r\n      \"Tản nhiệt\": \"NZXT Kraken X63\",\r\n      \"Quạt\": \"5 x 140mm\"\r\n  }', 'PC', 'pc_thunder_x_pro.jpg', 1, 0),
('SP000023', 'PC Build Lightning A', 36930000.00, 16, 'BPT', 'PC build cao cấp cho sáng tạo nội dung và gaming, Model Lightning A.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"Mainboard\": \"Gigabyte Z790 AORUS\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"1000W Platinum\",\r\n      \"Case\": \"Lian Li PC-O11 Dynamic\",\r\n      \"Tản nhiệt\": \"Noctua NH-D15\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_lightning_a.jpg', 0, 0),
('SP000024', 'PC Build Lightning B', 37930000.00, 16, 'BPT', 'PC build với bộ nhớ khủng 64GB DDR5, Model Lightning B.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"ASUS PRIME Z690\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"64GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"Fractal Design Define 7\",\r\n      \"Tản nhiệt\": \"Corsair Hydro H115\",\r\n      \"Quạt\": \"5 x 140mm\"\r\n  }', 'PC', 'pc_lightning_b.jpg', 0, 0),
('SP000025', 'PC Build Storm Alpha', 35530000.00, 16, 'BPT', 'PC build hiệu năng mạnh mẽ, lý tưởng cho game và đồ họa, Model Storm Alpha.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"MSI MAG Z690\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"750W Bronze\",\r\n      \"Case\": \"Cooler Master MasterBox TD500 Mesh\",\r\n      \"Tản nhiệt\": \"Cooler Master Hyper 212\",\r\n      \"Quạt\": \"3 x 120mm\"\r\n  }', 'PC', 'pc_storm_alpha.jpg', 0, 0),
('SP000026', 'PC Build Storm Beta', 35730000.00, 16, 'BPT', 'PC build với giải pháp tản nhiệt tiên tiến, Model Storm Beta.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"Mainboard\": \"Gigabyte B660\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"ASRock X570 Phantom\",\r\n      \"Tản nhiệt\": \"Deepcool Gammaxx\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_storm_beta.jpg', 0, 0),
('SP000027', 'PC Build Vortex', 36230000.00, 16, 'BPT', 'PC build mạnh mẽ cho gaming và sáng tạo nội dung, Model Vortex.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"ASRock X570 Taichi\",\r\n      \"CPU\": \"AMD Ryzen 9 5950X\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3070 8GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"750W Gold\",\r\n      \"Case\": \"Lian Li Lancool\",\r\n      \"Tản nhiệt\": \"NZXT Kraken Z73\",\r\n      \"Quạt\": \"5 x 140mm\"\r\n  }', 'PC', 'pc_vortex.jpg', 0, 0),
('SP000028', 'PC Build Vortex Plus', 37230000.00, 16, 'BPT', 'PC build với card đồ họa đỉnh cao, Model Vortex Plus.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"ASUS ROG STRIX Z790-E Gaming\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3080 10GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"1000W Platinum\",\r\n      \"Case\": \"Cooler Master HAF 700 EVO\",\r\n      \"Tản nhiệt\": \"Noctua NH-U12S\",\r\n      \"Quạt\": \"6 x 120mm\"\r\n  }', 'PC', 'pc_vortex_plus.jpg', 0, 0),
('SP000029', 'PC Build Nebula', 38230000.00, 16, 'BPT', 'PC build cho các tác vụ đồ họa nặng, Model Nebula.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"Mainboard\": \"MSI MEG X670E ACE\",\r\n      \"CPU\": \"AMD Ryzen 9 5950X\",\r\n      \"RAM\": \"64GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"1000W Gold\",\r\n      \"Case\": \"ASUS TUF Gaming\",\r\n      \"Tản nhiệt\": \"Corsair H150i\",\r\n      \"Quạt\": \"7 x 120mm\"\r\n  }', 'PC', 'pc_nebula.jpg', 0, 0),
('SP000030', 'PC Build Nebula Ultra', 39230000.00, 16, 'BPT', 'PC build cao cấp với hiệu năng vượt trội, Model Nebula Ultra.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"Mainboard\": \"ASRock X670E Taichi\",\r\n      \"CPU\": \"AMD Ryzen 9 5950X\",\r\n      \"RAM\": \"64GB DDR5\",\r\n      \"VGA\": \"RTX 3080 10GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"2TB NVMe\",\r\n      \"PSU\": \"1200W Platinum\",\r\n      \"Case\": \"Phanteks Enthoo Pro II\",\r\n      \"Tản nhiệt\": \"NZXT Kraken Z63\",\r\n      \"Quạt\": \"8 x 140mm\"\r\n  }', 'PC', 'pc_nebula_ultra.jpg', 0, 0),
('SP000031', 'PC Build Titan A', 35030000.00, 16, 'BPT', 'PC build mạnh mẽ, tối ưu cho game và làm việc, Model Titan A.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"Gigabyte B450M DS3H\",\r\n      \"CPU\": \"AMD Ryzen 5 5600X\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"GTX 1660 Super\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"PSU\": \"550W Bronze\",\r\n      \"Case\": \"Cooler Master MasterBox Q300L\",\r\n      \"Tản nhiệt\": \"Cooler Master Hyper 212\",\r\n      \"Quạt\": \"3 x 120mm\"\r\n  }', 'PC', 'pc_titan_a.jpg', 0, 0),
('SP000032', 'PC Build Titan B', 35930000.00, 16, 'BPT', 'PC build với hiệu năng vượt trội, Model Titan B.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"MSI PRO Z690-A\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3070 8GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"750W Gold\",\r\n      \"Case\": \"Corsair 4000D Airflow\",\r\n      \"Tản nhiệt\": \"NZXT Kraken X73\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_titan_b.jpg', 0, 0),
('SP000033', 'PC Build Titan X', 39930000.00, 16, 'BPT', 'PC build cao cấp dành cho game thủ chuyên nghiệp, Model Titan X.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"ASUS ROG CROSSHAIR\",\r\n      \"CPU\": \"AMD Ryzen 7 5800X\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3080 10GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"1000W Platinum\",\r\n      \"Case\": \"Fractal Design Define 7 XL\",\r\n      \"Tản nhiệt\": \"Corsair Hydro H1000\",\r\n      \"Quạt\": \"6 x 140mm\"\r\n  }', 'PC', 'pc_titan_x.jpg', 0, 0),
('SP000034', 'PC Build Quantum', 35130000.00, 16, 'BPT', 'PC build hiệu năng ổn định, Model Quantum.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"Mainboard\": \"Gigabyte Z590 AORUS ELITE AX\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"PSU\": \"650W Bronze\",\r\n      \"Case\": \"ASRock X570 Phantom\",\r\n      \"Tản nhiệt\": \"Deepcool Gammaxx\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_quantum.jpg', 0, 0),
('SP000035', 'PC Build Quantum Pro', 36130000.00, 16, 'BPT', 'PC build cao cấp với các thành phần hàng đầu, Model Quantum Pro.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"Mainboard\": \"MSI MAG Z790 TOMAHAWK WIFI\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3070 8GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"ASUS PRIME B550-PLUS\",\r\n      \"Tản nhiệt\": \"Cooler Master MasterLiquid\",\r\n      \"Quạt\": \"5 x 120mm\"\r\n  }', 'PC', 'pc_quantum_pro.jpg', 0, 0),
('SP000036', 'PC Build Apex', 35330000.00, 16, 'BPT', 'PC build đa năng, lý tưởng cho sáng tạo nội dung, Model Apex.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"Gigabyte Z690 AORUS\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"750W Gold\",\r\n      \"Case\": \"MSI PRO B660M-A\",\r\n      \"Tản nhiệt\": \"NZXT Kraken X63\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_apex.jpg', 0, 0),
('SP000037', 'PC Build Apex X', 36930000.00, 16, 'BPT', 'PC build với cấu hình khủng, Model Apex X.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"ASUS ROG STRIX Z790-E Gaming\",\r\n      \"CPU\": \"Intel Core i9-14900K\",\r\n      \"RAM\": \"64GB DDR5\",\r\n      \"VGA\": \"RTX 3080 10GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"1200W Platinum\",\r\n      \"Case\": \"Lian Li PC-O11 Dynamic\",\r\n      \"Tản nhiệt\": \"Corsair Hydro H150i\",\r\n      \"Quạt\": \"7 x 140mm\"\r\n  }', 'PC', 'pc_apex_x.jpg', 0, 0),
('SP000038', 'PC Build Fusion', 34530000.00, 16, 'BPT', 'PC build mạnh mẽ, thiết kế hiện đại, Model Fusion.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"Mainboard\": \"MSI MAG B760\",\r\n      \"CPU\": \"Intel Core i5-12600K\",\r\n      \"RAM\": \"16GB DDR5\",\r\n      \"VGA\": \"RTX 3060 12GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"PSU\": \"650W Bronze\",\r\n      \"Case\": \"NZXT H510\",\r\n      \"Tản nhiệt\": \"Cooler Master Hyper 212\",\r\n      \"Quạt\": \"3 x 120mm\"\r\n  }', 'PC', 'pc_fusion.jpg', 0, 0),
('SP000039', 'PC Build Fusion Plus', 35530000.00, 16, 'BPT', 'PC build với hiệu năng vượt trội, Model Fusion Plus.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"Mainboard\": \"Gigabyte Z590 AORUS ELITE\",\r\n      \"CPU\": \"Intel Core i7-12700K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3070 8GB\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"PSU\": \"750W Gold\",\r\n      \"Case\": \"MSI MAG Z790 TOMAHAWK WIFI\",\r\n      \"Tản nhiệt\": \"NZXT Kraken X73\",\r\n      \"Quạt\": \"5 x 120mm\"\r\n  }', 'PC', 'pc_fusion_plus.jpg', 0, 0),
('SP000040', 'PC Build Fusion Pro', 39530000.00, 16, 'BPT', 'PC build tối ưu cho công việc sáng tạo và gaming, Model Fusion Pro.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"Mainboard\": \"ASRock H510M-HDV\",\r\n      \"CPU\": \"Intel Core i7-10700K\",\r\n      \"RAM\": \"32GB DDR5\",\r\n      \"VGA\": \"RTX 3080 10GB\",\r\n      \"HDD\": \"2TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"PSU\": \"850W Gold\",\r\n      \"Case\": \"Corsair 4000D Airflow\",\r\n      \"Tản nhiệt\": \"Cooler Master MasterLiquid ML240L\",\r\n      \"Quạt\": \"4 x 120mm\"\r\n  }', 'PC', 'pc_fusion_pro.jpg', 0, 0),
('SP000042', 'Laptop ASUS ROG Strix G15', 26990000.00, 20, 'ASUS', 'Laptop ASUS ROG Strix G15 được thiết kế dành cho game thủ với hiệu năng ấn tượng.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.2\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.1\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"60Wh\",\r\n      \"Trọng lượng\": \"2.0kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"359 x 247 x 20 mm\"\r\n  }', 'Laptop', 'asus_rog_strix_g15.jpg', 0, 0),
('SP000043', 'Laptop Acer Predator Helios 300', 27990000.00, 18, 'Acer', 'Acer Predator Helios 300 mang đến hiệu năng mạnh mẽ cho game thủ.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB-C\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"70Wh\",\r\n      \"Trọng lượng\": \"2.5kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"360 x 250 x 22 mm\"\r\n  }', 'Laptop', 'acer_predator_helios_300.jpg', 0, 0),
('SP000044', 'Laptop Acer Nitro 5', 20990000.00, 30, 'Acer', 'Acer Nitro 5 là lựa chọn kinh tế cho game thủ với hiệu năng ổn định.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i5-12400H\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"GTX 1650\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 60Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.0\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.2\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"57Wh\",\r\n      \"Trọng lượng\": \"2.3kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"360 x 250 x 23 mm\"\r\n  }', 'Laptop', 'acer_nitro_5.jpg', 0, 0),
('SP000045', 'Laptop Dell G5 15', 25990000.00, 12, 'Dell', 'Dell G5 15 cung cấp hiệu năng vượt trội cho chơi game và sáng tạo.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.1\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"62Wh\",\r\n      \"Trọng lượng\": \"2.1kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"362 x 248 x 19 mm\"\r\n  }', 'Laptop', 'dell_g5_15.jpg', 0, 0),
('SP000046', 'Laptop Dell Alienware M15', 47990000.00, 5, 'Dell', 'Dell Alienware M15 cao cấp cho game thủ chuyên nghiệp.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i9-12900H\",\r\n      \"RAM\": \"32GB DDR4\",\r\n      \"VGA\": \"RTX 3080\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 240Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB-C\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.2\",\r\n      \"Webcam\": \"1080p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"86Wh\",\r\n      \"Trọng lượng\": \"2.8kg\",\r\n      \"Màu sắc\": \"Bạc\",\r\n      \"Kích thước\": \"370 x 260 x 22 mm\"\r\n  }', 'Laptop', 'dell_alienware_m15.jpg', 0, 0),
('SP000047', 'Laptop HP Omen 15', 26990000.00, 14, 'HP', 'HP Omen 15 thiết kế cho game thủ và sáng tạo với hiệu năng mạnh mẽ.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.0\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"70Wh\",\r\n      \"Trọng lượng\": \"2.2kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"365 x 255 x 21 mm\"\r\n  }', 'Laptop', 'hp_omen_15.jpg', 0, 0),
('SP000048', 'Laptop HP Pavilion Gaming', 21990000.00, 20, 'HP', 'HP Pavilion Gaming cung cấp hiệu năng ổn định cho game thủ mới bắt đầu.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i5-12400H\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"GTX 1650\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 60Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.0\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.2\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"50Wh\",\r\n      \"Trọng lượng\": \"2.0kg\",\r\n      \"Màu sắc\": \"Xám\",\r\n      \"Kích thước\": \"360 x 245 x 22 mm\"\r\n  }', 'Laptop', 'hp_pavilion_gaming.jpg', 0, 0),
('SP000049', 'Laptop Lenovo Legion 5', 25990000.00, 16, 'Lenovo', 'Lenovo Legion 5 kết hợp CPU AMD Ryzen 7, 16GB RAM và card đồ họa mạnh mẽ.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"AMD Ryzen 7 5800H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB-C\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"70Wh\",\r\n      \"Trọng lượng\": \"2.3kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"370 x 255 x 22 mm\"\r\n  }', 'Laptop', 'lenovo_legion_5.jpg', 0, 0),
('SP000050', 'Laptop Lenovo IdeaPad Gaming 3', 20990000.00, 22, 'Lenovo', 'Lenovo IdeaPad Gaming 3 là lựa chọn kinh tế cho học sinh và sinh viên.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"CPU\": \"AMD Ryzen 5 5600H\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"GTX 1650\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 60Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.0\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.2\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"45Wh\",\r\n      \"Trọng lượng\": \"1.9kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"355 x 240 x 20 mm\"\r\n  }', 'Laptop', 'lenovo_ideapad_gaming_3.jpg', 0, 0),
('SP000051', 'Laptop Razer Blade 15', 39990000.00, 8, 'Razer', 'Razer Blade 15 nổi tiếng với thiết kế mỏng nhẹ và hiệu năng cao cấp.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3070\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"Thunderbolt 4, USB-C\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.1\",\r\n      \"Webcam\": \"1080p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"80Wh\",\r\n      \"Trọng lượng\": \"2.0kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"362 x 247 x 19 mm\"\r\n  }', 'Laptop', 'razer_blade_15.jpg', 0, 0),
('SP000052', 'Laptop Razer Blade Stealth 13', 28990000.00, 10, 'Razer', 'Razer Blade Stealth 13 là ultrabook nhỏ gọn cho công việc sáng tạo.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"CPU\": \"Intel Core i5-1135G7\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"Intel Iris Xe\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"13.3 inch FHD\",\r\n      \"Cổng giao tiếp\": \"USB-C, HDMI\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"50Wh\",\r\n      \"Trọng lượng\": \"1.3kg\",\r\n      \"Màu sắc\": \"Bạc\",\r\n      \"Kích thước\": \"310 x 210 x 15 mm\"\r\n  }', 'Laptop', 'razer_blade_stealth_13.jpg', 0, 0),
('SP000053', 'Laptop Gigabyte Aorus 15G', 26990000.00, 15, 'Gigabyte', 'Laptop Gigabyte Aorus 15G được xây dựng cho game thủ với hiệu năng ổn định.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3060\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"USB-C, HDMI\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"70Wh\",\r\n      \"Trọng lượng\": \"2.0kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"360 x 250 x 22 mm\"\r\n  }', 'Laptop', 'gigabyte_aorus_15g.jpg', 0, 0),
('SP000054', 'Laptop Gigabyte Aero 15', 48990000.00, 4, 'Gigabyte', 'Laptop Gigabyte Aero 15 hướng tới chuyên gia sáng tạo với hiệu năng đồ họa cao.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"CPU\": \"Intel Core i9-12900H\",\r\n      \"RAM\": \"32GB DDR4\",\r\n      \"VGA\": \"RTX 3080\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 240Hz\",\r\n      \"Cổng giao tiếp\": \"Thunderbolt 4, USB-C\",\r\n      \"Bàn phím\": \"RGB Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.1\",\r\n      \"Webcam\": \"1080p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"90Wh\",\r\n      \"Trọng lượng\": \"2.5kg\",\r\n      \"Màu sắc\": \"Xám\",\r\n      \"Kích thước\": \"370 x 260 x 23 mm\"\r\n  }', 'Laptop', 'gigabyte_aero_15.jpg', 0, 0),
('SP000055', 'Laptop Microsoft Surface Laptop 4', 27990000.00, 12, 'Microsoft', 'Microsoft Surface Laptop 4 nổi bật với thiết kế tinh tế và hiệu năng ổn định.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"CPU\": \"Intel Core i7-1165G7\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"Intel Iris Xe\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"13.5 inch PixelSense\",\r\n      \"Cổng giao tiếp\": \"USB-C, Surface Connect\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"47Wh\",\r\n      \"Trọng lượng\": \"1.3kg\",\r\n      \"Màu sắc\": \"Bạc\",\r\n      \"Kích thước\": \"312 x 226 x 14 mm\"\r\n  }', 'Laptop', 'microsoft_surface_laptop_4.jpg', 0, 0),
('SP000056', 'Laptop Microsoft Surface Book 3', 31990000.00, 8, 'Microsoft', 'Microsoft Surface Book 3 có thiết kế độc đáo với màn hình tách rời.', '{\r\n      \"Nhu cầu\": \"Workstation\",\r\n      \"CPU\": \"Intel Core i7-1065G7\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"NVIDIA GTX 1660 Ti\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"13.5 inch PixelSense\",\r\n      \"Cổng giao tiếp\": \"USB-C, Surface Connect\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.2\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"62Wh\",\r\n      \"Trọng lượng\": \"1.6kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"320 x 230 x 16 mm\"\r\n  }', 'Laptop', 'microsoft_surface_book_3.jpg', 0, 0),
('SP000057', 'Laptop Samsung Galaxy Book Odyssey', 26990000.00, 10, 'Samsung', 'Samsung Galaxy Book Odyssey kết hợp thiết kế hiện đại với hiệu năng mạnh mẽ.', '{\r\n      \"Nhu cầu\": \"Graphics\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3050\",\r\n      \"HDD\": \"512GB\",\r\n      \"SSD\": \"512GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"USB-C, HDMI\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"57Wh\",\r\n      \"Trọng lượng\": \"1.8kg\",\r\n      \"Màu sắc\": \"Xám\",\r\n      \"Kích thước\": \"360 x 245 x 20 mm\"\r\n  }', 'Laptop', 'samsung_galaxy_book_odyssey.jpg', 0, 0),
('SP000058', 'Laptop Samsung Notebook 9 Pro', 22990000.00, 15, 'Samsung', 'Samsung Notebook 9 Pro là ultrabook nhẹ nhàng với hiệu năng ổn định.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"CPU\": \"Intel Core i5-1135G7\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"NVIDIA MX250\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD\",\r\n      \"Cổng giao tiếp\": \"USB-C, HDMI\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"52Wh\",\r\n      \"Trọng lượng\": \"1.5kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"350 x 240 x 18 mm\"\r\n  }', 'Laptop', 'samsung_notebook_9_pro.jpg', 0, 0),
('SP000059', 'Laptop ASUS TUF Gaming F15', 21990000.00, 25, 'ASUS', 'ASUS TUF Gaming F15 mang lại sự cân bằng giữa hiệu năng và giá thành.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i5-12400H\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"NVIDIA GTX 1650\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 60Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB 3.0\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 5\",\r\n      \"Bluetooth\": \"4.2\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 10\",\r\n      \"Pin\": \"45Wh\",\r\n      \"Trọng lượng\": \"2.0kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"360 x 250 x 22 mm\"\r\n  }', 'Laptop', 'asus_tuf_gaming_f15.jpg', 0, 0),
('SP000060', 'Laptop Acer Swift 3', 18990000.00, 30, 'Acer', 'Laptop Acer Swift 3 là lựa chọn tuyệt vời cho công việc văn phòng và du lịch.', '{\r\n      \"Nhu cầu\": \"Office\",\r\n      \"CPU\": \"Intel Core i5-1135G7\",\r\n      \"RAM\": \"8GB DDR4\",\r\n      \"VGA\": \"Intel Iris Xe\",\r\n      \"HDD\": \"256GB\",\r\n      \"SSD\": \"256GB NVMe\",\r\n      \"Màn hình\": \"14 inch FHD\",\r\n      \"Cổng giao tiếp\": \"USB-C, HDMI\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"48Wh\",\r\n      \"Trọng lượng\": \"1.2kg\",\r\n      \"Màu sắc\": \"Xám\",\r\n      \"Kích thước\": \"320 x 220 x 15 mm\"\r\n  }', 'Laptop', 'acer_swift_3.jpg', 0, 0),
('SP000061', 'VGA ASUS TUF RTX 4060 Ti 8GB', 12900000.00, 20, 'Asus', 'VGA ASUS TUF RTX 4060 Ti – tối ưu hiệu năng gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2535MHz\", \"Nhân CUDA\": \"2304\", \"Bộ nhớ\": \"8GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"200W\"}', 'Components', 'vga_asus_tuf_4060ti.jpg', 0, 0),
('SP000062', 'VGA ASUS ROG STRIX RTX 4070 12GB', 18700000.00, 15, 'Asus', 'VGA ASUS ROG STRIX RTX 4070 – hiệu năng cao cấp cho gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2610MHz\", \"Nhân CUDA\": \"5888\", \"Bộ nhớ\": \"12GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"750W\", \"Điện năng tiêu thụ\": \"250W\"}', 'Components', 'vga_asus_rog_4070.jpg', 0, 0),
('SP000063', 'VGA MSI GAMING X RTX 4070 Ti 12GB', 22800000.00, 12, 'MSI', 'VGA MSI GAMING X RTX 4070 Ti – hiệu năng đỉnh cao.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2730MHz\", \"Nhân CUDA\": \"7680\", \"Bộ nhớ\": \"12GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"750W\", \"Điện năng tiêu thụ\": \"300W\"}', 'Components', 'vga_msi_4070ti.jpg', 0, 0),
('SP000064', 'VGA Gigabyte AORUS RTX 4080 16GB', 33500000.00, 10, 'Gigabyte', 'Gigabyte AORUS RTX 4080 – card đồ họa cao cấp.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2505MHz\", \"Nhân CUDA\": \"9728\", \"Bộ nhớ\": \"16GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"850W\", \"Điện năng tiêu thụ\": \"320W\"}', 'Components', 'vga_giga_4080.jpg', 0, 0),
('SP000065', 'VGA ASUS TUF RTX 4090 24GB', 48500000.00, 7, 'Asus', 'ASUS TUF RTX 4090 – đỉnh cao card đồ họa cho AI & gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2520MHz\", \"Nhân CUDA\": \"16384\", \"Bộ nhớ\": \"24GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"1000W\", \"Điện năng tiêu thụ\": \"450W\"}', 'Components', 'vga_asus_4090.jpg', 0, 0),
('SP000066', 'VGA MSI RTX 4060 Gaming X 8GB', 10800000.00, 25, 'MSI', 'MSI RTX 4060 Gaming X – lựa chọn tuyệt vời cho gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2670MHz\", \"Nhân CUDA\": \"2304\", \"Bộ nhớ\": \"8GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"190W\"}', 'Components', 'vga_msi_4060.jpg', 0, 0),
('SP000067', 'VGA Zotac RTX 4070 AMP AIRO 12GB', 17900000.00, 13, 'Zotac', 'Zotac RTX 4070 AMP AIRO – thiết kế tản nhiệt ưu việt.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2520MHz\", \"Nhân CUDA\": \"5888\", \"Bộ nhớ\": \"12GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"750W\", \"Điện năng tiêu thụ\": \"240W\"}', 'Components', 'vga_zotac_4070.jpg', 0, 0),
('SP000068', 'VGA Palit RTX 4080 GameRock 16GB', 32900000.00, 9, 'Palit', 'Palit RTX 4080 GameRock – hiệu năng khủng cho gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2610MHz\", \"Nhân CUDA\": \"9728\", \"Bộ nhớ\": \"16GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"850W\", \"Điện năng tiêu thụ\": \"320W\"}', 'Components', 'vga_palit_4080.jpg', 0, 0),
('SP000069', 'VGA GALAX RTX 4060 EX 8GB', 10500000.00, 30, 'GALAX', 'GALAX RTX 4060 EX – card đồ họa phổ thông hiệu năng tốt.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2670MHz\", \"Nhân CUDA\": \"2304\", \"Bộ nhớ\": \"8GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"180W\"}', 'Components', 'vga_galax_4060.jpg', 0, 0),
('SP000070', 'VGA AMD Radeon RX 6700 XT 12GB', 12900000.00, 22, 'AMD', 'AMD RX 6700 XT – hiệu năng mạnh mẽ cho gaming 2K.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2581MHz\", \"Nhân CUDA\": \"\", \"Bộ nhớ\": \"12GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"230W\"}', 'Components', 'vga_amd_6700xt.jpg', 0, 0),
('SP000071', 'VGA AMD Radeon RX 7900 XTX 24GB', 28500000.00, 8, 'AMD', 'AMD RX 7900 XTX – card đồ họa đầu bảng của AMD.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2500MHz\", \"Nhân CUDA\": \"\", \"Bộ nhớ\": \"24GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"850W\", \"Điện năng tiêu thụ\": \"300W\"}', 'Components', 'vga_amd_7900xtx.jpg', 0, 0),
('SP000072', 'VGA Intel Arc A770 16GB', 10200000.00, 20, 'Intel', 'Intel Arc A770 – card đồ họa gaming từ Intel.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2400MHz\", \"Nhân CUDA\": \"\", \"Bộ nhớ\": \"16GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"600W\", \"Điện năng tiêu thụ\": \"210W\"}', 'Components', 'vga_intel_a770.jpg', 0, 0),
('SP000073', 'VGA Intel Arc A750 8GB', 8600000.00, 28, 'Intel', 'Intel Arc A750 – hiệu năng tốt với giá phải chăng.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2050MHz\", \"Nhân CUDA\": \"\", \"Bộ nhớ\": \"8GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"550W\", \"Điện năng tiêu thụ\": \"150W\"}', 'Components', 'vga_intel_a750.jpg', 0, 0),
('SP000074', 'VGA MSI RTX 3050 Ventus 8GB', 7500000.00, 35, 'MSI', 'MSI RTX 3050 Ventus – lựa chọn tốt cho gaming phổ thông.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"1777MHz\", \"Nhân CUDA\": \"2048\", \"Bộ nhớ\": \"8GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"500W\", \"Điện năng tiêu thụ\": \"120W\"}', 'Components', 'vga_msi_3050.jpg', 0, 0),
('SP000075', 'VGA Gigabyte RTX 3060 Gaming OC 12GB', 9500000.00, 27, 'Gigabyte', 'Gigabyte RTX 3060 Gaming OC – tối ưu cho gaming.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"1837MHz\", \"Nhân CUDA\": \"3584\", \"Bộ nhớ\": \"12GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"170W\"}', 'Components', 'vga_giga_3060.jpg', 0, 0),
('SP000076', 'VGA Zotac RTX 3080 Trinity 10GB', 18500000.00, 11, 'Zotac', 'Zotac RTX 3080 Trinity – hiệu suất mạnh mẽ, giá hợp lý.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"1710MHz\", \"Nhân CUDA\": \"8704\", \"Bộ nhớ\": \"10GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"750W\", \"Điện năng tiêu thụ\": \"320W\"}', 'Components', 'vga_zotac_3080.jpg', 0, 0),
('SP000077', 'VGA AMD Radeon RX 6800 XT 16GB', 16700000.00, 14, 'AMD', 'AMD RX 6800 XT – hiệu năng ấn tượng cho gaming 4K.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2250MHz\", \"Nhân CUDA\": \"\", \"Bộ nhớ\": \"16GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"750W\", \"Điện năng tiêu thụ\": \"250W\"}', 'Components', 'vga_amd_6800xt.jpg', 0, 0),
('SP000078', 'VGA ASUS Dual RTX 3060 12GB', 9200000.00, 26, 'Asus', 'ASUS Dual RTX 3060 – phù hợp cho gaming & đồ họa.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"1777MHz\", \"Nhân CUDA\": \"3584\", \"Bộ nhớ\": \"12GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"650W\", \"Điện năng tiêu thụ\": \"150W\"}', 'Components', 'vga_asus_3060.jpg', 0, 0),
('SP000079', 'VGA MSI RTX 4090 SUPRIM X 24GB', 49800000.00, 5, 'MSI', 'MSI RTX 4090 SUPRIM X – đỉnh cao card đồ họa.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"2625MHz\", \"Nhân CUDA\": \"16384\", \"Bộ nhớ\": \"24GB GDDR6X\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"1200W\", \"Điện năng tiêu thụ\": \"450W\"}', 'Components', 'vga_msi_4090.jpg', 0, 0),
('SP000080', 'VGA NVIDIA Titan RTX 24GB', 62000000.00, 3, 'NVIDIA', 'NVIDIA Titan RTX – chuyên biệt cho AI & Deep Learning.', '{\"Danh mục\": \"VGA\", \"Xung nhịp\": \"1770MHz\", \"Nhân CUDA\": \"4608\", \"Bộ nhớ\": \"24GB GDDR6\", \"Cổng kết nối\": \"HDMI, DisplayPort\", \"Nguồn khuyến nghị\": \"850W\", \"Điện năng tiêu thụ\": \"280W\"}', 'Components', 'vga_nvidia_titan.jpg', 0, 0),
('SP000081', 'Mainboard ASUS TUF B760M-PLUS D4', 3700000.00, 40, 'ASUS', 'Mainboard ASUS TUF B760M-PLUS D4 – ổn định cho hệ thống Intel.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asus_b760m.jpg', 0, 0),
('SP000082', 'Mainboard ASUS ROG STRIX Z790-E Gaming', 8990000.00, 25, 'ASUS', 'Mainboard cao cấp ASUS ROG STRIX Z790-E Gaming cho gaming.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asus_z790.jpg', 0, 0),
('SP000083', 'Mainboard MSI MAG B760 TOMAHAWK WIFI', 4200000.00, 38, 'MSI', 'Mainboard MSI MAG B760 TOMAHAWK WIFI hỗ trợ Intel Gen 13 & 14.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_msi_b760.jpg', 0, 0),
('SP000084', 'Mainboard MSI PRO B660M-A DDR4', 3100000.00, 50, 'MSI', 'Mainboard MSI PRO B660M-A – lựa chọn cho hệ thống Intel tầm trung.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"2\"}', 'Components', 'mainboard_msi_b660m.jpg', 0, 0),
('SP000085', 'Mainboard Gigabyte B760 AORUS ELITE AX', 4600000.00, 30, 'Gigabyte', 'Mainboard Gigabyte B760 AORUS ELITE AX – gaming đáng tin cậy.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_giga_b760.jpg', 0, 0),
('SP000086', 'Mainboard Gigabyte Z790 AORUS MASTER', 10990000.00, 15, 'Gigabyte', 'Mainboard cao cấp Gigabyte Z790 AORUS MASTER.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_giga_z790.jpg', 0, 0),
('SP000087', 'Mainboard ASRock B550M Steel Legend', 2950000.00, 55, 'ASRock', 'Mainboard ASRock B550M Steel Legend – cho hệ thống Ryzen.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM4\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asrock_b550m.jpg', 0, 0),
('SP000088', 'Mainboard ASRock X670E Taichi', 12990000.00, 12, 'ASRock', 'Mainboard cao cấp ASRock X670E Taichi cho Ryzen 7000.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM5\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asrock_x670e.jpg', 0, 0),
('SP000089', 'Mainboard ASUS PRIME H610M-K D4', 1950000.00, 60, 'ASUS', 'Mainboard ASUS PRIME H610M-K – giá rẻ, ổn định cho hệ thống Intel.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"2\"}', 'Components', 'mainboard_asus_h610m.jpg', 0, 0),
('SP000090', 'Mainboard ASUS PRO WS WRX80E-SAGE SE WIFI', 22500000.00, 5, 'ASUS', 'Mainboard workstation cao cấp ASUS PRO WS WRX80E-SAGE SE WIFI.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"sWRX8\", \"Kích thước\": \"E-ATX\", \"Số khe RAM\": \"8\"}', 'Components', 'mainboard_asus_wrx80.jpg', 0, 0),
('SP000091', 'Mainboard MSI MEG X670E ACE', 15900000.00, 10, 'MSI', 'Mainboard MSI MEG X670E ACE – tối ưu cho Ryzen 7000.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM5\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_msi_x670e.jpg', 0, 0),
('SP000092', 'Mainboard Gigabyte B450M DS3H', 1800000.00, 75, 'Gigabyte', 'Mainboard Gigabyte B450M DS3H – giá rẻ nhưng ổn định.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM4\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"2\"}', 'Components', 'mainboard_giga_b450m.jpg', 0, 0),
('SP000093', 'Mainboard ASRock H510M-HDV', 1750000.00, 80, 'ASRock', 'Mainboard ASRock H510M-HDV – phù hợp cho build văn phòng.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1200\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"2\"}', 'Components', 'mainboard_asrock_h510m.jpg', 0, 0),
('SP000094', 'Mainboard ASUS ROG CROSSHAIR X670E EXTREME', 19990000.00, 6, 'ASUS', 'Mainboard cao cấp ASUS ROG CROSSHAIR X670E EXTREME.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM5\", \"Kích thước\": \"E-ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asus_x670e.jpg', 0, 0),
('SP000095', 'Mainboard MSI PRO Z690-A DDR5', 4890000.00, 35, 'MSI', 'Mainboard MSI PRO Z690-A – hiệu năng cao với RAM DDR5.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_msi_z690.jpg', 0, 0),
('SP000096', 'Mainboard ASUS TUF Gaming B550M-PLUS', 2850000.00, 50, 'ASUS', 'Mainboard ASUS TUF Gaming B550M-PLUS – cho hệ thống Ryzen.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM4\", \"Kích thước\": \"Micro-ATX\", \"Số khe RAM\": \"2\"}', 'Components', 'mainboard_asus_b550m.jpg', 0, 0);
INSERT INTO `sanpham` (`IdSp`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000097', 'Mainboard Gigabyte Z590 AORUS ELITE AX', 5990000.00, 28, 'Gigabyte', 'Mainboard Gigabyte Z590 AORUS ELITE AX – cho build Intel.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1200\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_giga_z590.jpg', 0, 0),
('SP000098', 'Mainboard ASRock X570 Phantom Gaming 4', 3790000.00, 40, 'ASRock', 'Mainboard ASRock X570 Phantom Gaming 4 – cho build gaming.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM4\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asrock_x570.jpg', 0, 0),
('SP000099', 'Mainboard MSI MAG Z790 TOMAHAWK WIFI', 8990000.00, 18, 'MSI', 'Mainboard MSI MAG Z790 TOMAHAWK WIFI – hiệu năng mạnh mẽ.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"LGA1700\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_msi_z790.jpg', 0, 0),
('SP000100', 'Mainboard ASUS PRIME B550-PLUS', 2890000.00, 45, 'ASUS', 'Mainboard ASUS PRIME B550-PLUS – ổn định cho Ryzen.', '{\"Danh mục\": \"Mainboard\", \"Socket hỗ trợ\": \"AM4\", \"Kích thước\": \"ATX\", \"Số khe RAM\": \"4\"}', 'Components', 'mainboard_asus_b550.jpg', 0, 0),
('SP000101', 'RAM Corsair Vengeance RGB 32GB DDR5 6000MHz', 3200000.00, 50, 'Corsair', 'RAM Corsair Vengeance RGB 32GB DDR5 – hiệu năng cao với đèn LED RGB.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_corsair_32gb.jpg', 0, 0),
('SP000102', 'RAM Corsair Dominator Platinum 64GB DDR5 6600MHz', 7800000.00, 40, 'Corsair', 'RAM Corsair Dominator Platinum 64GB DDR5 – hiệu năng cho chuyên gia.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6600MHz\"}', 'Components', 'ram_corsair_dominator_64gb.jpg', 0, 0),
('SP000103', 'RAM G.Skill Trident Z5 RGB 32GB DDR5 6400MHz', 4100000.00, 55, 'G.Skill', 'RAM G.Skill Trident Z5 RGB 32GB DDR5 – phù hợp cho gaming.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_gskill_trident_32gb.jpg', 0, 0),
('SP000104', 'RAM G.Skill Ripjaws S5 64GB DDR5 6000MHz', 7100000.00, 35, 'G.Skill', 'RAM G.Skill Ripjaws S5 64GB DDR5 – tối ưu cho hiệu năng.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_gskill_ripjaws_64gb.jpg', 0, 0),
('SP000105', 'RAM Kingston Fury Beast 16GB DDR5 5200MHz', 2100000.00, 70, 'Kingston', 'RAM Kingston Fury Beast 16GB DDR5 – ổn định và nhanh.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"5200MHz\"}', 'Components', 'ram_kingston_fury_16gb.jpg', 0, 0),
('SP000106', 'RAM Kingston Fury Renegade 32GB DDR5 7200MHz', 8200000.00, 25, 'Kingston', 'RAM Kingston Fury Renegade 32GB DDR5 – tốc độ cực cao.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"7200MHz\"}', 'Components', 'ram_kingston_renegade_32gb.jpg', 0, 0),
('SP000107', 'RAM TeamGroup T-Force Delta RGB 64GB DDR5 6200MHz', 6990000.00, 30, 'TeamGroup', 'RAM TeamGroup T-Force Delta RGB 64GB DDR5 – hiệu năng và ánh sáng RGB.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6200MHz\"}', 'Components', 'ram_teamgroup_delta_64gb.jpg', 0, 0),
('SP000108', 'RAM TeamGroup Vulcan 32GB DDR5 6000MHz', 3600000.00, 40, 'TeamGroup', 'RAM TeamGroup Vulcan 32GB DDR5 – lựa chọn hợp lý.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_teamgroup_vulcan_32gb.jpg', 0, 0),
('SP000109', 'RAM Crucial DDR5 32GB 5600MHz', 3100000.00, 60, 'Crucial', 'RAM Crucial DDR5 32GB – hiệu năng ổn định.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_crucial_32gb.jpg', 0, 0),
('SP000110', 'RAM Crucial Ballistix 64GB DDR5 6200MHz', 6800000.00, 30, 'Crucial', 'RAM Crucial Ballistix 64GB DDR5 – dành cho gaming chuyên dụng.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6200MHz\"}', 'Components', 'ram_crucial_ballistix_64gb.jpg', 0, 0),
('SP000111', 'RAM Patriot Viper Venom RGB 32GB DDR5 6400MHz', 4200000.00, 40, 'Patriot', 'RAM Patriot Viper Venom RGB 32GB DDR5 – chất lượng và hiệu năng.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_patriot_viper_32gb.jpg', 0, 0),
('SP000112', 'RAM Patriot Signature Line 16GB DDR5 4800MHz', 1800000.00, 75, 'Patriot', 'RAM Patriot Signature Line 16GB DDR5 – giá tốt, hiệu năng ổn định.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"4800MHz\"}', 'Components', 'ram_patriot_signature_16gb.jpg', 0, 0),
('SP000113', 'RAM Samsung DDR5 32GB 5600MHz', 2950000.00, 50, 'Samsung', 'RAM Samsung DDR5 32GB – hiệu năng cao và đáng tin cậy.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_samsung_32gb.jpg', 0, 0),
('SP000114', 'RAM Samsung DDR5 64GB 6400MHz', 6900000.00, 35, 'Samsung', 'RAM Samsung DDR5 64GB – cho workstation hiệu năng cao.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_samsung_64gb.jpg', 0, 0),
('SP000115', 'RAM ADATA XPG Lancer RGB 32GB DDR5 6000MHz', 3900000.00, 45, 'ADATA', 'RAM ADATA XPG Lancer RGB 32GB DDR5 – dành cho game thủ.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6000MHz\"}', 'Components', 'ram_adata_xpg_32gb.jpg', 0, 0),
('SP000116', 'RAM ADATA XPG Lancer RGB 64GB DDR5 6800MHz', 7500000.00, 28, 'ADATA', 'RAM ADATA XPG Lancer RGB 64GB DDR5 – hiệu năng vượt trội.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6800MHz\"}', 'Components', 'ram_adata_xpg_64gb.jpg', 0, 0),
('SP000117', 'RAM Lexar ARES RGB 32GB DDR5 5600MHz', 3300000.00, 55, 'Lexar', 'RAM Lexar ARES RGB 32GB DDR5 – thiết kế đẹp mắt.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"32GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"5600MHz\"}', 'Components', 'ram_lexar_ares_32gb.jpg', 0, 0),
('SP000118', 'RAM Lexar ARES RGB 64GB DDR5 6400MHz', 6900000.00, 30, 'Lexar', 'RAM Lexar ARES RGB 64GB DDR5 – cho game và workstation.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"64GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"6400MHz\"}', 'Components', 'ram_lexar_ares_64gb.jpg', 0, 0),
('SP000119', 'RAM Corsair Vengeance LPX 16GB DDR5 5200MHz', 2000000.00, 65, 'Corsair', 'RAM Corsair Vengeance LPX 16GB – giá tốt, hiệu năng ổn định.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"16GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"5200MHz\"}', 'Components', 'ram_corsair_lpx_16gb.jpg', 0, 0),
('SP000120', 'RAM Corsair Dominator Platinum 128GB DDR5 7200MHz', 15900000.00, 10, 'Corsair', 'RAM Corsair Dominator Platinum 128GB DDR5 – hiệu năng cao cấp nhất.', '{\"Danh mục\": \"RAM\", \"Dung lượng\": \"128GB\", \"Chuẩn RAM\": \"DDR5\", \"Tốc độ\": \"7200MHz\"}', 'Components', 'ram_corsair_dominator_128gb.jpg', 0, 0),
('SP000121', 'PSU Cooler Master MWE 850 GOLD V2', 2290000.00, 15, 'Cooler Master', 'Nguồn 850W Gold với hiệu năng vượt trội.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_cooler_850w.jpg', 0, 0),
('SP000122', 'PSU Corsair RM750x 80 Plus Gold', 2790000.00, 18, 'Corsair', 'PSU 750W Gold ổn định cho hệ thống mạnh.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"750W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_corsair_rm750x.jpg', 0, 0),
('SP000123', 'PSU Corsair RM1000x 80 Plus Gold', 3590000.00, 10, 'Corsair', 'PSU 1000W chuẩn Gold cho build cao cấp.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_corsair_rm1000x.jpg', 0, 0),
('SP000124', 'PSU EVGA SuperNOVA 850 G5 80 Plus Gold', 2890000.00, 12, 'EVGA', 'Nguồn EVGA 850W hiệu suất cao, chuẩn Gold.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_evga_850w.jpg', 0, 0),
('SP000125', 'PSU EVGA SuperNOVA 1000 G5 80 Plus Gold', 3990000.00, 8, 'EVGA', 'Nguồn EVGA 1000W Gold – ổn định và bền bỉ.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_evga_1000w.jpg', 0, 0),
('SP000126', 'PSU Gigabyte P750GM 750W 80 Plus Gold', 1990000.00, 25, 'Gigabyte', 'PSU Gigabyte 750W Gold cho hệ thống ổn định.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"750W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_gigabyte_750w.jpg', 0, 0),
('SP000127', 'PSU Gigabyte AORUS P1000W 80 Plus Platinum', 4390000.00, 6, 'Gigabyte', 'Nguồn 1000W Platinum dành cho build cao cấp.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_gigabyte_1000w.jpg', 0, 0),
('SP000128', 'PSU ASUS ROG Strix 850W 80 Plus Gold', 3190000.00, 10, 'ASUS', 'Nguồn ASUS ROG Strix 850W – đáng tin cậy cho gaming.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_asus_850w.jpg', 0, 0),
('SP000129', 'PSU ASUS ROG Thor 1200W 80 Plus Platinum', 5990000.00, 5, 'ASUS', 'Nguồn ASUS ROG Thor 1200W – đỉnh cao công suất.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1200W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_asus_1200w.jpg', 0, 0),
('SP000130', 'PSU MSI MAG A650BN 650W 80 Plus Bronze', 1490000.00, 30, 'MSI', 'PSU MSI MAG A650BN – nguồn 650W chuẩn Bronze.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"650W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_msi_650w.jpg', 0, 0),
('SP000131', 'PSU MSI MPG A850GF 850W 80 Plus Gold', 2890000.00, 15, 'MSI', 'Nguồn MSI MPG A850GF 850W – hiệu năng cao.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_msi_850w.jpg', 0, 0),
('SP000132', 'PSU Thermaltake Toughpower GF1 750W 80 Plus Gold', 2590000.00, 18, 'Thermaltake', 'PSU Thermaltake Toughpower GF1 750W – hiệu năng gaming.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"750W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_thermaltake_750w.jpg', 0, 0),
('SP000133', 'PSU Thermaltake Toughpower PF1 1200W 80 Plus Platinum', 5290000.00, 7, 'Thermaltake', 'PSU Thermaltake Toughpower PF1 1200W – nguồn Platinum cao cấp.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1200W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_thermaltake_1200w.jpg', 0, 0),
('SP000134', 'PSU Antec NeoECO Gold 750W 80 Plus Gold', 2290000.00, 20, 'Antec', 'Nguồn Antec NeoECO Gold 750W – ổn định cho gaming.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"750W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_antec_750w.jpg', 0, 0),
('SP000135', 'PSU Antec HCG 1000W 80 Plus Platinum', 4790000.00, 9, 'Antec', 'Nguồn Antec HCG 1000W Platinum – hiệu suất cao.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_antec_1000w.jpg', 0, 0),
('SP000136', 'PSU Deepcool PQ1000M 1000W 80 Plus Platinum', 4590000.00, 6, 'Deepcool', 'PSU Deepcool PQ1000M – nguồn 1000W Platinum.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_deepcool_1000w.jpg', 0, 0),
('SP000137', 'PSU Seasonic Focus GX-850 850W 80 Plus Gold', 3190000.00, 12, 'Seasonic', 'Nguồn Seasonic Focus GX-850 850W – hiệu năng ổn định.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_seasonic_850w.jpg', 0, 0),
('SP000138', 'PSU Seasonic PRIME TX-1000 1000W 80 Plus Titanium', 6490000.00, 5, 'Seasonic', 'Nguồn Seasonic PRIME TX-1000 Titanium – đỉnh cao công nghệ.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1000W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_seasonic_1000w.jpg', 0, 0),
('SP000139', 'PSU Be Quiet! Dark Power 12 1200W 80 Plus Titanium', 7490000.00, 4, 'Be Quiet!', 'Nguồn Be Quiet! Dark Power 12 1200W – công suất lớn, hiệu suất tối đa.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"1200W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_bequiet_1200w.jpg', 0, 0),
('SP000140', 'PSU Cooler Master V850 850W 80 Plus Gold', 2390000.00, 20, 'Cooler Master', 'PSU Cooler Master V850 850W – ổn định và hiệu quả.', '{\"Danh mục\": \"PSU\", \"Công suất\": \"850W\", \"Kích thước\": \"ATX\"}', 'Components', 'psu_cooler_master_v850.jpg', 0, 0),
('SP000141', 'Case NZXT H510 ATX Mid Tower', 1650000.00, 15, 'NZXT', 'Case NZXT H510 với thiết kế hiện đại và kính cường lực.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"6kg\"}', 'Components', 'case_nzxt_h510.jpg', 0, 0),
('SP000142', 'Case NZXT H7 Flow ATX Mid Tower', 2650000.00, 10, 'NZXT', 'Case NZXT H7 Flow tối ưu luồng khí với thiết kế hiện đại.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"4 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7kg\"}', 'Components', 'case_nzxt_h7.jpg', 0, 0),
('SP000143', 'Case Corsair 4000D Airflow ATX', 1890000.00, 20, 'Corsair', 'Case Corsair 4000D Airflow – tối ưu luồng khí cho build gaming.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7kg\"}', 'Components', 'case_corsair_4000d.jpg', 0, 0),
('SP000144', 'Case Corsair iCUE 5000X RGB ATX', 3790000.00, 12, 'Corsair', 'Case Corsair iCUE 5000X RGB với đèn LED đa sắc và thiết kế sang trọng.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính, nhựa\", \"Khoang ổ đĩa\": \"4 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"8kg\"}', 'Components', 'case_corsair_5000x.jpg', 0, 0),
('SP000145', 'Case Cooler Master MasterBox TD500 Mesh', 2150000.00, 18, 'Cooler Master', 'Case Cooler Master TD500 Mesh với lưới thông gió tối ưu.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"6.5kg\"}', 'Components', 'case_cooler_td500.jpg', 0, 0),
('SP000146', 'Case Cooler Master HAF 700 EVO E-ATX', 7990000.00, 5, 'Cooler Master', 'Case full tower Cooler Master HAF 700 EVO cho khả năng mở rộng tối đa.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Full Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"5 x 3.5”, 3 x 2.5”\", \"Mainboard hỗ trợ\": \"E-ATX, ATX\", \"Khối lượng\": \"15kg\"}', 'Components', 'case_cooler_haf700.jpg', 0, 0),
('SP000147', 'Case Lian Li PC-O11 Dynamic ATX', 2890000.00, 10, 'Lian Li', 'Case Lian Li PC-O11 Dynamic – thiết kế kính cường lực ấn tượng.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Nhôm, kính\", \"Khoang ổ đĩa\": \"4 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7kg\"}', 'Components', 'case_lianli_o11.jpg', 0, 0),
('SP000148', 'Case Lian Li Lancool III RGB ATX', 2590000.00, 12, 'Lian Li', 'Case Lian Li Lancool III RGB – thiết kế đẹp, hỗ trợ tản nhiệt tốt.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7.5kg\"}', 'Components', 'case_lianli_lancool.jpg', 0, 0),
('SP000149', 'Case Fractal Design Meshify C ATX', 1750000.00, 14, 'Fractal Design', 'Case Fractal Design Meshify C – nhỏ gọn với khả năng tản nhiệt tốt.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"2 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"6kg\"}', 'Components', 'case_fractal_meshify.jpg', 0, 0),
('SP000150', 'Case Fractal Design Define 7 XL Full Tower', 4690000.00, 7, 'Fractal Design', 'Case Fractal Design Define 7 XL – full tower rộng rãi.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Full Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"5 x 3.5”, 3 x 2.5”\", \"Mainboard hỗ trợ\": \"E-ATX, ATX\", \"Khối lượng\": \"18kg\"}', 'Components', 'case_fractal_define7.jpg', 0, 0),
('SP000151', 'Case Thermaltake View 71 RGB ATX', 3950000.00, 8, 'Thermaltake', 'Case Thermaltake View 71 RGB – kính cường lực 4 mặt sang trọng.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Full Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"4 x 3.5”, 3 x 2.5”\", \"Mainboard hỗ trợ\": \"E-ATX, ATX\", \"Khối lượng\": \"17kg\"}', 'Components', 'case_tt_view71.jpg', 0, 0),
('SP000152', 'Case Thermaltake Core P3 Open Frame', 3290000.00, 9, 'Thermaltake', 'Case Thermaltake Core P3 – thiết kế Open Frame độc đáo.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép\", \"Khoang ổ đĩa\": \"Không có\", \"Mainboard hỗ trợ\": \"Micro-ATX, ATX\", \"Khối lượng\": \"6kg\"}', 'Components', 'case_tt_corep3.jpg', 0, 0),
('SP000153', 'Case Be Quiet! Silent Base 802 ATX', 3190000.00, 6, 'Be Quiet!', 'Case Be Quiet! Silent Base 802 – thiết kế yên tĩnh, tối ưu luồng khí.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, nhôm\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7kg\"}', 'Components', 'case_bequiet_802.jpg', 0, 0),
('SP000154', 'Case Phanteks Eclipse P500A D-RGB ATX', 2590000.00, 12, 'Phanteks', 'Case Phanteks Eclipse P500A D-RGB – lưới thông gió hiện đại.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"7.5kg\"}', 'Components', 'case_phanteks_p500a.jpg', 0, 0),
('SP000155', 'Case Phanteks Enthoo Pro II Full Tower', 4290000.00, 7, 'Phanteks', 'Case Phanteks Enthoo Pro II – full tower chuyên nghiệp.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Full Tower\", \"Chất liệu\": \"Thép\", \"Khoang ổ đĩa\": \"5 x 3.5”, 3 x 2.5”\", \"Mainboard hỗ trợ\": \"E-ATX, ATX\", \"Khối lượng\": \"16kg\"}', 'Components', 'case_phanteks_pro2.jpg', 0, 0),
('SP000156', 'Case InWin 303 ATX Mid Tower', 2090000.00, 10, 'InWin', 'Case InWin 303 – thiết kế tối giản, chắc chắn.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"3 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"6kg\"}', 'Components', 'case_inwin_303.jpg', 0, 0),
('SP000157', 'Case InWin D-Frame Mini ITX', 4990000.00, 5, 'InWin', 'Case InWin D-Frame Mini – dành cho build Mini ITX cao cấp.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mini ITX\", \"Chất liệu\": \"Nhôm\", \"Khoang ổ đĩa\": \"1 x 2.5”\", \"Mainboard hỗ trợ\": \"Mini-ITX\", \"Khối lượng\": \"3kg\"}', 'Components', 'case_inwin_dframe.jpg', 0, 0),
('SP000158', 'Case Deepcool CH510 Mesh Digital ATX', 1790000.00, 18, 'Deepcool', 'Case Deepcool CH510 Mesh Digital – tối ưu luồng khí.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép\", \"Khoang ổ đĩa\": \"2 x 3.5”, 2 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"6.5kg\"}', 'Components', 'case_deepcool_ch510.jpg', 0, 0),
('SP000159', 'Case Antec NX410 RGB Mid Tower', 1290000.00, 20, 'Antec', 'Case Antec NX410 RGB – thiết kế trẻ trung, giá rẻ.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Thép, kính\", \"Khoang ổ đĩa\": \"2 x 3.5”, 1 x 2.5”\", \"Mainboard hỗ trợ\": \"ATX, Micro-ATX\", \"Khối lượng\": \"5.5kg\"}', 'Components', 'case_antec_nx410.jpg', 0, 0),
('SP000160', 'Case Antec Torque Open-Air ATX', 6890000.00, 5, 'Antec', 'Case Antec Torque Open-Air – thiết kế Open-Air độc đáo.', '{\"Danh mục\": \"Case\", \"Kích thước\": \"Mid Tower\", \"Chất liệu\": \"Nhôm\", \"Khoang ổ đĩa\": \"Không có\", \"Mainboard hỗ trợ\": \"ATX\", \"Khối lượng\": \"4kg\"}', 'Components', 'case_antec_torque.jpg', 0, 0),
('SP000161', 'Speaker Razer Nommo V2 Pro 2.1 Model A', 10450000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro với âm thanh vòm sống động, Model A.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"Subwoofer + 2 Satellite\"}', 'Audio', 'speaker_modelA.jpg', 0, 0),
('SP000162', 'Speaker Logitech Z623 2.1 Model B', 8500000.00, 40, 'Logitech', 'Speaker Logitech Z623 cho âm bass sâu lắng, Model B.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Active\"}', 'Audio', 'speaker_modelB.jpg', 0, 0),
('SP000163', 'Speaker Bose Companion 2 Series III Model C', 9500000.00, 30, 'Bose', 'Speaker Bose Companion 2 Series III cho âm thanh trung thực, Model C.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.0\", \"Loại loa\": \"2.0 Stereo\"}', 'Audio', 'speaker_modelC.jpg', 0, 0),
('SP000164', 'Speaker JBL Professional 305P MkII Model D', 6700000.00, 35, 'JBL', 'Speaker JBL Professional 305P MkII mang đến âm thanh sống động, Model D.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Studio Monitor\"}', 'Audio', 'speaker_modelD.jpg', 0, 0),
('SP000165', 'Speaker Creative Pebble 2.0 Model E', 2500000.00, 60, 'Creative', 'Speaker Creative Pebble 2.0 với thiết kế nhỏ gọn và âm thanh sống động, Model E.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.0\", \"Loại loa\": \"2.0 Multimedia\"}', 'Audio', 'speaker_modelE.jpg', 0, 0),
('SP000166', 'Speaker Edifier R1280T Model F', 3200000.00, 45, 'Edifier', 'Speaker Edifier R1280T cho âm bass ấm áp, Model F.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Bookshelf\"}', 'Audio', 'speaker_modelF.jpg', 0, 0),
('SP000167', 'Speaker Klipsch ProMedia 2.1 Model G', 7200000.00, 38, 'Klipsch', 'Speaker Klipsch ProMedia 2.1 cho âm bass mạnh mẽ, Model G.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelG.jpg', 0, 0),
('SP000168', 'Speaker Logitech Z906 5.1 Model H', 14500000.00, 25, 'Logitech', 'Speaker Logitech Z906 với âm thanh vòm 5.1, Model H.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"5.1\", \"Loại loa\": \"5.1 Surround\"}', 'Audio', 'speaker_modelH.jpg', 0, 0),
('SP000169', 'Speaker Razer Nommo V2 Pro 2.1 Model I', 10530000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro cho trải nghiệm âm thanh chân thực, Model I.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"Subwoofer + Satellite\"}', 'Audio', 'speaker_modelI.jpg', 0, 0),
('SP000170', 'Speaker Bose Companion 50 Model J', 11000000.00, 30, 'Bose', 'Speaker Bose Companion 50 với âm thanh vòm mạnh mẽ, Model J.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelJ.jpg', 0, 0),
('SP000171', 'Speaker JBL 308BT Model K', 9800000.00, 40, 'JBL', 'Speaker JBL 308BT mang đến trải nghiệm âm thanh không dây mạnh mẽ, Model K.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Bluetooth\"}', 'Audio', 'speaker_modelK.jpg', 0, 0),
('SP000172', 'Speaker Creative Sound BlasterX Katana Model L', 8600000.00, 35, 'Creative', 'Speaker Creative Sound BlasterX Katana với hiệu suất âm thanh tuyệt vời, Model L.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelL.jpg', 0, 0),
('SP000173', 'Speaker Logitech Z337 Model M', 4500000.00, 55, 'Logitech', 'Speaker Logitech Z337 cho âm bass sâu lắng, Model M.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Desktop\"}', 'Audio', 'speaker_modelM.jpg', 0, 0),
('SP000174', 'Speaker Razer Nommo V2 Pro 2.1 Model N', 10580000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro cho âm thanh rực rỡ, Model N.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"Subwoofer + Satellite\"}', 'Audio', 'speaker_modelN.jpg', 0, 0),
('SP000175', 'Speaker Edifier S350DB Model O', 6400000.00, 47, 'Edifier', 'Speaker Edifier S350DB cho âm bass mạnh mẽ và chi tiết, Model O.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelO.jpg', 0, 0),
('SP000176', 'Speaker Bose Companion 20 Multimedia Speaker Model P', 7600000.00, 42, 'Bose', 'Speaker Bose Companion 20 với thiết kế compact và âm thanh trung thực, Model P.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.0\", \"Loại loa\": \"2.0 Multimedia\"}', 'Audio', 'speaker_modelP.jpg', 0, 0),
('SP000177', 'Speaker JBL Bar 2.1 Model Q', 11500000.00, 28, 'JBL', 'Speaker JBL Bar 2.1 mang đến trải nghiệm âm thanh vòm mạnh mẽ, Model Q.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Soundbar\"}', 'Audio', 'speaker_modelQ.jpg', 0, 0),
('SP000178', 'Speaker Logitech Z407 Model R', 5200000.00, 50, 'Logitech', 'Speaker Logitech Z407 cho âm thanh rõ ràng và bass sống động, Model R.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelR.jpg', 0, 0),
('SP000179', 'Speaker Creative Pebble Plus 2.1 Model S', 3300000.00, 65, 'Creative', 'Speaker Creative Pebble Plus 2.1 với thiết kế hiện đại và âm thanh sống động, Model S.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"2.1 Multimedia\"}', 'Audio', 'speaker_modelS.jpg', 0, 0),
('SP000180', 'Speaker Razer Nommo V2 Pro 2.1 Model T', 10640000.00, 52, 'Razer', 'Speaker Razer Nommo V2 Pro cao cấp, Model T.', '{\"Danh mục\": \"Speaker\", \"Kích thước\": \"2.1\", \"Loại loa\": \"Subwoofer + Satellite\"}', 'Audio', 'speaker_modelT.jpg', 0, 0),
('SP000181', 'Microphone Blue Yeti X USB Condenser Model A', 5250000.00, 40, 'Blue Yeti', 'Microphone Blue Yeti X USB cho thu âm chất lượng cao, Model A.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/192kHz\", \"Điện năng tiêu thụ\": \"5W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelA.jpg', 0, 0),
('SP000182', 'Microphone Rode NT-USB Condenser Model B', 5100000.00, 38, 'Rode', 'Microphone Rode NT-USB cho thu âm mượt mà, Model B.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-37dB\"}', 'Audio', 'mic_modelB.jpg', 0, 0),
('SP000183', 'Microphone Shure MV7 USB Dynamic Model C', 5200000.00, 42, 'Shure', 'Microphone Shure MV7 với công nghệ điều khiển cảm ứng, Model C.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"50Hz-16kHz\", \"Độ nhạy\": \"-54dB\"}', 'Audio', 'mic_modelC.jpg', 0, 0),
('SP000184', 'Microphone Audio-Technica AT2020USB+ Model D', 5300000.00, 45, 'Audio-Technica', 'Microphone AT2020USB+ cho thu âm trung thực, Model D.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-37dB\"}', 'Audio', 'mic_modelD.jpg', 0, 0),
('SP000185', 'Microphone Samson Meteor Mic USB Model E', 5400000.00, 47, 'Samson', 'Microphone Samson Meteor Mic USB với thiết kế cổ điển, Model E.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/44.1kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelE.jpg', 0, 0),
('SP000186', 'Microphone AKG Lyra USB Condenser Model F', 5500000.00, 40, 'AKG', 'Microphone AKG Lyra USB cho thu âm chuyên nghiệp, Model F.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"5W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-40dB\"}', 'Audio', 'mic_modelF.jpg', 0, 0),
('SP000187', 'Microphone HyperX SoloCast USB Model G', 5050000.00, 50, 'HyperX', 'Microphone HyperX SoloCast USB đơn giản, hiệu quả, Model G.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelG.jpg', 0, 0),
('SP000188', 'Microphone Elgato Wave:3 USB Condenser Model H', 5600000.00, 39, 'Elgato', 'Microphone Elgato Wave:3 USB cho livestream và podcast, Model H.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-36dB\"}', 'Audio', 'mic_modelH.jpg', 0, 0),
('SP000189', 'Microphone Razer Seiren X USB Model I', 5150000.00, 44, 'Razer', 'Microphone Razer Seiren X USB cho streaming, Model I.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"50Hz-18kHz\", \"Độ nhạy\": \"-42dB\"}', 'Audio', 'mic_modelI.jpg', 0, 0),
('SP000190', 'Microphone Blue Yeti Nano USB Model J', 5255000.00, 42, 'Blue Yeti', 'Microphone Blue Yeti Nano USB nhỏ gọn nhưng chất lượng cao, Model J.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelJ.jpg', 0, 0),
('SP000191', 'Microphone Rode Podcaster USB Model K', 5350000.00, 41, 'Rode', 'Microphone Rode Podcaster USB cho podcast chuyên nghiệp, Model K.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"50Hz-16kHz\", \"Độ nhạy\": \"-42dB\"}', 'Audio', 'mic_modelK.jpg', 0, 0),
('SP000192', 'Microphone Shure MV5 USB Model L', 5405000.00, 43, 'Shure', 'Microphone Shure MV5 USB với thiết kế hiện đại, Model L.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelL.jpg', 0, 0),
('SP000193', 'Microphone Audio-Technica AT2005USB Model M', 5450000.00, 44, 'Audio-Technica', 'Microphone AT2005USB với kết nối kép USB/XLR, Model M.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"20-bit/48kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"45Hz-16kHz\", \"Độ nhạy\": \"-54dB\"}', 'Audio', 'mic_modelM.jpg', 0, 0),
('SP000194', 'Microphone Samson C01U Pro USB Model N', 5505000.00, 42, 'Samson', 'Microphone Samson C01U Pro USB cho âm thanh sắc nét, Model N.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-37dB\"}', 'Audio', 'mic_modelN.jpg', 0, 0),
('SP000195', 'Microphone AKG P120 USB Condenser Model O', 5550000.00, 40, 'AKG', 'Microphone AKG P120 USB với khả năng thu âm chi tiết, Model O.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"5W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-34dB\"}', 'Audio', 'mic_modelO.jpg', 0, 0),
('SP000196', 'Microphone HyperX SoloCast USB Model P', 5605000.00, 44, 'HyperX', 'Microphone HyperX SoloCast USB mang lại chất lượng thu âm cao, Model P.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelP.jpg', 0, 0),
('SP000197', 'Microphone Elgato Wave:1 USB Model Q', 5650000.00, 43, 'Elgato', 'Microphone Elgato Wave:1 USB cho thu âm mượt mà, Model Q.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"4W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-36dB\"}', 'Audio', 'mic_modelQ.jpg', 0, 0),
('SP000198', 'Microphone Sennheiser e965 USB Model R', 5700000.00, 42, 'Sennheiser', 'Microphone Sennheiser e965 USB cho thu âm chuyên nghiệp, Model R.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"24-bit/96kHz\", \"Điện năng tiêu thụ\": \"5W\", \"Tần số đáp ứng\": \"20Hz-20kHz\", \"Độ nhạy\": \"-38dB\"}', 'Audio', 'mic_modelR.jpg', 0, 0),
('SP000199', 'Microphone Razer Seiren X USB Model S', 5750000.00, 44, 'Razer', 'Microphone Razer Seiren X USB với thiết kế độc đáo, Model S.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"50Hz-18kHz\", \"Độ nhạy\": \"-42dB\"}', 'Audio', 'mic_modelS.jpg', 0, 0),
('SP000200', 'Microphone Shure MV7 USB Model T', 5800000.00, 44, 'Shure', 'Microphone Shure MV7 USB với khả năng điều khiển cảm ứng, Model T.', '{\"Danh mục\": \"Microphone\", \"Sample / bit rate\": \"16-bit/48kHz\", \"Điện năng tiêu thụ\": \"3W\", \"Tần số đáp ứng\": \"50Hz-16kHz\", \"Độ nhạy\": \"-42dB\"}', 'Audio', 'mic_modelT.jpg', 0, 0),
('SP000201', 'Router ASUS RT-AX88U Dual-Band', 1999000.00, 5, 'ASUS', 'Router ASUS RT-AX88U hỗ trợ WiFi 6 với tốc độ cực cao.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 6000Mbps\", \"Bộ xử lý\": \"Quad-core\", \"Bộ nhớ\": \"512MB\"}', 'Network', 'router_asus_rtax88u.jpg', 0, 0),
('SP000202', 'Router Netgear Nighthawk RAX20 Dual-Band', 1499000.00, 7, 'Netgear', 'Router Netgear Nighthawk RAX20 với thiết kế hiện đại.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_netgear_rax20.jpg', 0, 0),
('SP000203', 'Router TP-Link Archer AX21 Dual-Band', 1299000.00, 8, 'TP-Link', 'Router TP-Link Archer AX21 hỗ trợ WiFi 6 hiệu năng ổn định.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 1800Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_tplink_ax21.jpg', 0, 0),
('SP000204', 'Router Linksys EA7500 Dual-Band', 1350000.00, 6, 'Linksys', 'Router Linksys EA7500 với chuẩn WiFi 5 cho kết nối ổn định.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1900Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_linksys_ea7500.jpg', 0, 0),
('SP000205', 'Router D-Link DIR-3060 Dual-Band', 1285000.00, 9, 'D-Link', 'Router D-Link DIR-3060 với thiết kế nhỏ gọn.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1700Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_dlink_dir3060.jpg', 0, 0),
('SP000206', 'Router Huawei WiFi Q2 Pro Tri-Band', 1590000.00, 4, 'Huawei', 'Router Huawei WiFi Q2 Pro tích hợp Mesh cho phủ sóng toàn diện.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 3000Mbps\", \"Bộ xử lý\": \"Quad-core\", \"Bộ nhớ\": \"512MB\"}', 'Network', 'router_huawei_q2pro.jpg', 0, 0),
('SP000207', 'Router Ubiquiti UniFi Dream Machine', 1790000.00, 3, 'Ubiquiti', 'Router Ubiquiti UniFi Dream Machine tích hợp bộ điều khiển mạng.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1300Mbps\", \"Bộ xử lý\": \"Quad-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_ubiquiti_dream.jpg', 0, 0),
('SP000208', 'Router Google Nest Wifi Dual-Band', 1690000.00, 5, 'Google', 'Router Google Nest Wifi với hệ thống Mesh thông minh.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1200Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_google_nest.jpg', 0, 0),
('SP000209', 'Router MikroTik hAP ac2 Dual-Band', 1190000.00, 10, 'MikroTik', 'Router MikroTik hAP ac2 với 5 cổng LAN, hiệu năng ổn định.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1000Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'router_mikrotik_hapac2.jpg', 0, 0),
('SP000210', 'Router Synology RT2600ac Dual-Band', 1890000.00, 4, 'Synology', 'Router Synology RT2600ac với tính năng bảo mật cao.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1800Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_synology_rt2600ac.jpg', 0, 0),
('SP000211', 'Router ASUS RT-AX86U Dual-Band', 1890000.00, 6, 'ASUS', 'Router ASUS RT-AX86U hỗ trợ WiFi 6 với tốc độ 5700Mbps.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 5700Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_asus_rtax86u.jpg', 0, 0),
('SP000212', 'Router Netgear Orbi RBK752 Tri-Band', 2090000.00, 3, 'Netgear', 'Router Netgear Orbi RBK752 với hệ thống Mesh Tri-Band.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 4000Mbps\", \"Bộ xử lý\": \"Quad-core\", \"Bộ nhớ\": \"512MB\"}', 'Network', 'router_netgear_orbi.jpg', 0, 0),
('SP000213', 'Router TP-Link Deco X60 Mesh WiFi Tri-Band', 1995000.00, 6, 'TP-Link', 'Router TP-Link Deco X60 với hệ thống Mesh Tri-Band.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 3600Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_tplink_decox60.jpg', 0, 0),
('SP000214', 'Router Linksys Velop MX10600 Mesh', 2290000.00, 2, 'Linksys', 'Router Linksys Velop MX10600 với công nghệ Mesh tiên tiến.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 5000Mbps\", \"Bộ xử lý\": \"Quad-core\", \"Bộ nhớ\": \"512MB\"}', 'Network', 'router_linksys_velop.jpg', 0, 0),
('SP000215', 'Router D-Link COVR-2202 Mesh WiFi Dual-Band', 1595000.00, 4, 'D-Link', 'Router D-Link COVR-2202 với thiết kế Mesh cho phủ sóng đều.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1200Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_dlink_covr2202.jpg', 0, 0),
('SP000216', 'Router Huawei 5G CPE Pro 2', 2590000.00, 3, 'Huawei', 'Router Huawei 5G CPE Pro 2 – kết nối 5G tốc độ cao.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"5G/4G\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2000Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_huawei_5g.jpg', 0, 0),
('SP000217', 'Router Ubiquiti EdgeRouter 4 Gigabit', 1895000.00, 5, 'Ubiquiti', 'Router Ubiquiti EdgeRouter 4 với 4 cổng Gigabit.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"Wired\", \"Công nghệ AX\": \"N/A\", \"Truyền/nhận\": \"4 x Gigabit\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_ubiquiti_edgerouter4.jpg', 0, 0),
('SP000218', 'Router Google Wifi Mesh', 1695000.00, 7, 'Google', 'Router Google Wifi với hệ thống Mesh đơn giản.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1000Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"128MB\"}', 'Network', 'router_google_wifi.jpg', 0, 0),
('SP000219', 'Router MikroTik RB2011UiAS-2HnD-IN Dual-Band', 1090000.00, 8, 'MikroTik', 'Router MikroTik RB2011UiAS-2HnD-IN với 10 cổng LAN, linh hoạt.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1000Mbps\", \"Bộ xử lý\": \"Multi-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_mikrotik_rb2011.jpg', 0, 0),
('SP000220', 'Router Synology MR2200ac Mesh WiFi Dual-Band', 2190000.00, 3, 'Synology', 'Router Synology MR2200ac với công nghệ Mesh tiên tiến.', '{\"Danh mục\": \"Router Wifi\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1400Mbps\", \"Bộ xử lý\": \"Dual-core\", \"Bộ nhớ\": \"256MB\"}', 'Network', 'router_synology_mr2200ac.jpg', 0, 0),
('SP000221', 'Wifi Card ASUS PCE-AX3000 Dual-Band', 690000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AX3000 hỗ trợ WiFi 6, tốc độ cao.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 3000Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_asus_pceax3000.jpg', 0, 0),
('SP000222', 'Wifi Card TP-Link Archer T6E Dual-Band', 695000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer T6E với WiFi 5 hiệu năng ổn định.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 867Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_tplink_archert6e.jpg', 0, 0),
('SP000223', 'Wifi Card Intel Wi-Fi 6 AX200 Dual-Band', 700000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6 AX200 cho kết nối nhanh và ổn định.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_intel_ax200.jpg', 0, 0),
('SP000224', 'Wifi Card Gigabyte GC-WBAX200 Dual-Band', 705000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WBAX200 hỗ trợ WiFi 6 và Bluetooth 5.0.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_gigabyte_gcwbax200.jpg', 0, 0),
('SP000225', 'Wifi Card MSI MW65 Dual-Band', 710000.00, 21, 'MSI', 'Wifi Card MSI MW65 với hiệu năng truyền tải ổn định.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 867Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_msi_mw65.jpg', 0, 0),
('SP000226', 'Wifi Card ASUS PCE-AX3000 Dual-Band Model F', 711000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AX3000 Model F hỗ trợ chuẩn WiFi 6.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 3000Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_asus_pceax3000_f.jpg', 0, 0),
('SP000227', 'Wifi Card TP-Link Archer TX3000E Dual-Band', 712000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer TX3000E với WiFi 6 và MU-MIMO.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_tplink_tx3000e.jpg', 0, 0),
('SP000228', 'Wifi Card Intel Wi-Fi 6E AX210 Dual-Band Model H', 713000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6E AX210 cho băng tần mở rộng.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6E\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_intel_ax210.jpg', 0, 0),
('SP000229', 'Wifi Card Gigabyte GC-WB1735D Dual-Band Model I', 714000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WB1735D hỗ trợ WiFi 6 và Bluetooth 5.1.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_gigabyte_gcwb1735d.jpg', 0, 0),
('SP000230', 'Wifi Card ASUS PCE-AC88 Dual-Band Model J', 715000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AC88 hỗ trợ WiFi 5 với 4 anten, Model J.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 2100Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_asus_pceac88.jpg', 0, 0),
('SP000231', 'Wifi Card MSI MPG AC1300 Dual-Band Model K', 716000.00, 21, 'MSI', 'Wifi Card MSI MPG AC1300 mang lại kết nối ổn định, Model K.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1300Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_msi_mpga1300.jpg', 0, 0),
('SP000232', 'Wifi Card TP-Link Archer T3U Plus Dual-Band Model L', 717000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer T3U Plus với khả năng tăng cường tín hiệu, Model L.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 867Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_tplink_t3u_plus.jpg', 0, 0),
('SP000233', 'Wifi Card Intel Wireless-AC 9560 Dual-Band Model M', 718000.00, 21, 'Intel', 'Wifi Card Intel Wireless-AC 9560 với kết nối nhanh chóng, Model M.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1300Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_intel_9560.jpg', 0, 0),
('SP000234', 'Wifi Card Samson MW65 PCIe Dual-Band Model N', 719000.00, 21, 'Samson', 'Wifi Card Samson MW65 PCIe cho kết nối ổn định, Model N.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 867Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_samson_mw65.jpg', 0, 0),
('SP000235', 'Wifi Card ASUS PCE-AC88 Dual-Band Model O', 720000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AC88 Model O – hiệu suất kết nối ổn định, Model O.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 2100Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_asus_pceac88_o.jpg', 0, 0),
('SP000236', 'Wifi Card TP-Link Archer T3U Dual-Band Model P', 721000.00, 21, 'TP-Link', 'Wifi Card TP-Link Archer T3U với thiết kế nhỏ gọn, Model P.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 867Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_tplink_t3u.jpg', 0, 0),
('SP000237', 'Wifi Card MSI AC1300 PCIe Dual-Band Model Q', 722000.00, 21, 'MSI', 'Wifi Card MSI AC1300 PCIe cho hiệu năng ổn định, Model Q.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1300Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_msi_ac1300.jpg', 0, 0),
('SP000238', 'Wifi Card Intel Wi-Fi 6E AX210NGW Dual-Band Model R', 723000.00, 21, 'Intel', 'Wifi Card Intel Wi-Fi 6E AX210NGW hỗ trợ băng tần mở rộng, Model R.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ax\", \"Công nghệ AX\": \"WiFi 6E\", \"Truyền/nhận\": \"Up to 2400Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"64MB\"}', 'Network', 'wifi_intel_ax210ngw.jpg', 0, 0),
('SP000239', 'Wifi Card ASUS PCE-AC88 Dual-Band Model S', 724000.00, 21, 'ASUS', 'Wifi Card ASUS PCE-AC88 Model S – hiệu suất cao, Model S.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 2100Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_asus_pceac88_s.jpg', 0, 0),
('SP000240', 'Wifi Card Gigabyte GC-WB1732 Dual-Band Model T', 725000.00, 21, 'Gigabyte', 'Wifi Card Gigabyte GC-WB1732 hỗ trợ WiFi 5, Model T.', '{\"Danh mục\": \"Wifi Card\", \"Tiêu chuẩn mạng\": \"802.11ac\", \"Công nghệ AX\": \"WiFi 5\", \"Truyền/nhận\": \"Up to 1300Mbps\", \"Bộ xử lý\": \"Single-core\", \"Bộ nhớ\": \"32MB\"}', 'Network', 'wifi_gigabyte_gcwb1732.jpg', 0, 0),
('SP000241', 'Màn hình ASUS TUF Gaming VG249Q3A 23.8\" 144Hz', 3050000.00, 34, 'ASUS', 'Monitor ASUS TUF Gaming VG249Q3A với tần số 144Hz và hình ảnh sắc nét.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'asus_tuf_vg249q3a.jpg', 0, 0),
('SP000242', 'Màn hình Acer Nitro XV242Q 23.8\" 144Hz', 3100000.00, 30, 'Acer', 'Monitor Acer Nitro XV242Q với thiết kế Predator và tần số 144Hz.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'acer_nitro_xv242q.jpg', 0, 0),
('SP000243', 'Màn hình LG UltraGear 24GN600-B 23.8\" 144Hz', 3080000.00, 28, 'LG', 'Monitor LG UltraGear 24GN600-B với thời gian phản hồi nhanh và màu sắc sống động.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'lg_ultragear_24gn600b.jpg', 0, 0),
('SP000244', 'Màn hình Dell S2421HGF 23.8\" 144Hz', 3070000.00, 26, 'Dell', 'Monitor Dell S2421HGF mang đến hình ảnh sắc nét cho game thủ.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'dell_s2421hgf.jpg', 0, 0),
('SP000245', 'Màn hình Samsung Odyssey G3 23.8\" 144Hz', 3060000.00, 32, 'Samsung', 'Monitor Samsung Odyssey G3 với công nghệ hiển thị tiên tiến.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"VA\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'samsung_odyssey_g3.jpg', 0, 0),
('SP000246', 'Màn hình BenQ ZOWIE XL2411P 23.8\" 144Hz', 3090000.00, 22, 'BenQ', 'Monitor BenQ ZOWIE XL2411P dành cho game competitive với thời gian phản hồi nhanh.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"DVI, VGA\"}', 'Monitor', 'benq_zowie_xl2411p.jpg', 0, 0),
('SP000247', 'Màn hình HP X24ih 23.8\" 144Hz', 3100000.00, 20, 'HP', 'Monitor HP X24ih cung cấp hình ảnh sống động cho game thủ.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'hp_x24ih.jpg', 0, 0);
INSERT INTO `sanpham` (`IdSp`, `tensanpham`, `gia`, `soluongton`, `thuonghieu`, `mota`, `thongsokythuat`, `loaisanpham`, `hinhanh`, `soluotxem`, `damuahang`) VALUES
('SP000248', 'Màn hình ASUS ROG Strix XG24V 23.8\" 144Hz', 3120000.00, 18, 'ASUS', 'Monitor ASUS ROG Strix XG24V với thiết kế sắc sảo cho game competitive.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'asus_rog_strix_xg24v.jpg', 0, 0),
('SP000249', 'Màn hình Acer Predator XB253Q 23.8\" 144Hz', 3130000.00, 24, 'Acer', 'Monitor Acer Predator XB253Q mang lại trải nghiệm hình ảnh tuyệt vời với độ trễ thấp.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'acer_predator_xb253q.jpg', 0, 0),
('SP000250', 'Màn hình Dell Alienware AW2521HF 23.8\" 144Hz', 3140000.00, 26, 'Dell', 'Monitor Dell Alienware AW2521HF với tốc độ làm mới cực nhanh.', '{\"Kích thước\": \"23.8 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'dell_alienware_aw2521hf.jpg', 0, 0),
('SP000251', 'Màn hình ASUS TUF Gaming VG279Q 27\" 165Hz', 3200000.00, 25, 'ASUS', 'Monitor ASUS TUF Gaming VG279Q với tần số 165Hz cho trải nghiệm mượt mà.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"165Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort, DVI\"}', 'Monitor', 'asus_tuf_vg279q.jpg', 0, 0),
('SP000252', 'Màn hình Acer Nitro XV272U 27\" 144Hz', 3190000.00, 34, 'Acer', 'Monitor Acer Nitro XV272U 27 inch với độ phân giải QHD, hình ảnh sắc nét.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"2560x1440\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort, USB-C\"}', 'Monitor', 'acer_nitro_xv272u.jpg', 0, 0),
('SP000253', 'Màn hình LG UltraGear 27GN750-B 27\" 240Hz', 3200000.00, 32, 'LG', 'Monitor LG UltraGear 27GN750-B với tốc độ làm mới 240Hz cho game competitive.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"240Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'lg_ultragear_27gn750b.jpg', 0, 0),
('SP000254', 'Màn hình Dell S2721DGF 27\" 165Hz', 3210000.00, 30, 'Dell', 'Monitor Dell S2721DGF với độ phân giải QHD và tần số 165Hz cho game thủ.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"2560x1440\", \"Tần số quét\": \"165Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'dell_s2721dgf.jpg', 0, 0),
('SP000255', 'Màn hình Samsung Odyssey G7 27\" 240Hz', 3220000.00, 28, 'Samsung', 'Monitor Samsung Odyssey G7 với màn hình cong, tần số 240Hz và độ phân giải QHD.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"2560x1440\", \"Tần số quét\": \"240Hz\", \"Công nghệ màn hình\": \"VA\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'samsung_odyssey_g7.jpg', 0, 0),
('SP000256', 'Màn hình BenQ ZOWIE XL2546K 24.5\" 240Hz', 3230000.00, 26, 'BenQ', 'Monitor BenQ ZOWIE XL2546K với thời gian phản hồi siêu nhanh, lý tưởng cho game competitive.', '{\"Kích thước\": \"24.5 inch\", \"Độ phân giải\": \"1920x1080\", \"Tần số quét\": \"240Hz\", \"Công nghệ màn hình\": \"TN\", \"Cổng kết nối\": \"HDMI\"}', 'Monitor', 'benq_zowie_xl2546k.jpg', 0, 0),
('SP000257', 'Màn hình HP Omen X 27\" 240Hz', 3240000.00, 34, 'HP', 'Monitor HP Omen X 27 inch với tốc độ 240Hz cho hình ảnh chất lượng cao.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"2560x1440\", \"Tần số quét\": \"240Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort\"}', 'Monitor', 'hp_omen_x_27.jpg', 0, 0),
('SP000258', 'Màn hình Acer Predator XB273K 27\" 144Hz', 3290000.00, 20, 'Acer', 'Monitor Acer Predator XB273K 27 inch với độ phân giải 4K và tần số 144Hz.', '{\"Kích thước\": \"27 inch\", \"Độ phân giải\": \"3840x2160\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort, USB-C\"}', 'Monitor', 'acer_predator_xb273k.jpg', 0, 0),
('SP000259', 'Màn hình Dell Alienware AW3423DW 34\" Curved', 4500000.00, 18, 'Dell', 'Monitor Dell Alienware AW3423DW 34 inch cong cho trải nghiệm gaming đỉnh cao.', '{\"Kích thước\": \"34 inch\", \"Độ phân giải\": \"3440x1440\", \"Tần số quét\": \"120Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort, USB-C\"}', 'Monitor', 'dell_alienware_aw3423dw.jpg', 0, 0),
('SP000260', 'Màn hình LG UltraGear 34GN850-B 34\" Ultrawide', 4600000.00, 16, 'LG', 'Monitor LG UltraGear 34GN850-B 34 inch ultrawide với tần số 144Hz và độ phân giải cao.', '{\"Kích thước\": \"34 inch\", \"Độ phân giải\": \"3440x1440\", \"Tần số quét\": \"144Hz\", \"Công nghệ màn hình\": \"IPS\", \"Cổng kết nối\": \"HDMI, DisplayPort, USB-C\"}', 'Monitor', 'monitor_lg_34gn850b.jpg', 0, 0),
('SP000261', 'Mouse ASUS TUF Gaming M3 Wired', 400000.00, 21, 'ASUS', 'Chuột gaming ASUS TUF Gaming M3 Wired với cảm biến 16000 DPI.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"80g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_asus_tuf_m3.jpg', 0, 0),
('SP000262', 'Mouse Logitech G502 HERO Wired', 450000.00, 30, 'Logitech', 'Chuột Logitech G502 HERO Wired với 11 nút lập trình.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"No\", \"Buttons\": \"11\", \"Weight\": \"105g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_logitech_g502.jpg', 0, 0),
('SP000263', 'Mouse Razer DeathAdder V2 Wired', 420000.00, 25, 'Razer', 'Chuột Razer DeathAdder V2 Wired mang lại phản hồi nhanh.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"No\", \"Buttons\": \"8\", \"Weight\": \"100g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_razer_deathadder_v2.jpg', 0, 0),
('SP000264', 'Mouse Corsair Harpoon RGB Wireless', 500000.00, 18, 'Corsair', 'Chuột Corsair Harpoon RGB Wireless với kết nối không dây ổn định.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"18000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\", \"Weight\": \"95g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_corsair_harpoon.jpg', 0, 0),
('SP000265', 'Mouse SteelSeries Rival 3 Wireless', 470000.00, 20, 'SteelSeries', 'Chuột SteelSeries Rival 3 Wireless với thiết kế nhẹ và độ chính xác cao.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"85g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_steelseries_rival3.jpg', 0, 0),
('SP000266', 'Mouse HyperX Pulsefire Surge Wired', 430000.00, 22, 'HyperX', 'Chuột HyperX Pulsefire Surge Wired với cảm biến 16000 DPI.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"7\", \"Weight\": \"90g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_hyperx_pulsefire_surge.jpg', 0, 0),
('SP000267', 'Mouse Logitech G Pro Wireless', 550000.00, 15, 'Logitech', 'Chuột Logitech G Pro Wireless nhẹ và linh hoạt cho game thủ.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"5\", \"Weight\": \"80g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_logitech_gpro_wireless.jpg', 0, 0),
('SP000268', 'Mouse Razer Viper Ultimate Wireless', 600000.00, 14, 'Razer', 'Chuột Razer Viper Ultimate Wireless với thiết kế siêu nhẹ.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"Yes\", \"Buttons\": \"8\", \"Weight\": \"75g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_razer_viper_ultimate.jpg', 0, 0),
('SP000269', 'Mouse ASUS ROG Strix Impact II Wireless', 520000.00, 19, 'ASUS', 'Chuột ASUS ROG Strix Impact II Wireless với độ chính xác cao.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"70g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_asus_rog_strix_impact_ii.jpg', 0, 0),
('SP000270', 'Mouse Cooler Master MM710 Wired Lightweight', 390000.00, 24, 'Cooler Master', 'Chuột Cooler Master MM710 Wired với thiết kế siêu nhẹ.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"53g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_cooler_master_mm710.jpg', 0, 0),
('SP000271', 'Mouse Glorious Model O Wired', 410000.00, 23, 'Glorious', 'Chuột Glorious Model O Wired với thiết kế dạng lưới, siêu nhẹ.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"67g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_glorious_model_o.jpg', 0, 0),
('SP000272', 'Mouse Razer Basilisk V3 Wired', 430000.00, 21, 'Razer', 'Chuột Razer Basilisk V3 Wired với tính năng tùy chỉnh nút bấm.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"20000\", \"Wireless\": \"No\", \"Buttons\": \"11\", \"Weight\": \"105g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_razer_basilisk_v3.jpg', 0, 0),
('SP000273', 'Mouse Logitech G304 LIGHTSPEED Wireless', 450000.00, 22, 'Logitech', 'Chuột Logitech G304 LIGHTSPEED không dây với cảm biến HERO 25000.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"75g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_logitech_g304.jpg', 0, 0),
('SP000274', 'Mouse Corsair Ironclaw RGB Wireless', 480000.00, 20, 'Corsair', 'Chuột Corsair Ironclaw RGB Wireless thiết kế cho tay lớn.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\", \"Weight\": \"100g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_corsair_ironclaw.jpg', 0, 0),
('SP000275', 'Mouse SteelSeries Rival 650 Wireless', 530000.00, 18, 'SteelSeries', 'Chuột SteelSeries Rival 650 Wireless với tính năng sạc không dây.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"12000\", \"Wireless\": \"Yes\", \"Buttons\": \"7\", \"Weight\": \"95g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_steelseries_rival650.jpg', 0, 0),
('SP000276', 'Mouse HyperX Pulsefire Dart Wireless', 510000.00, 17, 'HyperX', 'Chuột HyperX Pulsefire Dart Wireless với thiết kế êm tay.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"18000\", \"Wireless\": \"Yes\", \"Buttons\": \"8\", \"Weight\": \"80g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_hyperx_pulsefire_dart.jpg', 0, 0),
('SP000277', 'Mouse Logitech G305 LIGHTSPEED Wireless', 420000.00, 25, 'Logitech', 'Chuột Logitech G305 LIGHTSPEED không dây nhẹ và hiệu năng cao.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"25000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"70g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_logitech_g305.jpg', 0, 0),
('SP000278', 'Mouse Razer Viper Mini Wired', 380000.00, 26, 'Razer', 'Chuột Razer Viper Mini với thiết kế siêu nhẹ, lý tưởng cho game thủ di động.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"8500\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"62g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_razer_viper_mini.jpg', 0, 0),
('SP000279', 'Mouse Cooler Master MM710 Pro Wired', 397000.00, 23, 'Cooler Master', 'Chuột Cooler Master MM710 Pro với độ bền cao, cho game thủ chuyên nghiệp.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"16000\", \"Wireless\": \"No\", \"Buttons\": \"6\", \"Weight\": \"55g\", \"Kết nối\": \"Wired\"}', 'Peripherals', 'mouse_cooler_master_mm710_pro.jpg', 0, 0),
('SP000280', 'Mouse Glorious Model O Wireless', 560000.00, 20, 'Glorious', 'Chuột Glorious Model O Wireless với thiết kế dạng lưới siêu nhẹ và tốc độ phản hồi nhanh.', '{\"Danh mục\": \"Mouse\", \"DPI\": \"19000\", \"Wireless\": \"Yes\", \"Buttons\": \"6\", \"Weight\": \"66g\", \"Kết nối\": \"Wireless\"}', 'Peripherals', 'mouse_glorious_model_o_wireless.jpg', 0, 0),
('SP000281', 'Headphone HyperX Cloud III Over-Ear Model A', 2250000.00, 40, 'HyperX', 'Headphone HyperX Cloud III Over-Ear mang lại âm thanh vòm sống động, Model A.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"50mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"Yes\", \"Trọng lượng\": \"300g\"}', 'Peripherals', 'headphone_modelA.jpg', 0, 0),
('SP000282', 'Headphone Razer Kraken X Over-Ear Model B', 2100000.00, 35, 'Razer', 'Headphone Razer Kraken X Over-Ear với thiết kế nhẹ và âm thanh sống động, Model B.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"40mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"Yes\", \"Trọng lượng\": \"310g\"}', 'Peripherals', 'headphone_modelB.jpg', 0, 0),
('SP000283', 'Headphone Logitech G Pro X Over-Ear Model C', 2400000.00, 38, 'Logitech', 'Headphone Logitech G Pro X Over-Ear với microphone Blue VO!CE, Model C.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"50mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"Blue VO!CE\", \"Trọng lượng\": \"320g\"}', 'Peripherals', 'headphone_modelC.jpg', 0, 0),
('SP000284', 'Headphone Sennheiser HD 660 S Over-Ear Model D', 2600000.00, 30, 'Sennheiser', 'Headphone Sennheiser HD 660 S Over-Ear cho âm thanh tự nhiên, Model D.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"45mm\", \"Dải tần số\": \"15Hz-28kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"No\", \"Trọng lượng\": \"400g\"}', 'Peripherals', 'headphone_modelD.jpg', 0, 0),
('SP000285', 'Headphone Audio-Technica ATH-M50x Over-Ear Model E', 2300000.00, 50, 'Audio-Technica', 'Headphone ATH-M50x Over-Ear mang lại âm bass mạnh mẽ, Model E.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"45mm\", \"Dải tần số\": \"15Hz-28kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"No\", \"Trọng lượng\": \"310g\"}', 'Peripherals', 'headphone_modelE.jpg', 0, 0),
('SP000286', 'Headphone SteelSeries Arctis 7 Over-Ear Model F', 2350000.00, 33, 'SteelSeries', 'Headphone SteelSeries Arctis 7 với kết nối không dây ổn định, Model F.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"40mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wireless\", \"Micro\": \"Yes\", \"Trọng lượng\": \"320g\"}', 'Peripherals', 'headphone_modelF.jpg', 0, 0),
('SP000287', 'Headphone Beyerdynamic DT 990 Pro Over-Ear Model G', 2450000.00, 42, 'Beyerdynamic', 'Headphone DT 990 Pro Over-Ear mang đến chất âm mở và rõ nét, Model G.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"45mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wired\", \"Micro\": \"No\", \"Trọng lượng\": \"350g\"}', 'Peripherals', 'headphone_modelG.jpg', 0, 0),
('SP000288', 'Headphone Bose QuietComfort 35 II Over-Ear Model H', 2800000.00, 37, 'Bose', 'Headphone Bose QuietComfort 35 II với công nghệ khử tiếng ồn chủ động, Model H.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"40mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wireless\", \"Micro\": \"Yes\", \"Trọng lượng\": \"310g\"}', 'Peripherals', 'headphone_modelH.jpg', 0, 0),
('SP000289', 'Headphone Sony WH-1000XM4 Over-Ear Model I', 2900000.00, 29, 'Sony', 'Headphone Sony WH-1000XM4 với công nghệ chống ồn hàng đầu, Model I.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"40mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wireless\", \"Micro\": \"Yes\", \"Trọng lượng\": \"290g\"}', 'Peripherals', 'headphone_modelI.jpg', 0, 0),
('SP000290', 'Headphone Jabra Elite 85h Over-Ear Model J', 2750000.00, 47, 'Jabra', 'Headphone Jabra Elite 85h với pin lâu và chất lượng âm thanh cao, Model J.', '{\"Danh mục\": \"Headphone\", \"Drivers\": \"40mm\", \"Dải tần số\": \"20Hz-20kHz\", \"Kết nối\": \"Wireless\", \"Micro\": \"Yes\", \"Trọng lượng\": \"350g\"}', 'Peripherals', 'headphone_modelJ.jpg', 0, 0),
('SP000301', 'Keyboard ASUS ROG Strix Scope Mechanical', 210000.00, 22, 'ASUS', 'Keyboard ASUS ROG Strix Scope với layout FPS chuyên dụng, Model A.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX Red\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"PBT\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_asus_rog_strix_scope.jpg', 0, 0),
('SP000302', 'Keyboard Logitech G513 Mechanical RGB', 180000.00, 25, 'Logitech', 'Keyboard Logitech G513 với switch Romer-G và đèn RGB, Model B.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Romer-G\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"Keycaps\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_logitech_g513.jpg', 0, 0),
('SP000303', 'Keyboard Razer BlackWidow V3 Mechanical RGB', 220000.00, 18, 'Razer', 'Keyboard Razer BlackWidow V3 với switch Razer Green, Model C.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Razer Green\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_razer_bw_v3.jpg', 0, 0),
('SP000304', 'Keyboard Corsair K70 RGB MK.2 Mechanical', 250000.00, 20, 'Corsair', 'Keyboard Corsair K70 RGB MK.2 với switch Cherry MX Speed, Model D.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX Speed\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"Wrist rest\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_corsair_k70.jpg', 0, 0),
('SP000305', 'Keyboard HyperX Alloy FPS Pro Mechanical', 150000.00, 30, 'HyperX', 'Keyboard HyperX Alloy FPS Pro tenkeyless cho gaming, Model E.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"87 keys\"}', 'Peripherals', 'keyboard_hyperx_alloy_fps_pro.jpg', 0, 0),
('SP000306', 'Keyboard SteelSeries Apex Pro Mechanical', 300000.00, 15, 'SteelSeries', 'Keyboard SteelSeries Apex Pro với switch OmniPoint, Model F.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"OmniPoint\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_steelseries_apex_pro.jpg', 0, 0),
('SP000307', 'Keyboard Ducky One 2 Mini Mechanical RGB', 190000.00, 25, 'Ducky', 'Keyboard Ducky One 2 Mini với thiết kế 60% gọn nhẹ, Model G.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX Brown\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"PBT\", \"Phụ kiện\": \"None\", \"Kích thước\": \"60% keys\"}', 'Peripherals', 'keyboard_ducky_one2_mini.jpg', 0, 0),
('SP000308', 'Keyboard ASUS ROG Falchion Wireless', 190000.00, 23, 'ASUS', 'Keyboard ASUS ROG Falchion Wireless tenkeyless, Model H.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Custom\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wireless\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"87 keys\"}', 'Peripherals', 'keyboard_asus_rog_falchion.jpg', 0, 0),
('SP000309', 'Keyboard Cooler Master CK552 Mechanical RGB', 170000.00, 28, 'Cooler Master', 'Keyboard Cooler Master CK552 với switch Outemu, Model I.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Outemu\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_cooler_master_ck552.jpg', 0, 0),
('SP000310', 'Keyboard MSI Vigor GK70 Mechanical RGB', 165000.00, 24, 'MSI', 'Keyboard MSI Vigor GK70 với thiết kế độc đáo, Model J.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Custom\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_msi_vigor_gk70.jpg', 0, 0),
('SP000311', 'Keyboard E-DRA EK506 Wireless Mechanical RGB', 162000.00, 21, 'E-DRA', 'Bàn phím E-DRA EK506 phiên bản không dây, Model K.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Custom\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wireless\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_edra_ek506_wireless.jpg', 0, 0),
('SP000312', 'Keyboard Logitech G Pro X Mechanical RGB', 175000.00, 23, 'Logitech', 'Keyboard Logitech G Pro X với khả năng thay switch hot-swappable, Model L.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Hot-swappable\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"Keycaps\", \"Kích thước\": \"87 keys\"}', 'Peripherals', 'keyboard_logitech_gprox.jpg', 0, 0),
('SP000313', 'Keyboard Razer Huntsman Elite Mechanical RGB', 240000.00, 20, 'Razer', 'Keyboard Razer Huntsman Elite với switch opto-mechanical, Model M.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Opto-mechanical\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_razer_huntsman_elite.jpg', 0, 0),
('SP000314', 'Keyboard Corsair K95 RGB Platinum XT Mechanical', 280000.00, 18, 'Corsair', 'Keyboard Corsair K95 RGB Platinum XT với khung hợp kim, Model N.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX Speed\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"Macro Keys\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_corsair_k95_rgb_platinum_xt.jpg', 0, 0),
('SP000315', 'Keyboard HyperX Alloy Elite 2 Mechanical RGB', 195000.00, 22, 'HyperX', 'Keyboard HyperX Alloy Elite 2 với khung kim loại chắc chắn, Model O.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_hyperx_alloy_elite2.jpg', 0, 0),
('SP000316', 'Keyboard SteelSeries Apex 7 Mechanical', 210000.00, 19, 'SteelSeries', 'Keyboard SteelSeries Apex 7 với switch OmniPoint, Model P.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"OmniPoint\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"Aluminum\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_steelseries_apex7.jpg', 0, 0),
('SP000317', 'Keyboard Ducky Shine 7 Mechanical RGB', 230000.00, 20, 'Ducky', 'Keyboard Ducky Shine 7 với keycaps PBT cao cấp, Model Q.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Cherry MX Blue\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"PBT\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_ducky_shine7.jpg', 0, 0),
('SP000318', 'Keyboard ASUS ROG Falchion Wireless Mechanical', 190000.00, 23, 'ASUS', 'Keyboard ASUS ROG Falchion Wireless với thiết kế tenkeyless, Model R.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Custom\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wireless\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"87 keys\"}', 'Peripherals', 'keyboard_asus_rog_falchion.jpg', 0, 0),
('SP000319', 'Keyboard Cooler Master CK550 Mechanical', 175000.00, 25, 'Cooler Master', 'Keyboard Cooler Master CK550 với switch Outemu, Model S.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Outemu\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_cooler_master_ck550.jpg', 0, 0),
('SP000320', 'Keyboard MSI Vigor GK72 Mechanical RGB', 180000.00, 22, 'MSI', 'Keyboard MSI Vigor GK72 với thiết kế độc đáo, Model T.', '{\"Danh mục\": \"Keyboard\", \"Switch\": \"Custom\", \"Màu sắc\": \"Black\", \"Kết nối\": \"Wired\", \"Chất liệu\": \"ABS\", \"Phụ kiện\": \"None\", \"Kích thước\": \"104 keys\"}', 'Peripherals', 'keyboard_msi_vigor_gk72.jpg', 0, 0),
('SP000321', 'Webcam Logitech C310 HD 720p Model A', 690000.00, 85, 'Logitech', 'Webcam Logitech C310 HD 720p cho video call chất lượng, Model A.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelA.jpg', 0, 0),
('SP000322', 'Webcam Logitech C920 HD Pro 1080p Model B', 990000.00, 80, 'Logitech', 'Webcam Logitech C920 HD Pro 1080p cho video call chuyên nghiệp, Model B.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelB.jpg', 0, 0),
('SP000323', 'Webcam Microsoft LifeCam HD-3000 Model C', 750000.00, 90, 'Microsoft', 'Webcam Microsoft LifeCam HD-3000 với chất lượng HD, Model C.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelC.jpg', 0, 0),
('SP000324', 'Webcam Razer Kiyo Model D', 850000.00, 70, 'Razer', 'Webcam Razer Kiyo tích hợp đèn LED điều chỉnh, Model D.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelD.jpg', 0, 0),
('SP000325', 'Webcam Logitech Brio 4K Model E', 2500000.00, 60, 'Logitech', 'Webcam Logitech Brio 4K cho hình ảnh vượt trội, Model E.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelE.jpg', 0, 0),
('SP000326', 'Webcam Dell UltraSharp WB702 Model F', 1500000.00, 75, 'Dell', 'Webcam Dell UltraSharp WB702 cho video call HD, Model F.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelF.jpg', 0, 0),
('SP000327', 'Webcam Logitech C270 HD 720p Model G', 650000.00, 100, 'Logitech', 'Webcam Logitech C270 HD 720p cho video call chất lượng, Model G.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelG.jpg', 0, 0),
('SP000328', 'Webcam A4Tech PK-910H Model H', 500000.00, 95, 'A4Tech', 'Webcam A4Tech PK-910H với thiết kế nhỏ gọn, Model H.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelH.jpg', 0, 0),
('SP000329', 'Webcam Creative Live! Cam Chat HD Model I', 700000.00, 88, 'Creative', 'Webcam Creative Live! Cam Chat HD cho cuộc gọi trực tuyến, Model I.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelI.jpg', 0, 0),
('SP000330', 'Webcam Logitech C922 Pro Stream Model J', 1200000.00, 82, 'Logitech', 'Webcam Logitech C922 Pro Stream cho trải nghiệm livestream chuyên nghiệp, Model J.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelJ.jpg', 0, 0),
('SP000331', 'Webcam Microsoft LifeCam Studio Model K', 1300000.00, 80, 'Microsoft', 'Webcam Microsoft LifeCam Studio cho video call chất lượng cao, Model K.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelK.jpg', 0, 0),
('SP000332', 'Webcam Logitech C615 HD 720p Model L', 800000.00, 85, 'Logitech', 'Webcam Logitech C615 HD 720p với khả năng xoay 360°, Model L.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelL.jpg', 0, 0),
('SP000333', 'Webcam Razer Kiyo Pro Model M', 1800000.00, 75, 'Razer', 'Webcam Razer Kiyo Pro cho hình ảnh xuất sắc, Model M.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelM.jpg', 0, 0),
('SP000334', 'Webcam Logitech StreamCam Model N', 1500000.00, 70, 'Logitech', 'Webcam Logitech StreamCam cho livestream chuyên nghiệp, Model N.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"60fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelN.jpg', 0, 0),
('SP000335', 'Webcam AUSDOM AF640 Full HD Model O', 1100000.00, 65, 'AUSDOM', 'Webcam AUSDOM AF640 Full HD với hình ảnh sắc nét, Model O.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelO.jpg', 0, 0),
('SP000336', 'Webcam Logitech Brio 4K Model P', 3000000.00, 55, 'Logitech', 'Webcam Logitech Brio 4K cho hình ảnh chuyên nghiệp, Model P.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelP.jpg', 0, 0),
('SP000337', 'Webcam Creative Live! Cam IP HD Model Q', 850000.00, 90, 'Creative', 'Webcam Creative Live! Cam IP HD cho cuộc gọi trực tuyến, Model Q.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"720p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Fixed\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelQ.jpg', 0, 0),
('SP000338', 'Webcam Dell UltraSharp WB702 Model R', 1400000.00, 78, 'Dell', 'Webcam Dell UltraSharp WB702 cho video call chuyên nghiệp, Model R.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelR.jpg', 0, 0),
('SP000339', 'Webcam Logitech C930e Model S', 1300000.00, 82, 'Logitech', 'Webcam Logitech C930e với góc quay rộng, Model S.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"1080p\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelS.jpg', 0, 0),
('SP000340', 'Webcam Razer Kiyo Ultra Model T', 2200000.00, 80, 'Razer', 'Webcam Razer Kiyo Ultra cho hình ảnh siêu nét, Model T.', '{\"Danh mục\": \"Webcam\", \"Resolution\": \"4K\", \"Frame Rate\": \"30fps\", \"Focus\": \"Auto\", \"Cổng kết nối\": \"USB\"}', 'Peripherals', 'webcam_modelT.jpg', 0, 0),
('SP000341', 'HDD Western Caviar Blue 1TB 7200rpm - Model A', 1350000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model A: Đáng tin cậy, phù hợp cho lưu trữ cá nhân.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelA.jpg', 0, 0),
('SP000342', 'HDD Western Caviar Blue 1TB 7200rpm - Model B', 1355000.00, 20, 'Western', 'HDD 1TB 7200rpm – Model B: Hiệu năng cao cho lưu trữ.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelB.jpg', 0, 0),
('SP000343', 'HDD Western Caviar Blue 1TB 7200rpm - Model C', 1360000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model C: Đáng tin cậy, hiệu năng ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelC.jpg', 0, 0),
('SP000344', 'HDD Western Caviar Blue 1TB 7200rpm - Model D', 1365000.00, 20, 'Western', 'HDD 1TB 7200rpm – Model D: Lựa chọn phổ thông.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelD.jpg', 0, 0),
('SP000345', 'HDD Western Caviar Blue 1TB 7200rpm - Model E', 1370000.00, 21, 'Western', 'HDD 1TB 7200rpm – Model E: Bền bỉ và ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_western_modelE.jpg', 0, 0),
('SP000346', 'HDD Seagate Barracuda 1TB 7200rpm - Model A', 1300000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model A: Hiệu năng ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelA.jpg', 0, 0),
('SP000347', 'HDD Seagate Barracuda 1TB 7200rpm - Model B', 1305000.00, 21, 'Seagate', 'HDD 1TB 7200rpm – Model B: Đáng tin cậy cho văn phòng.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelB.jpg', 0, 0),
('SP000348', 'HDD Seagate Barracuda 1TB 7200rpm - Model C', 1310000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model C: Truyền tải dữ liệu nhanh.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelC.jpg', 0, 0),
('SP000349', 'HDD Seagate Barracuda 1TB 7200rpm - Model D', 1315000.00, 21, 'Seagate', 'HDD 1TB 7200rpm – Model D: Hiệu năng phổ thông.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelD.jpg', 0, 0),
('SP000350', 'HDD Seagate Barracuda 1TB 7200rpm - Model E', 1320000.00, 22, 'Seagate', 'HDD 1TB 7200rpm – Model E: Lựa chọn ưu việt cho lưu trữ.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_seagate_modelE.jpg', 0, 0),
('SP000351', 'HDD Toshiba P300 2TB 7200rpm - Model A', 1280000.00, 23, 'Toshiba', 'HDD 2TB 7200rpm – Model A: Lưu trữ ổn định cho cá nhân.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"2TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelA.jpg', 0, 0),
('SP000352', 'HDD Toshiba P300 10TB 7200rpm - Model B', 1285000.00, 22, 'Toshiba', 'HDD 10TB 7200rpm – Model B: Cho doanh nghiệp nhỏ.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"10TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelB.jpg', 0, 0),
('SP000353', 'HDD Toshiba P300 5TB 7200rpm - Model C', 1290000.00, 23, 'Toshiba', 'HDD 5TB 7200rpm – Model C: Hiệu năng cao cho lưu trữ chuyên sâu.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"5TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelC.jpg', 0, 0),
('SP000354', 'HDD Toshiba P300 1TB 7200rpm - Model D', 1295000.00, 22, 'Toshiba', 'HDD 1TB 7200rpm – Model D: Truyền tải nhanh, tiết kiệm điện.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelD.jpg', 0, 0),
('SP000355', 'HDD Toshiba P300 500GB 7200rpm - Model E', 1300000.00, 23, 'Toshiba', 'HDD 500GB 7200rpm – Model E: Lựa chọn chuyên nghiệp cho doanh nghiệp.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"500GB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"64MB\"}', 'Storage', 'hdd_toshiba_modelE.jpg', 0, 0),
('SP000356', 'HDD HGST Ultrastar 1TB 7200rpm - Model A', 1400000.00, 20, 'HGST', 'HDD 1TB 7200rpm – Model A: Hiệu năng lưu trữ cao.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"1TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelA.jpg', 0, 0),
('SP000357', 'HDD HGST Ultrastar 10TB 7200rpm - Model B', 1405000.00, 19, 'HGST', 'HDD 10TB 7200rpm – Model B: Lưu trữ cho doanh nghiệp.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"10TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelB.jpg', 0, 0),
('SP000358', 'HDD HGST Ultrastar 4TB 7200rpm - Model C', 1410000.00, 20, 'HGST', 'HDD 4TB 7200rpm – Model C: Thiết kế chuyên nghiệp, ổn định.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"4TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelC.jpg', 0, 0),
('SP000359', 'HDD HGST Ultrastar 2TB 7200rpm - Model D', 1415000.00, 19, 'HGST', 'HDD 2TB 7200rpm – Model D: Tốc độ truy xuất nhanh.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"2TB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelD.jpg', 0, 0),
('SP000360', 'HDD HGST Ultrastar 500GB 7200rpm - Model E', 1420000.00, 20, 'HGST', 'HDD 500GB 7200rpm – Model E: Lựa chọn hoàn hảo cho hệ thống cao cấp.', '{\"Danh mục\": \"HDD\", \"Dung lượng\": \"500GB\", \"Tốc độ\": \"7200rpm\", \"Cache\": \"128MB\"}', 'Storage', 'hdd_hgst_modelE.jpg', 0, 0),
('SP000361', 'SSD Samsung 980 PRO 1TB PCIe NVMe - Model A', 2500000.00, 200, 'Samsung', 'SSD NVMe 1TB với hiệu năng cực đỉnh, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\", \"Tốc độ ghi\": \"5000MB/s\"}', 'Storage', 'ssd_samsung_980pro_a.jpg', 0, 0),
('SP000362', 'SSD Samsung 980 PRO 2TB PCIe NVMe - Model B', 4500000.00, 180, 'Samsung', 'SSD NVMe 2TB với hiệu năng cao, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"2TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\", \"Tốc độ ghi\": \"5000MB/s\"}', 'Storage', 'ssd_samsung_980pro_b.jpg', 0, 0),
('SP000363', 'SSD Crucial P5 1TB PCIe NVMe - Model A', 2200000.00, 210, 'Crucial', 'SSD NVMe 1TB với hiệu năng ổn định, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3400MB/s\", \"Tốc độ ghi\": \"2500MB/s\"}', 'Storage', 'ssd_crucial_p5_a.jpg', 0, 0),
('SP000364', 'SSD Crucial MX500 1TB SATA - Model A', 1800000.00, 250, 'Crucial', 'SSD SATA 1TB với hiệu năng đáng tin cậy, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\", \"Tốc độ ghi\": \"530MB/s\"}', 'Storage', 'ssd_crucial_mx500_a.jpg', 0, 0),
('SP000365', 'SSD WD Blue 3D NAND 1TB SATA - Model A', 1900000.00, 230, 'WD', 'SSD SATA 1TB với hiệu năng ổn định, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\", \"Tốc độ ghi\": \"530MB/s\"}', 'Storage', 'ssd_wd_blue_3dnand_a.jpg', 0, 0),
('SP000366', 'SSD WD Black SN750 1TB PCIe NVMe - Model A', 2600000.00, 220, 'WD', 'SSD NVMe 1TB với hiệu năng mạnh mẽ, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3470MB/s\", \"Tốc độ ghi\": \"3000MB/s\"}', 'Storage', 'ssd_wd_black_sn750_a.jpg', 0, 0),
('SP000367', 'SSD Kingston KC3000 1TB PCIe NVMe - Model A', 2700000.00, 210, 'Kingston', 'SSD NVMe 1TB với tốc độ siêu nhanh, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"7000MB/s\", \"Tốc độ ghi\": \"5000MB/s\"}', 'Storage', 'ssd_kingston_kc3000_a.jpg', 0, 0),
('SP000368', 'SSD Kingston A400 480GB SATA - Model A', 1200000.00, 250, 'Kingston', 'SSD SATA 480GB giá rẻ, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"480GB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"500MB/s\", \"Tốc độ ghi\": \"450MB/s\"}', 'Storage', 'ssd_kingston_a400_a.jpg', 0, 0),
('SP000369', 'SSD Crucial MX500 500GB SATA - Model B', 1300000.00, 240, 'Crucial', 'SSD SATA 500GB với độ bền cao, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"500GB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\", \"Tốc độ ghi\": \"520MB/s\"}', 'Storage', 'ssd_crucial_mx500_b.jpg', 0, 0),
('SP000370', 'SSD SanDisk Ultra 3D 1TB SATA - Model A', 1850000.00, 230, 'SanDisk', 'SSD SATA 1TB với hiệu năng ổn định, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"560MB/s\", \"Tốc độ ghi\": \"520MB/s\"}', 'Storage', 'ssd_sandisk_ultra3d_a.jpg', 0, 0),
('SP000371', 'SSD Seagate FireCuda 520 1TB PCIe NVMe - Model A', 2800000.00, 220, 'Seagate', 'SSD NVMe 1TB với tốc độ đọc 5000MB/s, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"5000MB/s\", \"Tốc độ ghi\": \"3500MB/s\"}', 'Storage', 'ssd_seagate_firecuda520_a.jpg', 0, 0),
('SP000372', 'SSD Seagate FireCuda 520 2TB PCIe NVMe - Model B', 4500000.00, 210, 'Seagate', 'SSD NVMe 2TB cao cấp cho gaming và đồ họa, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"2TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"5000MB/s\", \"Tốc độ ghi\": \"3500MB/s\"}', 'Storage', 'ssd_seagate_firecuda520_b.jpg', 0, 0),
('SP000373', 'SSD PNY CS900 480GB SATA - Model A', 1100000.00, 240, 'PNY', 'SSD SATA 480GB với giá phải chăng, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"480GB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"535MB/s\", \"Tốc độ ghi\": \"500MB/s\"}', 'Storage', 'ssd_pny_cs900_a.jpg', 0, 0),
('SP000374', 'SSD Intel 660p 1TB PCIe NVMe - Model A', 2000000.00, 230, 'Intel', 'SSD NVMe 1TB Intel 660p hiệu năng tốt, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"1800MB/s\", \"Tốc độ ghi\": \"1500MB/s\"}', 'Storage', 'ssd_intel_660p_a.jpg', 0, 0),
('SP000375', 'SSD Intel 660p 2TB PCIe NVMe - Model B', 3500000.00, 220, 'Intel', 'SSD NVMe 2TB Intel 660p với dung lượng lớn, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"2TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"1800MB/s\", \"Tốc độ ghi\": \"1500MB/s\"}', 'Storage', 'ssd_intel_660p_b.jpg', 0, 0),
('SP000376', 'SSD ADATA XPG SX8200 Pro 1TB PCIe NVMe - Model A', 2300000.00, 230, 'ADATA', 'SSD NVMe 1TB ADATA XPG SX8200 Pro hiệu năng cao, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3500MB/s\", \"Tốc độ ghi\": \"3000MB/s\"}', 'Storage', 'ssd_adata_xpg_a.jpg', 0, 0),
('SP000377', 'SSD ADATA XPG SX8200 Pro 512GB PCIe NVMe - Model B', 1500000.00, 240, 'ADATA', 'SSD NVMe 512GB ADATA XPG SX8200 Pro với hiệu năng ấn tượng, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"512GB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"3500MB/s\", \"Tốc độ ghi\": \"3000MB/s\"}', 'Storage', 'ssd_adata_xpg_b.jpg', 0, 0),
('SP000378', 'SSD Corsair MP600 1TB PCIe NVMe - Model A', 2600000.00, 230, 'Corsair', 'SSD NVMe 1TB Corsair MP600 với hiệu năng vượt trội, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"4950MB/s\", \"Tốc độ ghi\": \"4500MB/s\"}', 'Storage', 'ssd_corsair_mp600_a.jpg', 0, 0),
('SP000379', 'SSD Corsair MP600 2TB PCIe NVMe - Model B', 4000000.00, 220, 'Corsair', 'SSD NVMe 2TB Corsair MP600 cho các ứng dụng nặng, Model B.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"2TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"PCIe NVMe\", \"Tốc độ đọc\": \"4950MB/s\", \"Tốc độ ghi\": \"4500MB/s\"}', 'Storage', 'ssd_corsair_mp600_b.jpg', 0, 0),
('SP000380', 'SSD Crucial BX500 1TB SATA - Model A', 1700000.00, 240, 'Crucial', 'SSD SATA 1TB Crucial BX500 cho hiệu năng ổn định, Model A.', '{\"Loại ổ cứng\": \"SSD\", \"Dung lượng\": \"1TB\", \"Kích thước\": \"2.5 inch\", \"Chuẩn kết nối\": \"SATA\", \"Tốc độ đọc\": \"540MB/s\", \"Tốc độ ghi\": \"500MB/s\"}', 'Storage', 'ssd_crucial_bx500_a.jpg', 0, 0),
('SP000421', 'Laptop MSI Raider Fury', 32990000.00, 10, 'MSI', 'Laptop MSI Raider Fury kết hợp thiết kế sang trọng và hiệu năng cao.', '{\r\n      \"Nhu cầu\": \"Gaming\",\r\n      \"CPU\": \"Intel Core i7-12700H\",\r\n      \"RAM\": \"16GB DDR4\",\r\n      \"VGA\": \"RTX 3070\",\r\n      \"HDD\": \"1TB\",\r\n      \"SSD\": \"1TB NVMe\",\r\n      \"Màn hình\": \"15.6 inch FHD 144Hz\",\r\n      \"Cổng giao tiếp\": \"HDMI, USB-C\",\r\n      \"Bàn phím\": \"Backlit\",\r\n      \"Chuẩn LAN\": \"Gigabit\",\r\n      \"Chuẩn Wifi\": \"WiFi 6\",\r\n      \"Bluetooth\": \"5.0\",\r\n      \"Webcam\": \"720p\",\r\n      \"Hệ điều hành\": \"Windows 11\",\r\n      \"Pin\": \"76Wh\",\r\n      \"Trọng lượng\": \"2.2kg\",\r\n      \"Màu sắc\": \"Đen\",\r\n      \"Kích thước\": \"357 x 235 x 18 mm\"\r\n  }', 'Laptop', 'msi_raider_fury.jpg', 0, 0);

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
('TK000020', 'user19', 'user19', '2025-01-30', NULL, 'khachhang'),
('TK00021', 'nupniichan089@', 'nupniichan', '2025-02-19', NULL, 'khachhang'),
('TK230104', '230104', 'thepinkcat', '2025-02-21', NULL, 'khachhang');

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
  ADD CONSTRAINT `FK_ChiTietGioHang_GioHang` FOREIGN KEY (`IdGh`) REFERENCES `giohang` (`IdGh`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_fk_1` FOREIGN KEY (`IdGh`) REFERENCES `giohang` (`IdGh`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_fk_2` FOREIGN KEY (`IdSp`) REFERENCES `sanpham` (`IdSp`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietgiohang_ibfk_2` FOREIGN KEY (`IdSp`) REFERENCES `sanpham` (`IdSp`) ON DELETE CASCADE;

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
