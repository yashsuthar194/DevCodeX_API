using DevCodeX_API.Data.Enums;

namespace DevCodeX_API.Data.DTO_s
{
    public class TechnologyListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public TechnologyType TechnologyType { get; set; }
        public int QuestionCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }
}
