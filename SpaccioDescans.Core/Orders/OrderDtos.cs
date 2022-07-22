namespace SpaccioDescans.Core.Orders;

public class OrderDto
{
    public long Id { get; set; }

    public string Customer { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string Store { get; set; } = default!;

    public OrderStatus OrderStatus { get; set; }

    public decimal Total { get; set; }
}

public record OrderDetailDto(long Id, CustomerDto Customer);

public record CustomerDto(string Name, string? Address, string Nif, string? Phone);