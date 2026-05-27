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

### Cách 1: Sử dụng Docker Compose (Khuyên dùng)
Yêu cầu hệ thống đã cài đặt Docker và Docker Desktop. Từ thư mục gốc của dự án, chạy lệnh:
```bash
docker-compose up --build -d
```
Docker sẽ tự động tải các container:
- **Cơ sở dữ liệu (SQL Server)**: Port `1433`
- **Bộ nhớ đệm (Redis)**: Port `6379`
- **Backend API**: Port `5000` (Swagger tài liệu chạy tại: `http://localhost:5000/swagger`)
- **Frontend SSR**: Port `4200` (Truy cập tại: `http://localhost:4200`)

### Cách 2: Khởi chạy thủ công từng phần

#### Bước 1: Khởi tạo database
1. Chạy SQL Server cục bộ trên máy của bạn.
2. Thực thi toàn bộ script [schema_and_seed.sql](file:///d:/PersonalProject/TravelVietNam/database/schema_and_seed.sql) trong SQL Server Management Studio (SSMS) để tạo database `TravelVietnamDb` và nạp dữ liệu.

#### Bước 2: Chạy Backend API
1. Đảm bảo cấu hình đúng chuỗi kết nối trong `backend/src/TravelVietnam.WebApi/appsettings.json`.
2. Truy cập thư mục `backend` và chạy lệnh:
   ```bash
   dotnet build
   dotnet run --project src/TravelVietnam.WebApi/TravelVietnam.WebApi.csproj
   ```

#### Bước 3: Chạy Frontend Angular
1. Truy cập thư mục `frontend` và cài đặt các thư viện:
   ```bash
   npm install
   ```
2. Chạy ứng dụng dưới chế độ Development (hoặc SSR):
   ```bash
   npm run dev
   ```
3. Truy cập địa chỉ `http://localhost:4200`.

---

## 5. Các tính năng AI & Hiệu năng nâng cao
1. **AI Recommendations**: Gửi tham số tới `/api/v1/AIRecommendations` nhận đề xuất hành trình riêng biệt cho gia đình, cặp đôi hoặc phượt thủ dựa trên phân tích ngân sách.
2. **Redis Invalidation**: Khi Admin thay đổi hoặc cập nhật thông tin tỉnh thành thông qua `POST /api/v1/provinces`, hệ thống tự động invalidates cache của tỉnh đó và danh sách trang chủ để đảm bảo tính thời gian thực.
3. **Hydration-Safe Animations**: Do chạy SSR, các thuộc tính của window được bọc an toàn trong `isPlatformBrowser(platformId)` để tránh gây lỗi biên dịch trên server Node.
