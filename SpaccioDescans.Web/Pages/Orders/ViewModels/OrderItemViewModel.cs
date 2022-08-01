using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class OrderItemViewModel
{
    public long Id { get; set; }

    public string Customer { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Store { get; set; } = string.Empty;

    public string OrderStatus { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public static explicit operator OrderItemViewModel(OrderSummaryDto orderDto)
    {
        ArgumentNullException.ThrowIfNull(orderDto);

        return new OrderItemViewModel
        {
            Id = orderDto.Id,
            Customer = orderDto.Customer,
            CreatedAt = orderDto.CreatedAt,
            Store = orderDto.Store,
            OrderStatus = orderDto.OrderStatus.ToString(),
            Total = orderDto.Total
        };
    }

    public static OrderItemViewModel ToOrderViewModel(OrderItemViewModel left, OrderItemViewModel right)
    {
        throw new NotImplementedException();
    }
}
