using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class OutboxPublisher : IOutboxPublisher
{
    private readonly OrdersDbContext _db;
    private readonly IProducer<string, string> _producer;
    private readonly IConfiguration _config;

    public OutboxPublisher(OrdersDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task PublishAsync(CancellationToken ct)
    {
        var topic = _config["Kafka:OrdersTopic"]!;
        var dlq = _config["Kafka:DlqTopic"]!;

        var pending = await _db.OutboxEvents
            .Where(e => e.PublishedAt == null && e.RetryCount < 5)
            .OrderBy(e => e.OccurredAt)
            .Take(50)
            .ToListAsync(ct);

        foreach (var evt in pending)
        {
            while (evt.RetryCount < 5 && evt.PublishedAt == null)
            {
                try
                {
                    await _producer.ProduceAsync(
                        topic,
                        new Message<string, string>
                        {
                            Key = evt.Id.ToString(),
                            Value = evt.Payload
                        },
                        ct
                    );

                    evt.PublishedAt = DateTime.UtcNow;
                    evt.Error = null;
                }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    evt.RetryCount += 1;
                    evt.Error = ex.Message;

                    if (evt.RetryCount >= 5)
                    {
                        await _producer.ProduceAsync(
                            dlq,
                            new Message<string, string>
                            {
                                Key = evt.Id.ToString(),
                                Value = evt.Payload
                            },
                            ct
                        );
                        evt.PublishedAt = DateTime.UtcNow;
                        break;
                    }

                    var delayMs = (int)(200 * Math.Pow(2, evt.RetryCount - 1)) + Random.Shared.Next(0, 100);
                    await Task.Delay(TimeSpan.FromMilliseconds(delayMs), ct);
                }
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}
