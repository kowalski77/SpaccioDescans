using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Web.Pages.Orders.List;

public class OrderViewModel
{
    public long Id { get; set; }

    public string Customer { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Store { get; set; } = string.Empty;

    public string OrderStatus { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public static explicit operator OrderViewModel(OrderDto orderDto)
    {
        ArgumentNullException.ThrowIfNull(orderDto);

        return new OrderViewModel
        {
            Id = orderDto.Id,
            Customer = orderDto.Customer,
            CreatedAt = orderDto.CreatedAt,
            Store = orderDto.Store,
            OrderStatus = orderDto.OrderStatus.ToString(),
            Total = orderDto.Total
        };
    }

    public static OrderViewModel ToOrderViewModel(OrderViewModel left, OrderViewModel right)
    {
        throw new NotImplementedException();
    }
}
