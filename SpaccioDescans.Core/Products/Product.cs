#pragma warning disable 8618
using SpaccioDescans.Core.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public sealed class Product : Entity, IAggregateRoot
{
    private readonly List<ProductStore> productStores = new();

    private Product() { }

    public Product(string vendor, string name, string description, string measures, Price netPrice)
    {
        this.Vendor = vendor;
        this.Name = name;
        this.Description = description;
        this.Measures = measures;
        this.NetPrice = netPrice ?? throw new ArgumentNullException(nameof(netPrice));
    }

    public long Id { get; private set; }

    public string Vendor { get; internal set; }

    public string Name { get; internal set; }

    public string Description { get; internal set; }

    public string Measures { get; internal set; }

    public Price NetPrice { get; internal set; }

    public void AddToStore(Store store, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(quantity);

        this.productStores.Add(new ProductStore
        {
            Product = this,
            Store = store,
            Quantity = quantity
        });
    }

    public void EditInStore(Store store, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(quantity);

        var productStore = this.productStores.First(x => x.Store.Id == store.Id);
        productStore.Quantity = quantity;
    }

    public IReadOnlyList<ProductStore> ProductStores => this.productStores;
}