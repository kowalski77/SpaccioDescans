#pragma warning disable 8618
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class Customer : Entity
{
    private Customer() { }

    public Customer(string name, string address, string nif, string phone)
    {
        this.Name = name;
        this.Address = address;
        this.Nif = nif;
        this.Phone = phone;
    }

    public long Id { get; private set; }

    public string Name { get; private set; }

    public string? Address { get; private set; }

    public string Nif { get; private set; }

    public string? Phone { get; private set; }
}