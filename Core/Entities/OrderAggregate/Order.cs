namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(string userEmail, Address shippingAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems, decimal subtotal, string paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; }
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }

        public decimal GetTotal(){
            if(DeliveryMethod != null){
                return Subtotal + DeliveryMethod.Price;
            }
            return Subtotal;
        }
    }
}