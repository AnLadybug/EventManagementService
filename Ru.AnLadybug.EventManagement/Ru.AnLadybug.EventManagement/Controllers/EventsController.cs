using Microsoft.AspNetCore.Mvc;
using Ru.AnLadybug.EventManagement.Controllers.Convertors;
using Ru.AnLadybug.EventManagement.Interfaces;
using Ru.AnLadybug.EventManagement.Models.Search;
using Ru.AnLadybug.EventManagement.ModeslDto;

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
        /// Получить постраничный список событий с возможностью фильтрации.
        /// </summary>
        /// <param name="title">Название события (поиск по частичному совпадению).</param>
        /// <param name="from">Начальная дата диапазона для фильтрации событий (включительно).</param>
        /// <param name="to">Конечная дата диапазона для фильтрации событий (включительно).</param>
        /// <param name="page">Номер запрашиваемой страницы (начиная с 1).</param>
        /// <param name="pageSize">Количество элементов на одной странице.</param>
        /// <returns>Объект пагинации, содержащий список DTO событий и метаданные о страницах.</returns>
        /// <response code="200">Список событий успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResultDto<EventDto>), StatusCodes.Status200OK)]
        public ActionResult<PaginatedResultDto<EventDto>> GetAll(
            [FromQuery] string? title,
            [FromQuery] DateTime? from, 
            [FromQuery] DateTime? to,
            [FromQuery] int? page,
            [FromQuery] int? pageSize)
        {
            var searchParam = EventSearchParams.CreatePage(title, from, to, page, pageSize);
            var pageEvents = _eventService.GetAll(searchParam);
            return PaginatedResultConvertor.ToDto(pageEvents);
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
        /// <param name="id">Уникальный идентификатор события (Guid).</param>
        /// <param name="updateEventDto">DTO данные для обновления события.</param>
        /// <returns>DTO обновленного события.</returns>
        /// <response code="200">Событие успешно обновлено.</response>
        /// <response code="404">Событие для обновления не найдено.</response>
        /// <response code="400">DTO данные для обновления события не прошли валидацию.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<EventDto> Update(Guid id, [FromBody] UpdateEventDto updateEventDto)
        {
            var updateEvent = UpdateEventDtoConvertor.ToUpdateEvent(updateEventDto);
            var renewEvent = _eventService.Update(id, updateEvent);
            if (renewEvent == null) 
                return NotFound($"Событие с идентификатором {id} не найдено");

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
