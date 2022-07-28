using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Orders;

public class Discount : ValueObject
{
    private const string DiscountShouldBePositive = "Discount should be positive";

    private Discount(decimal value)
    {
        this.Value = value;
    }

    public decimal Value { get; private set; }

    public static Discount CreateInstance(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidOperationException(DiscountShouldBePositive);
        }

        return new Discount(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}