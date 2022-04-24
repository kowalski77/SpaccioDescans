using SpaccioDescans.SharedKernel.DDD;
using SpaccioDescans.SharedKernel.Results;

namespace SpaccioDescans.Core.Products;

public sealed class Price : ValueObject
{
    private const string PriceShouldBeGreaterThanZero = "Price should be greater than 0";

    private Price(decimal value)
    {
        this.Value = value;
    }

    public static Result<Price> CreateInstance(decimal value)
    {
        return value <= 0 ? PriceShouldBeGreaterThanZero : Result.Ok(new Price(value));
    }

    public decimal Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}