using System.Collections.Generic;

namespace TravelVietnam.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!; // e.g. "provinces:manage"

        // Navigation properties
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
