using System.Text.Json;
using OrderService.Application.Interfaces;
using OrderService.Contracts;
using OrderService.Infrastructure.Events;
using OrderService.Infrastructure.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public OrderService(IOrderRepository orderRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, string correlationId, string? causationId)
    {
        var orderId = Guid.NewGuid();
        var total = request.Items.Sum(i => i.UnitPrice * i.Qty);

        var order = new Order
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            Status = "Placed",
            TotalAmount = total,
            Currency = request.Currency,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Sku = item.Sku,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice
            });
        }

        var outbox = CreateOutboxEvent(
            "OrderPlaced",
            order.Id,
            correlationId,
            causationId,
            new
            {
                orderId = order.Id,
                customerId = order.CustomerId,
                items = order.Items.Select(i => new { sku = i.Sku, qty = i.Qty, unitPrice = i.UnitPrice }),
                totalAmount = order.TotalAmount,
                currency = order.Currency
            });

        await _orderRepository.ExecuteInTransactionAsync(async () =>
        {
            await _orderRepository.AddAsync(order);
            await _outboxRepository.AddAsync(outbox);
            await _unitOfWork.SaveChangesAsync();
        });

        return new CreateOrderResponse(orderId);
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, string correlationId, string? causationId)
    {
        return await UpdateStatusAsync(orderId, "Cancelled", "OrderCancelled", correlationId, causationId);
    }

    public async Task<bool> ConfirmOrderAsync(Guid orderId, string correlationId, string? causationId)
    {
        return await UpdateStatusAsync(orderId, "Confirmed", "OrderConfirmed", correlationId, causationId);
    }

    public async Task<bool> FulfillOrderAsync(Guid orderId, string correlationId, string? causationId)
    {
        return await UpdateStatusAsync(orderId, "Fulfilled", "OrderFulfilled", correlationId, causationId);
    }

    private async Task<bool> UpdateStatusAsync(
        Guid orderId,
        string nextStatus,
        string eventType,
        string correlationId,
        string? causationId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return false;
        }

        if (string.Equals(order.Status, nextStatus, StringComparison.Ordinal))
        {
            return true;
        }

        order.Status = nextStatus;
        var outbox = CreateOutboxEvent(eventType, orderId, correlationId, causationId, new { orderId });

        await _orderRepository.ExecuteInTransactionAsync(async () =>
        {
            await _outboxRepository.AddAsync(outbox);
            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }

    private static OutboxEvent CreateOutboxEvent<TPayload>(
        string eventType,
        Guid orderId,
        string correlationId,
        string? causationId,
        TPayload payload)
    {
        var eventId = Guid.NewGuid();
        var occurredAt = DateTime.UtcNow;
        var envelope = new EventEnvelope<TPayload>(
            eventId,
            eventType,
            "1",
            occurredAt,
            "order-service",
            correlationId,
            causationId,
            "Order",
            orderId.ToString(),
            payload,
            new Dictionary<string, string>());

        return new OutboxEvent
        {
            Id = eventId,
            EventType = eventType,
            Payload = JsonSerializer.Serialize(envelope, JsonOptions),
            OccurredAt = occurredAt
        };
    }
}
