using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class InboxRepository : IInboxRepository
{
    private readonly OrdersDbContext _db;

    public InboxRepository(OrdersDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(Guid eventId)
        => _db.InboxEvents.AnyAsync(e => e.Id == eventId);

    public Task AddAsync(InboxEvent evt)
    {
        _db.InboxEvents.Add(evt);
        return Task.CompletedTask;
    }
}
