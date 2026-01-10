using DevCodeX_API.Data.Enums;
using DevCodeX_API.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace DevCodeX_API.Data.Entities
{
    public class Question : BaseField
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid TechnologyId { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; } 
    }
}
