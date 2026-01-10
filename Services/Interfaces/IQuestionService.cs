using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Filters;
using DevCodeX_API.Data.Shared;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllAsync();
        Task<PaginatedList<QuestionListDto>> GetListAsync(QuestionFilter filter);
        Task<QuestionDetailDto?> GetByIdAsync(Guid id);
        Task<Question> CreateAsync(Question entity);
        Task<Question?> UpdateAsync(Guid id, Question entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IQueryable<Question>> Where(Expression<Func<Question, bool>> expression);
    }
}
