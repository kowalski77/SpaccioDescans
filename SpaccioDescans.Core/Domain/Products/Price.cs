using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Products;

public sealed class Price : ValueObject
{
    private const string PriceShouldBeGreaterThanZero = "Price should be greater than 0";

    private Price(decimal value)
    {
        this.Value = value;
    }

    public static Price CreateInstance(decimal value)
    {
        if (value <= 0)
        {
            throw new InvalidOperationException(PriceShouldBeGreaterThanZero);
        }

        return new Price(value);
    }

    public decimal Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}