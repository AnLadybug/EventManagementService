using Ru.AnLadybug.EventManagement.Model;
using Ru.AnLadybug.EventManagement.Model.Create;
using Ru.AnLadybug.EventManagement.Model.Update;

namespace Ru.AnLadybug.EventManagement.Interfaces;

public interface IEventService
{
    IReadOnlyCollection<Event> GetAll();
    Event? Get(Guid id);
    Event Create(CreateEvent createEvent);
    Event? Update(Guid id, UpdateEvent updateEvent);
    bool Delete(Guid id);
}
