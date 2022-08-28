using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Basket> SetPaymentIntent(string basketId);
        Task<Order> UpdateOrder(string paymentIntentId, OrderStatus status);
    }
}