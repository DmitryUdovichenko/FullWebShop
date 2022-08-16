using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redis;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
           return await _redis.KeyDeleteAsync(basketId);
        }

        public async Task<Basket> GetBasketAsync(string basketId)
        {
            var basket = await _redis.HashGetAsync("hashcart", basketId);
            if (String.IsNullOrEmpty(basket))
                return null;

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(basket);
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            if (basket == null)
                throw new ArgumentOutOfRangeException(nameof(basket));

            var serilizedBasket = JsonSerializer.Serialize(basket);

            _redis.HashSet($"hashcart", new HashEntry[]
                {new HashEntry(basket.Id, serilizedBasket)});
            _redis.KeyExpire($"hashcart", TimeSpan.FromDays(30));

            return await GetBasketAsync(basket.Id);
        }
    }
}