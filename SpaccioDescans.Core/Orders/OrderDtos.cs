namespace SpaccioDescans.Core.Orders;

public record OrderDto(long Id, string Customer, DateTime CreatedAt, string Store, OrderStatus OrderStatus, decimal Total);