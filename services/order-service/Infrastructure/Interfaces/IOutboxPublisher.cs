namespace OrderService.Infrastructure.Interfaces;

public interface IOutboxPublisher
{
    Task PublishAsync(CancellationToken ct);
}
