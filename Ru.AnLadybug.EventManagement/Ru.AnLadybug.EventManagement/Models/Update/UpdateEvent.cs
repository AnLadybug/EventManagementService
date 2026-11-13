namespace Ru.AnLadybug.EventManagement.Models.Update;

public record UpdateEvent
{
    public required string Title { get; init; }
    public string Description { get; init; }
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
}