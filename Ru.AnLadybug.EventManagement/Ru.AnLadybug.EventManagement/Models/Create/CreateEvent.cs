namespace Ru.AnLadybug.EventManagement.Models.Create;

public record CreateEvent
{
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
}
