using Microsoft.AspNetCore.Http.HttpResults;
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
            var filter = EventSearchParams.CreatePage(null, null, null, null, null);

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
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

        #endregion

        #region Успешные сценарии: Фильтрация и пагинация

        [Fact]
        public void GetAll_FilterByTitle_ReturnsMatchingEventsCaseInsensitive()
        {
            // Arrange
            CreateSampleEvent("Конференция C# Pro", new DateTime(2026, 12, 1), new DateTime(2026, 12, 2));
            CreateSampleEvent("Хакатон по Python", new DateTime(2026, 12, 3), new DateTime(2026, 12, 4));
            CreateSampleEvent("Вебинар по c#", new DateTime(2026, 12, 5), new DateTime(2026, 12, 6));
            var filter = EventSearchParams.CreatePage("C#", null, null, null, null);

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.Items, e => Assert.Contains("c#", e.Title, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void GetAll_FilterByDateRange_ReturnsEventsWithinRange()
        {
            // Arrange
            var rangeStart = new DateTime(2026, 12, 1);
            var rangeEnd = new DateTime(2026, 12, 20);
            CreateSampleEvent("Раннее", rangeStart.AddMonths(-2), rangeStart.AddMonths(-1));
            var expected = CreateSampleEvent("Подходящее", rangeStart.AddDays(2), rangeEnd.AddDays(-2));
            CreateSampleEvent("Позднее", rangeEnd.AddDays(5), rangeEnd.AddDays(10));
            var filter = EventSearchParams.CreatePage(null, rangeStart, rangeEnd, null, null);

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal(expected.Id, result.Items.First().Id);
        }

        [Fact]
        public void GetAll_Pagination_ReturnsCorrectPageAndTotalCount()
        {
            // Arrange
            var baseDate = new DateTime(2026, 12, 1);
            for (int i = 1; i <= 5; i++)
            {
                CreateSampleEvent(
                    $"Событие {i}",
                    baseDate.AddDays(i),
                    baseDate.AddDays(i + 1));
            }

            var page = 2;
            var pageSize = 2;
            var filter = EventSearchParams.CreatePage(null, null, null, page, pageSize);

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(5, result.TotalCount); // Общее количество не зависит от страницы
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Событие 3", result.Items.First().Title);
            Assert.Equal("Событие 4", result.Items.Last().Title);
        }

        [Fact]
        public void GetAll_CombinedFilters_ReturnsStrictlyMatchingEvents()
        {
            // Arrange
            var rangeStart = new DateTime(2026, 12, 1);
            var rangeEnd = new DateTime(2026, 12, 20);
            var page = 1;
            var pageSize = 10;
            CreateSampleEvent("DevOps Митап", rangeStart.AddDays(-20), rangeStart.AddDays(-10)); // Не подходит по дате
            var expected = CreateSampleEvent("DevOps Конференция", rangeStart.AddDays(5), rangeEnd.AddDays(-5)); // Подходит
            CreateSampleEvent("Frontend Митап", rangeEnd.AddDays(5), rangeEnd.AddDays(10)); // Не подходит по названию

            var filter = EventSearchParams.CreatePage(
                "DevOps",
                rangeStart,
                rangeEnd,
                page,
                pageSize
            );

            // Act
            var result = _service.GetAll(filter);

            // Assert
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal(expected.Id, result.Items.First().Id);
            Assert.Same(result.Items.First(), expected);
        }
        #endregion

        #region Неуспешные сценарии

        [Fact]
        public void Get_NonExistentId_ReturnsNull()
        {
            // Act
            var result = _service.Get(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_NonExistentId_ReturnsNull()
        {
            // Arrange
            var updateDto = new UpdateEvent
            {
                Title = "Новое имя",
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow.AddHours(1)
            };

            // Act
            var result = _service.Update(Guid.NewGuid(), updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_NonExistentId_ReturnsFalse()
        {
            // Act
            var result = _service.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_EndAtBeforeStartAt_ThrowsArgumentException()
        {
            // Arrange
            var createEvent = new CreateEvent
            {
                Title = "Некорректное событие",
                StartAt = new DateTime(2026, 12, 10, 15, 0, 0),
                EndAt = new DateTime(2026, 12, 10, 10, 0, 0) // Раньше StartAt
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.Create(createEvent));
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
    }
}
