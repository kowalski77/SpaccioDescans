#pragma warning disable 8618
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public sealed class Product : Entity, IAggregateRoot
{
    private Product() { }

    public Product(string name, string description, string measures, Price netPrice)
    {
        this.Name = name;
        this.Description = description;
        this.Measures = measures;
        this.NetPrice = netPrice;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Measures { get; private set; }

    public Price NetPrice { get; private set; }

    public void UpdatePrice(Price price)
    {
        ArgumentNullException.ThrowIfNull(price);

        this.NetPrice = price;
    }
}