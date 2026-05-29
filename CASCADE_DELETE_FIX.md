# EF Core Cascade Delete Fix - Complete Solution

## Problem

```
Introducing FOREIGN KEY constraint 'FK_Destinations_Regions_RegionId' on table 'Destinations' 
may cause cycles or multiple cascade paths
```

## Root Cause

SQL Server detected **multiple cascade paths** from `Region` to `Destination`:

1. **Path 1:** Region → Province → Destination (CASCADE)
2. **Path 2:** Region → Destination (CASCADE)

When you delete a Region, SQL Server doesn't know which path to follow, creating ambiguity. SQL Server prohibits this to prevent data integrity issues.

## Why This Happens

EF Core's default convention for **required relationships** (non-nullable foreign keys) is `DeleteBehavior.Cascade`. 

In this schema:
- `Destination.ProvinceId` is required (int, not nullable)
- `Destination.RegionId` is required (int, not nullable)
- `Province.RegionId` is required (int, not nullable)

This creates the cascade cycle that SQL Server rejects.

---

## Solution Applied

### Modified File: `ApplicationDbContext.cs`

Added explicit `OnDelete(DeleteBehavior.NoAction)` for all relationships that could create cascade paths:

```csharp
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
```

---

## Migration Commands

### 1. Remove Failed Migration (Already Done)
```bash
cd backend/src/TravelVietnam.WebApi
dotnet ef migrations remove --project ../TravelVietnam.Infrastructure --force
```

### 2. Create New Migration (Already Done)
```bash
dotnet ef migrations add AddDestinationsCultureBlogsFeaturesFixed --project ../TravelVietnam.Infrastructure
```

### 3. Apply Migration to Database
```bash
# Start Docker and SQL Server first
docker-compose -f docker-compose.dev.yml up -d db

# Wait 30 seconds for SQL Server to initialize
sleep 30  # Linux/Mac
timeout /t 30  # Windows

# Apply migration
cd backend/src/TravelVietnam.WebApi
dotnet ef database update --project ../TravelVietnam.Infrastructure
```

---

## Verification

### Check Migration Status
```bash
cd backend/src/TravelVietnam.WebApi
dotnet ef migrations list --project ../TravelVietnam.Infrastructure
```

Expected output:
```
20260527024831_InitialCreate
20260528064805_AddDestinationsCultureBlogsFeaturesFixed
```
(No "Pending" marker)

### Test API
```bash
# Start backend
docker-compose -f docker-compose.dev.yml up -d

# Test endpoints
curl http://localhost:5000/api/v1/blogs
curl http://localhost:5000/api/v1/destinations
curl http://localhost:5000/api/v1/cultures
```

All should return `200 OK`.

---

## What Changed in the Migration

The new migration `20260528064805_AddDestinationsCultureBlogsFeaturesFixed.cs`:

### Blog Table Updates
✅ Adds: Author, BannerUrl, Summary, Tags, ReadTime, ThumbnailUrl, ViewCount, UserId  
✅ Renames: IsPublished → IsFeatured  
✅ Removes: AuthorId FK

### Destination Table Updates
✅ Adds: Slug, ShortDescription, BannerUrl, Category, BestTimeToVisit, EstimatedBudget, Rating, IsFeatured, RegionId  
✅ Renames: Address → ThumbnailUrl  
✅ Removes: EntryFee  
✅ **Foreign Keys:** All use `NoAction` delete behavior

### New Cultures Table
✅ Complete table with: Id, Title, Slug, Description, Content, ThumbnailUrl, BannerUrl, RegionId, CultureType, FestivalSeason, IsFeatured  
✅ **Foreign Key:** Region relationship uses `NoAction`

### Foreign Key Changes
✅ **Drops existing CASCADE foreign keys:**
- FK_Destinations_Provinces_ProvinceId
- FK_Provinces_Regions_RegionId

✅ **Recreates with NoAction:**
- FK_Destinations_Provinces_ProvinceId (NoAction)
- FK_Destinations_Regions_RegionId (NoAction)
- FK_Provinces_Regions_RegionId (NoAction)
- FK_Cultures_Regions_RegionId (NoAction)

---

## Impact of NoAction Delete Behavior

### What NoAction Means
When you try to delete a parent record (e.g., Region), SQL Server will:
1. Check if any child records exist (e.g., Provinces, Destinations)
2. If children exist, **throw an error** and prevent the delete
3. You must manually delete children first

### Example
```csharp
// This will FAIL if the region has provinces or destinations
await dbContext.Regions.Where(r => r.Id == 1).ExecuteDeleteAsync();

// Correct approach: Delete children first
var region = await dbContext.Regions
    .Include(r => r.Provinces)
    .Include(r => r.Destinations)
    .FirstOrDefaultAsync(r => r.Id == 1);

dbContext.Destinations.RemoveRange(region.Destinations);
dbContext.Provinces.RemoveRange(region.Provinces);
dbContext.Regions.Remove(region);
await dbContext.SaveChangesAsync();
```

### Why This Is Better
- **Prevents accidental data loss** - You can't accidentally delete a Region and lose all Provinces/Destinations
- **Explicit control** - Application code decides deletion order
- **Production safe** - No cascade surprises in production
- **Soft delete compatible** - Works well with your soft delete implementation

---

## Alternative: Use Soft Delete Instead

Since your entities inherit from `BaseAuditableEntity` with soft delete support, you rarely need hard deletes:

```csharp
// Soft delete (recommended)
var region = await dbContext.Regions.FindAsync(1);
dbContext.Regions.Remove(region);  // Sets IsDeleted = true
await dbContext.SaveChangesAsync();

// Children remain accessible but filtered by global query filter
```

---

## Summary

**Problem:** Multiple cascade paths from Region to Destination  
**Solution:** Changed all foreign keys to `DeleteBehavior.NoAction`  
**Migration:** `20260528064805_AddDestinationsCultureBlogsFeaturesFixed`  
**Status:** Ready to apply to database  

**Next Step:** Run `dotnet ef database update` to apply the migration.
