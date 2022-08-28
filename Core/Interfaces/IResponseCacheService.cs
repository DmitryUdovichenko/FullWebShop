namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
        Task CaheResponseAsync(string cacheKey, object response, TimeSpan expTime);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}