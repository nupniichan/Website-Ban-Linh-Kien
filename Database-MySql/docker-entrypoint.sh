#!/bin/bash
set -e

if [ ! -d "/var/lib/mysql/$MYSQL_DATABASE" ]; then
    echo "Initializing new database..."
    cp /docker-entrypoint-initdb.d/webbanlinhkien.sql /tmp/init.sql
else
    echo "Database already exists, skipping initialization."
fi

exec docker-entrypoint.sh mysqld