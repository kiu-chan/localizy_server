# Localizy Server

REST API backend cho hệ thống quản lý địa điểm và xác thực thông tin địa lý.

## 📋 Mục lục
- [Giới thiệu](#giới-thiệu)
- [Tính năng](#tính-năng)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Kiến trúc](#kiến-trúc)
- [Cài đặt](#cài-đặt)
- [Cấu hình](#cấu-hình)
- [Chạy dự án](#chạy-dự-án)
- [Testing](#testing)
- [API Documentation](#api-documentation)
- [Deployment](#deployment)

---

## 🎯 Giới thiệu

**Localizy Server** là REST API được xây dựng bằng .NET 10, theo kiến trúc Clean Architecture. Hệ thống cung cấp các API để quản lý địa điểm, xác thực thông tin địa lý, và quản lý người dùng với phân quyền chi tiết.

### Demo
- **API Base URL**: `http://localhost:5088/api`
- **Swagger UI**: `http://localhost:5088/swagger`

### Tài khoản mặc định
| Role  | Email                 | Password   |
|-------|-----------------------|------------|
| Admin | admin@localizy.com    | Admin@123  |
| User  | user@localizy.com     | User@123   |

---

## ✨ Tính năng

### 🔐 Authentication & Authorization
- JWT Bearer token authentication
- Role-based access control (Admin, User)
- Password hashing với BCrypt
- Token expiration & refresh

### 👥 User Management
- CRUD operations cho users
- User statistics & analytics
- Search & filter users
- Toggle user status (Active/Suspended)
- Change password
- Role management

### 📍 Address Management
- CRUD operations cho addresses
- Address verification system
- Search & filter by status, type, location
- View tracking
- Rating system
- Coordinates (latitude/longitude)

### ✅ Validation System
- Validation request workflow
- Priority management (Low, Medium, High)
- Request types: NewAddress, UpdateInformation, DeleteRequest
- Verification & rejection with notes
- Auto-generate request IDs
- Track validation history

### ⚙️ Settings Management
- Dynamic website configuration
- Categories: AppDownload, SocialMedia, Contact, General
- Public access for website config
- Admin-only for updates

---

## 🛠 Công nghệ sử dụng

### Backend Framework
- **.NET 10** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API

### Database & ORM
- **SQL Server 2022** - Relational database
- **Entity Framework Core 10** - ORM
- **Code-First Migrations** - Database versioning

### Authentication & Security
- **JWT Bearer Authentication** - Token-based auth
- **BCrypt.Net-Next** - Password hashing
- **CORS** - Cross-origin resource sharing

### Development Tools
- **Swagger/OpenAPI** - API documentation
- **DotNetEnv** - Environment variables
- **Docker** - Containerization (SQL Server)

### Libraries
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT auth
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Microsoft.EntityFrameworkCore.Tools` - EF Core CLI tools
- `Swashbuckle.AspNetCore` - Swagger UI
- `BCrypt.Net-Next` - Password hashing
- `DotNetEnv` - Environment config

---

## 🏗 Kiến trúc

### Clean Architecture Layers

```
localizy_server/
├── src/
│   ├── Localizy.Domain/              # Enterprise Business Rules
│   │   ├── Entities/                 # Domain entities
│   │   │   ├── User.cs
│   │   │   ├── Address.cs
│   │   │   ├── Validation.cs
│   │   │   ├── Setting.cs
│   │   │   └── BaseEntity.cs
│   │   └── Enums/                    # Domain enums
│   │       ├── UserRole.cs
│   │       ├── AddressStatus.cs
│   │       ├── ValidationStatus.cs
│   │       └── ValidationPriority.cs
│   │
│   ├── Localizy.Application/         # Application Business Rules
│   │   ├── Common/
│   │   │   ├── Interfaces/           # Repository interfaces
│   │   │   └── Models/               # Shared models
│   │   └── Features/                 # Feature modules
│   │       ├── Auth/
│   │       │   ├── DTOs/
│   │       │   └── Services/
│   │       ├── Users/
│   │       │   ├── DTOs/
│   │       │   └── Services/
│   │       ├── Addresses/
│   │       │   ├── DTOs/
│   │       │   └── Services/
│   │       ├── Validations/
│   │       │   ├── DTOs/
│   │       │   └── Services/
│   │       └── Settings/
│   │           ├── DTOs/
│   │           └── Services/
│   │
│   ├── Localizy.Infrastructure/      # Infrastructure
│   │   ├── Persistence/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── DataSeeder.cs
│   │   │   └── Repositories/         # Repository implementations
│   │   └── Services/
│   │       └── JwtService.cs
│   │
│   └── Localizy.API/                 # Presentation
│       ├── Controllers/              # API controllers
│       │   ├── AuthController.cs
│       │   ├── UsersController.cs
│       │   ├── AddressesController.cs
│       │   ├── ValidationsController.cs
│       │   └── SettingsController.cs
│       ├── Properties/
│       ├── appsettings.json
│       └── Program.cs
│
├── .env                              # Environment variables
├── .gitignore
├── README.md
├── API_DOCUMENTATION.md
└── Localizy.sln                      # Solution file
```

### Dependencies Flow

```
API (Presentation)
    ↓
Application (Business Logic)
    ↓
Infrastructure (Data Access)
    ↓
Domain (Entities & Rules)
```

**Dependency Rule**: Inner layers have no knowledge of outer layers.

---

## 🚀 Cài đặt

### Prerequisites

Đảm bảo đã cài đặt:
- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- [SQL Server 2022](https://www.microsoft.com/sql-server) hoặc [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

### Bước 1: Clone Repository

```bash
git clone <repository-url>
cd localizy_server
```

### Bước 2: Restore Dependencies

```bash
dotnet restore
```

### Bước 3: Setup Database

#### Option 1: Sử dụng SQL Server có sẵn
Bỏ qua bước này nếu đã có SQL Server.

#### Option 2: Chạy SQL Server với Docker
```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd123" \
  -p 1433:1433 \
  --name sqlserver2022 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Kiểm tra container đang chạy:
```bash
docker ps
```

### Bước 4: Cài đặt EF Core Tools

```bash
dotnet tool install --global dotnet-ef
# hoặc update nếu đã có
dotnet tool update --global dotnet-ef
```

---

## ⚙️ Cấu hình

### 1. Environment Variables

Tạo file `.env` ở thư mục gốc dự án:

```env
# Database Configuration
DB_SERVER=localhost,1433
DB_DATABASE=LocalizyDb
DB_USER_ID=sa
DB_PASSWORD=YourStrong@Passw0rd123

# JWT Configuration
JWT_SECRET=your-super-secret-key-at-least-32-characters-long-for-security
JWT_ISSUER=LocalizyAPI
JWT_AUDIENCE=LocalizyClient
JWT_EXPIRATION_MINUTES=1440

# CORS Configuration
CORS_ORIGINS=http://localhost:5173,http://localhost:3000,http://localhost:4200
```

### 2. appsettings.json

File `src/Localizy.API/appsettings.json` sẽ được override bởi `.env`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=LocalizyDb;..."
  },
  "JwtSettings": {
    "Secret": "...",
    "Issuer": "LocalizyAPI",
    "Audience": "LocalizyClient",
    "ExpirationInMinutes": 1440
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3. Database Migration

```bash
cd src/Localizy.API

# Tạo migration (nếu chưa có)
dotnet ef migrations add InitialCreate \
  --project ../Localizy.Infrastructure/Localizy.Infrastructure.csproj

# Apply migration và tạo database
dotnet ef database update
```

**Note**: Lệnh `database update` sẽ tự động:
- Tạo database nếu chưa tồn tại
- Chạy tất cả migrations
- Seed dữ liệu mặc định (admin user, sample data)

---

## 🏃 Chạy dự án

### Development Mode

```bash
cd src/Localizy.API
dotnet run
```

Server sẽ chạy tại:
- **HTTP**: `http://localhost:5088`
- **Swagger UI**: `http://localhost:5088/swagger`

### Watch Mode (Auto-reload)

```bash
dotnet watch run
```

### Production Mode

```bash
dotnet run --configuration Release
```

---

## 🧪 Testing

### 1. Sử dụng Swagger UI

Truy cập: `http://localhost:5088/swagger`

**Các bước test:**
1. Đăng nhập để lấy token (POST `/api/auth/login`)
2. Click button **Authorize** ở góc phải trên
3. Nhập: `Bearer {your-token}`
4. Click **Authorize**
5. Test các endpoints

### 2. Sử dụng cURL

**Đăng nhập:**
```bash
curl -X POST http://localhost:5088/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@localizy.com","password":"Admin@123"}'
```

**Lấy danh sách users:**
```bash
curl http://localhost:5088/api/users \
  -H "Authorization: Bearer {your-token}"
```

### 3. Sử dụng Postman

1. Import Swagger JSON: `http://localhost:5088/swagger/v1/swagger.json`
2. Tạo Environment với variable `token`
3. Set Authorization: `Bearer {{token}}`

### 4. Unit Tests (Coming soon)

```bash
dotnet test
```

---

## 📚 API Documentation

Chi tiết đầy đủ về API endpoints, xem file: **[API_DOCUMENTATION.md](./API_DOCUMENTATION.md)**

### Quick Links:
- [Auth APIs](./API_DOCUMENTATION.md#auth-apis) - Đăng ký, đăng nhập
- [User APIs](./API_DOCUMENTATION.md#user-apis) - Quản lý người dùng
- [Address APIs](./API_DOCUMENTATION.md#address-apis) - Quản lý địa điểm
- [Validation APIs](./API_DOCUMENTATION.md#validation-apis) - Xác thực địa điểm
- [Setting APIs](./API_DOCUMENTATION.md#setting-apis) - Cấu hình website

### API Summary:

| Module       | Endpoints | Public | Auth Required | Admin Only |
|--------------|-----------|--------|---------------|------------|
| Auth         | 2         | ✅     | ❌            | ❌         |
| Users        | 11        | ❌     | ✅            | ✅         |
| Addresses    | 13        | Partial| ✅            | Partial    |
| Validations  | 14        | ❌     | ✅            | Partial    |
| Settings     | 5         | 1      | ❌            | ✅         |
| **Total**    | **45**    | -      | -             | -          |

---

## 🚀 Deployment

### Docker Deployment (Recommended)

**1. Tạo Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Localizy.API.dll"]
```

**2. Build & Run:**
```bash
docker build -t localizy-server .
docker run -p 5088:80 localizy-server
```

### Azure Deployment

```bash
# Login to Azure
az login

# Create resource group
az group create --name LocalizyRG --location eastasia

# Create app service
az webapp up --name localizy-api --resource-group LocalizyRG
```

### IIS Deployment

1. Publish project:
```bash
dotnet publish -c Release -o ./publish
```

2. Copy `./publish` folder to IIS server
3. Configure IIS application pool (.NET CLR version: No Managed Code)
4. Set environment variables in IIS

---

## 📝 Database Schema

### Main Tables:
- **Users** - User accounts & profiles
- **Addresses** - Location data
- **Validations** - Validation requests
- **Settings** - System configuration
- **Projects** - User projects (future)
- **Translations** - Multi-language support (future)

### Relationships:
```
Users (1) ─────── (*) Addresses
  │                      │
  │                      │
  └──────── (*) Validations (*) ──┘
```

---

## 🔧 Troubleshooting

### Common Issues:

**1. Database connection failed**
```
Solution: Kiểm tra SQL Server đang chạy, check connection string trong .env
```

**2. Migration error**
```bash
# Reset database
dotnet ef database drop --force
dotnet ef database update
```

**3. JWT token invalid**
```
Solution: Kiểm tra JWT_SECRET trong .env, đảm bảo >= 32 ký tự
```

**4. CORS error**
```
Solution: Thêm origin của frontend vào CORS_ORIGINS trong .env
```

---

## 🤝 Contributing

### Workflow:
1. Fork repository
2. Create feature branch: `git checkout -b feature/AmazingFeature`
3. Commit changes: `git commit -m 'Add AmazingFeature'`
4. Push to branch: `git push origin feature/AmazingFeature`
5. Open Pull Request

### Code Standards:
- Follow Clean Architecture principles
- Use meaningful variable/method names
- Add XML documentation comments
- Write unit tests for business logic
- Update API documentation

---

## 📄 License

Copyright © 2024 Localizy. All rights reserved.

---

## 📞 Contact

- **Email**: contact@localizy.com
- **Website**: https://localizy.com
- **GitHub**: [Repository URL]

---

## 🙏 Acknowledgments

- [.NET](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [JWT](https://jwt.io/)
- [Swagger](https://swagger.io/)

---

## 📈 Roadmap

- [ ] Unit & Integration Tests
- [ ] CI/CD Pipeline
- [ ] Rate Limiting
- [ ] Caching (Redis)
- [ ] File Upload (Images)
- [ ] Email Notifications
- [ ] Real-time Updates (SignalR)
- [ ] GraphQL Support
- [ ] Multi-language Support
- [ ] Analytics Dashboard