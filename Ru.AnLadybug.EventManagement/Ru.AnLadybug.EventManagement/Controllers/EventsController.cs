using Microsoft.AspNetCore.Mvc;
using Ru.AnLadybug.EventManagement.Controllers.Convertors;
using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.ModelDto;

namespace Ru.AnLadybug.EventManagement.Controllers
{
    /// <summary>
    /// Контроллер для управления событиями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EventsController(IEventService _eventService) : ControllerBase
    {
        /// <summary>
        /// Получить список всех событий.
        /// </summary>
        /// <returns>Коллекция DTO всех существующих событий.</returns>
        /// <response code="200">Список событий успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<EventDto>), StatusCodes.Status200OK)]
        public ActionResult<IReadOnlyCollection<EventDto>> GetAll()
        {
            return _eventService.GetAll().Select(EventConvertor.ToDto).ToList();
        }

        /// <summary>
        /// Получить событие по его идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор события (Guid).</param>
        /// <returns>DTO события или статус-код ошибки.</returns>
        /// <response code="200">Успешное выполнение. Возвращает объект события.</response>
        /// <response code="404">Событие с указанным идентификатором не найдено.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionResult<EventDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<EventDto> Get(Guid id)
        {
            var findEvent = _eventService.Get(id);
            if (findEvent == null) 
                return NotFound($"Событие с идентификатором {id} не найдено");

            return EventConvertor.ToDto(findEvent);
        }

        /// <summary>
        /// Создать новое событие.
        /// </summary>
        /// <param name="createEventDto">DTO данные для создания нового события.</param>
        /// <returns>DTO созданного события и HTTP-заголовок Location со ссылкой на него.</returns>
        /// <response code="201">Событие успешно создано.</response>
        /// <response code="400">DTO данные для создания события не прошли валидацию.</response>
        [HttpPost]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<EventDto> Create([FromBody] CreateEventDto createEventDto)
        {
            var createEvent = CreateEventDtoConvertor.ToCreateEvent(createEventDto);
            var newEvent = _eventService.Create(createEvent);
            var eventDto = EventConvertor.ToDto(newEvent);
            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, eventDto);
        }

        /// <summary>
        /// Обновить данные существующего события.
        /// </summary>
        /// <param name="updateEventDto">DTO данные для обновления события.</param>
        /// <returns>DTO обновленного события.</returns>
        /// <response code="200">Событие успешно обновлено.</response>
        /// <response code="404">Событие для обновления не найдено либо</response>
        /// <response code="400">DTO данные для обновления события не прошли валидацию.</response>
        [HttpPut]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<EventDto> Update([FromBody] UpdateEventDto updateEventDto)
        {
            var updateEvent = UpdateEventDtoConvertor.ToUpdateEvent(updateEventDto);
            var renewEvent = _eventService.Update(updateEvent);
            if (renewEvent == null) 
                return NotFound($"Событие с идентификатором {updateEventDto.Id} не найдено");

            return EventConvertor.ToDto(renewEvent);
        }

        /// <summary>
        /// Удалить событие по его идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемого события (Guid).</param>
        /// <returns>Статус выполнения операции без тела ответа.</returns>
        /// <response code="204">Событие успешно удалено.</response>
        /// <response code="404">Событие для удаления не найдено.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var result = _eventService.Delete(id);
            return result ? NoContent() : NotFound($"Событие с идентификатором {id} не найдено");
        }
    }
}
