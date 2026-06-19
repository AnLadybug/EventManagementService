using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.Model;
using Ru.AnLadybug.EventManagement.Model.Create;
using Ru.AnLadybug.EventManagement.Model.Update;


namespace Ru.AnLadybug.EventManagement.Services;

public class EventService : IEventService
{
    private List<Event> _events = [];

    public IReadOnlyCollection<Event> GetAll()
    {
        return _events;
    }

    public Event? Get(Guid id)
    {
        return _events.Find(ev => ev.Id == id);
    }

    public Event Create(CreateEvent createEvent)
    {
        var ev = new Event(
            createEvent.Title,
            createEvent.StartAt,
            createEvent.EndAt,
            createEvent.Description);
        _events.Add(ev);
        return ev;            
    }

    public Event? Update(UpdateEvent updateEvent)
    {
        var findEvent = Get(updateEvent.Id);
        if (findEvent == null) return null;

        findEvent.Update(
            updateEvent.Title,
            updateEvent.StartAt,
            updateEvent.EndAt,
            updateEvent.Description);
        return findEvent;
    }

    public bool Delete(Guid id)
    {
        var inputCount = _events.Count;
        _events = [.. _events.Where(ev => ev.Id != id)];
        return _events.Count < inputCount;
    }
}
