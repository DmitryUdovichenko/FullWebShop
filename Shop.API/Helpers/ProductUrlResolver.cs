using AutoMapper;
using Core.Entities;
using Shop.API.Dtos;

namespace Shop.API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductReturnDto, string>
    {
        public IConfiguration _config { get; }
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
            
        }

        public string Resolve(Product source, ProductReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ImageUrl))
            {
                return _config["ApiUrl"] + source.ImageUrl;
            }
            return null;
        }
    }
}