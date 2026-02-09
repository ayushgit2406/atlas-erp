using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Interfaces;

public interface IInboxRepository
{
    Task<bool> ExistsAsync(Guid eventId);
    Task AddAsync(InboxEvent evt);
}
