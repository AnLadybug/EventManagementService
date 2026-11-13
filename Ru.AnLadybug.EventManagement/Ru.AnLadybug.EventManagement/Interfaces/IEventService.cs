using Ru.AnLadybug.EventManagement.Models;
using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.Models.Search;
using Ru.AnLadybug.EventManagement.Models.Update;

namespace Ru.AnLadybug.EventManagement.Interfaces;

public interface IEventService
{
    PaginatedResult<Event> GetAll(EventSearchParams filter);
    Event? Get(Guid id);
    Event Create(CreateEvent createEvent);
    Event? Update(Guid id, UpdateEvent updateEvent);
    bool Delete(Guid id);
}
