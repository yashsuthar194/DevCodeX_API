using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Services.Interfaces;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Implementation
{
    public class AnswerService : IAnswerService
    {
        private readonly IBaseRepository<Answer> _repository;

        public AnswerService(IBaseRepository<Answer> repository)
        {
            _repository = repository;
        }

        public async Task<List<Answer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PaginatedList<Answer>> GetListAsync(Filter filter)
        {
            // Build base query
            IQueryable<Answer> baseQuery = await _repository.Where(a => !a.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.Query))
            {
                baseQuery = baseQuery.Where(a => a.Content != null && a.Content.Contains(filter.Query));
            }

            // Get total count before pagination
            int totalCount = baseQuery.Count();

            // Apply pagination and ordering
            List<Answer> items = baseQuery
                .OrderByDescending(a => a.CreatedAt)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PaginatedList<Answer>(items, totalCount, filter.PageIndex, filter.PageSize);
        }

        public async Task<AnswerDetailDto?> GetByIdAsync(Guid id)
        {
            Answer? answer = await _repository.GetByIdAsync(id);

            if (answer == null || answer.IsDeleted)
            {
                throw new Exception("Answer not found");
            }

            return new AnswerDetailDto() 
            { 
                Content = answer.Content
            };
        }

        public async Task<Answer> CreateAsync(Answer entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            return await _repository.CreateAsync(entity);
        }

        public async Task<Answer?> UpdateAsync(Guid id, Answer entity)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null || existing.IsDeleted)
            {
                throw new Exception("Answer not found");
            }

            // Update properties on the tracked entity instead of attaching a new one
            existing.Content = entity.Content;
            existing.QuestionId = entity.QuestionId;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(id, existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var answer = await _repository.GetByIdAsync(id);

            if (answer == null || answer.IsDeleted)
            {
                return false;
            }

            answer.IsDeleted = true;
            answer.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(id, answer);

            return true;
        }

        public async Task<IQueryable<Answer>> Where(Expression<Func<Answer, bool>> expression)
        {
            return await _repository.Where(expression);
        }
    }
}
