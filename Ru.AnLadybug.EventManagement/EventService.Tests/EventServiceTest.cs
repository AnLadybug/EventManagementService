using Ru.AnLadybug.EventManagement.Models.Create;
using Ru.AnLadybug.EventManagement.Services;

namespace Ru.AnLadybug.EventManagement.Tests
{
    public class EventServiceTest
    {
        private readonly EventService _sut;
        public EventServiceTest()
        {
            _sut = new EventService();
        }
        //Успешные сценарии:
        //        создание события;
        //        получение всех событий;
        //          получение события по ID;
        //        обновление существующего события;
        //          удаление существующего события;
        //          фильтрация по названию;
        //          фильтрация по датам(startDate, endDate);
        //        пагинация событий;
        //        комбинированная фильтрация.
        //   Неуспешные сценарии:
        //          попытка получить событие с несуществующим ID;
        //        попытка обновить событие с несуществующим ID;
        //        создание события с некорректными данными(если валидация в сервисе);
        //        обновление события с некорректными датами(EndAt раньше StartAt).
        [Fact]
        public void Create_ValidData_CreatesAndReturnsEvent()
        {
            var fullEvent = new CreateEvent
            {
                Title = "Стратегическая сессия",
                Description = "Планирование целей и ключевых результатов на текущий квартал",
                StartAt = DateTime.UtcNow.AddDays(1),
                EndAt = DateTime.UtcNow.AddDays(2)
            };

            var partialEvent = new CreateEvent
            {
                Title = "Стратегическая сессия",
                Description = "",
                StartAt = DateTime.UtcNow.AddDays(1),
                EndAt = DateTime.UtcNow.AddDays(2)
            };

            var result1 = _sut.Create(fullEvent);
            var result2 = _sut.Create(partialEvent);




        }
    }
}
