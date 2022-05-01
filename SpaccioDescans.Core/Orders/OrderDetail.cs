#pragma warning disable 8618
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class OrderDetail : Entity
{
    private decimal subTotal;

    private OrderDetail() { }

    public OrderDetail(Product product, OrderQuantity quantity, Discount discount)
    {
        this.Product = product;
        this.Quantity = quantity;
        this.Discount = discount;
    }

    public int Id { get; private set; }

    public Product Product { get; private set; }

    public OrderQuantity Quantity { get; private set; }

    public Discount Discount { get; private set; }

    public decimal SubTotal
    {
        get => this.subTotal;
        private set => this.subTotal = this.Product.NetPrice.Value - (this.Product.NetPrice.Value / this.Discount.Value);
    }
}