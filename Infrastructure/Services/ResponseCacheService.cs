using System.Text.Json;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _redisDb;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        public async Task CaheResponseAsync(string cacheKey, object response, TimeSpan expTime)
        {
            if(response == null){
                return;
            }
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response, serializeOptions);
            await _redisDb.StringSetAsync(cacheKey, serializedResponse,expTime);

        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _redisDb.StringGetAsync(cacheKey);

            if(cachedResponse.IsNullOrEmpty) return null;

            return cachedResponse;
        }
    }
}