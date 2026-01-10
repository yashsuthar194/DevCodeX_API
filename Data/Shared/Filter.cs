namespace DevCodeX_API.Data.Shared
{
    public class Filter
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Query { get; set; }
        public DateTime Date { get; set; }
    }
}
