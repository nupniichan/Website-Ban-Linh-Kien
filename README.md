# Website Bán Linh Kiện (Computer Parts E-commerce)

Dự án Website Bán Linh Kiện là một ứng dụng e-commerce hoàn chỉnh cho cửa hàng bán linh kiện máy tính, bao gồm cả phần user (khách hàng) và admin (quản trị). Dự án được xây dựng trên nền tảng ASP.NET Core và sử dụng Docker để triển khai.

## Cấu trúc dự án

Dự án được chia thành các phần chính:

- **User-WBLK**: Ứng dụng dành cho khách hàng, cho phép xem sản phẩm, đặt hàng, thanh toán...
- **Admin-WBLK**: Ứng dụng quản trị viên, quản lý sản phẩm, đơn hàng, tài khoản...
- **Database-MySql**: Cơ sở dữ liệu MySQL/MariaDB
- **Docker**: Cấu hình Docker để triển khai toàn bộ hệ thống

## Tính năng chính

### User-WBLK
- Hiển thị danh sách sản phẩm
- Đăng nhập/đăng ký tài khoản (local hoặc thông qua Google, Facebook)
- Quản lý giỏ hàng
- Đặt hàng và thanh toán (hỗ trợ thanh toán qua MoMo, PayPal)
- Xem lịch sử đơn hàng
- Quản lý thông tin cá nhân

### Admin-WBLK
- Đăng nhập quản trị
- Quản lý sản phẩm
- Quản lý đơn hàng
- Quản lý tài khoản người dùng
- Xử lý các yêu cầu từ người dùng

## Yêu cầu hệ thống

- Docker và Docker Compose
- .NET 9.0 SDK (để phát triển)

## Cài đặt và triển khai

### Sử dụng Docker Compose (Khuyến nghị)

Đây là cách đơn giản nhất để chạy toàn bộ hệ thống:

```bash
# Triển khai tất cả các dịch vụ
docker compose up -d --build

# Khởi động lại nếu cần
docker compose restart
```

### Triển khai thủ công (HTTP only)

#### Cài đặt cơ sở dữ liệu
```bash
cd Database-MySql
docker build -t mysql-database-image .
docker run --name mysql-database-container -dp 3306:3306 mysql-database-image
```

#### Tạo network
```bash
docker network create webbanlinhkien
docker network connect webbanlinhkien mysql-database-container
```

#### Triển khai User-WBLK
```bash
cd User-WBLK
docker build -t user-wblk-image .
docker run --name user-wblk-container -d --network webbanlinhkien -p 5124:5124 -e ASPNETCORE_URLS="http://+:5124" user-wblk-image
docker network connect webbanlinhkien user-wblk-container
```

#### Triển khai Admin-WBLK
```bash
cd Admin-WBLK
docker build -t admin-wblk-image .
docker run --name admin-wblk-container -d --network webbanlinhkien -p 5177:5177 -e ASPNETCORE_URLS="http://+:5177" admin-wblk-image
docker network connect webbanlinhkien admin-wblk-container
```

### Thiết lập HTTPS (Sau khi đã thiết lập HTTP thành công)

```bash
# Tạo chứng chỉ SSL
dotnet dev-certs https --trust
dotnet dev-certs https -ep "Docker/aspnetapp.pfx" -p "root"

# Chạy User-WBLK với HTTPS
docker run --name user-wblk-container -d --network webbanlinhkien -p 5124:5124 -p 7050:7050 -v "Docker:/https:ro" -e ASPNETCORE_URLS="http://+:5124;https://+:7050" -e ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx" -e ASPNETCORE_Kestrel__Certificates__Default__Password="root" user-wblk-image

# Chạy Admin-WBLK với HTTPS
docker run --name admin-wblk-container -d --network webbanlinhkien -p 5177:5177 -p 7012:7012 -v "Docker:/https:ro" -e ASPNETCORE_URLS="http://+:5177;https://+:7012" -e ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx" -e ASPNETCORE_Kestrel__Certificates__Default__Password="root" admin-wblk-image
```

## Truy cập ứng dụng

Sau khi triển khai:

- **User-WBLK (Trang người dùng)**: http://localhost:5124 và https://localhost:7050
- **Admin-WBLK (Trang quản trị)**: http://localhost:5177 và https://localhost:7012
- **Nginx Proxy Manager**: http://localhost:81 (admin panel)

## Cấu hình và thông tin kết nối

### Cơ sở dữ liệu
- Server: mariadb-database-container
- Database: WebBanLinhKien
- User: WBLK_USER
- Password: Wblk@TMDT2025

### Cổng và endpoints
- User-WBLK: 5124 (HTTP), 7050 (HTTPS)
- Admin-WBLK: 5177 (HTTP), 7012 (HTTPS)
- Database: 3306
- Nginx: 80 (HTTP), 443 (HTTPS), 81 (admin)

## Tích hợp thanh toán

### MoMo
Đã tích hợp sẵn API thanh toán MoMo, cấu hình trong file User-WBLK/appsettings.json

### PayPal
Đã tích hợp sẵn API thanh toán PayPal, cấu hình trong file User-WBLK/appsettings.json

## Đăng nhập xã hội

Hệ thống hỗ trợ đăng nhập qua:
- Google
- Facebook

## Phát triển

Để phát triển dự án, bạn cần:
1. Cài đặt .NET 9.0 SDK
2. Cài đặt Docker và Docker Compose
3. Clone repository và mở trong IDE ưa thích (Visual Studio, VS Code...)
4. Chạy cơ sở dữ liệu bằng Docker
5. Chạy các ứng dụng trong chế độ development

## Giấy phép

© Website-Ban-Linh-Kien - Bạn có thể sử dụng mã nguồn này cho mục đích học tập và phi thương mại. 