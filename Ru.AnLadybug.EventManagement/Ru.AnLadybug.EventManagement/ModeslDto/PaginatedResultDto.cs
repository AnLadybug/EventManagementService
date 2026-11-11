namespace Ru.AnLadybug.EventManagement.ModeslDto;

public class PaginatedResultDto<TDto>
{
    /// <summary>
    /// Общее количество событий
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Перечисление событий
    /// </summary>
    public IReadOnlyCollection<TDto> Items { get; set; } = [];

    /// <summary>
    /// Номер текущей страницы
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Количество элементов на текущей странице
    /// </summary>
    public int PageSize { get; set; }
}
