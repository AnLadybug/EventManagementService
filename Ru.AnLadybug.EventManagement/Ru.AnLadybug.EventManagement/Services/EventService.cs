using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.Models;
using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.Models.Search;
using Ru.AnLadybug.EventManagement.Models.Update;

namespace Ru.AnLadybug.EventManagement.Services;

public class EventService : IEventService
{
    private List<Event> _events = [];

    public PaginatedResult<Event> GetAll(EventSearchParams filter)
    {
        IEnumerable<Event> query = Query(_events, filter);
        var totalCount = Query(_events, filter).Count();
  
        query = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

        var result = new PaginatedResult<Event>
        {
            TotalCount = totalCount,
            Items = [.. query],
            PageNumber = filter.Page,
            PageSize = filter.PageSize
        };
        return result;
    }

    private static IEnumerable<Event> Query(IEnumerable<Event> query, EventSearchParams filter)
    {
        if (!string.IsNullOrEmpty(filter.Title))
            query = query.Where(e => e.Title.Contains(filter.Title, StringComparison.CurrentCultureIgnoreCase));
        if (filter.From.HasValue)
            query = query.Where(e => e.StartAt >= filter.From);
        if (filter.To.HasValue)
            query = query.Where(e => e.EndAt <= filter.To);

        return query;
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
