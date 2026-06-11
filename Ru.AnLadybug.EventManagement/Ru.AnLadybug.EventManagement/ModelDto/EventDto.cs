using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.ModelDto;

public class EventDto
{
    [Required(ErrorMessage = "Идентификатор события обязателен для заполнения")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Название события обязательно для заполнения")]
    public string Title { get; set; }
    public string Description { get; set; }

    [Required(ErrorMessage = "Дата начала события обязательно для заполнения")]
    public DateTime StartAt { get; set; }

    [Required(ErrorMessage = "Дата окончания события обязательно для заполнения")]
    public DateTime EndAt { get; set; }
}
