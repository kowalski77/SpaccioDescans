#pragma warning disable 8618
using SpaccioDescans.Core.Orders;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Stores;

public class Store : Entity, IAggregateRoot
{
    private readonly List<ProductStore> productStores = new();
    private readonly List<Order> orders = new();

    private Store() { }

    public Store(int id, string name, string address)
    {
        this.Id = id;
        this.Name = name;
        this.Address = address;
    }

    public long Id { get; internal set; }

    public string Name { get; internal set; }

    public string Address { get; internal set; }

    public IReadOnlyList<ProductStore> ProductStores => this.productStores;

    public IReadOnlyList<Order> Orders => this.orders;
}