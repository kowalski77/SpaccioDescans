using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class OrderViewModel
{
    public long Id { get; set; }

    [ValidateComplexType]
    public CustomerInfoViewModel CustomerInfo { get; set; } = new();

    public ICollection<OrderDetailViewModel> OrderDetail { get; set; } = new List<OrderDetailViewModel>();

    public decimal NetAmount { get; set; }

    public decimal TotalAmount => this.NetAmount + this.NetAmount * SpaccioConstants.Vat / 100;

    public decimal CreditCardAmount { get; set; }

    public decimal CashAmount { get; set; }

    public decimal FinancedAmount { get; set; }

    [Range(0, 100000, ErrorMessage = "El importe pendiente no puede ser negativo.")]
    public decimal PendingAmount => this.TotalAmount - this.CashAmount - this.CreditCardAmount - this.FinancedAmount;

    public OrderStatus OrderStatus { get; set; }

    public static explicit operator CreateOrderCommand(OrderViewModel orderViewModel)
    {
        ArgumentNullException.ThrowIfNull(orderViewModel);

        var customerInfo = new CustomerInfo(
            orderViewModel.CustomerInfo.Name, orderViewModel.CustomerInfo.Address,
            orderViewModel.CustomerInfo.Nif, orderViewModel.CustomerInfo.Phone);

        var orderDetailItemCollection = orderViewModel.OrderDetail.Select(x => new OrderDetailItem(x.ProductId, x.StoreId, x.Quantity, x.Discount));

        var paymentDataCollection = new List<PaymentData>
        {
            new(PaymentMethod.CreditCard, orderViewModel.CreditCardAmount),
            new(PaymentMethod.Cash, orderViewModel.CashAmount),
            new(PaymentMethod.Financed, orderViewModel.FinancedAmount)
        };

        return new CreateOrderCommand(customerInfo, orderDetailItemCollection, paymentDataCollection);
    }

    public static explicit operator EditCustomerInfoCommand(OrderViewModel orderViewModel)
    {
        ArgumentNullException.ThrowIfNull(orderViewModel);

        return new EditCustomerInfoCommand(
            orderViewModel.Id,
            orderViewModel.CustomerInfo.Name,
            orderViewModel.CustomerInfo.Address,
            orderViewModel.CustomerInfo.Nif,
            orderViewModel.CustomerInfo.Phone);
    }

    public static explicit operator EditPaymentCommand(OrderViewModel orderViewModel)
    {
        ArgumentNullException.ThrowIfNull(orderViewModel);

        return new EditPaymentCommand(
            orderViewModel.Id,
            orderViewModel.CashAmount,
            orderViewModel.CreditCardAmount,
            orderViewModel.FinancedAmount);
    }
}
