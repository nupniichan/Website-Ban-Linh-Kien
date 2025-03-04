Cách chạy docker của user/admin và mysql ( Sử dụng dockerfile )
================ mysql ================
docker build -t mysql-database-image .
docker run --name mysql-database-container -dp 3306:3306 mysql-database-image
docker exec -it mysql-database-container mysql -u root -p

================ user ================
docker build -t user-wblk-image .

# Chạy cả http và https ( yêu cầu ssl cho https )
# docker run --name user-wblk-container -dp 5124:5124 -dp 7050:7050 user-wblk-image

# Chạy http ( Khỏi chứng chỉ gì hết )
# docker run --name user-wblk-container -d -p 5124:5124 -e ASPNETCORE_URLS="http://+:5124" user-wblk-image

================ admin ================
...

================ Setup network ================
Vì sao cần cái của nợ này? Đơn giản là để 2 cái container có kể giao tiếp đc với nhau
docker network create webbanlinhkien
docker network connect webbanlinhkien mysql-database-container
docker network connect webbanlinhkien user-wblk-container
docker network connect webbanlinhkien admin-wblk-container

Cách chạy docker của user/admin và mysql ( Sử dụng dockercompose )
Từ từ làm sau