using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public class Quantity : ValueObject
{
    private const string QuantityShouldBePositive = "Quantity should be positive";

    private Quantity(int value)
    {
        this.Value = value;
    }

    public static Quantity CreateInstance(int value)
    {
        if (value < 0)
        {
            throw new InvalidOperationException(QuantityShouldBePositive);
        }

        return new Quantity(value);
    }

    public int Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}