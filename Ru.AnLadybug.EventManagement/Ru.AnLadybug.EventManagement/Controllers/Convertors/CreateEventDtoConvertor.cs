using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.ModeslDto;

namespace Ru.AnLadybug.EventManagement.Controllers.Convertors;

public static class CreateEventDtoConvertor
{
    public static CreateEvent ToCreateEvent(CreateEventDto input)
    {
        return new CreateEvent
        {
            Title = input.Title,
            Description = input.Description,
            StartAt = input.StartAt!.Value,
            EndAt = input.EndAt!.Value,
        };
    }
}
