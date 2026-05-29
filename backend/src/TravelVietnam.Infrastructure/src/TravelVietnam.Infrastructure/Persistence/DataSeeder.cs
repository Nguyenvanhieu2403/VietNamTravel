using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            // Seed Destinations
            if (!context.Destinations.Any())
            {
                var destinations = new List<Destination>
                {
                    new Destination
                    {
                        Name = "Vịnh Hạ Long",
                        Slug = "vinh-ha-long",
                        ShortDescription = "Di sản thiên nhiên thế giới với hàng nghìn đảo đá vôi",
                        Description = "Vịnh Hạ Long là một vịnh nhỏ thuộc phần bờ tây vịnh Bắc Bộ tại khu vực biển Đông Bắc Việt Nam, bao gồm vùng biển đảo của thành phố Hạ Long thuộc tỉnh Quảng Ninh.",
                        ThumbnailUrl = "/images/destinations/ha-long-bay-thumb.jpg",
                        BannerUrl = "/images/destinations/ha-long-bay-banner.jpg",
                        ProvinceId = 1,
                        RegionId = 1,
                        Category = "Natural Wonder",
                        BestTimeToVisit = "Tháng 3 - Tháng 5, Tháng 9 - Tháng 11",
                        EstimatedBudget = 2000000,
                        Latitude = 20.9101,
                        Longitude = 107.1839,
                        Rating = 4.8m,
                        IsFeatured = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Destination
                    {
                        Name = "Phố Cổ Hội An",
                        Slug = "pho-co-hoi-an",
                        ShortDescription = "Thành phố cổ với kiến trúc độc đáo và văn hóa đa dạng",
                        Description = "Hội An là một thành phố trực thuộc tỉnh Quảng Nam, cách thành phố Đà Nẵng khoảng 30 km về phía nam. Phố cổ Hội An từng là một thương cảng quốc tế sầm uất.",
                        ThumbnailUrl = "/images/destinations/hoi-an-thumb.jpg",
                        BannerUrl = "/images/destinations/hoi-an-banner.jpg",
                        ProvinceId = 2,
                        RegionId = 3,
                        Category = "Historical Site",
                        BestTimeToVisit = "Tháng 2 - Tháng 5",
                        EstimatedBudget = 1500000,
                        Latitude = 15.8801,
                        Longitude = 108.3380,
                        Rating = 4.9m,
                        IsFeatured = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Destination
                    {
                        Name = "Sapa",
                        Slug = "sapa",
                        ShortDescription = "Thị trấn miền núi với ruộng bậc thang tuyệt đẹp",
                        Description = "Sa Pa là một thị trấn thuộc huyện Sa Pa, tỉnh Lào Cai, nằm ở vùng Tây Bắc Việt Nam, cách Hà Nội khoảng 380 km về phía tây bắc.",
                        ThumbnailUrl = "/images/destinations/sapa-thumb.jpg",
                        BannerUrl = "/images/destinations/sapa-banner.jpg",
                        ProvinceId = 3,
                        RegionId = 1,
                        Category = "Mountain Town",
                        BestTimeToVisit = "Tháng 9 - Tháng 11, Tháng 3 - Tháng 5",
                        EstimatedBudget = 2500000,
                        Latitude = 22.3364,
                        Longitude = 103.8438,
                        Rating = 4.7m,
                        IsFeatured = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Destinations.AddRange(destinations);
                context.SaveChanges();
            }

            // Seed Culture
            if (!context.Cultures.Any())
            {
                var cultures = new List<Culture>
                {
                    new Culture
                    {
                        Title = "Tết Nguyên Đán",
                        Slug = "tet-nguyen-dan",
                        Description = "Lễ hội truyền thống quan trọng nhất của người Việt Nam",
                        Content = "Tết Nguyên Đán, hay còn gọi là Tết Âm lịch, là dịp lễ quan trọng nhất trong năm của người Việt Nam. Đây là thời gian để gia đình sum họp, thờ cúng tổ tiên và cầu mong một năm mới an khang thịnh vượng.",
                        ThumbnailUrl = "/images/culture/tet-thumb.jpg",
                        BannerUrl = "/images/culture/tet-banner.jpg",
                        RegionId = null,
                        CultureType = "Festival",
                        FestivalSeason = "Tháng 1 - Tháng 2 (Âm lịch)",
                        IsFeatured = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Culture
                    {
                        Title = "Múa Rối Nước",
                        Slug = "mua-roi-nuoc",
                        Description = "Nghệ thuật biểu diễn truyền thống độc đáo của Việt Nam",
                        Content = "Múa rối nước là một loại hình nghệ thuật biểu diễn truyền thống của Việt Nam, có nguồn gốc từ đồng bằng sông Hồng. Đây là một di sản văn hóa phi vật thể quốc gia.",
                        ThumbnailUrl = "/images/culture/water-puppet-thumb.jpg",
                        BannerUrl = "/images/culture/water-puppet-banner.jpg",
                        RegionId = 1,
                        CultureType = "Traditional Art",
                        FestivalSeason = null,
                        IsFeatured = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Cultures.AddRange(cultures);
                context.SaveChanges();
            }

            // Seed Blogs
            if (!context.Blogs.Any())
            {
                var blogs = new List<Blog>
                {
                    new Blog
                    {
                        Title = "10 Điểm Đến Không Thể Bỏ Qua Khi Du Lịch Việt Nam",
                        Slug = "10-diem-den-khong-the-bo-qua",
                        Summary = "Khám phá những địa điểm du lịch tuyệt vời nhất tại Việt Nam",
                        Content = "Việt Nam là một đất nước với vẻ đẹp đa dạng từ núi non hùng vĩ đến biển cả bao la. Trong bài viết này, chúng tôi sẽ giới thiệu 10 điểm đến không thể bỏ qua...",
                        ThumbnailUrl = "/images/blogs/top-10-thumb.jpg",
                        BannerUrl = "/images/blogs/top-10-banner.jpg",
                        Author = "Nguyễn Văn A",
                        Tags = "du lịch,việt nam,điểm đến",
                        ReadTime = 10,
                        ViewCount = 1250,
                        IsFeatured = true,
                        PublishedAt = DateTime.UtcNow.AddDays(-7),
                        CreatedAt = DateTime.UtcNow.AddDays(-7)
                    },
                    new Blog
                    {
                        Title = "Ẩm Thực Việt Nam: Hành Trình Khám Phá Hương Vị",
                        Slug = "am-thuc-viet-nam-hanh-trinh-kham-pha-huong-vi",
                        Summary = "Tìm hiểu về nền ẩm thực phong phú và đa dạng của Việt Nam",
                        Content = "Ẩm thực Việt Nam nổi tiếng thế giới với sự đa dạng và phong phú. Từ phở Hà Nội, bún bò Huế đến bánh mì Sài Gòn...",
                        ThumbnailUrl = "/images/blogs/cuisine-thumb.jpg",
                        BannerUrl = "/images/blogs/cuisine-banner.jpg",
                        Author = "Trần Thị B",
                        Tags = "ẩm thực,văn hóa,việt nam",
                        ReadTime = 8,
                        ViewCount = 980,
                        IsFeatured = true,
                        PublishedAt = DateTime.UtcNow.AddDays(-5),
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    }
                };

                context.Blogs.AddRange(blogs);
                context.SaveChanges();
            }
        }
    }
}
