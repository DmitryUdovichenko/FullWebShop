using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByIntentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}