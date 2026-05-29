#!/bin/bash

# Apply pending EF Core migrations to the database

set -e

echo "=========================================="
echo "Travel Vietnam - Apply Database Migrations"
echo "=========================================="
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker Desktop first."
    exit 1
fi

echo "✅ Docker is running"
echo ""

# Check if database container is running
if ! docker ps | grep -q travel_vietnam_db; then
    echo "🚀 Starting database container..."
    docker-compose -f docker-compose.dev.yml up -d db
    echo "⏳ Waiting 30 seconds for SQL Server to be ready..."
    sleep 30
else
    echo "✅ Database container is already running"
fi

echo ""
echo "🔄 Applying EF Core migrations..."
echo ""

cd backend/src/TravelVietnam.WebApi
dotnet ef database update --project ../TravelVietnam.Infrastructure

cd ../../..

echo ""
echo "=========================================="
echo "✅ Migrations applied successfully!"
echo "=========================================="
echo ""
echo "Database is now up to date with:"
echo "  - Blog columns: Author, BannerUrl, IsFeatured, ReadTime, Summary, Tags, ThumbnailUrl, ViewCount"
echo "  - Destination columns: Slug, Category, BestTimeToVisit, EstimatedBudget, Rating, IsFeatured, RegionId"
echo "  - Cultures table with all features"
echo "  - Fixed cascade delete behavior (NoAction) to prevent SQL Server cycles"
echo ""
echo "Migration applied: 20260528064805_AddDestinationsCultureBlogsFeaturesFixed"
echo ""
