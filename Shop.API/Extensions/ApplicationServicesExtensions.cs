using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Errors;

namespace Shop.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAplictionServices(this IServiceCollection services)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));  
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Configure<ApiBehaviorOptions>(o => 
            {
                o.InvalidModelStateResponseFactory = actionContext => 
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            }); 
            return services; 
        }
    }
}