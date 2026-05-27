using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Infrastructure.Persistence;
using TravelVietnam.Infrastructure.Services;

namespace TravelVietnam.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Application DbContext with SQL Server
            var dbConnectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Server=localhost;Database=TravelVietnamDb;Trusted_Connection=True;TrustServerCertificate=True;";
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Register repositories and UOW
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register context accessors
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Register Core Services
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddScoped<IJwtService, JwtService>();

            // Register Redis Multiplexer and Cache Service
            var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            
            // Connect StackExchange Redis
            try
            {
                var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                });
                services.AddScoped<ICacheService, RedisCacheService>();
            }
            catch
            {
                // Fallback cache registry in case local Redis is not running in design time
                services.AddDistributedMemoryCache();
                // Register a mock multiplexer or fake connection, but let's assume Redis is required or fail gracefully
                var mockMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { "localhost:6379" }, AbortOnConnectFail = false });
                services.AddSingleton<IConnectionMultiplexer>(mockMultiplexer);
                services.AddScoped<ICacheService, RedisCacheService>();
            }

            return services;
        }
    }
}
