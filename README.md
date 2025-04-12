<div align="center">

# Computer Parts E-Commerce Website

![E-Commerce](https://img.shields.io/badge/E--Commerce-Online-brightgreen)
![.NET](https://img.shields.io/badge/.NET-9.0-blue)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![MySQL](https://img.shields.io/badge/Database-MariaDB-orange)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

A complete e-commerce solution for computer parts retailing, including both user (customer) and admin (management) components. This project is built on ASP.NET Core and containerized with Docker for easy deployment.
</div>

## Project Structure

The project consists of these main components:

- **User-WBLK**: Customer-facing application for browsing products, placing orders, and payments
- **Admin-WBLK**: Administration application for managing products, orders, and user accounts
- **Database-MySql**: MySQL/MariaDB database for data storage
- **Docker**: Docker configuration for deploying the entire system

## Key Features

### User Portal
- Product catalog browsing
- User account management (local or social login via Google, Facebook)
- Shopping cart functionality
- Order placement and payment processing (MoMo, PayPal)
- Order history
- Personal profile management
- ...

### Admin Portal
- Administrative login
- Product management
- Order management
- User account management
- Customer request handling
- ...

## System Requirements

- Docker and Docker Compose
- .NET 9.0 SDK (for development)

## Installation and Deployment

### Using Docker Compose (Recommended)

This is the simplest way to run the entire system:

```bash
# Deploy all services
docker compose up -d --build

# Restart if needed
docker compose restart
```

### Manual Deployment (.NET Core)

#### Setup Database
```bash
# Option 1: Using XAMPP
# 1. Install XAMPP from https://www.apachefriends.org/
# 2. Start MySQL service from XAMPP Control Panel
# 3. Create database and import schema:
#    - Open phpMyAdmin (http://localhost/phpmyadmin)
#    - Create a new database named "WebBanLinhKien"
#    - Import ./Database-MySql/webbanlinhkien.sql file
#    - Create a user WBLK_USER with password Wblk@TMDT2025 with full privileges

# Option 2: Using MySQL Workbench
# 1. Install MySQL Community Server from https://dev.mysql.com/downloads/
# 2. Connect to your MySQL server using MySQL Workbench
# 3. Create a new database named "WebBanLinhKien"
# 4. Import schema from ./Database-MySql/webbanlinhkien.sql
# 5. Create a user WBLK_USER with password Wblk@TMDT2025
```

#### Configure Connection Strings
Edit the connection string in appsettings.json files for both User-WBLK and Admin-WBLK projects to point to your local MySQL instance, for example:
```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=WebBanLinhKien;user=WBLK_USER;password=Wblk@TMDT2025"
}
```

#### Run User Portal Application
```bash
# Navigate to User-WBLK directory
cd User-WBLK

# Restore packages
dotnet restore

# Run the application
dotnet run
```

#### Run Admin Portal Application
```bash
# Navigate to Admin-WBLK directory
cd Admin-WBLK

# Restore packages
dotnet restore

# Run the application
dotnet run
```

## Accessing Applications

After deployment:

- **User Portal**: http://localhost:5124 and https://localhost:7050
- **Admin Portal**: http://localhost:5177 and https://localhost:7012
- **Nginx Proxy Manager**: http://localhost:81 (admin panel)

## Configuration and Connection Details

### Database
- Server: mariadb-database-container
- Database: WebBanLinhKien
- User: WBLK_USER
- Password: Wblk@TMDT2025

### Ports and Endpoints
- User Portal: 5124 (HTTP), 7050 (HTTPS)
- Admin Portal: 5177 (HTTP), 7012 (HTTPS)
- Database: 3306
- Nginx: 80 (HTTP), 443 (HTTPS), 81 (admin)

## Payment Integration

### MoMo
MoMo payment API is integrated, configured in User-WBLK/appsettings.json

### PayPal
PayPal payment API is integrated, configured in User-WBLK/appsettings.json

## Social Login

The system supports login via:
- Google
- Facebook

## Development

To develop this project, you need:
1. Install .NET 9.0 SDK
2. Install Docker and Docker Compose
3. Clone the repository and open in your preferred IDE (Visual Studio, VS Code...)
4. Run the database using Docker
5. Run the applications in development mode

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributors

Thanks to all the amazing people who have contributed to this project:

- [Trần Hồng Phát](https://github.com/ThePinkKitten)
- [Lê Nguyễn Hoàng Thanh](https://github.com/KevzCz)

Feel free to contribute to this project by submitting issues or pull requests on our GitHub repository. 
