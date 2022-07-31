using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Domain.Orders;

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
            OrderStatus = orderEditDto.OrderStatus,
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

    public static CreateOrderCommand AsCreateOrderCommand(this OrderViewModel order, string userName)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));
        }

        var customerInfo = new CustomerInfo(
            order.CustomerInfo.Name, order.CustomerInfo.Address,
            order.CustomerInfo.Nif, order.CustomerInfo.Phone);

        var orderDetailItemCollection = order.OrderDetail.Select(x => new OrderDetailItem(x.ProductId, x.StoreId, x.Quantity, x.Discount));

        var paymentDataCollection = new List<PaymentData>
        {
            new(PaymentMethod.CreditCard, order.CreditCardAmount),
            new(PaymentMethod.Cash, order.CashAmount),
            new(PaymentMethod.Financed, order.FinancedAmount)
        };

        return new CreateOrderCommand(userName, customerInfo, orderDetailItemCollection, paymentDataCollection);
    }

    public static EditCustomerInfoCommand AsEditCustomerInfoCommand(this OrderViewModel order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new EditCustomerInfoCommand(
            order.Id,
            order.CustomerInfo.Name,
            order.CustomerInfo.Address,
            order.CustomerInfo.Nif,
            order.CustomerInfo.Phone);
    }

    public static EditPaymentCommand AsEditPaymentCommand(this OrderViewModel order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new EditPaymentCommand(
            order.Id,
            order.CashAmount,
            order.CreditCardAmount,
            order.FinancedAmount);
    }
}
