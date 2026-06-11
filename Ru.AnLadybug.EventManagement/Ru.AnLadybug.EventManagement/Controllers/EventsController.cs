using Microsoft.AspNetCore.Mvc;
using Ru.AnLadybug.EventManagement.Controllers.Convertors;
using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.ModelDto;

namespace Ru.AnLadybug.EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(IEventService _eventService) : ControllerBase
    {        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<EventDto>> GetAll()
        {
            return _eventService.GetAll().Select(EventConvertor.ToDto).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<EventDto> Get(Guid id)
        {
            var findEvent = _eventService.Get(id);
            if (findEvent == null) 
                return NotFound($"Событие с идентификатором {id} не найдено");

            return EventConvertor.ToDto(findEvent);
        }

        [HttpPost]
        public ActionResult<EventDto> Create([FromBody]CreateEventDto createEventDto)
        {
            var result = _eventService.Create(CreateEventDtoConvertor.ToCreateEvent(createEventDto));
            return EventConvertor.ToDto(result);
        }

        [HttpPut]
        public ActionResult<EventDto> Update(UpdateEventDto updateEventDto)
        {
            var result = _eventService.Update(UpdateEventDtoConvertor.ToUpdateEvent(updateEventDto));
            return EventConvertor.ToDto(result);
        }

        [HttpDelete("id")]
        public IActionResult Delete(Guid id)
        {
            var result = _eventService.Delete(id);
            return result ? NoContent() : NotFound($"Событие с идентификатором {id} не найдено");
        }
    }
}
