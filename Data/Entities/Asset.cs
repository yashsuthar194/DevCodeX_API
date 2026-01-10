using DevCodeX_API.Data.Shared;

namespace DevCodeX_API.Data.Entities
{
    public class Asset : BaseField
    {
        public Guid ParentId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FileUrl { get; set; }
    }
}
