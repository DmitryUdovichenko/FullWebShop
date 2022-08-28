using System.Text;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shop.API.Helpers
{
    public class CachingAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expTimeInSeconds;

        public CachingAttribute(int expTimeInSeconds)
        {
            _expTimeInSeconds = expTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CaheResponseAsync(cacheKey,okObjectResult.Value, TimeSpan.FromSeconds(_expTimeInSeconds));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}