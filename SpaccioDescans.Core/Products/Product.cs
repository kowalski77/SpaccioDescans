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

    public string Vendor { get; internal set; }

    public string Name { get; internal set; }

    public string Description { get; internal set; }

    public string Measures { get; internal set; }

    public Price NetPrice { get; internal set; }

    public Quantity Quantity { get; internal set; }

    public int Code { get; private set; }
}