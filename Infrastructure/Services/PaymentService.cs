using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            
        }
        public async Task<Basket> SetPaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket == null) return null;

            var shippingPrice = 0m;
            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if(item.Price != productItem.Price){
                    item.Price = productItem.Price;
                }
            }
            var paymentService = new PaymentIntentService();
            PaymentIntent intent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId)){
                var intentCreateOptions = new PaymentIntentCreateOptions{
                    
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new  List<string> {"card"}
                };
                intent = await paymentService.CreateAsync(intentCreateOptions);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var intentUpdateOptions = new PaymentIntentUpdateOptions{
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                };
                await paymentService.UpdateAsync(basket.PaymentIntentId, intentUpdateOptions);

            }
            await _basketRepository.UpdateBasketAsync(basket);            
            return basket;
        }

        public async Task<Order> UpdateOrder(string paymentIntentId, OrderStatus status)
        {
            var spec = new OrderByIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(order == null) return null;
            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return null;
        }
    }
}