@echo off
REM Travel Vietnam - Quick Start Script for Windows
REM This script helps you quickly start the development environment

setlocal enabledelayedexpansion

echo ==========================================
echo Travel Vietnam - Quick Start
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

:menu
echo Choose an option:
echo 1) Start Backend Only (SQL Server + Redis + API)
echo 2) Start Full Stack (Backend + Frontend in Docker)
echo 3) Stop All Services
echo 4) View Logs
echo 5) Reset Database (WARNING: Deletes all data)
echo 6) Exit
echo.
set /p choice="Enter your choice [1-6]: "

if "%choice%"=="1" goto start_backend
if "%choice%"=="2" goto start_fullstack
if "%choice%"=="3" goto stop_services
if "%choice%"=="4" goto view_logs
if "%choice%"=="5" goto reset_database
if "%choice%"=="6" goto exit
echo Invalid choice. Please try again.
goto menu

:start_backend
echo.
echo Starting Backend Services...
echo    - SQL Server 2022
echo    - Redis 7.2
echo    - .NET 8 Web API
echo.

docker-compose -f docker-compose.dev.yml up -d

echo.
echo Backend services started!
echo.
echo Services:
echo    - API: http://localhost:5000
echo    - Swagger: http://localhost:5000/swagger
echo    - SQL Server: localhost:1433 (sa/TravelVietNamPass@123)
echo    - Redis: localhost:6379
echo.
echo Next steps:
echo    1. Wait 30 seconds for database migrations
echo    2. cd frontend ^&^& npm install ^&^& npm start
echo    3. Open http://localhost:4200
echo.
pause
goto menu

:start_fullstack
echo.
echo Starting Full Stack...
echo    - SQL Server 2022
echo    - Redis 7.2
echo    - .NET 8 Web API
echo    - Angular 17 SSR
echo.

docker-compose up --build -d

echo.
echo All services started!
echo.
echo Services:
echo    - Frontend: http://localhost:4200
echo    - API: http://localhost:5000
echo    - Swagger: http://localhost:5000/swagger
echo.
echo Wait 1-2 minutes for all services to be ready
echo.
pause
goto menu

:stop_services
echo.
echo Stopping all services...

docker-compose down
docker-compose -f docker-compose.dev.yml down

echo.
echo All services stopped!
echo.
pause
goto menu

:view_logs
echo.
echo Choose service to view logs:
echo 1) API
echo 2) Frontend
echo 3) Database
echo 4) Redis
echo 5) All services
echo.
set /p log_choice="Enter your choice [1-5]: "

if "%log_choice%"=="1" docker-compose logs -f api
if "%log_choice%"=="2" docker-compose logs -f web
if "%log_choice%"=="3" docker-compose logs -f db
if "%log_choice%"=="4" docker-compose logs -f redis
if "%log_choice%"=="5" docker-compose logs -f

goto menu

:reset_database
echo.
echo WARNING: This will delete all database data!
set /p confirm="Are you sure? (yes/no): "

if /i "%confirm%"=="yes" (
    echo.
    echo Resetting database...

    docker-compose down -v
    docker-compose -f docker-compose.dev.yml down -v

    echo.
    echo Database reset complete!
    echo Start services again to recreate the database
    echo.
) else (
    echo Cancelled.
)
pause
goto menu

:exit
echo Goodbye!
exit /b 0
