using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OutboxEvent> OutboxEvents => Set<OutboxEvent>();
    public DbSet<InboxEvent> InboxEvents => Set<InboxEvent>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            b.ToTable("orders");
            b.HasKey(o => o.Id);
            b.Property(o => o.CustomerId).IsRequired();
            b.Property(o => o.Status).IsRequired();
            b.Property(o => o.TotalAmount).HasColumnType("numeric(18,2)");
            b.Property(o => o.Currency).HasMaxLength(3);
            b.Property(o => o.CreatedAt);

            b.HasMany(o => o.Items)
             .WithOne(i => i.Order)
             .HasForeignKey(i => i.OrderId);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.ToTable("order_items");
            b.HasKey(i => i.Id);
            b.Property(i => i.Sku).IsRequired();
            b.Property(i => i.Qty).IsRequired();
            b.Property(i => i.UnitPrice).HasColumnType("numeric(18,2)");
        });

        modelBuilder.Entity<OutboxEvent>(b =>
        {
            b.ToTable("outbox_events");
            b.HasKey(e => e.Id);
            b.Property(e => e.EventType).IsRequired();
            b.Property(e => e.Payload).IsRequired();
            b.Property(e => e.OccurredAt).IsRequired();
            b.Property(e => e.RetryCount).HasDefaultValue(0);
        });

        modelBuilder.Entity<InboxEvent>(b =>
{
    b.ToTable("inbox_events");
    b.HasKey(e => e.Id);
    b.Property(e => e.EventType).IsRequired();
    b.Property(e => e.ProcessedAt).IsRequired();
});

    }
}
