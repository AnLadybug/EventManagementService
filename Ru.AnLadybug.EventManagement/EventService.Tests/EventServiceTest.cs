using Microsoft.AspNetCore.Mvc.RazorPages;
using Ru.AnLadybug.EventManagement.Models;
using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.Models.Search;
using Ru.AnLadybug.EventManagement.Models.Update;
using Ru.AnLadybug.EventManagement.Services;

namespace Ru.AnLadybug.EventManagement.Tests
{
    public class EventServiceTest
    {
        private readonly EventService _service;
        public EventServiceTest()
        {
            _service = new EventService();
        }
        //Успешные сценарии:
        //        
        //          фильтрация по названию;
        //          фильтрация по датам(startDate, endDate);
        //        пагинация событий;
        //        комбинированная фильтрация.
        //   Неуспешные сценарии:
        //          попытка получить событие с несуществующим ID;
        //        попытка обновить событие с несуществующим ID;
        //        создание события с некорректными данными(если валидация в сервисе);
        //        обновление события с некорректными датами(EndAt раньше StartAt).
        #region Успешные сценарии: CRUD

        [Fact]
        public void Create_ValidData_CreatesAndReturnsEvent()
        {
            // Arrange
            var createEvent = new CreateEvent
            {
                Title = "Воркшоп по xUnit",
                Description = "Практическое занятие",
                StartAt = DateTime.UtcNow.AddDays(1),
                EndAt = DateTime.UtcNow.AddDays(2)
            };

            // Act
            var created = _service.Create(createEvent);
            var fetched = _service.Get(created.Id);

            // Assert
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.Equal(createEvent.Title, created.Title);
            Assert.Equal(createEvent.Description, created.Description);
            Assert.Equal(createEvent.StartAt, created.StartAt);
            Assert.Equal(createEvent.EndAt, created.EndAt);

            Assert.Same(created, fetched);
        }

        [Fact]
        public void Get_ExistingId_ReturnsEvent()
        {
            // Arrange
            var created = CreateSampleEvent(
                "Митап",
                new DateTime(2026, 12, 1),
                new DateTime(2026, 12, 2));

            // Act
            var result = _service.Get(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Митап", result.Title);
        }

        [Fact]
        public void GetAll_WithoutFilters_ReturnsAllEvents()
        {
            // Arrange
            CreateSampleEvent("Событие 1", new DateTime(2026, 12, 1), new DateTime(2026, 12, 2));
            CreateSampleEvent("Событие 2", new DateTime(2026, 12, 3), new DateTime(2026, 12, 4));
            var page = 1;
            var pageSize = 10;
            var filter = EventSearchParams.CreatePage(null, null, null, page, pageSize);

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
        }

        #endregion

        private Event CreateSampleEvent(
            string title, 
            DateTime startAt, 
            DateTime endAt, 
            string? description = null)
        {
            var createEvent = new CreateEvent
            {
                Title = title,
                StartAt = startAt,
                EndAt = endAt,
                Description = description
            };
            return _service.Create(createEvent);
        }

        [Fact]
        public void Update_ExistingEvent_UpdatesPropertiesAndReturnsEvent()
        {
            // Arrange
            var created = CreateSampleEvent(
                "Старое название", 
                new DateTime(2026, 12, 1), 
                new DateTime(2026, 12, 2));

            var updateDto = new UpdateEvent
            {
                Title = "Новое название",
                StartAt = new DateTime(2026, 12, 5, 14, 0, 0),
                EndAt = new DateTime(2026, 12, 5, 16, 0, 0),
                Description = "Обновленное описание"
            };

            // Act
            var updated = _service.Update(created.Id, updateDto);
            var fetched = _service.Get(created.Id);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(updateDto.Title, updated.Title);
            Assert.Equal(updateDto.StartAt, updated.StartAt);
            Assert.Equal(updateDto.EndAt, updated.EndAt);
            Assert.Equal(updateDto.Description, updated.Description);
            Assert.Equal("Новое название", fetched?.Title);
        }

        [Fact]
        public void Delete_ExistingEvent_RemovesEventAndReturnsTrue()
        {
            // Arrange
            var created = CreateSampleEvent(
                "Удаляемое событие", 
                new DateTime(2026, 12, 1), 
                new DateTime(2026, 12, 2));

            // Act
            var isDeleted = _service.Delete(created.Id);
            var fetched = _service.Get(created.Id);

            // Assert
            Assert.True(isDeleted);
            Assert.Null(fetched);
        }
    }
}
