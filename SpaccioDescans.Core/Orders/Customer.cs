#pragma warning disable 8618
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class Customer : Entity
{
    private Customer() { }

    public Customer(string name, string address, string city, string phone)
    {
        this.Name = name;
        this.Address = address;
        this.City = city;
        this.Phone = phone;
    }

    public long Id { get; private set; }

    public string Name { get; private set; }

    public string Address { get; private set; }

    public string City { get; private set; }

    public string Phone { get; private set; }
}