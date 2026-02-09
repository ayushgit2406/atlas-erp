using OrderService.Contracts;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, string correlationId, string? causationId);
    Task<bool> CancelOrderAsync(Guid orderId, string correlationId, string? causationId);
    Task<bool> ConfirmOrderAsync(Guid orderId, string correlationId, string? causationId);
    Task<bool> FulfillOrderAsync(Guid orderId, string correlationId, string? causationId);
}
