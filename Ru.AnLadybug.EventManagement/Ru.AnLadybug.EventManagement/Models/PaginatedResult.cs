namespace Ru.AnLadybug.EventManagement.Models;

public class PaginatedResult<T>
{
    public int TotalCount { get; set; }
    public IReadOnlyCollection<T> Items { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
