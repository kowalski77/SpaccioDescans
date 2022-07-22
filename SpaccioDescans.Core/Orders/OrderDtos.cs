namespace SpaccioDescans.Core.Orders;

public record OrderDto(long Id, string Customer, DateTime CreatedAt, string Store, OrderStatus OrderStatus, decimal Total);

public record OrderDetailDto(long Id, CustomerDto Customer);

public record CustomerDto(string Name, string? Address, string Nif, string? Phone);