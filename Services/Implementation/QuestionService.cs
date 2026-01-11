using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Filters;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Repositories.Interfaces;
using DevCodeX_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Implementation
{
    public class QuestionService : IQuestionService
    {
        private readonly IBaseRepository<Question> _repository;
        private readonly IAnswerService _answerService;
        private readonly ITechnologyService _technologyService;

        public QuestionService(
            IBaseRepository<Question> repository,
            IAnswerService answerService,
            ITechnologyService technologyService)
        {
            _repository = repository;
            _answerService = answerService;
            _technologyService = technologyService;
        }

        // Get all questions without filtering
        public async Task<List<Question>> GetAllAsync()
        {
            return (await Where(a => !a.IsDeleted)).ToList();
        }

        /// <summary>
        /// Get paginated list of questions with optional filters
        /// </summary>
        public async Task<PaginatedList<QuestionListDto>> GetListAsync(QuestionFilter filter)
        {
            // Build base query with filters
            IQueryable<Question> baseQuery = await Where(q => !q.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.Query))
            {
                baseQuery = baseQuery.Where(q => q.Title.Contains(filter.Query));
            }

            // Apply technology filter
            if (filter.TechnologyId.HasValue)
            {
                baseQuery = baseQuery.Where(q => q.TechnologyId.Equals(filter.TechnologyId.Value));
            }

            // Apply difficulty filter
            if (filter.DifficultyLevel.HasValue)
            {
                baseQuery = baseQuery.Where(q => q.DifficultyLevel.Equals(filter.DifficultyLevel.Value));
            }

            // Get total count before pagination (using EF Core async)
            int totalCount = await baseQuery.CountAsync();

            // Apply pagination and ordering
            IQueryable<Question> pagedQuery = baseQuery
                .OrderByDescending(q => q.CreatedAt)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);

            IQueryable<Technology> technologyServices = await _technologyService.Where(t => !t.IsDeleted);

            // Join with technologies and project to DTO (using EF Core async)
            List<QuestionListDto> items = await (from q in pagedQuery
                 join t in technologyServices on q.TechnologyId equals t.Id
                 select new QuestionListDto
                 {
                     Id = q.Id,
                     Title = q.Title,
                     TechnologyName = t.Name,
                     DifficultyLevel = q.DifficultyLevel,
                     CreatedAt = q.CreatedAt
                 }).ToListAsync();

            return new PaginatedList<QuestionListDto>(items, totalCount, filter.PageIndex, filter.PageSize);
        }

        /// <summary>
        /// Get question by ID with related data
        /// </summary>
        public async Task<QuestionDetailDto?> GetByIdAsync(Guid id)
        {
            Question? question = await _repository.GetByIdAsync(id);

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            Answer? answer = (await _answerService.Where(a => !a.IsDeleted && a.QuestionId.Equals(id)))
                .FirstOrDefault();
            
            Technology? technology = (await _technologyService.Where(t => !t.IsDeleted && t.Id.Equals(question.TechnologyId))).FirstOrDefault();

            return new QuestionDetailDto() 
            { 
                Title = question.Title,
                Description = question.Description,
                TechnologyName = technology?.Name,
                TechnologyId = technology?.Id ?? Guid.Empty,
                AnswerId = answer?.Id,
                AnswerContent = answer?.Content,
                DifficultyLevel = question.DifficultyLevel
            };
        }

        /// <summary>
        /// Create new question
        /// </summary>
        public async Task<Question> CreateAsync(Question entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            var answer = new Answer()
            {
                Id = Guid.Empty,
                Content = string.Empty,
                QuestionId = entity.Id
            };

            await _answerService.CreateAsync(answer);

            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Update existing question
        /// </summary>
        public async Task<Question?> UpdateAsync(Guid id, Question entity)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null || existing.IsDeleted)
            {
                throw new Exception("Question not found");
            }

            entity.Id = id;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CreatedAt = existing.CreatedAt;

            return await _repository.UpdateAsync(id, entity);
        }

        /// <summary>
        /// Soft delete question
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var question = await _repository.GetByIdAsync(id);

            if (question == null || question.IsDeleted)
            {
                return false;
            }

            question.IsDeleted = true;
            question.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(id, question);

            return true;
        }
        
        /// <summary>
        /// Query questions by expression
        /// </summary>
        public async Task<IQueryable<Question>> Where(Expression<Func<Question, bool>> expression)
        {
            return await _repository.Where(expression);
        }
    }
}
