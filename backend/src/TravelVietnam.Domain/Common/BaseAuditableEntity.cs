using System;

namespace TravelVietnam.Domain.Common
{
    public abstract class BaseAuditableEntity
    {
        public int Id { get; set; }
        
        // Audit Metadata
        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        
        // Soft Delete support
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
