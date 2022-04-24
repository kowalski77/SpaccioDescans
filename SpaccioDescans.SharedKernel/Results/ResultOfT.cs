#pragma warning disable CA2225
namespace SpaccioDescans.SharedKernel.Results;

public sealed class Result<T> : Result
{
    private readonly T value;

    public Result(T value)
    {
        this.value = value;
    }

    public Result(string error) : base(error)
    {
        this.value = default!;
    }

    public T Value
    {
        get
        {
            if (this.Failure)
            {
                throw new InvalidOperationException("The result object has no value.");
            }

            return this.value;
        }
    }

    public static implicit operator Result<T>(string errorResult)
    {
        return new Result<T>(errorResult);
    }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }
}
