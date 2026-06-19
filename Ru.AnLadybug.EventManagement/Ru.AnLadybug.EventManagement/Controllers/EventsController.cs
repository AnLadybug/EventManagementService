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
            var createEvent = CreateEventDtoConvertor.ToCreateEvent(createEventDto);
            var newEvent = _eventService.Create(createEvent);
            var eventDto = EventConvertor.ToDto(newEvent);
            return Created($"Событие с идентификатором {newEvent.Id} создано", eventDto);
        }

        [HttpPut]
        public ActionResult<EventDto> Update(UpdateEventDto updateEventDto)
        {
            var updateEvent = UpdateEventDtoConvertor.ToUpdateEvent(updateEventDto);
            var renewEvent = _eventService.Update(updateEvent);
            if (renewEvent == null) 
                return NotFound($"Событие с идентификатором {updateEventDto.Id} не найдено");

            return EventConvertor.ToDto(renewEvent);
        }

        [HttpDelete("id")]
        public IActionResult Delete(Guid id)
        {
            var result = _eventService.Delete(id);
            return result ? NoContent() : NotFound($"Событие с идентификатором {id} не найдено");
        }
    }
}
