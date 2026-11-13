namespace Ru.AnLadybug.EventManagement.ModeslDto;

/// <summary>
/// Информация о существующем событии (используется для ответов API).
/// </summary>
public class EventDto
{
    /// <summary>
    /// Уникальный идентификатор события.
    /// </summary>
    /// <example>d3b07384-d113-4956-d5c1-e8d1a1215496</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Название события.
    /// </summary>
    /// <example>Стратегическая сессия</example>
    public string Title { get; set; }

    /// <summary>
    /// Детальное описание события.
    /// </summary>
    /// <example>Планирование целей и ключевых результатов на текущий квартал.</example>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Запланированная дата и время начала события (UTC).
    /// </summary>
    /// <example>2026-08-15T09:00:00Z</example>
    public DateTime StartAt { get; set; }

    /// <summary>
    /// Запланированная дата и время окончания события (UTC).
    /// </summary>
    /// <example>2026-08-15T12:00:00Z</example>
    public DateTime EndAt { get; set; }
}
