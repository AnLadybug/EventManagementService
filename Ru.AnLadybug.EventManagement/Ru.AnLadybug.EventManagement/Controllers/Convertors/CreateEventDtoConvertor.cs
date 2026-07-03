using Ru.AnLadybug.EventManagement.Model.Create;
using Ru.AnLadybug.EventManagement.ModelDto;

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
