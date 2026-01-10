# Localizy Server API Documentation

## 📋 Mục lục
- [Giới thiệu](#giới-thiệu)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Cài đặt & Chạy](#cài-đặt--chạy)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
  - [Auth APIs](#auth-apis)
  - [User APIs](#user-apis)
  - [Address APIs](#address-apis)
  - [Validation APIs](#validation-apis)
  - [Setting APIs](#setting-apis)
- [Error Handling](#error-handling)
- [Testing](#testing)

---

## 🎯 Giới thiệu

**Localizy Server** là REST API backend cho hệ thống quản lý địa điểm và xác thực thông tin địa lý. API được xây dựng theo kiến trúc Clean Architecture với .NET 10.

### Tính năng chính:
- 🔐 Xác thực JWT với role-based authorization
- 👥 Quản lý người dùng (User & Admin)
- 📍 Quản lý địa điểm (CRUD + Verification)
- ✅ Hệ thống validation requests
- ⚙️ Cấu hình website động
- 🔍 Tìm kiếm & lọc dữ liệu

---

## 🛠 Công nghệ sử dụng

- **.NET 10** - Framework chính
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **JWT Bearer** - Authentication
- **BCrypt.Net** - Password hashing
- **Swagger/OpenAPI** - API Documentation

### Architecture:
```
├── Localizy.Domain        # Entities, Enums
├── Localizy.Application   # Business Logic, DTOs, Interfaces
├── Localizy.Infrastructure # Data Access, Repositories
└── Localizy.API           # Controllers, Middleware
```

---

## 🚀 Cài đặt & Chạy

### Prerequisites:
- .NET SDK 10.0+
- SQL Server (hoặc Docker)
- Git

### Bước 1: Clone repository
```bash
git clone <repository-url>
cd localizy_server
```

### Bước 2: Cấu hình Database
Tạo file `.env` ở thư mục gốc:
```env
DB_SERVER=localhost,1433
DB_DATABASE=LocalizyDb
DB_USER_ID=sa
DB_PASSWORD=YourStrong@Passw0rd123

JWT_SECRET=your-super-secret-key-at-least-32-characters-long
JWT_ISSUER=LocalizyAPI
JWT_AUDIENCE=LocalizyClient
JWT_EXPIRATION_MINUTES=1440

CORS_ORIGINS=http://localhost:5173,http://localhost:3000
```

### Bước 3: Chạy SQL Server với Docker (optional)
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd123" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

### Bước 4: Build & Run
```bash
cd src/Localizy.API
dotnet restore
dotnet ef database update
dotnet run
```

Server sẽ chạy tại: `http://localhost:5088`

Swagger UI: `http://localhost:5088/swagger`

### Tài khoản mặc định:
- **Admin**: `admin@localizy.com` / `Admin@123`
- **User**: `user@localizy.com` / `User@123`

---

## 🔐 Authentication

### JWT Token
API sử dụng JWT Bearer token để xác thực. Token có thời gian hết hạn 24 giờ (mặc định).

### Cách sử dụng:
1. Đăng nhập để lấy token
2. Thêm token vào header của các request tiếp theo:
```
Authorization: Bearer {your-token}
```

### Roles:
- **User**: Người dùng thông thường
- **Admin**: Quản trị viên (có quyền cao nhất)

---

## 📚 API Endpoints

### Base URL: `http://localhost:5088/api`

---

## 🔑 Auth APIs

### 1. Đăng ký
```http
POST /api/auth/register
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "fullName": "Nguyen Van A",
  "password": "Password123"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "fullName": "Nguyen Van A",
  "role": "User",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

### 2. Đăng nhập
```http
POST /api/auth/login
```

**Request Body:**
```json
{
  "email": "admin@localizy.com",
  "password": "Admin@123"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@localizy.com",
  "fullName": "System Administrator",
  "role": "Admin",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

## 👥 User APIs

### 1. Lấy thống kê users
```http
GET /api/users/stats
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
{
  "totalUsers": 100,
  "activeUsers": 85,
  "suspendedUsers": 10,
  "inactiveUsers": 5,
  "adminUsers": 2,
  "validatorUsers": 5,
  "businessUsers": 8,
  "regularUsers": 85
}
```

---

### 2. Tìm kiếm users
```http
GET /api/users/search?searchTerm=john
Authorization: Bearer {admin-token}
```

---

### 3. Lấy tất cả users
```http
GET /api/users
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "fullName": "John Smith",
    "phone": "+84 123 456 789",
    "location": "Hanoi, Vietnam",
    "avatar": null,
    "isActive": true,
    "role": "User",
    "lastLoginAt": "2024-01-10T10:30:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "totalAddresses": 12,
    "verifiedAddresses": 10
  }
]
```

---

### 4. Lấy user theo ID
```http
GET /api/users/{id}
Authorization: Bearer {token}
```

---

### 5. Tạo user mới
```http
POST /api/users
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "email": "newuser@example.com",
  "fullName": "New User",
  "password": "Password123",
  "phone": "+84 987 654 321",
  "location": "Ho Chi Minh, Vietnam",
  "role": "User"
}
```

---

### 6. Cập nhật user
```http
PUT /api/users/{id}
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "fullName": "Updated Name",
  "phone": "+84 999 888 777",
  "location": "Da Nang, Vietnam",
  "isActive": true,
  "role": "Admin"
}
```

---

### 7. Xóa user
```http
DELETE /api/users/{id}
Authorization: Bearer {admin-token}
```

**Response:** `204 No Content`

---

### 8. Toggle trạng thái user
```http
PATCH /api/users/{id}/toggle-status
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
{
  "message": "Đã cập nhật trạng thái user"
}
```

---

### 9. Đổi mật khẩu
```http
POST /api/users/{id}/change-password
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "currentPassword": "OldPassword123",
  "newPassword": "NewPassword456"
}
```

---

### 10. Lọc users theo role
```http
GET /api/users/filter/role/{role}
Authorization: Bearer {admin-token}
```

**Roles:** `Admin`, `User`, `Validator`, `Business`

---

### 11. Lọc users theo status
```http
GET /api/users/filter/status?isActive=true
Authorization: Bearer {admin-token}
```

---

## 📍 Address APIs

### 1. Lấy thống kê addresses
```http
GET /api/addresses/stats
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
{
  "totalAddresses": 250,
  "verifiedAddresses": 200,
  "pendingAddresses": 35,
  "rejectedAddresses": 15,
  "totalViews": 125000,
  "averageRating": 4.5
}
```

---

### 2. Tìm kiếm addresses
```http
GET /api/addresses/search?searchTerm=hồ hoàn kiếm
```

---

### 3. Lấy tất cả addresses
```http
GET /api/addresses
```

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Hồ Hoàn Kiếm",
    "address": "Đinh Tiên Hoàng, Hoàn Kiếm, Hà Nội",
    "city": "Hà Nội",
    "country": "Việt Nam",
    "type": "Landmark",
    "category": "Lake",
    "status": "Verified",
    "coordinates": {
      "lat": 21.0285,
      "lng": 105.8542
    },
    "description": "Hồ nước ngọt ở trung tâm Hà Nội",
    "phone": null,
    "website": null,
    "openingHours": null,
    "rating": 4.8,
    "views": 15234,
    "totalReviews": 523,
    "submittedBy": {
      "userId": "...",
      "name": "John Smith",
      "email": "user@example.com"
    },
    "submittedDate": "2024-01-01T00:00:00Z",
    "verifiedBy": {
      "userId": "...",
      "name": "Admin"
    },
    "verifiedDate": "2024-01-02T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

---

### 4. Lấy address theo ID
```http
GET /api/addresses/{id}
```

**Note:** Tự động tăng view count khi gọi endpoint này

---

### 5. Tạo address mới
```http
POST /api/addresses
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "name": "Cafe ABC",
  "address": "123 Nguyen Trai, Thanh Xuan, Ha Noi",
  "city": "Hà Nội",
  "country": "Việt Nam",
  "type": "Restaurant",
  "category": "Cafe",
  "latitude": 21.0285,
  "longitude": 105.8542,
  "description": "Quán cafe yên tĩnh",
  "phone": "+84 123 456 789",
  "website": "www.cafeabc.com",
  "openingHours": "08:00 - 22:00"
}
```

---

### 6. Cập nhật address
```http
PUT /api/addresses/{id}
Authorization: Bearer {token}
```

---

### 7. Xóa address
```http
DELETE /api/addresses/{id}
Authorization: Bearer {admin-token}
```

---

### 8. Verify address
```http
POST /api/addresses/{id}/verify
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "notes": "Đã xác thực thông tin từ nguồn chính thức"
}
```

---

### 9. Reject address
```http
POST /api/addresses/{id}/reject
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "reason": "Thông tin không chính xác, thiếu tài liệu"
}
```

---

### 10. Lọc addresses theo status
```http
GET /api/addresses/filter/status/{status}
```

**Status:** `Pending`, `Verified`, `Rejected`

---

### 11. Lọc addresses theo type
```http
GET /api/addresses/filter/type/{type}
```

**Types:** `Landmark`, `Museum`, `Restaurant`, `Religious`, `Street`, `Shopping`, etc.

---

### 12. Lấy addresses của user
```http
GET /api/addresses/user/{userId}
Authorization: Bearer {token}
```

---

### 13. Lấy addresses của user hiện tại
```http
GET /api/addresses/my-addresses
Authorization: Bearer {token}
```

---

## ✅ Validation APIs

### 1. Lấy thống kê validations
```http
GET /api/validations/stats
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
{
  "totalRequests": 150,
  "pendingRequests": 35,
  "verifiedRequests": 100,
  "rejectedRequests": 15,
  "highPriorityRequests": 12,
  "todayRequests": 8
}
```

---

### 2. Tìm kiếm validations
```http
GET /api/validations/search?searchTerm=VAL-2024-001
Authorization: Bearer {admin-token}
```

---

### 3. Lấy tất cả validations
```http
GET /api/validations
Authorization: Bearer {admin-token}
```

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "requestId": "VAL-2024-001",
    "status": "Pending",
    "priority": "High",
    "requestType": "NewAddress",
    "address": {
      "id": "...",
      "name": "Test Location",
      "address": "123 Test St",
      "city": "Hanoi",
      "country": "Vietnam",
      "type": "Restaurant",
      "category": "Cafe",
      "coordinates": {
        "lat": 21.0285,
        "lng": 105.8542
      }
    },
    "submittedBy": {
      "userId": "...",
      "name": "John Smith",
      "email": "user@example.com"
    },
    "submittedDate": "2024-01-10T10:00:00Z",
    "notes": "Cần xác thực địa điểm mới",
    "changes": null,
    "verificationData": {
      "photosProvided": true,
      "documentsProvided": true,
      "locationVerified": false
    },
    "attachmentsCount": 3,
    "processedBy": null,
    "processedDate": null,
    "createdAt": "2024-01-10T10:00:00Z"
  }
]
```

---

### 4. Lấy validation theo ID
```http
GET /api/validations/{id}
Authorization: Bearer {admin-token}
```

---

### 5. Lấy validation theo Request ID
```http
GET /api/validations/request/{requestId}
Authorization: Bearer {admin-token}
```

---

### 6. Tạo validation request
```http
POST /api/validations
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "requestType": "NewAddress",
  "priority": "High",
  "notes": "Địa điểm mới cần xác thực",
  "oldData": null,
  "newData": null,
  "photosProvided": true,
  "documentsProvided": true,
  "attachmentsCount": 3
}
```

**Request Types:** `NewAddress`, `UpdateInformation`, `DeleteRequest`

**Priorities:** `Low`, `Medium`, `High`

---

### 7. Cập nhật validation
```http
PUT /api/validations/{id}
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "priority": "High",
  "notes": "Cập nhật ghi chú",
  "photosProvided": true,
  "documentsProvided": true,
  "locationVerified": true
}
```

---

### 8. Xóa validation
```http
DELETE /api/validations/{id}
Authorization: Bearer {admin-token}
```

---

### 9. Verify validation
```http
POST /api/validations/{id}/verify
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "notes": "Đã xác thực thành công, thông tin chính xác"
}
```

**Note:** Khi verify validation, address tương ứng cũng sẽ được verify

---

### 10. Reject validation
```http
POST /api/validations/{id}/reject
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "reason": "Thông tin không chính xác, cần bổ sung tài liệu"
}
```

**Note:** Khi reject validation, address tương ứng cũng sẽ bị reject

---

### 11. Lọc validations theo status
```http
GET /api/validations/filter/status/{status}
Authorization: Bearer {admin-token}
```

**Status:** `Pending`, `Verified`, `Rejected`

---

### 12. Lọc validations theo priority
```http
GET /api/validations/filter/priority/{priority}
Authorization: Bearer {admin-token}
```

**Priorities:** `Low`, `Medium`, `High`

---

### 13. Lấy validations của user
```http
GET /api/validations/user/{userId}
Authorization: Bearer {token}
```

---

### 14. Lấy validations của user hiện tại
```http
GET /api/validations/my-validations
Authorization: Bearer {token}
```

---

## ⚙️ Setting APIs

### 1. Lấy cấu hình website (Public)
```http
GET /api/settings/website-config
```

**Response:** `200 OK`
```json
{
  "appDownload": {
    "iosLink": "https://apps.apple.com/app/localizy",
    "androidLink": "https://play.google.com/store/apps/details?id=com.localizy"
  },
  "socialMedia": {
    "facebook": "https://facebook.com/localizy",
    "twitter": "https://twitter.com/localizy",
    "instagram": "https://instagram.com/localizy",
    "linkedIn": "https://linkedin.com/company/localizy",
    "youtube": "https://youtube.com/@localizy"
  },
  "contact": {
    "email": "contact@localizy.com",
    "phone": "+84 123 456 789",
    "address": "Hanoi, Vietnam"
  },
  "general": {
    "slogan": "Localizy - Khám phá địa điểm dễ dàng",
    "description": "Nền tảng quản lý địa điểm toàn diện",
    "aboutUs": "Chúng tôi cung cấp giải pháp quản lý địa điểm..."
  }
}
```

---

### 2. Lấy tất cả settings
```http
GET /api/settings
Authorization: Bearer {admin-token}
```

---

### 3. Lấy settings theo category
```http
GET /api/settings/category/{category}
Authorization: Bearer {admin-token}
```

**Categories:** `AppDownload`, `SocialMedia`, `Contact`, `General`

---

### 4. Lấy setting theo key
```http
GET /api/settings/{key}
Authorization: Bearer {admin-token}
```

---

### 5. Cập nhật setting
```http
PUT /api/settings/{key}
Authorization: Bearer {admin-token}
```

**Request Body:**
```json
{
  "value": "new-value@example.com",
  "description": "Updated description"
}
```

---

## ⚠️ Error Handling

### Error Response Format:
```json
{
  "message": "Error description"
}
```

### HTTP Status Codes:
- `200 OK` - Request thành công
- `201 Created` - Tạo mới thành công
- `204 No Content` - Xóa thành công
- `400 Bad Request` - Dữ liệu không hợp lệ
- `401 Unauthorized` - Chưa xác thực hoặc token không hợp lệ
- `403 Forbidden` - Không có quyền truy cập
- `404 Not Found` - Không tìm thấy resource
- `500 Internal Server Error` - Lỗi server

### Common Errors:

**401 Unauthorized:**
```json
{
  "message": "User không hợp lệ"
}
```

**400 Bad Request:**
```json
{
  "message": "Email đã được sử dụng"
}
```

**404 Not Found:**
```json
{
  "message": "Không tìm thấy address"
}
```

---

## 🧪 Testing

### Sử dụng Swagger UI:
1. Truy cập: `http://localhost:5088/swagger`
2. Click **Authorize** button
3. Nhập token: `Bearer {your-token}`
4. Test các endpoints

### Sử dụng cURL:

**Đăng nhập:**
```bash
curl -X POST http://localhost:5088/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@localizy.com","password":"Admin@123"}'
```

**Lấy danh sách users:**
```bash
curl http://localhost:5088/api/users \
  -H "Authorization: Bearer {your-admin-token}"
```

**Tạo address mới:**
```bash
curl -X POST http://localhost:5088/api/addresses \
  -H "Authorization: Bearer {your-token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Location",
    "address": "123 Test St",
    "city": "Hanoi",
    "country": "Vietnam",
    "type": "Restaurant",
    "category": "Cafe",
    "latitude": 21.0285,
    "longitude": 105.8542
  }'
```

### Sử dụng Postman:
1. Import Swagger JSON: `http://localhost:5088/swagger/v1/swagger.json`
2. Tạo Environment với biến `token`
3. Set Authorization header: `Bearer {{token}}`

---

## 📝 Notes

### Security:
- Tất cả passwords được hash bằng BCrypt
- JWT tokens có thời gian hết hạn
- Sensitive endpoints được bảo vệ bởi role-based authorization

### Performance:
- Database indexes được tạo cho các trường thường xuyên query
- Eager loading được sử dụng để giảm N+1 queries
- Response được cache khi có thể

### Best Practices:
- Luôn kiểm tra token hợp lệ trước khi gọi protected endpoints
- Sử dụng HTTPS trong production
- Validate input data trước khi gửi request
- Handle errors gracefully

---

## 📞 Support

Nếu có vấn đề hoặc câu hỏi, vui lòng liên hệ:
- Email: contact@localizy.com
- GitHub Issues: [Create Issue]

---

## 📄 License

Copyright © 2024 Localizy. All rights reserved.