using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class OrderQuantity : ValueObject
{
    private const string QuantityShouldBeGreaterThanZero = "Quantity should be greater than zero";

    private OrderQuantity(int value)
    {
        this.Value = value;
    }

    public int Value { get; private set; }

    public static OrderQuantity CreateInstance(int value)
    {
        if (value <= 0)
        {
            throw new InvalidOperationException(QuantityShouldBeGreaterThanZero);
        }

        return new OrderQuantity(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}