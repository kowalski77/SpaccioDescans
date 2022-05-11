#pragma warning disable 8618
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class OrderDetail : Entity
{
    private OrderDetail() { }

    public OrderDetail(Product product, OrderQuantity quantity, Discount discount)
    {
        this.Product = product;
        this.Quantity = quantity;
        this.Discount = discount;
    }

    public long Id { get; private set; }

    public Product Product { get; private set; }

    public OrderQuantity Quantity { get; private set; }

    public Order Order { get; private set; }

    public Discount Discount { get; private set; }

    public decimal SubTotal => this.Discount.Value > 0 ? 
        this.Product.NetPrice.Value - (this.Product.NetPrice.Value / this.Discount.Value) : 
        this.Product.NetPrice.Value;
}