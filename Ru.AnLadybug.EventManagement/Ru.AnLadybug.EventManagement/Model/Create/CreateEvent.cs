namespace Ru.AnLadybug.EventManagement.Model.Create;

public record CreateEvent
{
    public required string Title { get; set; }
    public required string Description { get; set; }

    public required DateTime StartAt { get; set; }

    public required DateTime EndAt { get; set; }
}
