namespace Ru.AnLadybug.EventManagement.Models.Update;

public record UpdateEvent
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}
