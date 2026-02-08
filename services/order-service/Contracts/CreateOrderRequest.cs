namespace OrderService.Contracts;

public record CreateOrderRequest(
    string CustomerId,
    List<CreateOrderItem> Items,
    string Currency
);

public record CreateOrderItem(
    string Sku,
    int Qty,
    decimal UnitPrice
);
