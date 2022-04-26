using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public class Quantity : ValueObject
{
    private const string QuantityShouldBeGreaterThanZero = "Quantity should be greater than 0";

    private Quantity(int value)
    {
        this.Value = value;
    }

    public static Quantity CreateInstance(int value)
    {
        if (value <= 0)
        {
            throw new InvalidOperationException(QuantityShouldBeGreaterThanZero);
        }

        return new Quantity(value);
    }

    public int Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}