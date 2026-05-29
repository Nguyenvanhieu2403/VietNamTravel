# Travel Vietnam - Fullstack Integration Guide

## Architecture Overview

**Frontend:** Angular 17 SSR + NgModule + GSAP animations  
**Backend:** .NET 8 Web API + Clean Architecture + MediatR CQRS  
**Database:** SQL Server 2022  
**Cache:** Redis 7.2  
**Authentication:** JWT Bearer tokens

---

## Local Development Setup

### Prerequisites

- Docker Desktop installed and running
- .NET 8 SDK (for local backend development)
- Node.js 18+ and npm (for local frontend development)
- Git

### Option 1: Run Backend + Database Only (Recommended for Development)

This allows you to develop the frontend with hot-reload while backend runs in Docker.

#### Step 1: Start Backend Services

```bash
# Start SQL Server, Redis, and Backend API
docker-compose -f docker-compose.dev.yml up -d

# Check services are running
docker-compose -f docker-compose.dev.yml ps

# View backend logs
docker-compose -f docker-compose.dev.yml logs -f api
```

**Backend API will be available at:** `http://localhost:5000`  
**Swagger UI:** `http://localhost:5000/swagger`

#### Step 2: Run Frontend Locally

```bash
cd frontend

# Install dependencies (first time only)
npm install

# Start Angular dev server with SSR
npm start

# Or for SSR build and serve
npm run build
npm run serve:ssr:frontend
```

**Frontend will be available at:** `http://localhost:4200`

---

### Option 2: Run Full Stack with Docker

```bash
# Build and start all services
docker-compose up --build

# Or run in detached mode
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

**Services:**
- Frontend: `http://localhost:4200`
- Backend API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- SQL Server: `localhost:1433`
- Redis: `localhost:6379`

---

## Database Setup

### Automatic Migration

The backend automatically applies EF Core migrations on startup. Check logs:

```bash
docker-compose logs api | grep "migration"
```

### Manual Migration (if needed)

```bash
cd backend/src/TravelVietnam.WebApi

# Add new migration
dotnet ef migrations add MigrationName --project ../TravelVietnam.Infrastructure

# Apply migrations
dotnet ef database update --project ../TravelVietnam.Infrastructure
```

### Seed Sample Data

Connect to SQL Server and run the seed script:

```bash
# Using SQL Server Management Studio (SSMS)
Server: localhost,1433
Login: sa
Password: TravelVietNamPass@123
Database: TravelVietnamDb

# Or using sqlcmd
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -d TravelVietnamDb -i /path/to/seed.sql
```

---

## API Endpoints

### Authentication
- `POST /api/v1/auth/register` - Register new user
- `POST /api/v1/auth/login` - Login and get JWT token
- `POST /api/v1/auth/refresh-token` - Refresh access token

### Regions
- `GET /api/v1/regions` - Get all regions
- `GET /api/v1/regions/{slug}` - Get region by slug

### Provinces
- `GET /api/v1/provinces` - Get provinces (paginated)
- `GET /api/v1/provinces/{slug}` - Get province details by slug

### Destinations
- `GET /api/v1/destinations` - Get destinations (paginated)
- `GET /api/v1/destinations/{id}` - Get destination by ID

### Blogs
- `GET /api/v1/blogs` - Get blogs (paginated)
- `GET /api/v1/blogs/{slug}` - Get blog by slug

### AI Recommendations
- `GET /api/v1/airecommendations` - Get AI travel recommendations

---

## Environment Configuration

### Frontend Environment Files

**Development:** `frontend/src/environments/environment.ts`
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api/v1',
  apiTimeout: 30000,
  enableDebugLogs: true
};
```

**Production:** `frontend/src/environments/environment.production.ts`
```typescript
export const environment = {
  production: true,
  apiUrl: 'http://api:80/api/v1',
  apiTimeout: 30000,
  enableDebugLogs: false
};
```

### Backend Configuration

**appsettings.json** (local development)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TravelVietnamDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "SecretKey": "TravelVietnam_SuperSecureKey_For_JwtBearer_SHA256_Authentication_2026",
    "Issuer": "TravelCoreAPI",
    "Audience": "TravelCoreClient",
    "AccessTokenExpirationMinutes": 15
  }
}
```

---

## Testing the Integration

### 1. Test Backend API

```bash
# Health check
curl http://localhost:5000/api/v1/regions

# Register a user
curl -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test@123",
    "fullName": "Test User"
  }'

# Login
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test@123"
  }'
```

### 2. Test Frontend

1. Open `http://localhost:4200`
2. Navigate to Regions page
3. Navigate to Provinces page
4. Click on a province to view details
5. Check browser console for any errors
6. Verify API calls in Network tab

---

## Troubleshooting

### Backend won't start
```bash
# Check if SQL Server is ready
docker-compose logs db

# Check backend logs
docker-compose logs api

# Restart services
docker-compose restart api
```

### Frontend can't connect to backend
- Verify backend is running: `curl http://localhost:5000/api/v1/regions`
- Check CORS configuration in `Program.cs`
- Check environment.ts has correct API URL
- Clear browser cache and restart Angular dev server

### Database connection issues
```bash
# Test SQL Server connection
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -Q "SELECT @@VERSION"

# Check if database exists
docker exec -it travel_vietnam_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TravelVietNamPass@123 -Q "SELECT name FROM sys.databases"
```

### Redis connection issues
```bash
# Test Redis connection
docker exec -it travel_vietnam_redis redis-cli ping

# Should return: PONG
```

---

## Development Workflow

### Making Backend Changes

1. Edit code in `backend/src/`
2. Rebuild API container: `docker-compose up --build api`
3. Or run locally: `cd backend/src/TravelVietnam.WebApi && dotnet run`

### Making Frontend Changes

1. Edit code in `frontend/src/`
2. Changes auto-reload if using `npm start`
3. For SSR testing: `npm run build && npm run serve:ssr:frontend`

### Adding New Features

1. **Backend:** Create Command/Query in Application layer
2. **Backend:** Add Controller endpoint in WebApi layer
3. **Frontend:** Create service method in `core/services/`
4. **Frontend:** Update component to call service
5. **Frontend:** Update models in `core/models/`

---

## Production Deployment

### Build Production Images

```bash
# Build all services
docker-compose build

# Tag images
docker tag travel_vietnam_api:latest your-registry/travel-vietnam-api:v1.0
docker tag travel_vietnam_web:latest your-registry/travel-vietnam-web:v1.0

# Push to registry
docker push your-registry/travel-vietnam-api:v1.0
docker push your-registry/travel-vietnam-web:v1.0
```

### Environment Variables for Production

Update `docker-compose.yml` or use `.env` file:

```env
DB_PASSWORD=<strong-password>
JWT_SECRET=<strong-secret-key>
REDIS_PASSWORD=<redis-password>
API_URL=https://api.yourdomain.com/api/v1
```

---

## Monitoring

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f web

# Last 100 lines
docker-compose logs --tail=100 api
```

### Check Service Health

```bash
# Container status
docker-compose ps

# Resource usage
docker stats
```

---

## Cleanup

```bash
# Stop all services
docker-compose down

# Remove volumes (WARNING: deletes database data)
docker-compose down -v

# Remove images
docker-compose down --rmi all
```

---

## Support

For issues or questions:
1. Check logs: `docker-compose logs -f`
2. Verify all services are running: `docker-compose ps`
3. Check network connectivity between containers
4. Review CORS settings if frontend can't reach backend
