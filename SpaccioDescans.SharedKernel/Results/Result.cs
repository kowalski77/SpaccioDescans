namespace SpaccioDescans.SharedKernel.Results;

public class Result
{
    protected Result(string error)
    {
        this.Success = false;
        this.Error = error;
    }

    protected Result()
    {
        this.Success = true;
    }

    public string? Error { get; }

    public bool Success { get; }

    public bool Failure => !this.Success;

    public static Result Init => Ok();

    public static Result Ok()
    {
        return new Result();
    }

    public static Result Fail(string error)
    {
        return new Result(error);
    }

    public static implicit operator Result(string error)
    {
        return new Result(error);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Fail<T>(string error)
    {
        return new Result<T>(error);
    }

    public Result ToResult()
    {
        throw new NotImplementedException();
    }
}
