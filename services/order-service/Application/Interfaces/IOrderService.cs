using OrderService.Contracts;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request);
}
