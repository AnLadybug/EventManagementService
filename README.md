# EventManagementService
Проектная работа курса "Продвинутая разработка на C# и .NET" Яндекс Практикума.
Модуль 1. Спринт 1. Разработка каркаса API

## Быстрый запуск

### Требования
* [.NET 10.0 SDK](https://microsoft.com) (или выше)

### Запуск локально
1. Склонируйте репозиторий:
   ```bash
   git clone https://github.com/AnLadybug/EventManagementService
   ```
2. Перейдите в папку проекта:
   ```bash
   cd Ru.AnLadybug.EventManagement/Ru.AnLadybug.EventManagement
   ```
3. Восстановите зависимости и запустите проект:
   ```bash
   dotnet restore
   dotnet run
   ```
4. Откройте Swagger в браузере: `http://localhost:5201/swagger` (или по порту из вашего `launchSettings.json`).

---

## Краткая документация API

### Схемы данных (DTO)

#### 1. CreateEventDto (Создание события)
* `Title` (string, обязательное): Новое название события.
* `Description` (string, опционально): Новое описание.
* `StartAt` (DateTime, обязательное): Дата начала (UTC).
* `EndAt` (DateTime, обязательное): Дата окончания (UTC).
* *Валидация:* `EndAt` должно быть строго позже `StartAt`.

#### 2. UpdateEventDto (Обновление события)
* `Title` (string, обязательное): Новое название события.
* `Description` (string, опционально): Новое описание.
* `StartAt` (DateTime, обязательное): Дата начала (UTC).
* `EndAt` (DateTime, обязательное): Дата окончания (UTC).
* *Валидация:* `EndAt` должно быть строго позже `StartAt`.

#### 3. EventDto (Информация о событии)
* `Id` (Guid, обязательное): Сгенерированный идентификатор события.
* `Title` (string, обязательное): Новое название события.
* `Description` (string, опционально): Новое описание.
* `StartAt` (DateTime, обязательное): Дата начала (UTC).
* `EndAt` (DateTime, обязательное): Дата окончания (UTC).
* *Валидация:* `EndAt` должно быть строго позже `StartAt`.

### Эндпоинты

| Метод | Путь | Описание | Тело запроса | Возможные ответы (Код / Тип) |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/events` | Получить все события | *Нет* | `200 OK` (`IReadOnlyCollection<EventDto>`) |
| **GET** | `/api/events/{id:Guid}` | Получить событие по ID | *Нет* | `200 OK` (`EventDto`) <br> `404 NotFound` (`string`)|
| **POST** | `/api/events/` | Создать событие | `CreateEventDto` | `201 Created` (`EventDto`) <br> `400 BadRequest` (`ValidationProblemDetails`)|
| **PUT** | `/api/events/{id:Guid}` | Обновить событие | `UpdateEventDto` | `200 OK` (`EventDto`) <br> `404 NotFound` (`string`) <br> `400 BadRequest` (`ValidationProblemDetails`)|
| **DELETE**| `/api/events/{id:Guid}` | Удалить событие | *Нет* | `204 NoContent` <br> `404 NotFound` (`string`)|

---

## Разработка

Для генерации актуальной документации в Swagger используются XML-комментарии прямо в коде. При сборке проекта автоматически обновляется файл спецификации OpenAPI.
