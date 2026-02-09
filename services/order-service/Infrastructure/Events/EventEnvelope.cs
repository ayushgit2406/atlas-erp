namespace OrderService.Infrastructure.Events;

public record EventEnvelope<TPayload>(
    Guid EventId,
    string EventType,
    string SchemaVersion,
    DateTime OccurredAt,
    string Producer,
    string CorrelationId,
    string? CausationId,
    string AggregateType,
    string AggregateId,
    TPayload Payload,
    Dictionary<string, string>? Metadata);
