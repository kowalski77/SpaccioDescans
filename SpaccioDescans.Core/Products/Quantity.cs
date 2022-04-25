using SpaccioDescans.SharedKernel.DDD;
using SpaccioDescans.SharedKernel.Results;

namespace SpaccioDescans.Core.Products;

public class Quantity : ValueObject
{
    private const string QuantityShouldBeGreaterThanZero = "Quantity should be greater than 0";

    private Quantity(int value)
    {
        this.Value = value;
    }

    public static Result<Quantity> CreateInstance(int value)
    {
        return value <= 0 ? QuantityShouldBeGreaterThanZero : Result.Ok(new Quantity(value));
    }

    public int Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}