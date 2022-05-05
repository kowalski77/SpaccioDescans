using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core;

namespace SpaccioDescans.Web.ViewModels;

public class OrderViewModel
{
    public CustomerInfoViewModel CustomerInfoViewModel { get; set; } = new();

    public ICollection<OrderDetailViewModel> OrderDetailViewModels { get; set; } = new List<OrderDetailViewModel>();

    public decimal NetAmount { get; set; }

    public decimal TotalAmount => this.NetAmount + (this.NetAmount * SpaccioConstants.Vat / 100);

    public decimal CreditCardAmount { get; set; }

    public decimal CashAmount { get; set; }

    public decimal FinancedAmount { get; set; }

    public decimal PendingAmount => this.TotalAmount - this.CashAmount - this.CreditCardAmount - this.FinancedAmount;
}

public class CustomerInfoViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}

public class OrderDetailViewModel
{
    public int ProductCode { get; set; }

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Discount { get; set; }

    public decimal Total => (this.Price * this.Quantity) - (this.Price * this.Quantity) * (this.Discount / 100);
}