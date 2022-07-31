using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class OrderViewModel
{
    public long Id { get; set; }

    [ValidateComplexType]
    public CustomerInfoViewModel CustomerInfo { get; set; } = new();

    public ICollection<OrderDetailViewModel> OrderDetail { get; set; } = new List<OrderDetailViewModel>();

    public decimal NetAmount { get; set; }

    public decimal TotalAmount => this.NetAmount + (this.NetAmount * SpaccioConstants.Vat / 100);

    public decimal CreditCardAmount { get; set; }

    public decimal CashAmount { get; set; }

    public decimal FinancedAmount { get; set; }

    [Range(0, 100000, ErrorMessage = "El importe pendiente no puede ser negativo.")]
    public decimal PendingAmount => this.TotalAmount - this.CashAmount - this.CreditCardAmount - this.FinancedAmount;

    public OrderStatus OrderStatus { get; set; }
}
