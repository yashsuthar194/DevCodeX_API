using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Services.Interfaces;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Implementation
{
    public class TechnologyService : ITechnologyService
    {
        private readonly IBaseRepository<Technology> _repository;

        public TechnologyService(IBaseRepository<Technology> repository)
        {
            _repository = repository;
        }

        public async Task<List<Technology>> GetAllAsync()
        {
            return (await Where(a => !a.IsDeleted)).ToList();
        }

        public async Task<PaginatedList<Technology>> GetListAsync(Filter filter)
        {
            // Build base query with filters
            IQueryable<Technology> baseQuery = await Where(t => !t.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.Query))
            {
                baseQuery = baseQuery.Where(t => t.Name.Contains(filter.Query) || 
                    (t.Description != null && t.Description.Contains(filter.Query)));
            }

            // Get total count before pagination
            int totalCount = baseQuery.Count();

            // Apply pagination and ordering
            List<Technology> items = baseQuery
                .OrderByDescending(t => t.CreatedAt)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PaginatedList<Technology>(items, totalCount, filter.PageIndex, filter.PageSize);
        }

        public async Task<Technology?> GetByIdAsync(Guid id)
        {
            Technology? technology = await _repository.GetByIdAsync(id);

            if (technology == null || technology.IsDeleted)
            {
                throw new Exception("Technology not found");
            }

            return technology;
        }

        public async Task<Technology> CreateAsync(Technology entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            return await _repository.CreateAsync(entity);
        }

        public async Task<Technology?> UpdateAsync(Guid id, Technology entity)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null || existing.IsDeleted)
            {
                throw new Exception("Technology not found");
            }

            // Update the existing tracked entity's properties instead of attaching a new entity
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.TechnologyType = entity.TechnologyType;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(id, existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var technology = await _repository.GetByIdAsync(id);

            if (technology == null || technology.IsDeleted)
            {
                return false;
            }

            technology.IsDeleted = true;
            technology.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(id, technology);

            return true;
        }

        public async Task<IQueryable<Technology>> Where(Expression<Func<Technology, bool>> expression)
        {
            return await _repository.Where(expression);
        }
    }
}
