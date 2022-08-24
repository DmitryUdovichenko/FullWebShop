using AutoMapper;
using Core.Entities.OrderAggregate;
using Shop.API.Dtos;

namespace Shop.API.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public IConfiguration _config { get; }
        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
            
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.OrderedItem.ImageUrl))
            {
                return _config["ApiUrl"] + source.OrderedItem.ImageUrl;
            }
            return null;
        }
    }
}