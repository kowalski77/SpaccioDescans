using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class OrderViewModel
{
    [ValidateComplexType]
    public CustomerInfoViewModel CustomerInfoViewModel { get; set; } = new();

    public ICollection<OrderDetailViewModel> OrderDetailViewModels { get; } = new List<OrderDetailViewModel>();

    public decimal NetAmount { get; set; }

    public decimal TotalAmount => this.NetAmount + this.NetAmount * SpaccioConstants.Vat / 100;

    public decimal CreditCardAmount { get; set; }

    public decimal CashAmount { get; set; }

    public decimal FinancedAmount { get; set; }

    [Range(0, 100000, ErrorMessage = "El importe pendiente no puede ser negativo.")]
    public decimal PendingAmount => this.TotalAmount - this.CashAmount - this.CreditCardAmount - this.FinancedAmount;

    public static explicit operator CreateOrderCommand(OrderViewModel orderViewModel)
    {
        ArgumentNullException.ThrowIfNull(orderViewModel);

        var customerInfo = new CustomerInfo(
            orderViewModel.CustomerInfoViewModel.Name, orderViewModel.CustomerInfoViewModel.Address,
            orderViewModel.CustomerInfoViewModel.Nif, orderViewModel.CustomerInfoViewModel.Phone);

        var orderDetailItemCollection = orderViewModel.OrderDetailViewModels.Select(x => new OrderDetailItem(x.ProductId, x.StoreId, x.Quantity, x.Discount));

        var paymentDataCollection = new List<PaymentData>
        {
            new(PaymentMethod.CreditCard, orderViewModel.CreditCardAmount),
            new(PaymentMethod.Cash, orderViewModel.CashAmount),
            new(PaymentMethod.Financed, orderViewModel.FinancedAmount)
        };

        return new CreateOrderCommand(customerInfo, orderDetailItemCollection, paymentDataCollection);
    }
}
