using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItensSpecification : BaseSpecification<Order>
    {
        public OrdersWithItensSpecification(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.CreatedDate);
        }

        public OrdersWithItensSpecification(int id, string email) 
            : base(o => o.Id == id && o.UserEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}