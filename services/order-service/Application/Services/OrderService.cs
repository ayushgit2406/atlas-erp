using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Contracts;
using OrderService.Infrastructure.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
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

        var payload = JsonSerializer.Serialize(new
        {
            orderId = order.Id,
            customerId = order.CustomerId,
            items = order.Items.Select(i => new { sku = i.Sku, qty = i.Qty, unitPrice = i.UnitPrice }),
            totalAmount = order.TotalAmount,
            currency = order.Currency
        });

        var outbox = new OutboxEvent
        {
            Id = Guid.NewGuid(),
            EventType = "OrderPlaced",
            Payload = payload,
            OccurredAt = DateTime.UtcNow
        };

        await _orderRepository.ExecuteInTransactionAsync(async () =>
        {
            await _orderRepository.AddAsync(order);
            await _outboxRepository.AddAsync(outbox);
            await _unitOfWork.SaveChangesAsync();
        });

        return new CreateOrderResponse(orderId);
    }
}
