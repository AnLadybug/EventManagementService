using Ru.AnLadybug.EventManagement.Model.Update;
using Ru.AnLadybug.EventManagement.ModelDto;

namespace Ru.AnLadybug.EventManagement.Controllers.Convertors;

public static class UpdateEventDtoConvertor
{
    public static UpdateEvent ToUpdateEvent(UpdateEventDto input)
    {
        return new UpdateEvent
        {
            Id = input.Id,
            Title = input.Title,
            Description = input.Description,
            StartAt = input.StartAt,
            EndAt = input.EndAt,
        };
    }
}
