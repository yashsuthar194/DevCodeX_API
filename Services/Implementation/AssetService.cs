using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Services.Interfaces;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Implementation
{
    public class AssetService : IAssetService
    {
        private readonly IBaseRepository<Asset> _repository;

        public AssetService(IBaseRepository<Asset> repository)
        {
            _repository = repository;
        }

        public async Task<List<Asset>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PaginatedList<Asset>> GetListAsync(Filter filter)
        {
            // Build base query
            IQueryable<Asset> baseQuery = await _repository.Where(a => !a.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.Query))
            {
                baseQuery = baseQuery.Where(a => a.FileName != null && a.FileName.Contains(filter.Query));
            }

            // Get total count before pagination
            int totalCount = baseQuery.Count();

            // Apply pagination and ordering
            List<Asset> items = baseQuery
                .OrderByDescending(a => a.CreatedAt)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PaginatedList<Asset>(items, totalCount, filter.PageIndex, filter.PageSize);
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            Asset? asset = await _repository.GetByIdAsync(id);

            if (asset == null || asset.IsDeleted)
            {
                throw new Exception("Asset not found");
            }

            return asset;
        }

        public async Task<Asset> CreateAsync(Asset entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            return await _repository.CreateAsync(entity);
        }

        public async Task<Asset?> UpdateAsync(Guid id, Asset entity)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null || existing.IsDeleted)
            {
                throw new Exception("Asset not found");
            }

            entity.Id = id;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CreatedAt = existing.CreatedAt;

            return await _repository.UpdateAsync(id, entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var asset = await _repository.GetByIdAsync(id);

            if (asset == null || asset.IsDeleted)
            {
                return false;
            }

            asset.IsDeleted = true;
            asset.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(id, asset);

            return true;
        }

        public async Task<IQueryable<Asset>> Where(Expression<Func<Asset, bool>> expression)
        {
            return await _repository.Where(expression);
        }
    }
}
