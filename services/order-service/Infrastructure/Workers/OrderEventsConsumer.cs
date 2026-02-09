using System.Diagnostics.Metrics;
using System.Text.Json;
using Confluent.Kafka;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Workers;

public class OrderEventsConsumer : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;
    private readonly ILogger<OrderEventsConsumer> _logger;
    private static readonly Meter Meter = new("OrderService.Events", "1.0.0");
    private static readonly Counter<long> EventsConsumed = Meter.CreateCounter<long>("events_consumed_count");
    private static readonly Counter<long> EventsFailed = Meter.CreateCounter<long>("events_failed_count");
    private static readonly IReadOnlyDictionary<string, string> StatusByEventType = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["InventoryReserved"] = "InventoryReserved",
        ["InventoryReservationFailed"] = "InventoryReservationFailed",
        ["InvoiceCreated"] = "InvoiceCreated",
        ["PaymentSucceeded"] = "PaymentSucceeded",
        ["PaymentFailed"] = "PaymentFailed",
        ["ApprovalGranted"] = "ApprovalGranted",
        ["ApprovalRejected"] = "ApprovalRejected"
    };

    public OrderEventsConsumer(IServiceProvider services, IConfiguration config, ILogger<OrderEventsConsumer> logger)
    {
        _services = services;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"],
            GroupId = _config["Kafka:ConsumerGroup"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"]
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        using var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        var dlqTopic = _config["Kafka:DlqTopic"]!;
        var topics = new[]
        {
            _config["Kafka:InventoryTopic"],
            _config["Kafka:BillingTopic"],
            _config["Kafka:WorkflowTopic"]
        }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        consumer.Subscribe(topics);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);
            if (result == null) continue;

            using var scope = _services.CreateScope();
            var inboxRepo = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            if (!Guid.TryParse(result.Message.Key, out var eventId))
            {
                consumer.Commit(result);
                continue;
            }

            var alreadyProcessed = await inboxRepo.ExistsAsync(eventId);
            if (alreadyProcessed)
            {
                consumer.Commit(result);
                continue;
            }

            var eventType = "OrderEvent";
            Guid? orderId = null;
            var statusUpdated = false;
            string? correlationId = null;
            Exception? lastException = null;
            var attempt = 0;

            while (attempt < 5)
            {
                try
                {
                    (eventType, orderId, statusUpdated, correlationId) =
                        await HandleEventPayloadAsync(result.Message.Value, db, stoppingToken);
                    lastException = null;
                    break;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    attempt += 1;
                    lastException = ex;
                    if (attempt >= 5)
                    {
                        break;
                    }

                    var delayMs = (int)(200 * Math.Pow(2, attempt - 1)) + Random.Shared.Next(0, 100);
                    await Task.Delay(TimeSpan.FromMilliseconds(delayMs), stoppingToken);
                }
            }

            if (lastException != null)
            {
                EventsFailed.Add(1,
                    new KeyValuePair<string, object?>("eventType", "Unknown"),
                    new KeyValuePair<string, object?>("error_code", "handler_exception"));
                _logger.LogError(lastException, "Failed to handle event payload (eventId={EventId})", eventId);
                await producer.ProduceAsync(
                    dlqTopic,
                    new Message<string, string>
                    {
                        Key = result.Message.Key,
                        Value = result.Message.Value
                    },
                    stoppingToken
                );
                consumer.Commit(result);
                continue;
            }

            EventsConsumed.Add(1, new KeyValuePair<string, object?>("eventType", eventType));
            if (string.Equals(eventType, "MalformedEvent", StringComparison.Ordinal))
            {
                EventsFailed.Add(1,
                    new KeyValuePair<string, object?>("eventType", eventType),
                    new KeyValuePair<string, object?>("error_code", "json_parse"));
                await producer.ProduceAsync(
                    dlqTopic,
                    new Message<string, string>
                    {
                        Key = result.Message.Key,
                        Value = result.Message.Value
                    },
                    stoppingToken
                );
            }
            _logger.LogInformation(
                "Consumed event {EventType} (eventId={EventId}, orderId={OrderId}, statusUpdated={StatusUpdated}, correlationId={CorrelationId})",
                eventType,
                eventId,
                orderId,
                statusUpdated,
                correlationId
            );

            await inboxRepo.AddAsync(new InboxEvent
            {
                Id = eventId,
                EventType = eventType
            });

            await unitOfWork.SaveChangesAsync();
            consumer.Commit(result);
        }
    }

    internal static async Task<(string EventType, Guid? OrderId, bool StatusUpdated, string? CorrelationId)> HandleEventPayloadAsync(
        string? messageValue,
        OrdersDbContext db,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(messageValue))
        {
            return ("MalformedEvent", null, false, null);
        }

        try
        {
            using var doc = JsonDocument.Parse(messageValue);
            var root = doc.RootElement;
            var payload = root;
            var eventType = "OrderEvent";
            string? correlationId = null;

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("eventType", out var eventTypeElement))
                {
                    eventType = eventTypeElement.GetString() ?? eventType;
                }

                if (root.TryGetProperty("correlationId", out var correlationElement))
                {
                    correlationId = correlationElement.GetString();
                }

                if (root.TryGetProperty("payload", out var payloadElement))
                {
                    payload = payloadElement;
                }
            }

            if (payload.ValueKind != JsonValueKind.Object ||
                !payload.TryGetProperty("orderId", out var orderIdElement) ||
                !Guid.TryParse(orderIdElement.GetString(), out var orderId))
            {
                return (eventType, null, false, correlationId);
            }

            if (!StatusByEventType.TryGetValue(eventType, out var nextStatus))
            {
                return (eventType, orderId, false, correlationId);
            }

            var order = await db.Orders.FindAsync([orderId], ct);
            if (order == null)
            {
                return (eventType, orderId, false, correlationId);
            }

            if (!string.Equals(order.Status, nextStatus, StringComparison.Ordinal))
            {
                order.Status = nextStatus;
                return (eventType, orderId, true, correlationId);
            }

            return (eventType, orderId, false, correlationId);
        }
        catch (JsonException)
        {
            return ("MalformedEvent", null, false, null);
        }
    }
}
