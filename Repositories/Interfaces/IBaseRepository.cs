using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevCodeX_API.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(Guid id, T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
