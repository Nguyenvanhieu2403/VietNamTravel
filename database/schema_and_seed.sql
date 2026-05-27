-- ==========================================
-- KHÁM PHÁ VIỆT NAM - DATABASE SCHEMA & SEED DATA
-- Target DBMS: Microsoft SQL Server
-- ==========================================

USE [master];
GO

-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'TravelVietnamDb')
BEGIN
    CREATE DATABASE [TravelVietnamDb];
END
GO

USE [TravelVietnamDb];
GO

-- ==========================================
-- 1. DROP TABLES (IF EXISTS)
-- ==========================================
IF OBJECT_ID('dbo.TravelPlanDestinations', 'U') IS NOT NULL DROP TABLE dbo.TravelPlanDestinations;
IF OBJECT_ID('dbo.TravelPlans', 'U') IS NOT NULL DROP TABLE dbo.TravelPlans;
IF OBJECT_ID('dbo.RolePermissions', 'U') IS NOT NULL DROP TABLE dbo.RolePermissions;
IF OBJECT_ID('dbo.Permissions', 'U') IS NOT NULL DROP TABLE dbo.Permissions;
IF OBJECT_ID('dbo.RefreshTokens', 'U') IS NOT NULL DROP TABLE dbo.RefreshTokens;
IF OBJECT_ID('dbo.Reviews', 'U') IS NOT NULL DROP TABLE dbo.Reviews;
IF OBJECT_ID('dbo.MediaFiles', 'U') IS NOT NULL DROP TABLE dbo.MediaFiles;
IF OBJECT_ID('dbo.Blogs', 'U') IS NOT NULL DROP TABLE dbo.Blogs;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
IF OBJECT_ID('dbo.TravelSeasons', 'U') IS NOT NULL DROP TABLE dbo.TravelSeasons;
IF OBJECT_ID('dbo.Festivals', 'U') IS NOT NULL DROP TABLE dbo.Festivals;
IF OBJECT_ID('dbo.Foods', 'U') IS NOT NULL DROP TABLE dbo.Foods;
IF OBJECT_ID('dbo.Destinations', 'U') IS NOT NULL DROP TABLE dbo.Destinations;
IF OBJECT_ID('dbo.Provinces', 'U') IS NOT NULL DROP TABLE dbo.Provinces;
IF OBJECT_ID('dbo.Regions', 'U') IS NOT NULL DROP TABLE dbo.Regions;
GO

-- ==========================================
-- 2. CREATE TABLES WITH AUDIT FIELDS & SOFT DELETE
-- ==========================================

-- Regions Table
CREATE TABLE dbo.Regions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL
);

-- Provinces Table
CREATE TABLE dbo.Provinces (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RegionId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    CultureDescription NVARCHAR(MAX) NULL,
    BestTimeToVisit NVARCHAR(200) NULL,
    AverageBudget DECIMAL(18,2) NOT NULL DEFAULT 0,
    VideoUrl NVARCHAR(500) NULL,
    ThumbnailUrl NVARCHAR(500) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Provinces_Regions FOREIGN KEY (RegionId) REFERENCES dbo.Regions(Id) ON DELETE CASCADE
);

-- Destinations Table
CREATE TABLE dbo.Destinations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Address NVARCHAR(500) NULL,
    Latitude FLOAT NULL,
    Longitude FLOAT NULL,
    EntryFee DECIMAL(18,2) NOT NULL DEFAULT 0,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Destinations_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE CASCADE
);

-- Foods Table
CREATE TABLE dbo.Foods (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    RecipeLink NVARCHAR(500) NULL,
    ThumbnailUrl NVARCHAR(500) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Foods_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE CASCADE
);

-- Festivals Table
CREATE TABLE dbo.Festivals (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    HeldDate NVARCHAR(100) NULL,
    LunarDate NVARCHAR(100) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Festivals_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE CASCADE
);

-- TravelSeasons Table
CREATE TABLE dbo.TravelSeasons (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceId INT NOT NULL,
    SeasonName NVARCHAR(50) NOT NULL, -- Spring, Summer, Autumn, Winter, Dry, Rainy
    Months VARCHAR(50) NULL,           -- e.g. "1,2,3"
    WeatherCondition NVARCHAR(500) NULL,
    Tips NVARCHAR(MAX) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_TravelSeasons_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE CASCADE
);

-- Roles Table
CREATE TABLE dbo.Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

-- Users Table
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(256) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE
);

-- RefreshTokens Table
CREATE TABLE dbo.RefreshTokens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Token VARCHAR(500) NOT NULL UNIQUE,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RevokedAt DATETIME2 NULL,
    
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
);

-- Blogs Table
CREATE TABLE dbo.Blogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AuthorId INT NOT NULL,
    Title NVARCHAR(300) NOT NULL,
    Slug VARCHAR(300) NOT NULL UNIQUE,
    Content NVARCHAR(MAX) NOT NULL,
    PublishedAt DATETIME2 NULL,
    IsPublished BIT NOT NULL DEFAULT 0,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Blogs_Users FOREIGN KEY (AuthorId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
);

-- MediaFiles Table
CREATE TABLE dbo.MediaFiles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(500) NOT NULL,
    FileType VARCHAR(20) NOT NULL, -- Image, Video
    ProvinceId INT NULL,
    DestinationId INT NULL,
    BlogId INT NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_MediaFiles_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_MediaFiles_Destinations FOREIGN KEY (DestinationId) REFERENCES dbo.Destinations(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_MediaFiles_Blogs FOREIGN KEY (BlogId) REFERENCES dbo.Blogs(Id) ON DELETE NO ACTION
);

-- Reviews Table
CREATE TABLE dbo.Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    DestinationId INT NULL,
    ProvinceId INT NULL,
    Rating INT NOT NULL CHECK(Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(MAX) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Destinations FOREIGN KEY (DestinationId) REFERENCES dbo.Destinations(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Reviews_Provinces FOREIGN KEY (ProvinceId) REFERENCES dbo.Provinces(Id) ON DELETE NO ACTION
);

-- Permissions Table
CREATE TABLE dbo.Permissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Code VARCHAR(100) NOT NULL UNIQUE
);

-- RolePermissions Table
CREATE TABLE dbo.RolePermissions (
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    PRIMARY KEY (RoleId, PermissionId),
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES dbo.Permissions(Id) ON DELETE CASCADE
);

-- TravelPlans Table
CREATE TABLE dbo.TravelPlans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Budget DECIMAL(18,2) NOT NULL DEFAULT 0,
    DurationDays INT NOT NULL DEFAULT 1,
    Season NVARCHAR(100) NULL,
    
    -- Audit fields
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'System',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy NVARCHAR(100) NULL,
    LastModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedBy NVARCHAR(100) NULL,
    DeletedAt DATETIME2 NULL,
    
    CONSTRAINT FK_TravelPlans_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
);

-- TravelPlanDestinations Table
CREATE TABLE dbo.TravelPlanDestinations (
    TravelPlanId INT NOT NULL,
    DestinationId INT NOT NULL,
    VisitOrder INT NOT NULL DEFAULT 0,
    PRIMARY KEY (TravelPlanId, DestinationId),
    CONSTRAINT FK_TravelPlanDestinations_TravelPlans FOREIGN KEY (TravelPlanId) REFERENCES dbo.TravelPlans(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TravelPlanDestinations_Destinations FOREIGN KEY (DestinationId) REFERENCES dbo.Destinations(Id) ON DELETE CASCADE
);
GO

-- ==========================================
-- 3. CREATE INDEXES
-- ==========================================
CREATE INDEX IX_Provinces_RegionId ON dbo.Provinces(RegionId) WHERE IsDeleted = 0;
CREATE INDEX IX_Destinations_ProvinceId ON dbo.Destinations(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_Foods_ProvinceId ON dbo.Foods(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_Festivals_ProvinceId ON dbo.Festivals(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_TravelSeasons_ProvinceId ON dbo.TravelSeasons(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_Users_RoleId ON dbo.Users(RoleId) WHERE IsDeleted = 0;
CREATE INDEX IX_Reviews_UserId ON dbo.Reviews(UserId) WHERE IsDeleted = 0;
CREATE INDEX IX_Reviews_ProvinceId ON dbo.Reviews(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_Reviews_DestinationId ON dbo.Reviews(DestinationId) WHERE IsDeleted = 0;
CREATE INDEX IX_Blogs_AuthorId ON dbo.Blogs(AuthorId) WHERE IsDeleted = 0;
CREATE INDEX IX_MediaFiles_ProvinceId ON dbo.MediaFiles(ProvinceId) WHERE IsDeleted = 0;
CREATE INDEX IX_MediaFiles_DestinationId ON dbo.MediaFiles(DestinationId) WHERE IsDeleted = 0;
CREATE INDEX IX_TravelPlans_UserId ON dbo.TravelPlans(UserId) WHERE IsDeleted = 0;
GO

-- ==========================================
-- 4. SEED DATA - ROLES, PERMISSIONS, REGIONS & ALL 63 PROVINCES
-- ==========================================

-- Seed Roles
INSERT INTO dbo.Roles (Name) VALUES ('Admin'), ('User');
GO

-- Seed Permissions
INSERT INTO dbo.Permissions (Name, Code) VALUES
('Manage Regions', 'regions:manage'),
('Manage Provinces', 'provinces:manage'),
('Manage Destinations', 'destinations:manage'),
('Manage Blogs', 'blogs:manage'),
('Manage Users', 'users:manage'),
('Read Travel Content', 'content:read'),
('Write Reviews', 'reviews:write');
GO

-- Seed RolePermissions (Admin gets all, User gets content:read and reviews:write)
DECLARE @AdminRoleId INT, @UserRoleId INT;
SELECT @AdminRoleId = Id FROM dbo.Roles WHERE Name = 'Admin';
SELECT @UserRoleId = Id FROM dbo.Roles WHERE Name = 'User';

INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
SELECT @AdminRoleId, Id FROM dbo.Permissions;

INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
SELECT @UserRoleId, Id FROM dbo.Permissions WHERE Code IN ('content:read', 'reviews:write');
GO

-- Seed Regions
INSERT INTO dbo.Regions (Name, Slug, Description) VALUES
(N'Miền Bắc', 'mien-bac', N'Vùng đất ngàn năm văn hiến với cảnh sắc hùng vĩ, núi non điệp trùng và bốn mùa rõ rệt.'),
(N'Miền Trung', 'mien-trung', N'Mảnh đất di sản văn hóa, những bãi biển cát trắng nắng vàng dài bất tận và ẩm thực đậm đà.'),
(N'Miền Nam', 'mien-nam', N'Vùng kinh tế năng động, nhịp sống hiện đại hòa quyện nét phóng khoáng, hiền hòa của con người.'),
(N'Tây Nguyên', 'tay-nguyen', N'Cao nguyên đại ngàn lộng gió, hùng vĩ của tiếng cồng chiêng, đồi cà phê bạt ngàn và thác nước dữ dội.'),
(N'Đồng bằng sông Cửu Long', 'dong-bang-song-cuu-long', N'Vùng sông nước êm đềm, những cánh đồng lúa thẳng cánh cò bay, chợ nổi tấp nập và vườn cây trái sum suê.');
GO

-- Seed All 63 Provinces (with categorized regions)
DECLARE @MienBacId INT, @MienTrungId INT, @MienNamId INT, @TayNguyenId INT, @MekongId INT;
SELECT @MienBacId = Id FROM dbo.Regions WHERE Slug = 'mien-bac';
SELECT @MienTrungId = Id FROM dbo.Regions WHERE Slug = 'mien-trung';
SELECT @MienNamId = Id FROM dbo.Regions WHERE Slug = 'mien-nam';
SELECT @TayNguyenId = Id FROM dbo.Regions WHERE Slug = 'tay-nguyen';
SELECT @MekongId = Id FROM dbo.Regions WHERE Slug = 'dong-bang-song-cuu-long';

-- MIỀN BẮC (25 Tỉnh/Thành)
INSERT INTO dbo.Provinces (RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, VideoUrl, ThumbnailUrl) VALUES
(@MienBacId, N'Hà Nội', 'ha-noi', N'Thủ đô ngàn năm văn hiến, cổ kính và thanh lịch với Hồ Gươm, 36 phố phường.', N'Nét thanh lịch Tràng An, nghệ thuật rối nước, ca trù và nét ẩm thực tinh tế mang tính di sản.', N'Tháng 9 đến tháng 11 (Mùa thu Hà Nội)', 2000000.00, 'https://www.youtube.com/embed/dQw4w9WgXcQ', '/assets/images/provinces/hanoi.jpg'),
(@MienBacId, N'Hải Phòng', 'hai-phong', N'Thành phố hoa phượng đỏ, cảng biển sầm uất và thiên đường ẩm thực đường phố.', N'Lối sống phóng khoáng của người dân miền biển, lễ hội chọi trâu Đồ Sơn truyền thống.', N'Tháng 4 đến tháng 10', 1500000.00, NULL, '/assets/images/provinces/haiphong.jpg'),
(@MienBacId, N'Quảng Ninh', 'quang-ninh', N'Địa danh sở hữu vịnh Hạ Long - kỳ quan thiên nhiên thế giới mới.', N'Văn hóa vùng mỏ và ngư dân làng chài trên vịnh, di tích tâm linh Yên Tử cổ kính.', N'Tháng 3 đến tháng 5 và tháng 9 đến tháng 11', 3500000.00, 'https://www.youtube.com/embed/example', '/assets/images/provinces/quangninh.jpg'),
(@MienBacId, N'Lào Cai', 'lao-cai', N'Nổi tiếng với thị xã sương mù Sa Pa, đỉnh Fansipan - nóc nhà Đông Dương.', N'Bản sắc phong phú của đồng bào H''Mông, Dao đỏ, Giáy cùng các phiên chợ tình đặc sắc.', N'Tháng 9 đến tháng 11 (mùa lúa chín) và tháng 3 đến tháng 5', 3000000.00, NULL, '/assets/images/provinces/laocai.jpg'),
(@MienBacId, N'Ninh Bình', 'ninh-binh', N'Tràng An - Bái Đính, nơi non nước hữu tình được mệnh danh là Vịnh Hạ Long trên cạn.', N'Cố đô Hoa Lư lịch sử, vùng đất sinh thành nhiều lễ hội Phật giáo truyền thống.', N'Tháng 1 đến tháng 3 âm lịch (mùa lễ hội) và tháng 5 đến tháng 6 (mùa lúa chín)', 1500000.00, NULL, '/assets/images/provinces/ninhbinh.jpg'),
(@MienBacId, N'Hà Giang', 'ha-giang', N'Cực Bắc Tổ quốc với cao nguyên đá Đồng Văn hùng vĩ và mùa hoa tam giác mạch.', N'Văn hóa vùng cao biên giới, chợ lùi độc đáo và tiếng khèn Mông gọi bạn.', N'Tháng 10 đến tháng 12 (mùa hoa tam giác mạch)', 2500000.00, NULL, '/assets/images/provinces/hagiang.jpg'),
(@MienBacId, N'Cao Bằng', 'cao-bang', N'Sở hữu thác Bản Giốc hùng vĩ nhất Việt Nam, biên giới thơ mộng.', N'Cội nguồn cách mạng Pác Bó, hát Then đàn Tính của người Tày, Nùng.', N'Tháng 8 đến tháng 10 (thác nhiều nước nhất)', 2000000.00, NULL, '/assets/images/provinces/caobang.jpg'),
(@MienBacId, N'Lạng Sơn', 'lang-son', N'Cửa ngõ biên giới phía Bắc với các danh thắng Ải Chi Lăng, động Tam Thanh.', N'Lễ hội đền Kỳ Cùng - Tả Phủ, văn hóa giao thương sầm uất.', N'Tháng 1 đến tháng 3 âm lịch', 1500000.00, NULL, '/assets/images/provinces/langson.jpg'),
(@MienBacId, N'Tuyên Quang', 'tuyen-quang', N'Vùng đất lịch sử cách mạng Tân Trào thơ mộng sông Gâm.', N'Lễ hội trung thu khổng lồ nổi tiếng cả nước, văn hóa hát Then.', N'Tháng 9 (dịp Trung Thu)', 1200000.00, NULL, '/assets/images/provinces/tuyenquang.jpg'),
(@MienBacId, N'Thái Nguyên', 'thai-nguyen', N'Thủ phủ trà xanh Việt Nam, nổi tiếng với đồi chè Tân Cương.', N'Văn hóa thưởng trà độc đáo, bảo tàng văn hóa các dân tộc Việt Nam.', N'Tháng 8 đến tháng 10', 1000000.00, NULL, '/assets/images/provinces/thainguyen.jpg'),
(@MienBacId, N'Phú Thọ', 'phu-tho', N'Đất Tổ vua Hùng, cội nguồn của dân tộc Việt Nam.', N'Giỗ Tổ Hùng Vương (10/3 âm lịch), hát xoan cổ được UNESCO công nhận.', N'Tháng 3 âm lịch', 1000000.00, NULL, '/assets/images/provinces/phutho.jpg'),
(@MienBacId, N'Bắc Giang', 'bac-giang', N'Vùng đất của vải thiều Lục Ngạn và các ngôi chùa cổ kính.', N'Làng gốm Thổ Hà cổ kính, làn điệu quan họ bờ bắc sông Cầu.', N'Tháng 6 (mùa vải chín)', 1000000.00, NULL, '/assets/images/provinces/bacgiang.jpg'),
(@MienBacId, N'Bắc Kạn', 'bac-kan', N'Sở hữu Hồ Ba Bể - một trong 20 hồ nước ngọt tự nhiên lớn nhất thế giới.', N'Truyền thuyết hồ Ba Bể, đời sống bình yên bên lòng hồ của người Tày.', N'Tháng 5 đến tháng 9', 1500000.00, NULL, '/assets/images/provinces/backan.jpg'),
(@MienBacId, N'Điện Biên', 'dien-bien', N'Gắn liền với chiến thắng Điện Biên Phủ lừng lẫy năm châu.', N'Di tích lịch sử oai hùng, lễ hội hoa ban trắng của đồng bào Thái.', N'Tháng 3 (mùa hoa ban)', 2500000.00, NULL, '/assets/images/provinces/dienbien.jpg'),
(@MienBacId, N'Lai Châu', 'lai-chau', N'Vùng núi non trùng điệp, đèo Ô Quy Hồ tráng lệ bậc nhất.', N'Bản sắc nguyên sơ của 20 dân tộc, chợ phiên sừng sững sương mù.', N'Tháng 9 đến tháng 11', 2000000.00, NULL, '/assets/images/provinces/laichau.jpg'),
(@MienBacId, N'Sơn La', 'son-la', N'Nổi tiếng với thảo nguyên Mộc Châu xanh mướt, đồi chè hình trái tim.', N'Văn hóa tắm suối nước nóng, múa xòe Thái, nhà tù Sơn La lịch sử.', N'Tháng 11 đến tháng 2 năm sau (mùa hoa cải, hoa mơ)', 1800000.00, NULL, '/assets/images/provinces/sonla.jpg'),
(@MienBacId, N'Hòa Bình', 'hoa-binh', N'Cửa ngõ Tây Bắc thơ mộng với lòng hồ sông Đà hùng vĩ.', N'Cội nguồn văn hóa Mường cổ, lễ hội cồng chiêng độc đáo.', N'Tháng 10 đến tháng 4 năm sau', 1200000.00, NULL, '/assets/images/provinces/hoabinh.jpg'),
(@MienBacId, N'Yên Bái', 'yen-bai', N'Danh thắng ruộng bậc thang Mù Cang Chải tráng lệ.', N'Lễ hội dù lượn trên đèo Khau Phạ, văn hóa sinh hoạt ruộng bậc thang.', N'Tháng 9 (mùa lúa chín vàng)', 2000000.00, NULL, '/assets/images/provinces/yenbai.jpg'),
(@MienBacId, N'Bắc Ninh', 'bac-ninh', N'Cội nguồn của dân ca quan họ Kinh Bắc ngọt ngào.', N'Hát quan họ đối đáp, hội Lim nhộn nhịp, tranh dân gian Đông Hồ.', N'Tháng Giêng âm lịch (hội Lim)', 800000.00, NULL, '/assets/images/provinces/bacninh.jpg'),
(@MienBacId, N'Hà Nam', 'ha-nam', N'Ngôi chùa Tam Chúc lớn nhất thế giới nằm giữa lòng hồ thơ mộng.', N'Lễ hội Tịch điền Đọi Sơn cổ xưa khích lệ khuyến nông.', N'Tháng 1 đến tháng 3', 1000000.00, NULL, '/assets/images/provinces/hanam.jpg'),
(@MienBacId, N'Hải Dương', 'hai-duong', N'Vùng đất của bánh đậu xanh lừng danh và danh nhân Chu Văn An.', N'Khu di tích lịch sử Côn Sơn - Kiếp Bạc linh thiêng gắn liền Nguyễn Trãi.', N'Tháng 8 đến tháng 10', 800000.00, NULL, '/assets/images/provinces/haiduong.jpg'),
(@MienBacId, N'Hưng Yên', 'hung-yen', N'Thương cảng Phố Hiến xưa kia nổi tiếng "Thứ nhất Kinh Kỳ, thứ nhì Phố Hiến".', N'Nhãn lồng tiến vua ngọt lịm, di tích chùa Chuông trang nghiêm.', N'Tháng 7 - tháng 8 (mùa nhãn chín)', 800000.00, NULL, '/assets/images/provinces/hungyen.jpg'),
(@MienBacId, N'Nam Định', 'nam-dinh', N'Nổi tiếng với Đền Trần oai hùng và các nhà thờ Thiên chúa giáo cổ kính.', N'Lễ khai ấn Đền Trần đêm 14 tháng giêng âm lịch thu hút hàng vạn khách.', N'Tháng Giêng âm lịch', 1000000.00, NULL, '/assets/images/provinces/namdinh.jpg'),
(@MienBacId, N'Thái Bình', 'thai-binh', N'Quê hương của lúa gạo sông Hồng và các bãi biển vô cực hoang sơ.', N'Văn hóa chèo truyền thống, múa rối nước làng Nguyễn.', N'Tháng 9 đến tháng 11', 900000.00, NULL, '/assets/images/provinces/thaibinh.jpg'),
(@MienBacId, N'Vĩnh Phúc', 'vinh-phuc', N'Thị trấn Tam Đảo mát mẻ quanh năm cùng thiền viện Tây Thiên.', N'Văn hóa Phật giáo Trúc Lâm cổ kính, lễ hội Tây Thiên.', N'Tháng 5 đến tháng 9', 1500000.00, NULL, '/assets/images/provinces/vinhphuc.jpg');
GO

-- MIỀN TRUNG (14 Tỉnh/Thành)
INSERT INTO dbo.Provinces (RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, VideoUrl, ThumbnailUrl) VALUES
(@MienTrungId, N'Thừa Thiên Huế', 'thua-thien-hue', N'Cố đô xưa oai nghiêm của triều Nguyễn, thơ mộng bên dòng sông Hương.', N'Nhã nhạc cung đình Huế di sản thế giới, ca Huế trên sông Hương, ẩm thực cung đình cầu kỳ.', N'Tháng 1 đến tháng 4 (thời tiết dịu mát)', 2500000.00, NULL, '/assets/images/provinces/hue.jpg'),
(@MienTrungId, N'Đà Nẵng', 'da-nang', N'Thành phố đáng sống nhất Việt Nam với bãi biển Mỹ Khê và Cầu Vàng.', N'Lễ hội pháo hoa quốc tế DIFF hoành tráng, văn hóa ẩm thực miền Trung hiện đại.', N'Tháng 3 đến tháng 8 (mùa biển đẹp)', 3500000.00, NULL, '/assets/images/provinces/danang.jpg'),
(@MienTrungId, N'Quảng Nam', 'quang-nam', N'Sở hữu hai di sản thế giới là Phố cổ Hội An và Thánh địa Mỹ Sơn.', N'Nếp sống mộc mạc bên sông Hoài, lễ hội thả đèn hoa đăng lung linh đêm rằm.', N'Tháng 2 đến tháng 4', 3000000.00, NULL, '/assets/images/provinces/quangnam.jpg'),
(@MienTrungId, N'Thanh Hóa', 'thanh-hoa', N'Vùng đất du lịch biển Sầm Sơn náo nhiệt và Thành nhà Hồ di sản thế giới.', N'Di sản trống đồng Đông Sơn, trò diễn Xuân Phả độc đáo.', N'Tháng 5 đến tháng 8', 1500000.00, NULL, '/assets/images/provinces/thanhhoa.jpg'),
(@MienTrungId, N'Nghệ An', 'nghe-an', N'Quê hương Chủ tịch Hồ Chí Minh vĩ đại, bãi biển Cửa Lò mát rượi.', N'Làn điệu dân ca ví giặm Nghệ Tĩnh sâu lắng, nghĩa tình.', N'Tháng 5 đến tháng 8', 1500000.00, NULL, '/assets/images/provinces/nghean.jpg'),
(@MienTrungId, N'Hà Tĩnh', 'ha-tinh', N'Địa danh Ngã ba Đồng Lộc oai hùng, hồ Kẻ Gỗ thơ mộng.', N'Làn điệu ví giặm, văn hóa hiếu học dòng họ xứ Nghệ.', N'Tháng 3 đến tháng 6', 1200000.00, NULL, '/assets/images/provinces/hatinh.jpg'),
(@MienTrungId, N'Quảng Bình', 'quang-binh', N'Vương quốc hang động thế giới Phong Nha - Kẻ Bàng, Sơn Đoòng.', N'Lối sống cư dân vùng đá vôi, hát ca trù truyền thống.', N'Tháng 4 đến tháng 8 (mùa khô để đi hang động)', 4000000.00, NULL, '/assets/images/provinces/quangbinh.jpg'),
(@MienTrungId, N'Quảng Trị', 'quang-tri', N'Mảnh đất thiêng liêng lưu dấu lịch sử kháng chiến khốc liệt (Thành cổ, vĩ tuyến 17).', N'Lễ hội tri ân các anh hùng liệt sĩ thả hoa trên dòng sông Thạch Hãn.', N'Tháng 3 đến tháng 6', 1500000.00, NULL, '/assets/images/provinces/quangtri.jpg'),
(@MienTrungId, N'Quảng Ngãi', 'quang-ngai', N'Sở hữu đảo núi lửa Lý Sơn xinh đẹp - thiên đường tỏi lý sơn.', N'Văn hóa Sa Huỳnh cổ xưa, lễ khao lề thế lính Hoàng Sa hào hùng trên đảo.', N'Tháng 4 đến tháng 8', 2500000.00, NULL, '/assets/images/provinces/quangngai.jpg'),
(@MienTrungId, N'Bình Định', 'binh-dinh', N'Đất võ trời văn Quy Nhơn, biển Kỳ Co - Eo Gió kỳ vĩ.', N'Cái nôi nghệ thuật hát Bội (Tuồng) và nhạc võ Tây Sơn kiêu hùng.', N'Tháng 3 đến tháng 8', 2800000.00, NULL, '/assets/images/provinces/binhdinh.jpg'),
(@MienTrungId, N'Phú Yên', 'phu-yen', N'Mảnh đất "hoa vàng trên cỏ xanh" nổi tiếng danh thắng Gành Đá Đĩa.', N'Lối sống bình dị của ngư dân đầm Ô Loan, lễ hội cầu ngư.', N'Tháng 2 đến tháng 8', 2200000.00, NULL, '/assets/images/provinces/phuyen.jpg'),
(@MienTrungId, N'Khánh Hòa', 'khanh-hoa', N'Nha Trang - một trong những vịnh biển đẹp nhất thế giới.', N'Văn hóa Vương quốc Chăm Pa cổ qua Tháp Bà Ponagar, lễ hội Yến Sào.', N'Tháng 1 đến tháng 9', 3500000.00, NULL, '/assets/images/provinces/khanhhoa.jpg'),
(@MienTrungId, N'Ninh Thuận', 'ninh-thuan', N'Vùng đất của nho xanh ngọt lịm, tháp Chàm rực rỡ nắng gió Phan Rang.', N'Lễ hội Katê của đồng bào Chăm cực kỳ sắc màu, gốm Bàu Trúc cổ nhất Đông Nam Á.', N'Tháng 4 đến tháng 9', 2000000.00, NULL, '/assets/images/provinces/ninhthuan.jpg'),
(@MienTrungId, N'Bình Thuận', 'binh-thuan', N'Phan Thiết - Mũi Né cát trắng trập trùng biển xanh ngập nắng.', N'Văn hóa biển và di tích tháp Chăm Pô Sah Inư huyền bí.', N'Tháng 11 đến tháng 4 năm sau', 2500000.00, NULL, '/assets/images/provinces/binhthuan.jpg');
GO

-- TÂY NGUYÊN (5 Tỉnh)
INSERT INTO dbo.Provinces (RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, VideoUrl, ThumbnailUrl) VALUES
(@TayNguyenId, N'Lâm Đồng', 'lam-dong', N'Thành phố Đà Lạt mộng mơ, ngập tràn thông xanh và ngàn hoa khoe sắc.', N'Không gian văn hóa Cồng chiêng Tây Nguyên linh thiêng được UNESCO vinh danh.', N'Tháng 11 đến tháng 3 năm sau (mùa hoa dã quỳ và mai anh đào)', 3000000.00, NULL, '/assets/images/provinces/lamdong.jpg'),
(@TayNguyenId, N'Đắk Lắk', 'dak-lak', N'Thủ phủ cà phê Buôn Ma Thuột, cưỡi voi Bản Đôn huyền thoại.', N'Lễ hội cà phê lớn nhất nước, sử thi Đam San huyền thoại bên bếp lửa nhà sàn.', N'Tháng 12 đến tháng 3', 2000000.00, NULL, '/assets/images/provinces/daklak.jpg'),
(@TayNguyenId, N'Gia Lai', 'gia-lai', N'Hồ Tơ Nưng (Biển Hồ) - đôi mắt Pleiku trong veo giữa cao nguyên hoang sơ.', N'Kiến trúc nhà Rông cao vút, lễ hội mừng lúa mới đậm đà bản sắc.', N'Tháng 11 đến tháng 2 năm sau', 1800000.00, NULL, '/assets/images/provinces/gialai.jpg'),
(@TayNguyenId, N'Kon Tum', 'kon-tum', N'Biên giới ngã ba Đông Dương, nổi tiếng với nhà thờ gỗ cổ trăm tuổi.', N'Văn hóa người Ba Na, Xơ Đăng mến khách, cồng chiêng vang vọng.', N'Tháng 10 đến tháng 1', 1500000.00, NULL, '/assets/images/provinces/kontum.jpg'),
(@TayNguyenId, N'Đắk Nông', 'dak-nong', N'Sở hữu công viên địa chất toàn cầu UNESCO với hệ thống hang động núi lửa kì vĩ.', N'Âm thanh cồng chiêng cổ xưa, đời sống bình yên bên hồ Tà Đùng (Vịnh Hạ Long của Tây Nguyên).', N'Tháng 12 đến tháng 4', 1800000.00, NULL, '/assets/images/provinces/daknong.jpg');
GO

-- MIỀN NAM (6 Tỉnh/Thành)
INSERT INTO dbo.Provinces (RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, VideoUrl, ThumbnailUrl) VALUES
(@MienNamId, N'Thành phố Hồ Chí Minh', 'thanh-pho-ho-chi-minh', N'Đô thị phồn hoa bậc nhất Việt Nam, nhịp sống không ngủ sôi động.', N'Hòa quyện lối sống hiện đại phương Tây và nét Á Đông truyền thống, ẩm thực đa dạng.', N'Tháng 12 đến tháng 4 (mùa khô)', 3500000.00, NULL, '/assets/images/provinces/tphcm.jpg'),
(@MienNamId, N'Bà Rịa - Vũng Tàu', 'ba-ria-vung-tau', N'Bãi biển ngập nắng kề sát Sài Gòn, phố biển thanh bình nghỉ dưỡng.', N'Văn hóa cầu ngư, các di tích lịch sử và lối sống hướng biển.', N'Quanh năm, đẹp nhất tháng 12 đến tháng 4', 2000000.00, NULL, '/assets/images/provinces/vungtau.jpg'),
(@MienNamId, N'Bình Dương', 'binh-duong', N'Thủ phủ công nghiệp mới nổi, có khu du lịch Đại Nam hoành tráng.', N'Văn hóa làm gốm sứ truyền thống Lái Thiêu nổi tiếng.', N'Tháng 5 đến tháng 8 (mùa trái cây trĩu quả)', 1000000.00, NULL, '/assets/images/provinces/binhduong.jpg'),
(@MienNamId, N'Đồng Nai', 'dong-nai', N'Thác đá Giang Điền thơ mộng, khu du lịch sinh thái rừng Nam Cát Tiên bí ẩn.', N'Nếp sống ven sông Đồng Nai hiền hòa, bảo tồn thiên nhiên đa sinh học cực cao.', N'Tháng 12 đến tháng 5', 1200000.00, NULL, '/assets/images/provinces/dongnai.jpg'),
(@MienNamId, N'Tây Ninh', 'tay-ninh', N'Thánh địa Cao Đài linh thiêng và núi Bà Đen huyền thoại mây phủ.', N'Vùng đất khai sinh Đạo Cao Đài độc nhất, lễ hội xuân Núi Bà Đen linh thiêng.', N'Tháng Giêng và tháng 8 âm lịch', 1200000.00, NULL, '/assets/images/provinces/tayninh.jpg'),
(@MienNamId, N'Bình Phước', 'binh-phuoc', N'Vùng đất của cao su trút lá đỏ và những rẫy điều bạt ngàn.', N'Bản sắc văn hóa các dân tộc thiểu số S''tiêng, M''nông mộc mạc.', N'Tháng 12 đến tháng 3', 1000000.00, NULL, '/assets/images/provinces/binhphuoc.jpg');
GO

-- ĐỒNG BẰNG SÔNG CỬU LONG (13 Tỉnh/Thành)
INSERT INTO dbo.Provinces (RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, VideoUrl, ThumbnailUrl) VALUES
(@MekongId, N'Cần Thơ', 'can-tho', N'Thủ phủ sông nước Tây Đô sầm uất với Chợ nổi Cái Răng đặc sắc.', N'Lối sinh hoạt gắn liền sông nước mênh mang, làn điệu đờn ca tài tử Nam Bộ di sản.', N'Tháng 9 đến tháng 11 (mùa nước nổi)', 1800000.00, NULL, '/assets/images/provinces/cantho.jpg'),
(@MekongId, N'An Giang', 'an-giang', N'Vùng đất Thất Sơn linh thiêng huyền bí, rừng tràm Trà Sư xanh thẳm.', N'Giao thoa văn hóa đặc sắc Kinh, Hoa, Chăm, Khmer cùng Lễ vía Bà Chúa Xứ đền Sam oai nghiêm.', N'Tháng 8 đến tháng 11 (mùa nước nổi miền Tây)', 1800000.00, NULL, '/assets/images/provinces/angiang.jpg'),
(@MekongId, N'Kiên Giang', 'kien-giang', N'Đảo ngọc Phú Quốc biển xanh cát trắng như thiên đường Địa Trung Hải.', N'Văn hóa vùng biển đảo phía Tây Nam, nghề nước mắm truyền thống lâu đời.', N'Tháng 11 đến tháng 4 năm sau', 5000000.00, NULL, '/assets/images/provinces/kiengiang.jpg'),
(@MekongId, N'Bến Tre', 'ben-tre', N'Xứ dừa đồng khởi thanh bình với hệ thống kênh rạch chằng chịt mát rượi.', N'Lối sống trù phú vùng cù lao sông nước, các sản phẩm thủ công từ dừa nghệ thuật.', N'Quanh năm, đẹp nhất tháng 5 đến tháng 8', 1000000.00, NULL, '/assets/images/provinces/bentre.jpg'),
(@MekongId, N'Cà Mau', 'ca-mau', N'Mũi đất tận cùng Cực Nam Tổ quốc, rừng ngập mặn đầm lầy xanh tươi.', N'Lối sống khai hoang vùng đất mũi, ẩm thực đặc trưng sông nước ngập mặn (cua Cà Mau).', N'Tháng 12 đến tháng 4 năm sau', 2500000.00, NULL, '/assets/images/provinces/camau.jpg'),
(@MekongId, N'Đồng Tháp', 'dong-thap', N'Thủ phủ hoa sen ngát hương, vườn cò Sa Đéc đầy thơ mộng.', N'Làng hoa Sa Đéc hàng trăm tuổi rực rỡ, lối sống đậm đà hồn quê miền Tây.', N'Tháng 8 đến tháng 11 (mùa nước nổi rực rỡ hoa sen)', 1200000.00, NULL, '/assets/images/provinces/dongthap.jpg'),
(@MekongId, N'Long An', 'long-an', N'Cửa ngõ miền Tây hiền hòa với sông Vàm Cỏ Đông lịch sử.', N'Nghề làm trống Bình An nổi tiếng và văn hóa đờn ca tài tử Nam Bộ.', N'Tháng 9 đến tháng 11', 1000000.00, NULL, '/assets/images/provinces/longan.jpg'),
(@MekongId, N'Tiền Giang', 'tien-giang', N'Sở hữu chợ nổi Cái Bè mộc mạc và trại rắn Đồng Tâm lớn nhất nước.', N'Đời sống miệt vườn sum suê cây trái cù lao Thới Sơn.', N'Tháng 5 đến tháng 8', 1000000.00, NULL, '/assets/images/provinces/tiengiang.jpg'),
(@MekongId, N'Trà Vinh', 'tra-vinh', N'Thành phố xanh với hàng vạn cây cổ thụ trăm tuổi và nhiều chùa Khmer cổ.', N'Nét văn hóa giao thoa Kinh - Khmer, lễ hội Ok Om Bok cầu mùa trăng đặc sắc.', N'Tháng 10 đến tháng 12', 1200000.00, NULL, '/assets/images/provinces/travinh.jpg'),
(@MekongId, N'Vĩnh Long', 'vinh-long', N'Mảnh đất miệt vườn sông nước, nằm giữa sông Tiền và sông Hậu.', N'Lối sống bình dị cư dân cù lao An Bình, nghề làm gốm đỏ mang nét đặc trưng.', N'Tháng 5 đến tháng 9', 1100000.00, NULL, '/assets/images/provinces/vinhlong.jpg'),
(@MekongId, N'Sóc Trăng', 'soc-trang', N'Đặc sản bánh pía, chùa Dơi kỳ bí và chùa Chén Kiểu rực rỡ sắc màu.', N'Hội đua ghe Ngo rộn rã của đồng bào Khmer cực kỳ náo nhiệt.', N'Tháng 10 âm lịch (Lễ hội Ok Om Bok và đua ghe Ngo)', 1200000.00, NULL, '/assets/images/provinces/soctrang.jpg'),
(@MekongId, N'Bạc Liêu', 'bac-lieu', N'Quê hương điệu "Dạ cổ hoài lang" và dinh thự Công tử Bạc Liêu.', N'Giai thoại hào phóng Công tử Bạc Liêu, văn hóa ca cổ đờn ca tài tử sâu lắng.', N'Tháng 8 đến tháng 10', 1500000.00, NULL, '/assets/images/provinces/baclieu.jpg'),
(@MekongId, N'Hậu Giang', 'hau-giang', N'Vùng đất của kênh rạch hiền hòa và những cánh đồng khóm Cầu Đúc bạt ngàn.', N'Văn hóa vùng sông nước trù phú miền Tây, chợ nổi Ngã Bảy xưa cũ.', N'Tháng 9 đến tháng 12', 1000000.00, NULL, '/assets/images/provinces/haugiang.jpg');
GO

-- ==========================================
-- 5. SEED DETAILED SAMPLES - DESTINATIONS, FOODS, FESTIVALS, SEASONS
-- ==========================================

-- Let's grab some specific Province IDs for detailed sample data
DECLARE @HanoiId INT, @LaoCaiId INT, @DanangId INT, @HueId INT, @LamDongId INT, @TphcmId INT, @KienGiangId INT;
SELECT @HanoiId = Id FROM dbo.Provinces WHERE Slug = 'ha-noi';
SELECT @LaoCaiId = Id FROM dbo.Provinces WHERE Slug = 'lao-cai';
SELECT @DanangId = Id FROM dbo.Provinces WHERE Slug = 'da-nang';
SELECT @HueId = Id FROM dbo.Provinces WHERE Slug = 'thua-thien-hue';
SELECT @LamDongId = Id FROM dbo.Provinces WHERE Slug = 'lam-dong';
SELECT @TphcmId = Id FROM dbo.Provinces WHERE Slug = 'thanh-pho-ho-chi-minh';
SELECT @KienGiangId = Id FROM dbo.Provinces WHERE Slug = 'kien-giang';

-- Seed Destinations
INSERT INTO dbo.Destinations (ProvinceId, Name, Description, Address, Latitude, Longitude, EntryFee) VALUES
(@HanoiId, N'Hồ Hoàn Kiếm và Đền Ngọc Sơn', N'Trái tim của thủ đô Hà Nội, gắn liền với truyền thuyết vua Lê Lợi trả gươm báu cho Rùa Thần.', N'Phố Đinh Tiên Hoàng, Hoàn Kiếm, Hà Nội', 21.0285, 105.8522, 30000.00),
(@HanoiId, N'Lăng Chủ tịch Hồ Chí Minh', N'Nơi an nghỉ vĩnh hằng của vị lãnh tụ vĩ đại của dân tộc Việt Nam.', N'Hùng Vương, Điện Biên, Ba Đình, Hà Nội', 21.0368, 105.8346, 0.00),
(@LaoCaiId, N'Đỉnh Fansipan', N'Nóc nhà Đông Dương với độ cao 3,143m, sở hữu tuyến cáp treo đạt nhiều kỷ lục thế giới.', N'Sa Pa, Lào Cai', 22.3033, 103.7750, 850000.00),
(@LaoCaiId, N'Bản Cát Cát', N'Bản làng cổ xưa của người H''Mông với cảnh sắc thiên nhiên hoang sơ và nghề dệt thổ cẩm truyền thống.', N'Sa Pa, Lào Cai', 22.3308, 103.8291, 150000.00),
(@DanangId, N'Cầu Vàng (Bà Nà Hills)', N'Cây cầu đi bộ nổi tiếng thế giới được nâng đỡ bởi đôi bàn tay khổng lồ rêu phong.', N'Hòa Ninh, Hòa Vang, Đà Nẵng', 15.9984, 107.9964, 900000.00),
(@HueId, N'Đại Nội Huế', N'Hoàng cung cổ kính nơi trị vì của 13 vị vua triều Nguyễn - triều đại phong kiến cuối cùng tại Việt Nam.', N'Phú Hậu, Thành phố Huế, Thừa Thiên Huế', 16.4691, 107.5779, 200000.00),
(@LamDongId, N'Hồ Xuân Hương', N'Hồ nước thơ mộng nằm ngay trung tâm thành phố Đà Lạt, biểu tượng của sự lãng mạn.', N'Phường 1, Đà Lạt, Lâm Đồng', 11.9427, 108.4452, 0.00),
(@TphcmId, N'Dinh Độc Lập', N'Di tích quốc gia đặc biệt ghi dấu sự kiện giải phóng miền Nam thống nhất đất nước ngày 30/4/1975.', N'135 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM', 10.7770, 106.6953, 650000.00),
(@KienGiangId, N'Bãi Sao Phú Quốc', N'Một trong những bãi biển đẹp nhất Phú Quốc với làn nước trong vắt và cát trắng mịn màng.', N'An Thới, Phú Quốc, Kiên Giang', 10.0526, 104.0322, 0.00);

-- Seed Foods
INSERT INTO dbo.Foods (ProvinceId, Name, Description, RecipeLink, ThumbnailUrl) VALUES
(@HanoiId, N'Phở Bò Hà Nội', N'Món ăn tinh túy đại diện cho ẩm thực Việt Nam với nước dùng ninh xương bò trong vắt và bánh phở dai mềm.', NULL, '/assets/images/foods/phohanoi.jpg'),
(@HanoiId, N'Bún Chả', N'Thịt nướng xém cạnh thơm phức ăn kèm bún, rau sống và nước mắm chua ngọt pha tỏi ớt hấp dẫn.', NULL, '/assets/images/foods/buncha.jpg'),
(@LaoCaiId, N'Thắng Cố Sa Pa', N'Món ăn đặc sản truyền thống của người Mông làm từ ngựa, kết hợp các loại thảo mọc rừng Tây Bắc thơm nồng.', NULL, NULL),
(@DanangId, N'Mì Quảng', N'Món sợi trứ danh xứ Quảng với tôm, thịt, trứng cút cùng nước dùng sền sệt đậm đà, ăn kèm bánh tráng nướng.', NULL, '/assets/images/foods/miquang.jpg'),
(@HueId, N'Bún Bò Huế', N'Món bún cay đậm vị mắm ruốc, chân giò béo ngậy và tiết luộc truyền thống xứ Huế.', NULL, '/assets/images/foods/bunbohue.jpg'),
(@TphcmId, N'Cơm Tấm Sài Gòn', N'Đặc sản đường phố quen thuộc với sườn nướng mật ong, bì, chả và nước mắm kẹo tỏi ớt đặc trưng.', NULL, '/assets/images/foods/comtam.jpg'),
(@KienGiangId, N'Gỏi Cá Trích Phú Quốc', N'Sự hòa quyện tuyệt vời giữa thịt cá trích tươi rói ngọt nước, dừa nạo và nước sốt đậu phộng béo bùi.', NULL, NULL);

-- Seed Festivals
INSERT INTO dbo.Festivals (ProvinceId, Name, Description, HeldDate, LunarDate) VALUES
(@HanoiId, N'Hội chùa Hương', N'Lễ hội Phật giáo lớn và kéo dài nhất Việt Nam, khách trẩy hội xuôi dòng suối Yến vào hang động tâm linh.', N'Tháng 1 đến tháng 3 âm lịch', N'Mùng 6 tháng Giêng đến hết tháng 3 âm lịch'),
(@LaoCaiId, N'Lễ hội Xuống Đồng Sa Pa', N'Lễ hội của người Tày cầu mong mùa màng tốt tươi, mưa thuận gió hòa với nhiều trò chơi dân gian rộn ràng.', N'Tháng Giêng âm lịch', N'Mùng 8 tháng Giêng âm lịch'),
(@HueId, N'Festival Huế', N'Sự kiện văn hóa nghệ thuật tầm cỡ quốc tế tôn vinh di sản cung đình và giao lưu văn hóa thế giới.', N'Tổ chức định kỳ 2 năm một lần (thường vào mùa hè)', NULL),
(@TphcmId, N'Lễ hội Sông nước TP.HCM', N'Lễ hội hiện đại tôn vinh giá trị văn hóa lịch sử dòng sông Sài Gòn qua các hoạt động nghệ thuật và thể thao dưới nước.', N'Tháng 6 hàng năm', NULL);

-- Seed Seasons
INSERT INTO dbo.TravelSeasons (ProvinceId, SeasonName, Months, WeatherCondition, Tips) VALUES
(@HanoiId, N'Mùa Thu', '9,10,11', N'Thời tiết mát mẻ, dễ chịu, trời xanh hanh hao và hương hoa sữa phảng phất.', N'Hãy dạo quanh Hồ Tây lúc hoàng hôn và thử cốm vòng thơm phức.'),
(@LaoCaiId, N'Mùa Đông (Mùa tuyết rơi)', '12,1,2', N'Lạnh giá buốt, thỉnh thoảng có tuyết rơi trên đỉnh Fansipan huyền ảo.', N'Trang bị đầy đủ quần áo ấm chuyên dụng chống nước và giữ nhiệt tốt.'),
(@DanangId, N'Mùa Hè (Mùa Biển)', '5,6,7,8', N'Nắng rực rỡ, trời trong xanh cực kỳ phù hợp cho các hoạt động tắm biển và vui chơi ngoài trời.', N'Nên đặt trước vé máy bay và phòng khách sạn sớm vì đây là mùa cao điểm du lịch.'),
(@LamDongId, N'Mùa Khô', '11,12,1,2,3', N'Se se lạnh vào buổi sáng và tối, ban ngày nắng ấm, độ ẩm thấp dễ chịu.', N'Mang theo áo khoác nhẹ khi ra đường vào buổi tối Đà Lạt.');
GO

-- Create a mock Admin user (Password is 'Admin@123' pbkdf2 or plain string hash here for simplicity, in production uses ASP.NET Identity or safe hashing)
-- Let's insert mock Admin User
DECLARE @AdminRole INT;
SELECT @AdminRole = Id FROM dbo.Roles WHERE Name = 'Admin';

INSERT INTO dbo.Users (RoleId, Username, Email, PasswordHash, FullName, CreatedBy)
VALUES (@AdminRole, 'admin', 'admin@travelvietnam.gov.vn', 'AQAAAAIAAYagAAAAEG3kX+tX7y5pXkP+1X7y5pXkP+==', N'Quản trị viên Hệ thống', 'System');
GO

PRINT 'Database script completed successfully. Created 63 provinces, sample destinations, foods, festivals, and security configuration.';
GO
