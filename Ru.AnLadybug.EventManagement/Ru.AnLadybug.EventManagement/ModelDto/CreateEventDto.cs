using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.ModelDto;

public class CreateEventDto : IValidatableObject
{
    [Required(ErrorMessage = "Название события обязательно для заполнения")]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Дата и время начала события обязательны для заполнения")]
    public DateTime StartAt { get; set; }

    [Required(ErrorMessage = "Дата и время окончания события обязательны для заполнения")]
    public DateTime EndAt { get; set; }

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
