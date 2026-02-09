using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Workers;

public class OutboxWorker : BackgroundService
{
    private readonly IServiceProvider _services;

    public OutboxWorker(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IOutboxPublisher>();

            await publisher.PublishAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
