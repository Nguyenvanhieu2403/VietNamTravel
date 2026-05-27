using System;
using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Blog : BaseAuditableEntity
    {
        public int AuthorId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }

        // Navigation properties
        public virtual User Author { get; set; } = null!;
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
    }
}
