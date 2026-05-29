@echo off
REM Apply pending EF Core migrations to the database

echo ==========================================
echo Travel Vietnam - Apply Database Migrations
echo ==========================================
echo.

REM Check if Docker is running
docker info >nul 2>&1
if errorlevel 1 (
    echo Error: Docker is not running. Please start Docker Desktop first.
    pause
    exit /b 1
)

echo Docker is running
echo.

REM Check if database container is running
docker ps | findstr travel_vietnam_db >nul 2>&1
if errorlevel 1 (
    echo Starting database container...
    docker-compose -f docker-compose.dev.yml up -d db
    echo Waiting 30 seconds for SQL Server to be ready...
    timeout /t 30 /nobreak
) else (
    echo Database container is already running
)

echo.
echo Applying EF Core migrations...
echo.

cd backend\src\TravelVietnam.WebApi
dotnet ef database update --project ..\TravelVietnam.Infrastructure

if errorlevel 1 (
    echo.
    echo Migration failed! Check the error above.
    cd ..\..\..
    pause
    exit /b 1
)

cd ..\..\..

echo.
echo ==========================================
echo Migrations applied successfully!
echo ==========================================
echo.
echo Database is now up to date with:
echo   - Blog columns: Author, BannerUrl, IsFeatured, ReadTime, Summary, Tags, ThumbnailUrl, ViewCount
echo   - Destination columns: Slug, Category, BestTimeToVisit, EstimatedBudget, Rating, IsFeatured, RegionId
echo   - Cultures table with all features
echo   - Fixed cascade delete behavior (NoAction) to prevent SQL Server cycles
echo.
echo Migration applied: 20260528064805_AddDestinationsCultureBlogsFeaturesFixed
echo.
pause
