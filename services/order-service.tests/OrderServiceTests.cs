using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderService.Contracts;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;
using Xunit;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrderAsync_WritesEnvelopeWithCorrelationId()
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase($"orders-{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var db = new OrdersDbContext(options);
        var orderRepo = new OrderRepository(db);
        var outboxRepo = new OutboxRepository(db);
        var uow = new UnitOfWork(db);

        var service = new OrderService.Application.Services.OrderService(orderRepo, outboxRepo, uow);
        var request = new CreateOrderRequest(
            "c-1",
            [new CreateOrderItem("sku-1", 2, 10)],
            "USD"
        );

        var correlationId = Guid.NewGuid().ToString();
        var response = await service.CreateOrderAsync(request, correlationId, "idem-1");

        var outbox = await db.OutboxEvents.SingleAsync();
        using var doc = JsonDocument.Parse(outbox.Payload);
        var root = doc.RootElement;

        Assert.Equal("OrderPlaced", root.GetProperty("eventType").GetString());
        Assert.Equal(correlationId, root.GetProperty("correlationId").GetString());
        Assert.Equal(response.OrderId.ToString(), root.GetProperty("aggregateId").GetString());
    }
}
