using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly OrdersDbContext _db;

    public OutboxRepository(OrdersDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(OutboxEvent evt)
    {
        _db.OutboxEvents.Add(evt);
        await Task.CompletedTask;
    }
}
