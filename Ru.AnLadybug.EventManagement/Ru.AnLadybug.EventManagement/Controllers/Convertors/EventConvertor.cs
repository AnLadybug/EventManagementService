using Ru.AnLadybug.EventManagement.Model;
using Ru.AnLadybug.EventManagement.ModelDto;

namespace Ru.AnLadybug.EventManagement.Controllers.Convertors;

public static class EventConvertor
{
    public static EventDto ToDto(Event input)
    {
        return new EventDto
        {
            Id = input.Id,
            Title = input.Title,
            Description = input.Description,
            StartAt = input.StartAt,
            EndAt = input.EndAt
        };
    }
}
