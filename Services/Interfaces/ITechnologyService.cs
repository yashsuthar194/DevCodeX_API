using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Interfaces
{
    public interface ITechnologyService
    {
        Task<List<Technology>> GetAllAsync();
        Task<PaginatedList<Technology>> GetListAsync(Filter filter);
        Task<Technology?> GetByIdAsync(Guid id);
        Task<Technology> CreateAsync(Technology entity);
        Task<Technology?> UpdateAsync(Guid id, Technology entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IQueryable<Technology>> Where(Expression<Func<Technology, bool>> expression);
    }
}
