using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.ModelDto;

/// <summary>
/// Данные для создания нового события.
/// </summary>
public class CreateEventDto : IValidatableObject
{
    /// <summary>
    /// Название нового события.
    /// </summary>
    /// <example>Стратегическая сессия</example>
    [Required(ErrorMessage = "Название события обязательно для заполнения")]
    public string Title { get; set; }

    /// <summary>
    /// Детальное описание сути и целей события (необязательное).
    /// </summary>
    /// <example>Планирование целей и ключевых результатов на текущий квартал.</example>
    public string Description { get; set; }

    /// <summary>
    /// Запланированная дата и время начала события (UTC).
    /// </summary>
    /// <example>2026-08-15T09:00:00Z</example>
    [Required(ErrorMessage = "Дата и время начала события обязательны для заполнения")]
    public DateTime? StartAt { get; set; }

    /// <summary>
    /// Запланированная дата и время окончания события (UTC).
    /// </summary>
    /// <example>2026-08-15T12:00:00Z</example>
    [Required(ErrorMessage = "Дата и время окончания события обязательны для заполнения")]
    public DateTime? EndAt { get; set; }

    /// <summary>
    /// Проверить временные рамки при создании события
    /// </summary>
    /// <param name="validationContext">Контекст выполнения валидации.</param>
    /// <returns>Результат проверки корректности дат.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartAt.HasValue && EndAt.HasValue && EndAt <= StartAt)
        {
            yield return new ValidationResult(
                "Дата и время окончания события должны быть строго позже начала события",
                [nameof(StartAt), nameof(EndAt)]                
            );
        }
    }
}
