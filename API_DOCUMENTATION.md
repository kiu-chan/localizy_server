# Localizy Server - API Documentation

Tài liệu chi tiết về tất cả API endpoints của Localizy Server.

## 📋 Table of Contents
- [Overview](#overview)
- [Authentication](#authentication)
- [Error Handling](#error-handling)
- [Auth APIs](#auth-apis)
- [User APIs](#user-apis)
- [Address APIs](#address-apis)
- [Validation APIs](#validation-apis)
- [Setting APIs](#setting-apis)
- [Common Use Cases](#common-use-cases)

---

## 🌐 Overview

### Base URL
```
http://localhost:5088/api
```

### API Version
```
v1
```

### Content Type
```
Content-Type: application/json
```

### Date Format
```
ISO 8601: 2024-01-10T10:30:00Z
```

---

## 🔐 Authentication

### JWT Bearer Token

API sử dụng JWT (JSON Web Token) để xác thực người dùng.

#### Cách lấy token:
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@localizy.com",
  "password": "Admin@123"
}
```

#### Sử dụng token:
Thêm token vào header của request:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Token Properties:
- **Expiration**: 24 giờ (1440 phút)
- **Algorithm**: HS256
- **Claims**: UserId, Email, Name, Role

### Authorization Levels

| Level | Description | Access |
|-------|-------------|--------|
| **Public** | Không cần token | Tất cả |
| **Authenticated** | Cần token hợp lệ | User + Admin |
| **Admin Only** | Cần token Admin | Chỉ Admin |

---

## ⚠️ Error Handling

### Error Response Format

```json
{
  "message": "Error description here"
}
```

### HTTP Status Codes

| Code | Status | Description |
|------|--------|-------------|
| 200 | OK | Request thành công |
| 201 | Created | Tạo resource thành công |
| 204 | No Content | Xóa thành công |
| 400 | Bad Request | Dữ liệu không hợp lệ |
| 401 | Unauthorized | Token không hợp lệ hoặc hết hạn |
| 403 | Forbidden | Không có quyền truy cập |
| 404 | Not Found | Resource không tồn tại |
| 500 | Internal Server Error | Lỗi server |

### Common Error Examples

**400 Bad Request:**
```json
{
  "message": "Email đã được sử dụng"
}
```

**401 Unauthorized:**
```json
{
  "message": "Email hoặc mật khẩu không đúng"
}
```

**404 Not Found:**
```json
{
  "message": "Không tìm thấy user"
}
```

---

## 🔑 Auth APIs

### 1. Đăng ký

Tạo tài khoản người dùng mới.

```http
POST /api/auth/register
```

**Authorization:** None (Public)

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

**Errors:**
- `400` - Email đã được sử dụng

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "fullName": "Nguyen Van A",
    "password": "Password123"
  }'
```

---

### 2. Đăng nhập

Xác thực người dùng và lấy JWT token.

```http
POST /api/auth/login
```

**Authorization:** None (Public)

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

**Errors:**
- `401` - Email hoặc mật khẩu không đúng
- `401` - Tài khoản đã bị vô hiệu hóa

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@localizy.com",
    "password": "Admin@123"
  }'
```

---

## 👥 User APIs

### 1. Lấy thống kê users

```http
GET /api/users/stats
```

**Authorization:** Admin

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

**cURL Example:**
```bash
curl http://localhost:5088/api/users/stats \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 2. Tìm kiếm users

```http
GET /api/users/search?searchTerm={term}
```

**Authorization:** Admin

**Query Parameters:**
- `searchTerm` (string, required): Từ khóa tìm kiếm (name, email, phone, location)

**Example:**
```
GET /api/users/search?searchTerm=john
```

**Response:** `200 OK` - Array of UserResponseDto

**cURL Example:**
```bash
curl "http://localhost:5088/api/users/search?searchTerm=john" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 3. Lấy tất cả users

```http
GET /api/users
```

**Authorization:** Admin

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
    "updatedAt": null,
    "totalAddresses": 12,
    "verifiedAddresses": 10
  }
]
```

**cURL Example:**
```bash
curl http://localhost:5088/api/users \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 4. Lấy user theo ID

```http
GET /api/users/{id}
```

**Authorization:** Authenticated

**Path Parameters:**
- `id` (guid, required): User ID

**Response:** `200 OK` - UserResponseDto

**Errors:**
- `404` - Không tìm thấy user

**cURL Example:**
```bash
curl http://localhost:5088/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

### 5. Tạo user mới

```http
POST /api/users
```

**Authorization:** Admin

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

**Roles:** `User`, `Admin`, `Validator`, `Business`

**Response:** `201 Created` - UserResponseDto

**Errors:**
- `400` - Email đã được sử dụng

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/users \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "newuser@example.com",
    "fullName": "New User",
    "password": "Password123",
    "phone": "+84 987 654 321",
    "location": "Ho Chi Minh, Vietnam",
    "role": "User"
  }'
```

---

### 6. Cập nhật user

```http
PUT /api/users/{id}
```

**Authorization:** Authenticated

**Path Parameters:**
- `id` (guid, required): User ID

**Request Body:**
```json
{
  "fullName": "Updated Name",
  "email": "newemail@example.com",
  "phone": "+84 999 888 777",
  "location": "Da Nang, Vietnam",
  "isActive": true,
  "role": "Admin"
}
```

**Note:** Tất cả fields đều optional

**Response:** `200 OK` - UserResponseDto

**Errors:**
- `404` - Không tìm thấy user
- `400` - Email đã được sử dụng

**cURL Example:**
```bash
curl -X PUT http://localhost:5088/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Updated Name",
    "phone": "+84 999 888 777"
  }'
```

---

### 7. Xóa user

```http
DELETE /api/users/{id}
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): User ID

**Response:** `204 No Content`

**Errors:**
- `404` - Không tìm thấy user

**cURL Example:**
```bash
curl -X DELETE http://localhost:5088/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 8. Toggle trạng thái user

Chuyển đổi trạng thái giữa Active và Suspended.

```http
PATCH /api/users/{id}/toggle-status
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): User ID

**Response:** `200 OK`
```json
{
  "message": "Đã cập nhật trạng thái user"
}
```

**Errors:**
- `404` - Không tìm thấy user

**cURL Example:**
```bash
curl -X PATCH http://localhost:5088/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/toggle-status \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 9. Đổi mật khẩu

```http
POST /api/users/{id}/change-password
```

**Authorization:** Authenticated

**Path Parameters:**
- `id` (guid, required): User ID

**Request Body:**
```json
{
  "currentPassword": "OldPassword123",
  "newPassword": "NewPassword456"
}
```

**Response:** `200 OK`
```json
{
  "message": "Đã đổi mật khẩu thành công"
}
```

**Errors:**
- `404` - Không tìm thấy user
- `401` - Mật khẩu hiện tại không đúng

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/change-password \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "OldPassword123",
    "newPassword": "NewPassword456"
  }'
```

---

### 10. Lọc users theo role

```http
GET /api/users/filter/role/{role}
```

**Authorization:** Admin

**Path Parameters:**
- `role` (string, required): `Admin`, `User`, `Validator`, `Business`

**Example:**
```
GET /api/users/filter/role/Admin
```

**Response:** `200 OK` - Array of UserResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/users/filter/role/Admin \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 11. Lọc users theo status

```http
GET /api/users/filter/status?isActive={boolean}
```

**Authorization:** Admin

**Query Parameters:**
- `isActive` (boolean, required): `true` hoặc `false`

**Example:**
```
GET /api/users/filter/status?isActive=true
```

**Response:** `200 OK` - Array of UserResponseDto

**cURL Example:**
```bash
curl "http://localhost:5088/api/users/filter/status?isActive=true" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

## 📍 Address APIs

### 1. Lấy thống kê addresses

```http
GET /api/addresses/stats
```

**Authorization:** Admin

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

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/stats \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 2. Tìm kiếm addresses

```http
GET /api/addresses/search?searchTerm={term}
```

**Authorization:** None (Public)

**Query Parameters:**
- `searchTerm` (string, required): Từ khóa tìm kiếm

**Example:**
```
GET /api/addresses/search?searchTerm=hồ hoàn kiếm
```

**Response:** `200 OK` - Array of AddressResponseDto

**cURL Example:**
```bash
curl "http://localhost:5088/api/addresses/search?searchTerm=hồ%20hoàn%20kiếm"
```

---

### 3. Lấy tất cả addresses

```http
GET /api/addresses
```

**Authorization:** None (Public)

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
    "verificationNotes": null,
    "rejectionReason": null,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
]
```

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses
```

---

### 4. Lấy address theo ID

```http
GET /api/addresses/{id}
```

**Authorization:** None (Public)

**Path Parameters:**
- `id` (guid, required): Address ID

**Note:** Endpoint này tự động tăng view count

**Response:** `200 OK` - AddressResponseDto

**Errors:**
- `404` - Không tìm thấy address

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

---

### 5. Tạo address mới

```http
POST /api/addresses
```

**Authorization:** Authenticated

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

**Address Types:** `Landmark`, `Museum`, `Restaurant`, `Religious`, `Street`, `Shopping`, `Park`, etc.

**Response:** `201 Created` - AddressResponseDto

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/addresses \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Cafe ABC",
    "address": "123 Nguyen Trai, Thanh Xuan, Ha Noi",
    "city": "Hà Nội",
    "country": "Việt Nam",
    "type": "Restaurant",
    "category": "Cafe",
    "latitude": 21.0285,
    "longitude": 105.8542,
    "description": "Quán cafe yên tĩnh"
  }'
```

---

### 6. Cập nhật address

```http
PUT /api/addresses/{id}
```

**Authorization:** Authenticated

**Path Parameters:**
- `id` (guid, required): Address ID

**Request Body:** (Tất cả fields optional)
```json
{
  "name": "Updated Name",
  "address": "Updated Address",
  "city": "Hà Nội",
  "country": "Việt Nam",
  "type": "Museum",
  "category": "Art Museum",
  "latitude": 21.0285,
  "longitude": 105.8542,
  "description": "Updated description",
  "phone": "+84 999 888 777",
  "website": "www.updated.com",
  "openingHours": "09:00 - 18:00"
}
```

**Response:** `200 OK` - AddressResponseDto

**Errors:**
- `404` - Không tìm thấy address

**cURL Example:**
```bash
curl -X PUT http://localhost:5088/api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Name",
    "description": "Updated description"
  }'
```

---

### 7. Xóa address

```http
DELETE /api/addresses/{id}
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Address ID

**Response:** `204 No Content`

**Errors:**
- `404` - Không tìm thấy address

**cURL Example:**
```bash
curl -X DELETE http://localhost:5088/api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 8. Verify address

Xác thực địa điểm là chính xác.

```http
POST /api/addresses/{id}/verify
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Address ID

**Request Body:**
```json
{
  "notes": "Đã xác thực thông tin từ nguồn chính thức"
}
```

**Response:** `200 OK` - AddressResponseDto

**Errors:**
- `404` - Không tìm thấy address

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6/verify \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "notes": "Đã xác thực thông tin từ nguồn chính thức"
  }'
```

---

### 9. Reject address

Từ chối địa điểm do thông tin không chính xác.

```http
POST /api/addresses/{id}/reject
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Address ID

**Request Body:**
```json
{
  "reason": "Thông tin không chính xác, thiếu tài liệu chứng minh"
}
```

**Response:** `200 OK` - AddressResponseDto

**Errors:**
- `404` - Không tìm thấy address
- `400` - Lý do từ chối không được để trống

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6/reject \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "reason": "Thông tin không chính xác, thiếu tài liệu chứng minh"
  }'
```

---

### 10. Lọc addresses theo status

```http
GET /api/addresses/filter/status/{status}
```

**Authorization:** None (Public)

**Path Parameters:**
- `status` (string, required): `Pending`, `Verified`, `Rejected`

**Example:**
```
GET /api/addresses/filter/status/Verified
```

**Response:** `200 OK` - Array of AddressResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/filter/status/Verified
```

---

### 11. Lọc addresses theo type

```http
GET /api/addresses/filter/type/{type}
```

**Authorization:** None (Public)

**Path Parameters:**
- `type` (string, required): Address type

**Example:**
```
GET /api/addresses/filter/type/Museum
```

**Response:** `200 OK` - Array of AddressResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/filter/type/Museum
```

---

### 12. Lấy addresses của user

```http
GET /api/addresses/user/{userId}
```

**Authorization:** Authenticated

**Path Parameters:**
- `userId` (guid, required): User ID

**Response:** `200 OK` - Array of AddressResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/user/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

### 13. Lấy addresses của user hiện tại

```http
GET /api/addresses/my-addresses
```

**Authorization:** Authenticated

**Response:** `200 OK` - Array of AddressResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/addresses/my-addresses \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## ✅ Validation APIs

### 1. Lấy thống kê validations

```http
GET /api/validations/stats
```

**Authorization:** Admin

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

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/stats \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 2. Tìm kiếm validations

```http
GET /api/validations/search?searchTerm={term}
```

**Authorization:** Admin

**Query Parameters:**
- `searchTerm` (string, required): Từ khóa tìm kiếm

**Example:**
```
GET /api/validations/search?searchTerm=VAL-2024-001
```

**Response:** `200 OK` - Array of ValidationResponseDto

**cURL Example:**
```bash
curl "http://localhost:5088/api/validations/search?searchTerm=VAL-2024-001" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 3. Lấy tất cả validations

```http
GET /api/validations
```

**Authorization:** Admin

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
    "processingNotes": null,
    "rejectionReason": null,
    "createdAt": "2024-01-10T10:00:00Z",
    "updatedAt": null
  }
]
```

**cURL Example:**
```bash
curl http://localhost:5088/api/validations \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 4. Lấy validation theo ID

```http
GET /api/validations/{id}
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Validation ID

**Response:** `200 OK` - ValidationResponseDto

**Errors:**
- `404` - Không tìm thấy validation request

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 5. Lấy validation theo Request ID

```http
GET /api/validations/request/{requestId}
```

**Authorization:** Admin

**Path Parameters:**
- `requestId` (string, required): Request ID (e.g., VAL-2024-001)

**Example:**
```
GET /api/validations/request/VAL-2024-001
```

**Response:** `200 OK` - ValidationResponseDto

**Errors:**
- `404` - Không tìm thấy validation request

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/request/VAL-2024-001 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 6. Tạo validation request

```http
POST /api/validations
```

**Authorization:** Authenticated

**Request Body:**
```json
{
  "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "requestType": "NewAddress",
  "priority": "High",
  "notes": "Địa điểm mới cần xác thực thông tin",
  "oldData": null,
  "newData": null,
  "photosProvided": true,
  "documentsProvided": true,
  "attachmentsCount": 3
}
```

**Request Types:**
- `NewAddress` - Địa điểm mới
- `UpdateInformation` - Cập nhật thông tin
- `DeleteRequest` - Yêu cầu xóa

**Priorities:**
- `Low` - Thấp
- `Medium` - Trung bình
- `High` - Cao

**For Update Requests:**
```json
{
  "addressId": "...",
  "requestType": "UpdateInformation",
  "priority": "Medium",
  "notes": "Cập nhật giờ mở cửa",
  "oldData": "{\"openingHours\":\"08:00-22:00\"}",
  "newData": "{\"openingHours\":\"09:00-21:00\"}",
  "photosProvided": true,
  "documentsProvided": false,
  "attachmentsCount": 1
}
```

**Response:** `201 Created` - ValidationResponseDto

**Note:** Request ID được tự động generate (VAL-YYYY-XXX)

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/validations \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "requestType": "NewAddress",
    "priority": "High",
    "notes": "Địa điểm mới cần xác thực",
    "photosProvided": true,
    "documentsProvided": true,
    "attachmentsCount": 3
  }'
```

---

### 7. Cập nhật validation

```http
PUT /api/validations/{id}
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Validation ID

**Request Body:** (Tất cả fields optional)
```json
{
  "priority": "High",
  "notes": "Cập nhật ghi chú",
  "photosProvided": true,
  "documentsProvided": true,
  "locationVerified": true
}
```

**Response:** `200 OK` - ValidationResponseDto

**Errors:**
- `404` - Không tìm thấy validation request

**cURL Example:**
```bash
curl -X PUT http://localhost:5088/api/validations/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "priority": "High",
    "locationVerified": true
  }'
```

---

### 8. Xóa validation

```http
DELETE /api/validations/{id}
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Validation ID

**Response:** `204 No Content`

**Errors:**
- `404` - Không tìm thấy validation request

**cURL Example:**
```bash
curl -X DELETE http://localhost:5088/api/validations/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 9. Verify validation

Xác thực validation request và address tương ứng.

```http
POST /api/validations/{id}/verify
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Validation ID

**Request Body:**
```json
{
  "notes": "Đã xác thực thành công, thông tin chính xác"
}
```

**Response:** `200 OK` - ValidationResponseDto

**Side Effects:**
- Validation status → Verified
- Address status → Verified

**Errors:**
- `404` - Không tìm thấy validation request

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/validations/3fa85f64-5717-4562-b3fc-2c963f66afa6/verify \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "notes": "Đã xác thực thành công, thông tin chính xác"
  }'
```

---

### 10. Reject validation

Từ chối validation request và address tương ứng.

```http
POST /api/validations/{id}/reject
```

**Authorization:** Admin

**Path Parameters:**
- `id` (guid, required): Validation ID

**Request Body:**
```json
{
  "reason": "Thông tin không chính xác, cần bổ sung tài liệu chứng minh"
}
```

**Response:** `200 OK` - ValidationResponseDto

**Side Effects:**
- Validation status → Rejected
- Address status → Rejected

**Errors:**
- `404` - Không tìm thấy validation request
- `400` - Lý do từ chối không được để trống

**cURL Example:**
```bash
curl -X POST http://localhost:5088/api/validations/3fa85f64-5717-4562-b3fc-2c963f66afa6/reject \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "reason": "Thông tin không chính xác, cần bổ sung tài liệu"
  }'
```

---

### 11. Lọc validations theo status

```http
GET /api/validations/filter/status/{status}
```

**Authorization:** Admin

**Path Parameters:**
- `status` (string, required): `Pending`, `Verified`, `Rejected`

**Example:**
```
GET /api/validations/filter/status/Pending
```

**Response:** `200 OK` - Array of ValidationResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/filter/status/Pending \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 12. Lọc validations theo priority

```http
GET /api/validations/filter/priority/{priority}
```

**Authorization:** Admin

**Path Parameters:**
- `priority` (string, required): `Low`, `Medium`, `High`

**Example:**
```
GET /api/validations/filter/priority/High
```

**Response:** `200 OK` - Array of ValidationResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/filter/priority/High \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 13. Lấy validations của user

```http
GET /api/validations/user/{userId}
```

**Authorization:** Authenticated

**Path Parameters:**
- `userId` (guid, required): User ID

**Response:** `200 OK` - Array of ValidationResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/user/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

### 14. Lấy validations của user hiện tại

```http
GET /api/validations/my-validations
```

**Authorization:** Authenticated

**Response:** `200 OK` - Array of ValidationResponseDto

**cURL Example:**
```bash
curl http://localhost:5088/api/validations/my-validations \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## ⚙️ Setting APIs

### 1. Lấy cấu hình website

```http
GET /api/settings/website-config
```

**Authorization:** None (Public)

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

**Use Case:** Frontend lấy config để hiển thị thông tin website

**cURL Example:**
```bash
curl http://localhost:5088/api/settings/website-config
```

---

### 2. Lấy tất cả settings

```http
GET /api/settings
```

**Authorization:** Admin

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "key": "Email",
    "value": "contact@localizy.com",
    "description": "Email liên hệ",
    "category": "Contact"
  }
]
```

**cURL Example:**
```bash
curl http://localhost:5088/api/settings \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 3. Lấy settings theo category

```http
GET /api/settings/category/{category}
```

**Authorization:** Admin

**Path Parameters:**
- `category` (string, required): Setting category

**Categories:**
- `AppDownload` - Link tải app
- `SocialMedia` - Mạng xã hội
- `Contact` - Thông tin liên hệ
- `General` - Thông tin chung

**Example:**
```
GET /api/settings/category/Contact
```

**Response:** `200 OK` - Array of SettingDto

**cURL Example:**
```bash
curl http://localhost:5088/api/settings/category/Contact \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 4. Lấy setting theo key

```http
GET /api/settings/{key}
```

**Authorization:** Admin

**Path Parameters:**
- `key` (string, required): Setting key

**Example:**
```
GET /api/settings/Email
```

**Response:** `200 OK` - SettingDto

**Errors:**
- `404` - Setting không tồn tại

**cURL Example:**
```bash
curl http://localhost:5088/api/settings/Email \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

---

### 5. Cập nhật setting

```http
PUT /api/settings/{key}
```

**Authorization:** Admin

**Path Parameters:**
- `key` (string, required): Setting key

**Request Body:**
```json
{
  "value": "newemail@localizy.com",
  "description": "Email liên hệ mới"
}
```

**Response:** `200 OK` - SettingDto

**Errors:**
- `404` - Setting không tồn tại

**cURL Example:**
```bash
curl -X PUT http://localhost:5088/api/settings/Email \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "value": "newemail@localizy.com",
    "description": "Email liên hệ mới"
  }'
```

---

## 📊 Common Use Cases

### Use Case 1: User Registration & Login Flow

**Bước 1: Đăng ký**
```bash
curl -X POST http://localhost:5088/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "newuser@example.com",
    "fullName": "New User",
    "password": "Password123"
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "...",
  "role": "User"
}
```

**Bước 2: Lưu token**
```javascript
localStorage.setItem('token', response.token);
```

**Bước 3: Sử dụng token cho các request tiếp theo**
```bash
curl http://localhost:5088/api/addresses/my-addresses \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### Use Case 2: Submit & Verify Address

**Bước 1: User tạo address mới**
```bash
curl -X POST http://localhost:5088/api/addresses \
  -H "Authorization: Bearer USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New Cafe",
    "address": "123 Street",
    "city": "Hanoi",
    "country": "Vietnam",
    "type": "Restaurant",
    "category": "Cafe",
    "latitude": 21.0285,
    "longitude": 105.8542
  }'
```

**Response:** Address với status = "Pending"

**Bước 2: User tạo validation request**
```bash
curl -X POST http://localhost:5088/api/validations \
  -H "Authorization: Bearer USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "addressId": "ADDRESS_ID_FROM_STEP_1",
    "requestType": "NewAddress",
    "priority": "Medium",
    "notes": "Please verify this new cafe",
    "photosProvided": true,
    "documentsProvided": true,
    "attachmentsCount": 2
  }'
```

**Bước 3: Admin xem danh sách pending validations**
```bash
curl http://localhost:5088/api/validations/filter/status/Pending \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Bước 4: Admin verify validation**
```bash
curl -X POST http://localhost:5088/api/validations/VALIDATION_ID/verify \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "notes": "Verified successfully"
  }'
```

**Result:** 
- Validation status → Verified
- Address status → Verified

---

### Use Case 3: Update Address Information

**Bước 1: Tạo validation request để update**
```bash
curl -X POST http://localhost:5088/api/validations \
  -H "Authorization: Bearer USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "addressId": "EXISTING_ADDRESS_ID",
    "requestType": "UpdateInformation",
    "priority": "Low",
    "notes": "Update opening hours",
    "oldData": "{\"openingHours\":\"08:00-22:00\"}",
    "newData": "{\"openingHours\":\"09:00-21:00\"}",
    "photosProvided": false,
    "documentsProvided": true,
    "attachmentsCount": 1
  }'
```

**Bước 2: Admin review changes**
```bash
curl http://localhost:5088/api/validations/VALIDATION_ID \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Bước 3: Admin approve và update address**
```bash
# Verify validation
curl -X POST http://localhost:5088/api/validations/VALIDATION_ID/verify \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"notes": "Approved"}'

# Update address
curl -X PUT http://localhost:5088/api/addresses/ADDRESS_ID \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"openingHours": "09:00-21:00"}'
```

---

### Use Case 4: Search & Filter Addresses

**Tìm kiếm theo từ khóa:**
```bash
curl "http://localhost:5088/api/addresses/search?searchTerm=cafe"
```

**Lọc theo type:**
```bash
curl http://localhost:5088/api/addresses/filter/type/Restaurant
```

**Lọc theo status:**
```bash
curl http://localhost:5088/api/addresses/filter/status/Verified
```

**Combine với pagination (frontend implementation):**
```javascript
const response = await fetch('http://localhost:5088/api/addresses');
const allAddresses = await response.json();

// Client-side pagination
const page = 1;
const perPage = 10;
const paginatedAddresses = allAddresses.slice(
  (page - 1) * perPage, 
  page * perPage
);
```

---

### Use Case 5: Admin Dashboard Statistics

**Lấy tất cả thống kê:**
```bash
# User stats
curl http://localhost:5088/api/users/stats \
  -H "Authorization: Bearer ADMIN_TOKEN"

# Address stats
curl http://localhost:5088/api/addresses/stats \
  -H "Authorization: Bearer ADMIN_TOKEN"

# Validation stats
curl http://localhost:5088/api/validations/stats \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response consolidation (frontend):**
```javascript
const [userStats, addressStats, validationStats] = await Promise.all([
  fetch('/api/users/stats', { headers }),
  fetch('/api/addresses/stats', { headers }),
  fetch('/api/validations/stats', { headers })
]).then(responses => Promise.all(responses.map(r => r.json())));

// Display in dashboard
```

---

### Use Case 6: User Profile Management

**Lấy thông tin user hiện tại:**
```bash
# Get user ID from token claims
USER_ID=$(echo $TOKEN | jq -R 'split(".") | .[1] | @base64d | fromjson | .nameid')

# Get user details
curl http://localhost:5088/api/users/$USER_ID \
  -H "Authorization: Bearer $TOKEN"
```

**Cập nhật profile:**
```bash
curl -X PUT http://localhost:5088/api/users/$USER_ID \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Updated Name",
    "phone": "+84 999 888 777",
    "location": "Da Nang, Vietnam"
  }'
```

**Đổi mật khẩu:**
```bash
curl -X POST http://localhost:5088/api/users/$USER_ID/change-password \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "OldPassword123",
    "newPassword": "NewPassword456"
  }'
```

---

## 🔄 API Workflow Diagrams

### Workflow 1: Address Submission & Verification

```
User                     API                      Admin
  |                       |                         |
  |-- POST /addresses --->|                         |
  |<-- 201 Created -------|                         |
  |                       |                         |
  |-- POST /validations ->|                         |
  |<-- 201 Created -------|                         |
  |                       |                         |
  |                       |<-- GET /validations ---|
  |                       |--- 200 OK ------------->|
  |                       |                         |
  |                       |<-- POST /verify --------|
  |                       |--- 200 OK ------------->|
  |                       |                         |
  |-- GET /my-addresses ->|                         |
  |<-- 200 OK (Verified) -|                         |
```

### Workflow 2: User Registration & First Address

```
1. POST /auth/register
   → Get token

2. POST /addresses (with token)
   → Create address (Pending status)

3. POST /validations (with token)
   → Create validation request

4. Admin reviews
   → GET /validations
   → POST /validations/{id}/verify

5. User sees verified address
   → GET /my-addresses
```

---

## 📱 Frontend Integration Examples

### React/JavaScript Example

```javascript
// api.js - API client setup
const API_BASE_URL = 'http://localhost:5088/api';

const getAuthHeader = () => {
  const token = localStorage.getItem('token');
  return token ? { 'Authorization': `Bearer ${token}` } : {};
};

export const api = {
  // Auth
  login: async (email, password) => {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    return response.json();
  },

  // Addresses
  getAddresses: async () => {
    const response = await fetch(`${API_BASE_URL}/addresses`);
    return response.json();
  },

  createAddress: async (addressData) => {
    const response = await fetch(`${API_BASE_URL}/addresses`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...getAuthHeader()
      },
      body: JSON.stringify(addressData)
    });
    return response.json();
  },

  // Validations
  getMyValidations: async () => {
    const response = await fetch(`${API_BASE_URL}/validations/my-validations`, {
      headers: getAuthHeader()
    });
    return response.json();
  }
};

// Usage in component
const MyAddresses = () => {
  const [addresses, setAddresses] = useState([]);

  useEffect(() => {
    api.getAddresses().then(setAddresses);
  }, []);

  return (
    <div>
      {addresses.map(addr => (
        <div key={addr.id}>{addr.name}</div>
      ))}
    </div>
  );
};
```

---

## 📞 Support & Resources

### Getting Help
- **Email**: contact@localizy.com
- **GitHub Issues**: [Create Issue]
- **Documentation**: [README.md](./README.md)

### Additional Resources
- [Swagger UI](http://localhost:5088/swagger) - Interactive API documentation
- [Postman Collection](#) - Import and test APIs
- [Database Schema](#) - Entity relationship diagram

---

## 📝 Changelog

### Version 1.0.0 (Current)
- ✅ Auth APIs (Register, Login)
- ✅ User Management (11 endpoints)
- ✅ Address Management (13 endpoints)
- ✅ Validation System (14 endpoints)
- ✅ Settings Management (5 endpoints)
- ✅ JWT Authentication
- ✅ Role-based Authorization
- ✅ CORS Support

### Upcoming Features
- 🔄 Refresh Token
- 🔄 Email Verification
- 🔄 Password Reset
- 🔄 File Upload (Images)
- 🔄 Pagination Support
- 🔄 Advanced Filtering
- 🔄 Export Data (CSV, PDF)

---

**Last Updated:** January 2026
**API Version:** 1.0.0