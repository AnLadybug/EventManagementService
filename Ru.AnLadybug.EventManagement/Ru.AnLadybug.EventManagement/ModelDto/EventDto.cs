using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.ModelDto;

/// <summary>
/// Информация о существующем событии (используется для ответов API).
/// </summary>
public class EventDto : IValidatableObject
{
    /// <summary>
    /// Уникальный идентификатор события.
    /// </summary>
    /// <example>d3b07384-d113-4956-d5c1-e8d1a1215496</example>
    [Required(ErrorMessage = "Идентификатор события обязателен для заполнения")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название события.
    /// </summary>
    /// <example>Стратегическая сессия</example>
    [Required(ErrorMessage = "Название события обязательно для заполнения")]
    public string Title { get; set; }

    /// <summary>
    /// Детальное описание события (необязательное).
    /// </summary>
    /// <example>Планирование целей и ключевых результатов на текущий квартал.</example>
    public string Description { get; set; }

    /// <summary>
    /// Запланированная дата и время начала события (UTC).
    /// </summary>
    /// <example>2026-08-15T09:00:00Z</example>
    [Required(ErrorMessage = "Дата и время начала события обязательны для заполнения")]
    public DateTime StartAt { get; set; }

    /// <summary>
    /// Запланированная дата и время окончания события (UTC).
    /// </summary>
    /// <example>2026-08-15T12:00:00Z</example>
    [Required(ErrorMessage = "Дата и время окончания события обязательны для заполнения")]
    public DateTime EndAt { get; set; }

    /// <summary>
    /// Проверить временные рамки при создании события
    /// </summary>
    /// <param name="validationContext">Контекст выполнения валидации.</param>
    /// <returns>Результат проверки корректности дат.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndAt <= StartAt)
        {
            yield return new ValidationResult(
                "Дата и время окончания события должны быть строго позже начала события",
                [nameof(StartAt), nameof(EndAt)]
            );
        }
    }
}
