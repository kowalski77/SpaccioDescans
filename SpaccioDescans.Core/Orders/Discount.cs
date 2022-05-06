using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class Discount : ValueObject
{
    private const string DiscountShouldBeGreaterThanZero = "Discount should be greater than zero";

    private Discount(decimal value)
    {
        this.Value = value;
    }

    public decimal Value { get; private set; }

    public static Discount CreateInstance(decimal value)
    {
        if (value <= 0)
        {
            throw new InvalidOperationException(DiscountShouldBeGreaterThanZero);
        }

        return new Discount(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}