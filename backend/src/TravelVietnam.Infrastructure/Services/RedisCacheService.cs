using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using TravelVietnam.Application.Interfaces;

namespace TravelVietnam.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedData = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30); // Default 30 min caching
            }

            var serializedData = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, serializedData, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{prefix}*");
                foreach (var key in keys)
                {
                    await _distributedCache.RemoveAsync(key.ToString());
                }
            }
        }
    }
}
