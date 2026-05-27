# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ["backend/src/TravelVietnam.Domain/TravelVietnam.Domain.csproj", "TravelVietnam.Domain/"]
COPY ["backend/src/TravelVietnam.Application/TravelVietnam.Application.csproj", "TravelVietnam.Application/"]
COPY ["backend/src/TravelVietnam.Infrastructure/TravelVietnam.Infrastructure.csproj", "TravelVietnam.Infrastructure/"]
COPY ["backend/src/TravelVietnam.WebApi/TravelVietnam.WebApi.csproj", "TravelVietnam.WebApi/"]

RUN dotnet restore "TravelVietnam.WebApi/TravelVietnam.WebApi.csproj"

# Copy source code
COPY backend/src/TravelVietnam.Domain/ TravelVietnam.Domain/
COPY backend/src/TravelVietnam.Application/ TravelVietnam.Application/
COPY backend/src/TravelVietnam.Infrastructure/ TravelVietnam.Infrastructure/
COPY backend/src/TravelVietnam.WebApi/ TravelVietnam.WebApi/

# Build
WORKDIR "/src/TravelVietnam.WebApi"
RUN dotnet build "TravelVietnam.WebApi.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "TravelVietnam.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "TravelVietnam.WebApi.dll"]
