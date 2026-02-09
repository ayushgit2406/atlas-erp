namespace OrderService.Domain.Entities;

public class InboxEvent
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}
