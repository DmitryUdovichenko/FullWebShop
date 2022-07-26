using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFilters : BaseSpecification<Product>
    {
        public ProductWithFilters(ProductParams productParams) 
               : base(x =>
               (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
               (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
               (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
           {
        }

        public ProductWithFilters(string userId) : base(x => x.CreatedBy == userId)
        {
        }

    }
}