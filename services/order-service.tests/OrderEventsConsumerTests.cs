using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Workers;
using Xunit;

public class OrderEventsConsumerTests
{
    [Fact]
    public async Task HandleEventPayloadAsync_UpdatesStatus_And_InboxEntryIsRecorded()
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase($"orders-{Guid.NewGuid()}")
            .Options;

        await using var db = new OrdersDbContext(options);
        var orderId = Guid.NewGuid();
        db.Orders.Add(new Order { Id = orderId, Status = "Placed", CustomerId = "c-1" });
        await db.SaveChangesAsync();

        var payload = JsonSerializer.Serialize(new
        {
            eventId = Guid.NewGuid(),
            eventType = "PaymentSucceeded",
            payload = new { orderId = orderId.ToString() }
        });

        var (eventType, handledOrderId, statusUpdated, correlationId) =
            await OrderEventsConsumer.HandleEventPayloadAsync(payload, db, CancellationToken.None);

        var inboxRepo = new InboxRepository(db);
        var unitOfWork = new UnitOfWork(db);
        await inboxRepo.AddAsync(new InboxEvent
        {
            Id = Guid.NewGuid(),
            EventType = eventType
        });
        await unitOfWork.SaveChangesAsync();

        var updated = await db.Orders.FindAsync(orderId);

        Assert.Equal("PaymentSucceeded", eventType);
        Assert.Equal(orderId, handledOrderId);
        Assert.True(statusUpdated);
        Assert.Equal("PaymentSucceeded", updated?.Status);
        Assert.Null(correlationId);
        Assert.True(await db.InboxEvents.AnyAsync());
    }
}
