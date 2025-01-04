namespace UrlShortener.Core;

public class Result<TValue>
{
    public bool Succeeded { get; }

    public TValue? Value { get; }

    public Error Error { get; }

    private Result(TValue value)
    {
        Succeeded = true;
        Value = value;
        Error = Error.None;
    }

    private Result(Error error)
    {
        Succeeded = false;
        Value = default;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<TResult> onFailure)
    {
        return Succeeded ? onSuccess(Value!) : onFailure();
    }
}