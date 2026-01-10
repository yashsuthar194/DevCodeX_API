using DevCodeX_API.Context;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevCodeX_API.Repositories.Implementation
{
    public class BaseReposioty<T> : IBaseRepository<T> where T : class
    {
        public CodeXContext DbContext { get; }
        private readonly DbSet<T> _dbSet;
        public BaseReposioty(CodeXContext context)
        {
            DbContext = context;
            _dbSet = DbContext.Set<T>();
        }

        public async Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(_dbSet.Where(expression));
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> UpdateAsync(Guid id, T entity)
        {
            _dbSet.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);
            if (entity == null)
                return false;
            _dbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}
