#pragma warning disable 8618
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Stores;

public class Store : Entity, IAggregateRoot
{
    private readonly List<ProductStore> productStores = new();

    private Store() { }

    public Store(int id, string name, string owner, string nif, string email, string address)
    {
        this.Id = id;
        this.Owner = owner;
        this.Name = name;
        this.Nif = nif;
        this.Email = email;
        this.Address = address;
    }

    public long Id { get; internal set; }

    public string Name { get; internal set; }

    public string Owner { get; internal set; }

    public string Nif { get; internal set; }

    public string Email { get; internal set; }

    public string Address { get; internal set; }

    public IReadOnlyList<ProductStore> ProductStores => this.productStores;
}