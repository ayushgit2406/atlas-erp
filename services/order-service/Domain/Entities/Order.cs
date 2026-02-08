namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string Status { get; set; } = "Placed";
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "INR";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = [];
    }
}