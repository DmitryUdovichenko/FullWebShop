using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetListAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetListWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
