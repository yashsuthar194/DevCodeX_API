using System.ComponentModel.DataAnnotations;

namespace DevCodeX_API.Data.Shared
{
    public class BaseField
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
