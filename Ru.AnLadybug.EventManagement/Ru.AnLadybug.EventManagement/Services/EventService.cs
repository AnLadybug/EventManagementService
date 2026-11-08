using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.Models;
using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.Models.Update;


namespace Ru.AnLadybug.EventManagement.Services;

public class EventService : IEventService
{
    private List<Event> _events = [];

    public IReadOnlyCollection<Event> GetAll(string? title, DateTime? from, DateTime? to)
    {
        IEnumerable<Event> query = _events;
        if (!string.IsNullOrEmpty(title)) query = query.Where(e => e.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase));
        if (from.HasValue) query = query.Where(e => e.StartAt >= from);
        if (to.HasValue) query = query.Where(e => e.EndAt <= to);

        return query.ToList();
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

    public Event? Update(Guid id, UpdateEvent updateEvent)
    {
        var findEvent = Get(id);
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
