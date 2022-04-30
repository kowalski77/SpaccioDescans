using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Stores;

public class Store : Entity, IAggregateRoot
{
    private List<ProductStore> productStores = new();

    public Store(int code, string name, string address)
    {
        this.Code = code;
        this.Name = name;
        this.Address = address;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public int Code { get; internal set; }

    public string Name { get; internal set; }

    public string Address { get; internal set; }

    public IReadOnlyList<ProductStore> ProductStores => this.productStores;
}