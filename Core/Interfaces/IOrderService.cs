using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userEmail, int deliveryMethod, string basketId, Address shippingAdress);
        Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userEmail);
        Task<Order> GetOrderByIdAsync(int id, string userEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}