# Khám phá Việt Nam - Cinematic Travel Portal

Website giới thiệu du lịch Việt Nam cực kỳ hiện đại, sáng tạo, premium và mang cảm giác cinematic giống website quảng bá quốc gia.

---

## 1. Công nghệ sử dụng

### Backend (.NET 8 Web API)
- **Clean Architecture**: Phân tách rõ rệt 4 tầng (Domain, Application, Infrastructure, WebApi).
- **CQRS + MediatR**: Tách biệt lệnh ghi (Commands) và truy vấn đọc (Queries).
- **Redis Caching**: Tích hợp StackExchange.Redis để cache thông tin điểm đến và danh sách tỉnh thành, tự động invalidate cache khi thực hiện các lệnh thêm/sửa/xóa.
- **Refresh Token Auth**: Hệ thống xác thực bảo mật lưu Refresh Token trong HTTP-only, Secure, SameSite-Strict cookie và validate dưới cơ sở dữ liệu.
- **API Versioning**: Phân chia các phiên bản API trên URL dẫn (ví dụ `/api/v1/provinces`).
- **Rate Limiting**: Hạn chế tấn công DDoS bằng chính sách giới hạn tần suất request (Fixed Window: tối đa 100 requests/phút).
- **Serilog Logging**: Ghi file nhật ký hệ thống xoay vòng theo ngày.

### Frontend (Angular 17 with SSR)
- **Traditional NgModule Layout**: Sử dụng cấu trúc NgModule truyền thống theo yêu cầu, nói không với Standalone Components.
- **Server-Side Rendering (SSR)**: Tối ưu hóa SEO tối đa cho việc index các địa danh, di sản Việt Nam trên các công cụ tìm kiếm.
- **GSAP Animations**: Thiết kế mượt mà, hiệu ứng parallax trượt mượt mà, reveal text, zoom hình ảnh bằng ScrollTrigger.
- **Interactive SVG Map**: Bản đồ Việt Nam tương tác dạng vẽ vector SVG, tự động preview chi tiết thông tin các vùng miền khi hover.
- **AI recommendation form**: Gợi ý hành trình, thời tiết đẹp nhất dựa trên ngân sách và số lượng người đi.
- **Image Lazy Loading**: Directive IntersectionObserver giúp giảm tải băng thông tải ảnh.

---

## 2. Thiết kế Cơ sở dữ liệu (SQL Server)
Hệ thống sử dụng cơ sở dữ liệu quan hệ SQL Server bao gồm các thực thể chính:
- `Regions` & `Provinces`: Danh sách vùng miền và 63 tỉnh thành Việt Nam.
- `Destinations`: Các địa điểm du lịch, tọa độ GPS và chi phí tham quan.
- `Foods` & `Festivals`: Đặc sản ẩm thực và các lễ hội truyền thống theo tỉnh thành.
- `TravelSeasons`: Thông tin thời tiết, tháng đẹp nhất và lời khuyên theo mùa.
- `Users`, `Roles`, `Permissions` & `RefreshTokens`: Quản lý tài khoản, phân quyền quản trị nội dung.

*Sơ đồ ERD chi tiết và script khởi tạo dữ liệu có tại thư mục [database/schema_and_seed.sql](file:///d:/PersonalProject/TravelVietNam/database/schema_and_seed.sql) bao gồm dữ liệu hạt giống (Seed Data) đầy đủ cho toàn bộ 63 tỉnh thành Việt Nam.*

---

## 3. Cấu trúc thư mục dự án

```
TravelVietnam/
│
├── database/
│   └── schema_and_seed.sql         # SQL Script khởi tạo bảng & dữ liệu hạt giống 63 tỉnh thành
│
├── backend/
│   ├── TravelVietnam.slnx          # Giải pháp solution .NET
│   └── src/
│       ├── TravelVietnam.Domain    # Các thực thể C# (Entities), BaseAuditable, exceptions
│       ├── TravelVietnam.Application  # MediatR CQRS handlers, Mappings AutoMapper, DTOs
│       ├── TravelVietnam.Infrastructure # DbContext EF, Caching Redis, Auth JWT, Repositories
│       └── TravelVietnam.WebApi    # Các Controllers API, Program.cs setup, Middlewares
│
├── frontend/
│   ├── src/app/
│   │   ├── core/                   # ApiService, AuthService, Jwt/Error interceptors, guards
│   │   ├── shared/                 # LazyImageDirective, SafeUrlPipe, các helper
│   │   └── modules/
│   │       └── home/               # Trang chủ landing page, bản đồ SVG, AI form
│   ├── Dockerfile
│   └── package.json
│
└── docker-compose.yml              # File điều phối khởi chạy SQL Server, Redis, API & Web SSR
```

---

## 4. Hướng dẫn cài đặt & Khởi chạy

### 🚀 Quick Start (Windows)
Chạy script tự động với menu tương tác:
```bash
start.bat
```

### 🚀 Quick Start (Linux/Mac)
```bash
chmod +x start.sh
./start.sh
```

Script cung cấp các tùy chọn:
1. **Start Backend Only** - Chạy SQL Server + Redis + API (khuyên dùng cho development)
2. **Start Full Stack** - Chạy toàn bộ hệ thống trong Docker
3. **Stop All Services** - Dừng tất cả services
4. **View Logs** - Xem logs của từng service
5. **Reset Database** - Xóa và tạo lại database

---

### Cách 1: Backend Only + Frontend Local (Khuyên dùng cho Development)

Cách này cho phép bạn phát triển frontend với hot-reload trong khi backend chạy trong Docker.

#### Bước 1: Khởi động Backend Services
```bash
docker-compose -f docker-compose.dev.yml up -d
```

Kiểm tra services đã chạy:
```bash
docker-compose -f docker-compose.dev.yml ps
```

**Services khả dụng:**
- Backend API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`
- SQL Server: `localhost:1433` (sa/TravelVietNamPass@123)
- Redis: `localhost:6379`

#### Bước 2: Chạy Frontend Locally
```bash
cd frontend
npm install
npm start
```

Frontend sẽ chạy tại: `http://localhost:4200`

**Lưu ý:** Đợi 30 giây để database migrations hoàn tất trước khi truy cập frontend.

---

### Cách 2: Full Stack với Docker Compose

Chạy toàn bộ hệ thống (Backend + Frontend) trong Docker:

```bash
docker-compose up --build -d
```

**Services khả dụng:**
- Frontend SSR: `http://localhost:4200`
- Backend API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`
- SQL Server: `localhost:1433`
- Redis: `localhost:6379`

**Lưu ý:** Đợi 1-2 phút để tất cả services khởi động hoàn tất.

#### Xem logs
```bash
# Tất cả services
docker-compose logs -f

# Service cụ thể
docker-compose logs -f api
docker-compose logs -f web
docker-compose logs -f db
```

#### Dừng services
```bash
docker-compose down

# Xóa cả volumes (database data)
docker-compose down -v
```

---

### Cách 3: Chạy thủ công (Development nâng cao)

#### Bước 1: Khởi tạo Database
1. Cài đặt SQL Server 2022 trên máy local
2. Tạo database `TravelVietnamDb`
3. Chạy migrations:
   ```bash
   cd backend/src/TravelVietnam.WebApi
   dotnet ef database update --project ../TravelVietnam.Infrastructure
   ```
4. Seed dữ liệu mẫu:
   ```bash
   # Kết nối SQL Server và chạy file
   sqlcmd -S localhost -U sa -P YourPassword -d TravelVietnamDb -i database/seed-data.sql
   ```

#### Bước 2: Cài đặt Redis
```bash
# Windows (với Chocolatey)
choco install redis-64

# Linux/Mac
brew install redis
redis-server
```

#### Bước 3: Chạy Backend API
1. Cập nhật connection strings trong `backend/src/TravelVietnam.WebApi/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=TravelVietnamDb;Trusted_Connection=True;TrustServerCertificate=True;",
       "Redis": "localhost:6379"
     }
   }
   ```

2. Chạy API:
   ```bash
   cd backend/src/TravelVietnam.WebApi
   dotnet restore
   dotnet build
   dotnet run
   ```

Backend API chạy tại: `http://localhost:5000`

#### Bước 4: Chạy Frontend Angular
1. Cài đặt dependencies:
   ```bash
   cd frontend
   npm install
   ```

2. Chạy development server:
   ```bash
   npm start
   ```

3. Hoặc build và chạy SSR:
   ```bash
   npm run build
   npm run serve:ssr:frontend
   ```

Frontend chạy tại: `http://localhost:4200`

---

### 🧪 Kiểm tra Integration

#### Test Backend API
```bash
# Health check
curl http://localhost:5000/api/v1/regions

# Register user
curl -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test@123",
    "fullName": "Test User"
  }'

# Login
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test@123"
  }'
```

#### Test Frontend
1. Mở `http://localhost:4200`
2. Kiểm tra trang Regions
3. Kiểm tra trang Provinces
4. Click vào một province để xem chi tiết
5. Mở Browser DevTools > Network tab để xem API calls
6. Kiểm tra Console không có errors

---

### 🔧 Troubleshooting

#### Backend không khởi động
```bash
# Kiểm tra SQL Server
docker-compose logs db

# Kiểm tra backend logs
docker-compose logs api

# Restart services
docker-compose restart api
```

#### Frontend không kết nối được Backend
- Kiểm tra backend đang chạy: `curl http://localhost:5000/api/v1/regions`
- Kiểm tra CORS configuration trong `Program.cs`
- Kiểm tra `environment.ts` có đúng API URL
- Clear browser cache và restart Angular dev server

#### Database connection issues
```bash
# Test SQL Server connection
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -Q "SELECT @@VERSION"

# Kiểm tra database tồn tại
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -Q "SELECT name FROM sys.databases"
```

#### Redis connection issues
```bash
# Test Redis
docker exec -it travel_vietnam_redis redis-cli ping
# Kết quả: PONG
```

---

### 📚 Tài liệu chi tiết

Xem [INTEGRATION_GUIDE.md](INTEGRATION_GUIDE.md) để biết thêm:
- API endpoints đầy đủ
- Environment configuration
- Development workflow
- Production deployment
- Monitoring và logging

---

## 5. Các tính năng AI & Hiệu năng nâng cao
1. **AI Recommendations**: Gửi tham số tới `/api/v1/AIRecommendations` nhận đề xuất hành trình riêng biệt cho gia đình, cặp đôi hoặc phượt thủ dựa trên phân tích ngân sách.
2. **Redis Invalidation**: Khi Admin thay đổi hoặc cập nhật thông tin tỉnh thành thông qua `POST /api/v1/provinces`, hệ thống tự động invalidates cache của tỉnh đó và danh sách trang chủ để đảm bảo tính thời gian thực.
3. **Hydration-Safe Animations**: Do chạy SSR, các thuộc tính của window được bọc an toàn trong `isPlatformBrowser(platformId)` để tránh gây lỗi biên dịch trên server Node.
