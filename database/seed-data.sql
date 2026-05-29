-- Travel Vietnam Database Seed Script
-- Run this after migrations are applied

USE TravelVietnamDb;
GO

-- Insert Regions
SET IDENTITY_INSERT Regions ON;

INSERT INTO Regions (Id, Name, Slug, Description, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, N'Miền Bắc', 'mien-bac', N'Vùng đất ngàn năm văn hiến với Thủ đô Hà Nội cổ kính, ruộng bậc thang Sapa hùng vĩ, Vịnh Hạ Long huyền bí.', GETUTCDATE(), 'System', 0),
(2, N'Miền Trung', 'mien-trung', N'Dải đất di sản với Cố đô Huế trầm mặc, phố cổ Hội An lung linh đèn lồng, Đà Nẵng hiện đại.', GETUTCDATE(), 'System', 0),
(3, N'Tây Nguyên', 'tay-nguyen', N'Cao nguyên bạt ngàn với đồi chè xanh mướt, hương cà phê đặc trưng, Đà Lạt sương mù thơ mộng.', GETUTCDATE(), 'System', 0),
(4, N'Miền Nam', 'mien-nam', N'Trung tâm kinh tế sôi động với TP.HCM không ngủ, Vũng Tàu tràn ngập nắng ấm, Côn Đảo hoang sơ.', GETUTCDATE(), 'System', 0),
(5, N'Đồng Bằng Sông Cửu Long', 'dong-bang-song-cuu-long', N'Vùng sông nước bình dị với chợ nổi Cái Răng tấp nập, rừng tràm Trà Sư xanh thẳm, đảo ngọc Phú Quốc.', GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Regions OFF;
GO

-- Insert Provinces
SET IDENTITY_INSERT Provinces ON;

INSERT INTO Provinces (Id, RegionId, Name, Slug, Description, CultureDescription, BestTimeToVisit, AverageBudget, ThumbnailUrl, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'Hà Nội', 'ha-noi', N'Thủ đô ngàn năm văn hiến với Hồ Gươm thơ mộng, Văn Miếu cổ kính và phố cổ 36 phường phố tấp nập.', N'Văn hóa Hà Nội mang đậm dấu ấn lịch sử với hơn 1000 năm Thăng Long - Hà Nội.', N'Tháng 9 - Tháng 11', 3000000, 'https://images.unsplash.com/photo-1509023464722-18d996393ca8?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(2, 1, N'Quảng Ninh', 'quang-ninh', N'Vịnh Hạ Long kỳ vĩ với hàng ngàn hòn đảo đá vôi nhô lên giữa làn nước xanh ngọc bích.', N'Nơi giao thoa văn hóa biển đảo và văn hóa đất liền, với nhiều lễ hội truyền thống.', N'Tháng 3 - Tháng 5', 5000000, 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(3, 1, N'Lào Cai', 'lao-cai', N'Sa Pa với ruộng bậc thang hùng vĩ, sương mù bao phủ và văn hóa dân tộc thiểu số đặc sắc.', N'Đa dạng văn hóa các dân tộc: Hmông, Dao, Tày, Giáy với trang phục và lễ hội độc đáo.', N'Tháng 9 - Tháng 11', 4000000, 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(4, 2, N'Đà Nẵng', 'da-nang', N'Thành phố đáng sống với bãi biển Mỹ Khê tuyệt đẹp, cầu Rồng phun lửa và Bà Nà Hills huyền ảo.', N'Thành phố trẻ, năng động với sự giao thoa văn hóa Chăm Pa cổ đại và hiện đại.', N'Tháng 2 - Tháng 5', 4500000, 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(5, 2, N'Quảng Nam', 'quang-nam', N'Hội An cổ kính với phố cổ lung linh đèn lồng, Mỹ Sơn linh thiêng và bãi biển An Bàng yên bình.', N'Di sản văn hóa thế giới với phố cổ Hội An và thánh địa Mỹ Sơn của vương quốc Chăm Pa.', N'Tháng 2 - Tháng 8', 4000000, 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(6, 3, N'Lâm Đồng', 'lam-dong', N'Đà Lạt thành phố ngàn hoa với khí hậu mát mẻ quanh năm, hồ Xuân Hương thơ mộng và đồi chè xanh mướt.', N'Thành phố của tình yêu và thơ ca, với kiến trúc Pháp cổ điển và văn hóa cao nguyên.', N'Tháng 11 - Tháng 3', 3500000, 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(7, 4, N'TP. Hồ Chí Minh', 'tp-ho-chi-minh', N'Thành phố năng động nhất Việt Nam với nhịp sống sôi động, ẩm thực phong phú và cuộc sống về đêm sầm uất.', N'Trung tâm kinh tế, văn hóa phương Nam với sự pha trộn giữa truyền thống và hiện đại.', N'Tháng 12 - Tháng 4', 5000000, 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0),
(8, 5, N'Kiên Giang', 'kien-giang', N'Phú Quốc đảo ngọc với bãi biển cát trắng mịn màng, làn nước trong xanh và rừng nhiệt đới nguyên sinh.', N'Văn hóa biển đảo đặc trưng với nghề làm nước mắm truyền thống và nuôi trồng ngọc trai.', N'Tháng 11 - Tháng 3', 6000000, 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=800&q=80', GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Provinces OFF;
GO

-- Insert Destinations
SET IDENTITY_INSERT Destinations ON;

INSERT INTO Destinations (Id, ProvinceId, Name, Description, Address, Latitude, Longitude, EntryFee, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'Hồ Hoàn Kiếm', N'Biểu tượng của Hà Nội với Tháp Rùa và đền Ngọc Sơn cổ kính.', N'Quận Hoàn Kiếm, Hà Nội', 21.0285, 105.8542, 0, GETUTCDATE(), 'System', 0),
(2, 1, N'Văn Miếu - Quốc Tử Giám', N'Trường đại học đầu tiên của Việt Nam, nơi thờ Khổng Tử.', N'58 Quốc Tử Giám, Đống Đa, Hà Nội', 21.0277, 105.8355, 30000, GETUTCDATE(), 'System', 0),
(3, 2, N'Vịnh Hạ Long', N'Di sản thiên nhiên thế giới với hàng ngàn hòn đảo đá vôi kỳ vĩ.', N'Thành phố Hạ Long, Quảng Ninh', 20.9101, 107.1839, 200000, GETUTCDATE(), 'System', 0),
(4, 3, N'Ruộng Bậc Thang Mù Cang Chải', N'Ruộng bậc thang đẹp nhất Việt Nam, di sản văn hóa phi vật thể.', N'Mù Cang Chải, Yên Bái', 21.8333, 104.0667, 0, GETUTCDATE(), 'System', 0),
(5, 4, N'Bà Nà Hills', N'Khu du lịch trên núi với Cầu Vàng nổi tiếng và làng Pháp cổ điển.', N'Hòa Ninh, Hòa Vang, Đà Nẵng', 15.9959, 107.9983, 750000, GETUTCDATE(), 'System', 0),
(6, 5, N'Phố Cổ Hội An', N'Di sản văn hóa thế giới với kiến trúc cổ kính và đèn lồng rực rỡ.', N'Thành phố Hội An, Quảng Nam', 15.8801, 108.3380, 120000, GETUTCDATE(), 'System', 0),
(7, 6, N'Hồ Xuân Hương', N'Hồ nước ngọt đẹp nhất Đà Lạt, trung tâm của thành phố.', N'Phường 10, Đà Lạt, Lâm Đồng', 11.9404, 108.4383, 0, GETUTCDATE(), 'System', 0),
(8, 7, N'Dinh Độc Lập', N'Công trình kiến trúc lịch sử, chứng nhân của nhiều sự kiện quan trọng.', N'135 Nam Kỳ Khởi Nghĩa, Quận 1, TP.HCM', 10.7769, 106.6955, 40000, GETUTCDATE(), 'System', 0),
(9, 8, N'Bãi Sao Phú Quốc', N'Bãi biển đẹp nhất Phú Quốc với cát trắng mịn và nước biển trong xanh.', N'An Thới, Phú Quốc, Kiên Giang', 10.1699, 103.9654, 0, GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Destinations OFF;
GO

-- Insert Foods
SET IDENTITY_INSERT Foods ON;

INSERT INTO Foods (Id, ProvinceId, Name, Description, ThumbnailUrl, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'Phở Hà Nội', N'Món ăn truyền thống với nước dùng trong, thịt bò mềm và bánh phở dai.', 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0),
(2, 1, N'Bún Chả', N'Thịt nướng thơm phức ăn kèm bún tươi và nước mắm chua ngọt.', 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0),
(3, 2, N'Chả Mực Hạ Long', N'Đặc sản từ mực tươi, giã nhuyễn và nướng thơm.', 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0),
(4, 4, N'Mì Quảng', N'Món mì đặc trưng với nước dùng đậm đà, tôm thịt và rau sống.', 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0),
(5, 5, N'Cao Lầu', N'Món mì độc đáo chỉ có ở Hội An với nước từ giếng Bá Lễ.', 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0),
(6, 7, N'Bánh Xèo', N'Bánh giòn rụm với nhân tôm thịt, giá đỗ ăn kèm rau sống.', 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=400&q=80', GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Foods OFF;
GO

-- Insert Festivals
SET IDENTITY_INSERT Festivals ON;

INSERT INTO Festivals (Id, ProvinceId, Name, Description, HeldDate, LunarDate, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'Lễ Hội Đền Hùng', N'Lễ hội lớn nhất tưởng nhớ các vua Hùng - tổ tiên dân tộc Việt.', N'10/03', N'10/03 Âm lịch', GETUTCDATE(), 'System', 0),
(2, 2, N'Lễ Hội Hạ Long', N'Lễ hội du lịch quốc tế với nhiều hoạt động văn hóa nghệ thuật.', N'Tháng 4-5', NULL, GETUTCDATE(), 'System', 0),
(3, 5, N'Lễ Hội Đèn Lồng Hội An', N'Phố cổ Hội An lung linh ánh đèn lồng vào rằm hàng tháng.', N'Rằm hàng tháng', N'14-15 Âm lịch', GETUTCDATE(), 'System', 0),
(4, 7, N'Lễ Hội Áo Dài', N'Tôn vinh vẻ đẹp áo dài truyền thống Việt Nam.', N'Tháng 3', NULL, GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Festivals OFF;
GO

-- Insert Travel Seasons
SET IDENTITY_INSERT TravelSeasons ON;

INSERT INTO TravelSeasons (Id, ProvinceId, SeasonName, Months, WeatherCondition, Tips, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'Mùa Thu', N'Tháng 9 - Tháng 11', N'Mát mẻ, khô ráo, nắng đẹp', N'Thời điểm lý tưởng nhất để khám phá Hà Nội với thời tiết dễ chịu.', GETUTCDATE(), 'System', 0),
(2, 2, N'Mùa Xuân', N'Tháng 3 - Tháng 5', N'Ấm áp, ít mưa', N'Thời tiết đẹp để du thuyền trên Vịnh Hạ Long.', GETUTCDATE(), 'System', 0),
(3, 4, N'Mùa Khô', N'Tháng 2 - Tháng 5', N'Nắng đẹp, ít mưa', N'Thích hợp cho các hoạt động ngoài trời và tắm biển.', GETUTCDATE(), 'System', 0),
(4, 6, N'Mùa Hoa', N'Tháng 11 - Tháng 3', N'Mát mẻ, nhiều hoa nở', N'Đà Lạt đẹp nhất với hoa dã quỳ, hoa mimosa nở rộ.', GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT TravelSeasons OFF;
GO

-- Insert Sample User (for testing)
SET IDENTITY_INSERT Users ON;

INSERT INTO Users (Id, Username, Email, PasswordHash, FullName, CreatedAt, IsDeleted)
VALUES
(1, 'admin', 'admin@travelvietnam.com', '$2a$11$XYZ...', N'Administrator', GETUTCDATE(), 0),
(2, 'testuser', 'test@example.com', '$2a$11$ABC...', N'Test User', GETUTCDATE(), 0);

SET IDENTITY_INSERT Users OFF;
GO

-- Insert Sample Blogs
SET IDENTITY_INSERT Blogs ON;

INSERT INTO Blogs (Id, AuthorId, Title, Slug, Content, PublishedAt, IsPublished, CreatedAt, CreatedBy, IsDeleted)
VALUES
(1, 1, N'10 Điểm Đến Không Thể Bỏ Qua Khi Du Lịch Việt Nam', '10-diem-den-khong-the-bo-qua', N'Việt Nam là một đất nước tuyệt đẹp với nhiều điểm đến hấp dẫn từ Bắc vào Nam...', GETUTCDATE(), 1, GETUTCDATE(), 'System', 0),
(2, 1, N'Khám Phá Ẩm Thực Đường Phố Hà Nội', 'kham-pha-am-thuc-duong-pho-ha-noi', N'Hà Nội nổi tiếng với nền ẩm thực đường phố phong phú và đa dạng...', GETUTCDATE(), 1, GETUTCDATE(), 'System', 0),
(3, 1, N'Hướng Dẫn Du Lịch Phú Quốc Tự Túc', 'huong-dan-du-lich-phu-quoc-tu-tuc', N'Phú Quốc là hòn đảo lớn nhất Việt Nam với những bãi biển tuyệt đẹp...', GETUTCDATE(), 1, GETUTCDATE(), 'System', 0);

SET IDENTITY_INSERT Blogs OFF;
GO

PRINT 'Database seeded successfully!';
GO
