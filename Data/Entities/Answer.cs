using DevCodeX_API.Data.Shared;

namespace DevCodeX_API.Data.Entities
{
    public class Answer : BaseField
    {
        public Guid QuestionId { get; set; }
        public string? Content { get; set; }
    }
}
