namespace SpaccioDescans.Core.Orders;

public record OrderDto(long Id, DateTime CreatedAt, string Store, OrderStatus OrderStatus, decimal Total, decimal Pending);