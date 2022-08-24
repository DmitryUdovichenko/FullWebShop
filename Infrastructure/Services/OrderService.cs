using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }
        public async Task<Order> CreateOrderAsync(string userEmail, int deliveryMethod, string basketId, Address shippingAdress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            foreach(var item in basket.Items){
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var orderedItem = new OrderedProductItem(productItem.Id,productItem.Name,productItem.ImageUrl);
                var orderItem = new OrderItem(orderedItem, productItem.Price, item.Quantity);
                orderItems.Add(orderItem);
            }
            var deliveryObject = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);
            var subtotal = orderItems.Sum(i => i.Price * i.Quantity);
            var order = new Order(userEmail, shippingAdress, deliveryObject, orderItems, subtotal);
            _unitOfWork.Repository<Order>().Add(order);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            await _basketRepository.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string userEmail)
        {
            var spec = new OrdersWithItensSpecification(id,userEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userEmail)
        {
            var spec = new OrdersWithItensSpecification(userEmail);
            return await _unitOfWork.Repository<Order>().GetListWithSpecAsync(spec);
        }
    }
}