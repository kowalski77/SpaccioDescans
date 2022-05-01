using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public class Discount : ValueObject
{
    private const string DiscountShouldBeGreaterThanZero = "Discount should be greater than zero";

    private Discount(int value)
    {
        this.Value = value;
    }

    public int Value { get; private set; }

    public static Discount CreateInstance(int value)
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