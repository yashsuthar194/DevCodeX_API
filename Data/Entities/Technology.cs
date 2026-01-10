using DevCodeX_API.Data.Enums;
using DevCodeX_API.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace DevCodeX_API.Data.Entities
{
    public class Technology : BaseField
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public TechnologyType TechnologyType { get; set; }
    }
}
