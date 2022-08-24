namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(OrderedProductItem orderedItem, decimal price, int quantity)
        {
            OrderedItem = orderedItem;
            Price = price;
            Quantity = quantity;
        }

        public OrderedProductItem OrderedItem { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}