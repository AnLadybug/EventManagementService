using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.ModelDto;

/// <summary>
/// Данные для обновления параметров существующего события.
/// </summary>
public class UpdateEventDto : IValidatableObject
{
    /// <summary>
    /// Новое название события.
    /// </summary>
    /// <example>Обновленная стратегическая сессия</example>
    [Required(ErrorMessage = "Название события обязательно для заполнения")]
    public string Title { get; set; }

    /// <summary>
    /// Новое описание события (необязательное).
    /// </summary>
    /// <example>Корректировка планов на основе итогов полугодия.</example>
    public string Description { get; set; }

    /// <summary>
    /// Новая дата и время начала события (UTC).
    /// </summary>
    /// <example>2026-08-25T09:00:00Z</example>
    [Required(ErrorMessage = "Дата и время начала события обязательны для заполнения")]
    public DateTime? StartAt { get; set; }

    /// <summary>
    /// Новая дата и время окончания события (UTC).
    /// </summary>
    /// <example>2026-08-25T12:00:00Z</example>
    [Required(ErrorMessage = "Дата и время окончания события обязательны для заполнения")]
    public DateTime? EndAt { get; set; }

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
