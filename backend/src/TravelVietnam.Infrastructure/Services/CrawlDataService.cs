using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TravelVietnam.Infrastructure.Services
{
    public class CrawlDataService
    {
        private readonly string _connectionString;
        private readonly HttpClient _httpClient;

        public CrawlDataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        }

        public async Task CrawlAndSeedAsync()
        {
            await SeedRegionsAsync();
            await SeedProvincesAsync();
            await SeedDestinationsAsync();
            await SeedCulturesAsync();
            await SeedBlogsAsync();
        }

        private async Task SeedRegionsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var check = new SqlCommand("SELECT COUNT(*) FROM Regions WHERE IsDeleted = 0", conn);
            if ((int)await check.ExecuteScalarAsync() > 0) return;

            var regions = new[]
            {
                ("Northern Vietnam", "northern-vietnam", "Explore the mountainous landscapes, ethnic minorities, and vibrant capital city of Hanoi.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b"),
                ("Central Vietnam", "central-vietnam", "Discover ancient imperial cities, pristine beaches, and UNESCO World Heritage Sites.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592"),
                ("Southern Vietnam", "southern-vietnam", "Experience the Mekong Delta, bustling Ho Chi Minh City, and tropical islands.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a")
            };

            var sql = "INSERT INTO Regions (Name, Slug, Description, ThumbnailUrl, BannerUrl, CreatedAt, IsDeleted) VALUES (@Name, @Slug, @Desc, @Thumb, @Banner, @Now, 0)";
            foreach (var (name, slug, desc, thumb, banner) in regions)
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Slug", slug);
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Thumb", thumb);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task SeedProvincesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var check = new SqlCommand("SELECT COUNT(*) FROM Provinces WHERE IsDeleted = 0", conn);
            if ((int)await check.ExecuteScalarAsync() > 0) return;

            var provinces = new[]
            {
                // Northern
                ("Hanoi", 1, "Capital city with thousand-year history, ancient temples, and French colonial architecture.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b"),
                ("Ha Long", 1, "UNESCO World Heritage Site famous for emerald waters and limestone karsts.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1583417319070-4a69db38a482"),
                ("Sapa", 1, "Mountainous region with terraced rice fields and ethnic minority villages.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a"),
                ("Ninh Binh", 1, "Halong Bay on land with limestone mountains, caves, and rivers.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592"),

                // Central
                ("Hue", 2, "Ancient imperial capital with royal tombs, pagodas, and citadel.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b"),
                ("Da Nang", 2, "Modern coastal city with beaches, Marble Mountains, and Dragon Bridge.", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1583417319070-4a69db38a482"),
                ("Hoi An", 2, "UNESCO World Heritage ancient town with lanterns, tailor shops, and riverside charm.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592"),
                ("Nha Trang", 2, "Beach resort city with islands, diving, and seafood.", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b"),

                // Southern
                ("Ho Chi Minh City", 3, "Largest city with French colonial landmarks, war museums, and vibrant street life.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a"),
                ("Mekong Delta", 3, "River delta with floating markets, fruit orchards, and traditional villages.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b"),
                ("Phu Quoc", 3, "Tropical island with white sand beaches, diving, and night markets.", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a"),
                ("Vung Tau", 3, "Beach city near Ho Chi Minh City with seafood and coastal views.", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1528127269322-539801943592")
            };

            var sql = "INSERT INTO Provinces (Name, Slug, Description, ThumbnailUrl, BannerUrl, RegionId, CreatedAt, IsDeleted) VALUES (@Name, @Slug, @Desc, @Thumb, @Banner, @RegionId, @Now, 0)";
            foreach (var (name, regionId, desc, thumb, banner) in provinces)
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Slug", ToSlug(name));
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Thumb", thumb);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@RegionId", regionId);
                cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task SeedDestinationsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var check = new SqlCommand("SELECT COUNT(*) FROM Destinations WHERE IsDeleted = 0", conn);
            if ((int)await check.ExecuteScalarAsync() > 0) return;

            var destinations = new[]
            {
                // Hanoi
                ("Hoan Kiem Lake", 1, 1, "Historic lake in the heart of Hanoi with Ngoc Son Temple.", "Cultural", "Year-round", 0, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", 4.5m, true),
                ("Old Quarter", 1, 1, "Ancient commercial district with narrow streets and traditional shops.", "Cultural", "Year-round", 50000, "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1528127269322-539801943592", 4.7m, true),
                ("Temple of Literature", 1, 1, "Vietnam's first university, dedicated to Confucius.", "Cultural", "Year-round", 30000, "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", 4.6m, false),

                // Ha Long
                ("Ha Long Bay", 2, 1, "UNESCO World Heritage Site with 1600 limestone islands.", "Nature", "Oct-Apr", 500000, "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592", 4.8m, true),
                ("Sung Sot Cave", 2, 1, "Largest cave in Ha Long Bay with stunning stalactites.", "Nature", "Oct-Apr", 200000, "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", 4.5m, false),

                // Sapa
                ("Fansipan Mountain", 3, 1, "Highest peak in Indochina, accessible by cable car.", "Adventure", "Sep-Nov, Mar-May", 700000, "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", 4.7m, true),
                ("Cat Cat Village", 3, 1, "Traditional H'mong village with waterfalls and handicrafts.", "Cultural", "Sep-Nov, Mar-May", 70000, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", 4.4m, false),

                // Hue
                ("Imperial City", 5, 2, "Walled fortress and palace of Nguyen Dynasty emperors.", "Cultural", "Feb-Apr", 200000, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", 4.6m, true),
                ("Thien Mu Pagoda", 5, 2, "Iconic seven-story pagoda overlooking Perfume River.", "Cultural", "Year-round", 0, "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", 4.5m, false),

                // Hoi An
                ("Ancient Town", 7, 2, "UNESCO World Heritage Site with preserved architecture and lanterns.", "Cultural", "Feb-May", 120000, "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592", 4.8m, true),
                ("An Bang Beach", 7, 2, "Pristine beach near Hoi An with seafood restaurants.", "Beach", "Feb-Aug", 0, "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", 4.4m, false),

                // Ho Chi Minh City
                ("Ben Thanh Market", 9, 3, "Historic market with food, souvenirs, and local life.", "Cultural", "Year-round", 0, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", 4.3m, true),
                ("Cu Chi Tunnels", 9, 3, "Underground tunnel network from Vietnam War.", "Historical", "Year-round", 110000, "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", 4.6m, true),

                // Phu Quoc
                ("Sao Beach", 11, 3, "White sand beach with crystal clear water.", "Beach", "Nov-Mar", 0, "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", 4.7m, true),
                ("Phu Quoc Night Market", 11, 3, "Seafood market with grilled fish and local snacks.", "Food", "Year-round", 200000, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", 4.5m, false)
            };

            var sql = @"INSERT INTO Destinations (Name, Slug, ShortDescription, ThumbnailUrl, BannerUrl, ProvinceId, RegionId, Category, BestTimeToVisit, EstimatedBudget, Rating, IsFeatured, CreatedAt, IsDeleted)
                        VALUES (@Name, @Slug, @Desc, @Thumb, @Banner, @ProvinceId, @RegionId, @Category, @BestTime, @Budget, @Rating, @Featured, @Now, 0)";

            foreach (var (name, provinceId, regionId, desc, category, bestTime, budget, thumb, banner, rating, featured) in destinations)
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Slug", ToSlug(name));
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Thumb", thumb);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@ProvinceId", provinceId);
                cmd.Parameters.AddWithValue("@RegionId", regionId);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@BestTime", bestTime);
                cmd.Parameters.AddWithValue("@Budget", budget);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.Parameters.AddWithValue("@Featured", featured);
                cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task SeedCulturesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var check = new SqlCommand("SELECT COUNT(*) FROM Cultures WHERE IsDeleted = 0", conn);
            if ((int)await check.ExecuteScalarAsync() > 0) return;

            var cultures = new[]
            {
                ("Water Puppetry", (int?)1, "Traditional Vietnamese art form performed on water with wooden puppets.", "Performing Arts", "Year-round", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", true),
                ("Ao Dai", (int?)null, "Traditional Vietnamese dress worn by women, symbol of elegance.", "Traditional Dress", (string?)null, "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1528127269322-539801943592", true),
                ("Tet Festival", (int?)null, "Vietnamese Lunar New Year, the most important celebration.", "Festival", "Jan-Feb", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", true),
                ("Hue Royal Court Music", (int?)2, "UNESCO Intangible Cultural Heritage, traditional court music.", "Music", (string?)null, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", false),
                ("Lantern Festival", (int?)2, "Monthly full moon celebration in Hoi An with colorful lanterns.", "Festival", "Monthly", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592", true),
                ("Don Ca Tai Tu", (int?)3, "Southern Vietnamese folk music, UNESCO heritage.", "Music", (string?)null, "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", false),
                ("Conical Hat Making", (int?)null, "Traditional craft of making non la, iconic Vietnamese hat.", "Handicraft", (string?)null, "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", false)
            };

            var sql = @"INSERT INTO Cultures (Title, Slug, Description, ThumbnailUrl, BannerUrl, RegionId, CultureType, FestivalSeason, IsFeatured, CreatedAt, IsDeleted)
                        VALUES (@Title, @Slug, @Desc, @Thumb, @Banner, @RegionId, @Type, @Season, @Featured, @Now, 0)";

            foreach (var (title, regionId, desc, type, season, thumb, banner, featured) in cultures)
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Slug", ToSlug(title));
                cmd.Parameters.AddWithValue("@Desc", desc);
                cmd.Parameters.AddWithValue("@Thumb", thumb);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@RegionId", (object?)regionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Season", (object?)season ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Featured", featured);
                cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task SeedBlogsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var check = new SqlCommand("SELECT COUNT(*) FROM Blogs WHERE IsDeleted = 0", conn);
            if ((int)await check.ExecuteScalarAsync() > 0) return;

            var blogs = new[]
            {
                ("Ultimate Guide to Visiting Ha Long Bay", "Complete travel guide with cruise options, best time to visit, and hidden gems.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592", "Travel Guide", "Ha Long Bay, UNESCO, Cruise", 8, true),
                ("10 Must-Try Vietnamese Street Foods", "From pho to banh mi, explore Vietnam's incredible street food culture.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "Food & Drink", "Street Food, Vietnamese Cuisine, Pho", 6, true),
                ("Exploring Hoi An Ancient Town at Night", "Experience the magical atmosphere of Hoi An's lantern-lit streets.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "Travel Guide", "Hoi An, Ancient Town, Lanterns", 7, true),
                ("Trekking Sapa: A Complete Guide", "Everything you need to know about trekking in Sapa's rice terraces.", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "https://images.unsplash.com/photo-1528127269322-539801943592", "Adventure", "Sapa, Trekking, Rice Terraces", 10, false),
                ("Best Beaches in Southern Vietnam", "Discover pristine beaches from Phu Quoc to Mui Ne.", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "Travel Guide", "Beaches, Phu Quoc, Southern Vietnam", 9, true),
                ("Understanding Vietnamese Coffee Culture", "Learn about Vietnam's unique coffee traditions and best cafes.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1583417319070-4a69db38a482", "Food & Drink", "Coffee, Vietnamese Culture, Cafes", 5, false),
                ("Motorbike Road Trip Through Central Vietnam", "Epic coastal route from Hue to Hoi An to Nha Trang.", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b", "Adventure", "Motorbike, Road Trip, Central Vietnam", 12, true),
                ("Hanoi's Hidden Temples and Pagodas", "Discover lesser-known spiritual sites in Vietnam's capital.", "https://images.unsplash.com/photo-1528127269322-539801943592", "https://images.unsplash.com/photo-1552465011-b4e21bf6e79a", "Culture", "Hanoi, Temples, Pagodas", 7, false)
            };

            var sql = @"INSERT INTO Blogs (Title, Slug, Summary, ThumbnailUrl, BannerUrl, Author, Tags, ReadTime, IsFeatured, ViewCount, PublishedAt, CreatedAt, IsDeleted)
                        VALUES (@Title, @Slug, @Summary, @Thumb, @Banner, @Author, @Tags, @ReadTime, @Featured, 0, @Published, @Now, 0)";

            foreach (var (title, summary, thumb, banner, author, tags, readTime, featured) in blogs)
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Slug", ToSlug(title));
                cmd.Parameters.AddWithValue("@Summary", summary);
                cmd.Parameters.AddWithValue("@Thumb", thumb);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@Author", "Travel Vietnam Team");
                cmd.Parameters.AddWithValue("@Tags", tags);
                cmd.Parameters.AddWithValue("@ReadTime", readTime);
                cmd.Parameters.AddWithValue("@Featured", featured);
                cmd.Parameters.AddWithValue("@Published", DateTime.UtcNow.AddDays(-new Random().Next(1, 90)));
                cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private string ToSlug(string text)
        {
            text = text.ToLowerInvariant();
            text = Regex.Replace(text, @"[àáạảãâầấậẩẫăằắặẳẵ]", "a");
            text = Regex.Replace(text, @"[èéẹẻẽêềếệểễ]", "e");
            text = Regex.Replace(text, @"[ìíịỉĩ]", "i");
            text = Regex.Replace(text, @"[òóọỏõôồốộổỗơờớợởỡ]", "o");
            text = Regex.Replace(text, @"[ùúụủũưừứựửữ]", "u");
            text = Regex.Replace(text, @"[ỳýỵỷỹ]", "y");
            text = Regex.Replace(text, @"đ", "d");
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"-+", "-");
            return text.Trim('-');
        }
    }
}
