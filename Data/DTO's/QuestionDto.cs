using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Enums;

namespace DevCodeX_API.Data.DTO_s
{
    public class QuestionDetailDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid TechnologyId { get; set; }
        public string? TechnologyName { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public Guid? AnswerId { get; set; }
        public string? AnswerContent { get; set; }
    }

    public class QuestionListDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? TechnologyName { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
