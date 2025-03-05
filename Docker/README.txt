Cách chạy docker của user/admin và mysql ( Sử dụng dockerfile )

1. Http only (dùng để test server/test ping sql/test là chính/...): 
================ mysql ================
docker build -t mysql-database-image .
docker run --name mysql-database-container -dp 3306:3306 mysql-database-image
docker exec -it mysql-database-container mysql -u root -p

//password: root
//Check xem đã có SQL chưa?

use WebBanLinkKien
SHOW TABLES;

================ user ================
docker build -t user-wblk-image .

# Chạy http ( Khỏi chứng chỉ gì hết )

docker run --name user-wblk-container -d -p 5124:5124 -e ASPNETCORE_URLS="http://+:5124" user-wblk-image

# Check xem có kết nối sql chưa?

docker exec -it user-wblk-container sh

apt update

apt install -y netcat-openbsd

nc -zv mysql-database-container 3306

# Nếu vẫn không ok?

docker stop user-wblk-container
docker rm user-wblk-container
docker build -t user-wblk-image .

docker run --name user-wblk-container -d --network webbanlinhkien -p 5124:5124 -e ASPNETCORE_URLS="http://+:5124" -e =ConnectionStrings__DefaultConnection="Server=mysql-database-container;Database=WebBanLinhKien;User=root;Password=root" user-wblk-image

================ admin ================

docker build -t admin-wblk-image .

docker run --name admin-wblk-container -d --network webbanlinhkien -p 5177:5177 -e ASPNETCORE_URLS="http://+:5177" -e ConnectionStrings__DefaultConnection="Server=mysql-database-container;Database=WebBanLinhKien;User=root;Password=root" admin-wblk-image



================ Setup network ================
Vì sao cần cái của nợ này? Đơn giản là để 2 cái container có kể giao tiếp đc với nhau
docker network create webbanlinhkien
docker network connect webbanlinhkien mysql-database-container
docker network connect webbanlinhkien user-wblk-container
docker network connect webbanlinhkien admin-wblk-container



2. Https (sau khi áp dụng thành công http):

================ Setup SSL (Admin/User) ================

dotnet dev-certs https --trust

Ấn Yes

dotnet dev-certs https -ep "D:\Documents\test\Web\TMDT\Website-Ban-Linh-Kien\Docker\aspnetapp.pfx" -p "root"

# Xóa container cũ đi để dùng container mới mà không bị conflict (xóa trong app/lệnh đều ok, ở đây xóa bằng app)

# Sau đấy run lại lần nữa: (User)

docker run --name user-wblk-container -d --network webbanlinhkien -p 5124:5124 -p 7050:7050 -v "D:\Documents\test\Web\TMDT\Website-Ban-Linh-Kien\Docker:/https:ro" -e ASPNETCORE_URLS="http://+:5124;https://+:7050" -e ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx" -e ASPNETCORE_Kestrel__Certificates__Default__Password="root" user-wblk-image

#                         (Admin)

docker run --name admin-wblk-container -d --network webbanlinhkien -p 5177:5177 -p 7012:7012 -v "D:\Documents\test\Web\TMDT\Website-Ban-Linh-Kien\Docker:/https:ro" -e ASPNETCORE_URLS="http://+:5177;https://+:7012" -e ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx" -e ASPNETCORE_Kestrel__Certificates__Default__Password="root" admin-wblk-image

Done!

3. Cách chạy docker của user/admin và mysql ( Sử dụng dockercompose )

docker compose up -d


docker compose restart
