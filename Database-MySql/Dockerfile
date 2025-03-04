FROM mysql:latest

ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_DATABASE=WebBanLinhKien
COPY ./webbanlinhkien.sql /docker-entrypoint-initdb.d/

EXPOSE 3306

# Cach chay:
# docker build -t mysql-database-image .
# docker run --name mysql-database-container -dp 3306:3306 mysql-database-image
# docker exec -it mysql-database-container mysql -u root -p
