using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _db;

    public OrderRepository(OrdersDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await Task.CompletedTask;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        return await _db.Orders.FindAsync(orderId);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        await operation();
        await tx.CommitAsync();
    }
}
