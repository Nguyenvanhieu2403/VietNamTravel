using System;
using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public int RoleId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;

        // Navigation properties
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<TravelPlan> TravelPlans { get; set; } = new List<TravelPlan>();
    }
}
