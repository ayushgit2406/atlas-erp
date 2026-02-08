namespace OrderService.Domain.Entities;

public class OutboxEvent
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public int RetryCount { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Error { get; set; }
}
