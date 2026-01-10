
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Interfaces
{
    public interface IBaseService<T, Tfilter> where T : class where Tfilter : class
    {
        Task<List<T>> GetAllAsync();
        Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression);
        Task<List<T>> GetListAsync(Tfilter filter);
        Task<T?> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(Guid id, T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
