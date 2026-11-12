namespace Ru.AnLadybug.EventManagement.Models;

public record PaginatedResult<T>
{
    public required int TotalCount { get; init; }
    public IReadOnlyCollection<T> Items { get; init; } = [];
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}
