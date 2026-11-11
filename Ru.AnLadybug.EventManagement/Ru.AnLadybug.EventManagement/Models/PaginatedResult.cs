namespace Ru.AnLadybug.EventManagement.Models;

public class PaginatedResult<T>
{
    //[Required(ErrorMessage = "Общее количество событий обязательно для заполнения")]

    /// <summary>
    /// Общее количество событий
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Перечисление событий
    /// </summary>
    public IReadOnlyCollection<T> Items { get; set; } = [];

    /// <summary>
    /// Номер текущей страницы
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Количество элементов на текущей странице
    /// </summary>
    public int PageSize { get; set; }
}
