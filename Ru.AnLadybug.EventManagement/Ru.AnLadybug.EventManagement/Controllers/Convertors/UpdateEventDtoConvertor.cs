using Ru.AnLadybug.EventManagement.ModeslDto;
using Ru.AnLadybug.EventManagement.Models.Update;

namespace Ru.AnLadybug.EventManagement.Controllers.Convertors;

public static class UpdateEventDtoConvertor
{
    public static UpdateEvent ToUpdateEvent(UpdateEventDto input)
    {
        return new UpdateEvent
        {
            Title = input.Title,
            Description = input.Description,
            StartAt = input.StartAt!.Value,
            EndAt = input.EndAt!.Value,
        };
    }
}
