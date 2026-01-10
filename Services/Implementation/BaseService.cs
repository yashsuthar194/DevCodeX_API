using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Services.Interfaces;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Implementation
{
    public abstract class BaseService<T, Tfilter> : IBaseService<T, Tfilter> where T : class where Tfilter : class
    {
        private readonly IBaseRepository<T> _baseRepository;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public abstract Task<List<T>> GetListAsync(Tfilter filter);

        public virtual async Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression)
        {
            return await _baseRepository.Where(expression);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            return await _baseRepository.CreateAsync(entity);
        }

        public virtual async Task<T?> UpdateAsync(Guid id, T entity)
        {
            return await _baseRepository.UpdateAsync(id, entity);
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            return await _baseRepository.DeleteAsync(id);
        }
    }
}
