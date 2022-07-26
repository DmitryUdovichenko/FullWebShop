using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithSpecification : BaseSpecification<Product>
    {
        public ProductWithSpecification()
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
        }

        public ProductWithSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
        }
    }
}