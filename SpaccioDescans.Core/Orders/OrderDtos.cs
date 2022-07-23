namespace SpaccioDescans.Core.Orders;

public class OrderDto
{
    public long Id { get; init; }

    public string Customer { get; init; } = default!;

    public DateTime CreatedAt { get; init; }

    public string Store { get; init; } = default!;

    public OrderStatus OrderStatus { get; init; }

    public decimal Total { get; init; }
}

public class OrderEditDto
{
    public long Id { get; init; }

    public CustomerEditDto Customer { get; internal set; } = default!;
}

public class CustomerEditDto
{
    public string Name { get; init; } = default!;

    public string Address { get; init; } = default!;

    public string Nif { get; init; } = default!;

    public string Phone { get; init; } = default!;
}