# Database Migration Fix Guide

## Problem

The backend API was failing with SQL errors:
```
Invalid column name: Author
Invalid column name: BannerUrl
Invalid column name: IsFeatured
Invalid column name: ReadTime
Invalid column name: Summary
Invalid column name: Tags
Invalid column name: ThumbnailUrl
Invalid column name: ViewCount
```

## Root Cause

The Entity Framework migration `20260528033414_AddDestinationsCultureBlogsFeatures` was created but **never applied** to the SQL Server database. This migration adds all the missing Blog columns plus Destination and Culture enhancements.

## Solution

Apply the pending migration to update the database schema.

---

## Quick Fix (Recommended)

### Windows
```bash
# 1. Start Docker Desktop
# 2. Run the migration script
apply-migrations.bat
```

### Linux/Mac
```bash
# 1. Start Docker Desktop
# 2. Run the migration script
chmod +x apply-migrations.sh
./apply-migrations.sh
```

---

## Manual Fix

If you prefer to run commands manually:

### Step 1: Start Docker and Database
```bash
# Start SQL Server container
docker-compose -f docker-compose.dev.yml up -d db

# Wait 30 seconds for SQL Server to initialize
```

### Step 2: Apply Migration
```bash
cd backend/src/TravelVietnam.WebApi
dotnet ef database update --project ../TravelVietnam.Infrastructure
```

### Step 3: Verify Migration
```bash
# List migrations (should show no pending)
dotnet ef migrations list --project ../TravelVietnam.Infrastructure
```

---

## What the Migration Does

The `AddDestinationsCultureBlogsFeatures` migration:

### Blogs Table Updates
- ✅ Adds `Author` (nvarchar, nullable)
- ✅ Adds `BannerUrl` (nvarchar, nullable)
- ✅ Adds `Summary` (nvarchar, nullable)
- ✅ Adds `Tags` (nvarchar, nullable)
- ✅ Adds `ReadTime` (int, nullable)
- ✅ Adds `ThumbnailUrl` (nvarchar, nullable)
- ✅ Adds `ViewCount` (int, not null, default 0)
- ✅ Adds `UserId` (int, nullable, FK to Users)
- ✅ Renames `IsPublished` → `IsFeatured`
- ✅ Removes `AuthorId` foreign key

### Destinations Table Updates
- ✅ Adds `Slug` (nvarchar(450), unique index)
- ✅ Adds `ShortDescription` (nvarchar, nullable)
- ✅ Adds `BannerUrl` (nvarchar, nullable)
- ✅ Adds `Category` (nvarchar, nullable)
- ✅ Adds `BestTimeToVisit` (nvarchar, nullable)
- ✅ Adds `EstimatedBudget` (decimal(18,2), nullable)
- ✅ Adds `Rating` (decimal(18,2), nullable)
- ✅ Adds `IsFeatured` (bit, not null, default false)
- ✅ Adds `RegionId` (int, not null, FK to Regions)
- ✅ Renames `Address` → `ThumbnailUrl`
- ✅ Removes `EntryFee`

### New Cultures Table
- ✅ Creates complete `Cultures` table with:
  - Id, Title, Slug, Description, Content
  - ThumbnailUrl, BannerUrl
  - RegionId (FK to Regions)
  - CultureType, FestivalSeason
  - IsFeatured
  - Full audit fields (CreatedBy, CreatedAt, etc.)

### MediaFiles Table Updates
- ✅ Adds `CultureId` (int, nullable, FK to Cultures)

---

## Verification

After applying the migration:

### 1. Check Migration Status
```bash
cd backend/src/TravelVietnam.WebApi
dotnet ef migrations list --project ../TravelVietnam.Infrastructure
```

Expected output:
```
20260527024831_InitialCreate
20260528033414_AddDestinationsCultureBlogsFeatures
```
(No "Pending" marker)

### 2. Test API Endpoints
```bash
# Start backend
docker-compose -f docker-compose.dev.yml up -d

# Test blogs endpoint
curl http://localhost:5000/api/v1/blogs

# Test destinations endpoint
curl http://localhost:5000/api/v1/destinations

# Test cultures endpoint
curl http://localhost:5000/api/v1/cultures
```

All should return `200 OK` with JSON data (or empty arrays if no data seeded).

### 3. Check Database Schema
```bash
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -Q "USE TravelVietnamDb; SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Blogs' ORDER BY ORDINAL_POSITION"
```

Should show all columns including: Author, BannerUrl, IsFeatured, ReadTime, Summary, Tags, ThumbnailUrl, ViewCount.

---

## Troubleshooting

### Error: "Docker is not running"
**Solution:** Start Docker Desktop and wait for it to fully initialize.

### Error: "Cannot connect to SQL Server"
**Solution:** 
```bash
# Restart database container
docker-compose -f docker-compose.dev.yml restart db

# Wait 30 seconds
sleep 30

# Try migration again
cd backend/src/TravelVietnam.WebApi
dotnet ef database update --project ../TravelVietnam.Infrastructure
```

### Error: "Migration already applied"
**Solution:** This is good! The migration is already in the database. No action needed.

### Error: "Column already exists"
**Solution:** The migration was partially applied. Options:
1. **Recommended:** Rollback and reapply:
   ```bash
   dotnet ef database update 20260527024831_InitialCreate --project ../TravelVietnam.Infrastructure
   dotnet ef database update --project ../TravelVietnam.Infrastructure
   ```
2. **Nuclear option:** Reset database (loses all data):
   ```bash
   docker-compose down -v
   docker-compose -f docker-compose.dev.yml up -d
   # Wait 30 seconds, then apply migrations
   ```

---

## Next Steps

After successful migration:

1. **Seed sample data** (optional):
   ```bash
   docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -d TravelVietnamDb -i database/seed-data.sql
   ```

2. **Start full stack**:
   ```bash
   # Backend + Frontend
   docker-compose up -d
   
   # Or backend only (for frontend dev)
   docker-compose -f docker-compose.dev.yml up -d
   cd frontend && npm start
   ```

3. **Test the application**:
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:5000/swagger
   - Test pages: /destinations, /culture, /blog

---

## Summary

The issue was a **pending migration**, not a schema design problem. The migration file exists and is correct—it just needed to be applied to the database. Running `dotnet ef database update` resolves all "Invalid column name" errors.
