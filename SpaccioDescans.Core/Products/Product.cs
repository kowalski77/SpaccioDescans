#pragma warning disable 8618
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public sealed class Product : Entity, IAggregateRoot
{
    private Product() { }

    public Product(Guid id,  string vendor, string name, string description, string measures, Price netPrice, Quantity quantity)
    {
        this.Id = id;
        this.Vendor = vendor;
        this.Name = name;
        this.Description = description;
        this.Measures = measures;
        this.NetPrice = netPrice ?? throw new ArgumentNullException(nameof(netPrice));
        this.Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Vendor { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Measures { get; private set; }

    public Price NetPrice { get; private set; }

    public Quantity Quantity { get; private set; }

    public int Code { get; private set; }

    public void UpdatePrice(Price price)
    {
        ArgumentNullException.ThrowIfNull(price);

        this.NetPrice = price;
    }
}