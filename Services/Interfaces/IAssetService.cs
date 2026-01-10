using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Interfaces
{
    public interface IAssetService
    {
        Task<List<Asset>> GetAllAsync();
        Task<PaginatedList<Asset>> GetListAsync(Filter filter);
        Task<Asset?> GetByIdAsync(Guid id);
        Task<Asset> CreateAsync(Asset entity);
        Task<Asset?> UpdateAsync(Guid id, Asset entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IQueryable<Asset>> Where(Expression<Func<Asset, bool>> expression);
    }
}
