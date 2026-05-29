#!/bin/bash

# Travel Vietnam - Quick Start Script
# This script helps you quickly start the development environment

set -e

echo "=========================================="
echo "Travel Vietnam - Quick Start"
echo "=========================================="
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker Desktop first."
    exit 1
fi

echo "✅ Docker is running"
echo ""

# Function to show menu
show_menu() {
    echo "Choose an option:"
    echo "1) Start Backend Only (SQL Server + Redis + API)"
    echo "2) Start Full Stack (Backend + Frontend in Docker)"
    echo "3) Stop All Services"
    echo "4) View Logs"
    echo "5) Reset Database (WARNING: Deletes all data)"
    echo "6) Exit"
    echo ""
    read -p "Enter your choice [1-6]: " choice
}

# Function to start backend only
start_backend() {
    echo ""
    echo "🚀 Starting Backend Services..."
    echo "   - SQL Server 2022"
    echo "   - Redis 7.2"
    echo "   - .NET 8 Web API"
    echo ""

    docker-compose -f docker-compose.dev.yml up -d

    echo ""
    echo "✅ Backend services started!"
    echo ""
    echo "📍 Services:"
    echo "   - API: http://localhost:5000"
    echo "   - Swagger: http://localhost:5000/swagger"
    echo "   - SQL Server: localhost:1433 (sa/TravelVietNamPass@123)"
    echo "   - Redis: localhost:6379"
    echo ""
    echo "💡 Next steps:"
    echo "   1. Wait 30 seconds for database migrations"
    echo "   2. cd frontend && npm install && npm start"
    echo "   3. Open http://localhost:4200"
    echo ""
}

# Function to start full stack
start_fullstack() {
    echo ""
    echo "🚀 Starting Full Stack..."
    echo "   - SQL Server 2022"
    echo "   - Redis 7.2"
    echo "   - .NET 8 Web API"
    echo "   - Angular 17 SSR"
    echo ""

    docker-compose up --build -d

    echo ""
    echo "✅ All services started!"
    echo ""
    echo "📍 Services:"
    echo "   - Frontend: http://localhost:4200"
    echo "   - API: http://localhost:5000"
    echo "   - Swagger: http://localhost:5000/swagger"
    echo ""
    echo "💡 Wait 1-2 minutes for all services to be ready"
    echo ""
}

# Function to stop services
stop_services() {
    echo ""
    echo "🛑 Stopping all services..."

    docker-compose down
    docker-compose -f docker-compose.dev.yml down

    echo ""
    echo "✅ All services stopped!"
    echo ""
}

# Function to view logs
view_logs() {
    echo ""
    echo "📋 Choose service to view logs:"
    echo "1) API"
    echo "2) Frontend"
    echo "3) Database"
    echo "4) Redis"
    echo "5) All services"
    echo ""
    read -p "Enter your choice [1-5]: " log_choice

    case $log_choice in
        1)
            docker-compose logs -f api
            ;;
        2)
            docker-compose logs -f web
            ;;
        3)
            docker-compose logs -f db
            ;;
        4)
            docker-compose logs -f redis
            ;;
        5)
            docker-compose logs -f
            ;;
        *)
            echo "Invalid choice"
            ;;
    esac
}

# Function to reset database
reset_database() {
    echo ""
    echo "⚠️  WARNING: This will delete all database data!"
    read -p "Are you sure? (yes/no): " confirm

    if [ "$confirm" = "yes" ]; then
        echo ""
        echo "🗑️  Resetting database..."

        docker-compose down -v
        docker-compose -f docker-compose.dev.yml down -v

        echo ""
        echo "✅ Database reset complete!"
        echo "💡 Start services again to recreate the database"
        echo ""
    else
        echo "Cancelled."
    fi
}

# Main loop
while true; do
    show_menu

    case $choice in
        1)
            start_backend
            ;;
        2)
            start_fullstack
            ;;
        3)
            stop_services
            ;;
        4)
            view_logs
            ;;
        5)
            reset_database
            ;;
        6)
            echo "Goodbye!"
            exit 0
            ;;
        *)
            echo "Invalid choice. Please try again."
            ;;
    esac

    echo ""
    read -p "Press Enter to continue..."
    clear
done
