using DevCodeX_API.Data.Enums;
using DevCodeX_API.Data.Shared;

namespace DevCodeX_API.Data.Filters
{
    public class QuestionFilter : Filter
    {
        public Guid? TechnologyId { get; set; }
        public DifficultyLevel? DifficultyLevel { get; set; }
    }
}
