using System;
using System.Collections.Generic;

namespace TravelVietnam.Application.DTOs.Travel
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public List<ProvinceListDto> Provinces { get; set; } = new();
    }

    public class ProvinceListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal AverageBudget { get; set; }
        public string? ThumbnailUrl { get; set; }
    }

    public class ProvinceDto
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? CultureDescription { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal AverageBudget { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }

        public List<DestinationDto> Destinations { get; set; } = new();
        public List<FoodDto> Foods { get; set; } = new();
        public List<FestivalDto> Festivals { get; set; } = new();
        public List<TravelSeasonDto> Seasons { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
        public List<MediaFileDto> MediaFiles { get; set; } = new();
    }

    public class DestinationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? BannerUrl { get; set; }
        public int ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public int RegionId { get; set; }
        public string? RegionName { get; set; }
        public string? Category { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal? EstimatedBudget { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? Rating { get; set; }
        public bool IsFeatured { get; set; }
        public List<MediaFileDto> MediaFiles { get; set; } = new();
    }

    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? RecipeLink { get; set; }
        public string? ThumbnailUrl { get; set; }
    }

    public class FestivalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? HeldDate { get; set; }
        public string? LunarDate { get; set; }
    }

    public class TravelSeasonDto
    {
        public int Id { get; set; }
        public string SeasonName { get; set; } = null!;
        public string? Months { get; set; }
        public string? WeatherCondition { get; set; }
        public string? Tips { get; set; }
    }

    public class ReviewDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string UserFullName { get; set; } = null!;
        public int? DestinationId { get; set; }
        public int? ProvinceId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReviewRequest
    {
        public int? DestinationId { get; set; }
        public int? ProvinceId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Summary { get; set; }
        public string Content { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? Author { get; set; }
        public string? Tags { get; set; }
        public int? ReadTime { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MediaFileDto> MediaFiles { get; set; } = new();
    }

    public class CultureDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? BannerUrl { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string? CultureType { get; set; }
        public string? FestivalSeason { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MediaFileDto> MediaFiles { get; set; } = new();
    }

    public class CreateBlogRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsPublished { get; set; }
    }

    public class MediaFileDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string FileType { get; set; } = null!;
    }
}
