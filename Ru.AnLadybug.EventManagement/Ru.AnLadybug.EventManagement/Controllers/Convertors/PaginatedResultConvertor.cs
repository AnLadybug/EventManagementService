using Ru.AnLadybug.EventManagement.Models;
using Ru.AnLadybug.EventManagement.ModeslDto;

namespace Ru.AnLadybug.EventManagement.Controllers.Convertors;

public static class PaginatedResultConvertor
{
    public static PaginatedResultDto<EventDto> ToDto(PaginatedResult<Event> input)
    {
        return new PaginatedResultDto<EventDto>
        {
            TotalCount = input.TotalCount,
            Items = (IReadOnlyCollection<EventDto>)input.Items.Select(EventConvertor.ToDto),
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }
}
