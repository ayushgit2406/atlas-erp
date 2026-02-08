using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(OutboxEvent evt);
}
