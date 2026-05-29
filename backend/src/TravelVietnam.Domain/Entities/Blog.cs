using System;
using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Blog : BaseAuditableEntity
    {
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

        // Navigation properties
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
    }
}
