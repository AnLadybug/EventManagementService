namespace Ru.AnLadybug.EventManagement.Models.Search;

public class EventSearchParams
{
    public string? Title { get; }
    public DateTime? From { get; }
    public DateTime? To { get; }
    public int Page { get; }
    public int PageSize { get; }

    private EventSearchParams(
        string? title,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize)
    {
        Title = title;
        From = from; 
        To = to; 
        Page = page; 
        PageSize = pageSize;
    }

    public static EventSearchParams CreatePage(
        string? title,
        DateTime? from,
        DateTime? to,
        int? page,
        int? pageSize)
    {
        var pageFinal = page ?? 1;
        var pageSizeFinal = pageSize ?? 10;
        var result = new EventSearchParams(title, from, to, pageFinal, pageSizeFinal);
        return result;
    }
}
