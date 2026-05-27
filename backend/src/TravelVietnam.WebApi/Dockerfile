# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore NuGet dependencies
COPY ["src/TravelVietnam.Domain/TravelVietnam.Domain.csproj", "TravelVietnam.Domain/"]
COPY ["src/TravelVietnam.Application/TravelVietnam.Application.csproj", "TravelVietnam.Application/"]
COPY ["src/TravelVietnam.Infrastructure/TravelVietnam.Infrastructure.csproj", "TravelVietnam.Infrastructure/"]
COPY ["src/TravelVietnam.WebApi/TravelVietnam.WebApi.csproj", "TravelVietnam.WebApi/"]

# Restore dependencies
RUN dotnet restore "TravelVietnam.WebApi/TravelVietnam.WebApi.csproj"

# Copy the entire source code
COPY src/ .

# Build and publish the application in Release mode
WORKDIR "/src/TravelVietnam.WebApi"
RUN dotnet publish "TravelVietnam.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .

# Define execution entry point
ENTRYPOINT ["dotnet", "TravelVietnam.WebApi.dll"]
