using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Shared;
using System.Linq.Expressions;

namespace DevCodeX_API.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<List<Answer>> GetAllAsync();
        Task<PaginatedList<Answer>> GetListAsync(Filter filter);
        Task<AnswerDetailDto?> GetByIdAsync(Guid id);
        Task<Answer> CreateAsync(Answer entity);
        Task<Answer?> UpdateAsync(Guid id, Answer entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IQueryable<Answer>> Where(Expression<Func<Answer, bool>> expression);
    }
}
