namespace Ru.AnLadybug.EventManagement.ModeslDto;

/// <summary>
/// Постраничный результат выборки данных.
/// </summary>
/// <typeparam name="TDto">Тип модели данных (DTO) элементов в коллекции.</typeparam>
public class PaginatedResultDto<TDto>
{
    /// <summary>
    /// Общее количество элементов, удовлетворяющих условиям запроса (без учета пагинации).
    /// </summary>
    /// <example>42</example>
    public int TotalCount { get; set; }

    /// <summary>
    /// Коллекция элементов на текущей странице.
    /// </summary>
    public IReadOnlyCollection<TDto> Items { get; set; } = [];

    /// <summary>
    /// Номер текущей страницы (начиная с 1).
    /// </summary>
    /// <example>1</example>
    public int PageNumber { get; set; }

    /// <summary>
    /// Максимальное количество элементов на одной странице (размер страницы).
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; set; }
}
