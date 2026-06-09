namespace Ru.AnLadybug.EventManagement.Model;

public class Event
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }

    public Event(string title, DateTime startAt, DateTime endAt, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название события обязательно", nameof(title));
        if (startAt > EndAt)
            throw new ArgumentException("Начало события не может быть позже его окончания");
        if (EndAt < startAt)
            throw new ArgumentException("Окончание события не может быть раньше его начала");

        Id = Guid.NewGuid();
        Title = title;
        StartAt = startAt;
        EndAt = endAt;
        Description = description;
    }

    public Event(string title, DateTime startAt, DateTime endAt)
        : this(title, startAt, endAt, string.Empty)
    {

    }
}
