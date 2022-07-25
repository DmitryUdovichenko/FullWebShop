using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}
