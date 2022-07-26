using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public static class OrderViewModelMapper
{
    public static OrderViewModel AsOrderViewModel(this OrderEditDto orderEditDto)
    {
        ArgumentNullException.ThrowIfNull(orderEditDto);

        return new OrderViewModel
        {
            Id = orderEditDto.Id,
            NetAmount = orderEditDto.SubTotal,
            CustomerInfo = new CustomerInfoViewModel
            {
                Name = orderEditDto.Customer.Name,
                Address = orderEditDto.Customer.Address,
                Nif = orderEditDto.Customer.Nif,
                Phone = orderEditDto.Customer.Phone
            },
            OrderDetail = orderEditDto.OrderDetails.Select(x => new OrderDetailViewModel
            {
                ProductId = x.ProductId,
                Name = x.ProductName,
                Quantity = x.Quantity,
                Discount = x.Discount,
                Price = x.NetPrice
            }).ToList(),
            CashAmount = orderEditDto.Payments.First(x => x.PaymentMethod == PaymentMethod.Cash).Amount,
            CreditCardAmount = orderEditDto.Payments.First(x => x.PaymentMethod == PaymentMethod.CreditCard).Amount,
            FinancedAmount = orderEditDto.Payments.First(x => x.PaymentMethod == PaymentMethod.Financed).Amount
        };
    }
}
