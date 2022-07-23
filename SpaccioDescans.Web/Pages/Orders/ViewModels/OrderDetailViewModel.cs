namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class OrderDetailViewModel
{
    public long ProductId { get; set; }

    public long StoreId { get; set; }

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Discount { get; set; }

    public decimal Total => this.Price * this.Quantity - this.Price * this.Quantity * (this.Discount / 100);
}