FROM mariadb:10.4

ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_DATABASE=WebBanLinhKien

COPY ./webbanlinhkien.sql /docker-entrypoint-initdb.d/

EXPOSE 3306

CMD ["mysqld"]