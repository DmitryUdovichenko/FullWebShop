using Core.Entities.OrderAggregate;

namespace Shop.API.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        
    }
}