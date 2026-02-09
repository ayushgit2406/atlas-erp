using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid orderId);
    Task ExecuteInTransactionAsync(Func<Task> operation);
}
