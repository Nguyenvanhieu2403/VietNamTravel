using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Domain.Common;
using TravelVietnam.Domain.Entities;
using TravelVietnam.Application.Interfaces;

namespace TravelVietnam.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<Destination> Destinations => Set<Destination>();
        public DbSet<Food> Foods => Set<Food>();
        public DbSet<Festival> Festivals => Set<Festival>();
        public DbSet<TravelSeason> TravelSeasons => Set<TravelSeason>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Blog> Blogs => Set<Blog>();
        public DbSet<Culture> Cultures => Set<Culture>();
        public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<TravelPlan> TravelPlans => Set<TravelPlan>();
        public DbSet<TravelPlanDestination> TravelPlanDestinations => Set<TravelPlanDestination>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Slugs Unique
            modelBuilder.Entity<Region>().HasIndex(r => r.Slug).IsUnique();
            modelBuilder.Entity<Province>().HasIndex(p => p.Slug).IsUnique();
            modelBuilder.Entity<Blog>().HasIndex(b => b.Slug).IsUnique();
            modelBuilder.Entity<Destination>().HasIndex(d => d.Slug).IsUnique();
            modelBuilder.Entity<Culture>().HasIndex(c => c.Slug).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Configure Cascade Delete Behavior to prevent multiple cascade paths
            // Region -> Destination: NoAction (prevents cycle with Region -> Province -> Destination)
            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Region)
                .WithMany(r => r.Destinations)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Province -> Destination: NoAction (safer for production)
            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Province)
                .WithMany(p => p.Destinations)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.NoAction);

            // Region -> Province: NoAction (prevents cascade to Destination)
            modelBuilder.Entity<Province>()
                .HasOne(p => p.Region)
                .WithMany(r => r.Provinces)
                .HasForeignKey(p => p.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Region -> Culture: NoAction (optional FK, safer)
            modelBuilder.Entity<Culture>()
                .HasOne(c => c.Region)
                .WithMany(r => r.Cultures)
                .HasForeignKey(c => c.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Join Tables
            modelBuilder.Entity<TravelPlanDestination>()
                .HasKey(tpd => new { tpd.TravelPlanId, tpd.DestinationId });

            modelBuilder.Entity<TravelPlanDestination>()
                .HasOne(tpd => tpd.TravelPlan)
                .WithMany(tp => tp.TravelPlanDestinations)
                .HasForeignKey(tpd => tpd.TravelPlanId);

            modelBuilder.Entity<TravelPlanDestination>()
                .HasOne(tpd => tpd.Destination)
                .WithMany(d => d.TravelPlanDestinations)
                .HasForeignKey(tpd => tpd.DestinationId);

            // Many-to-Many Role & Permission configuration
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                    j => j.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId")
                );

            // Setup Global Query Filters for Soft Delete
            modelBuilder.Entity<Region>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Province>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Destination>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Food>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Festival>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TravelSeason>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Blog>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Culture>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MediaFile>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TravelPlan>().HasQueryFilter(x => !x.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var username = _currentUserService.Username ?? "System";
            var now = _dateTimeService.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = username;
                        entry.Entity.CreatedAt = now;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = username;
                        entry.Entity.LastModifiedAt = now;
                        break;

                    case EntityState.Deleted:
                        // Soft delete configuration
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedBy = username;
                        entry.Entity.DeletedAt = now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    // RolePermission entity helper
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
