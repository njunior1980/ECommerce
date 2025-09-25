namespace ECommerce.Shared.Core.Base;

public class Result(bool isFailure, Error error)
{
    public bool IsFailure { get; } = isFailure;

    public Error Error { get; } = error;

    public string ErrorMessage => Error.Description;

    public static Result Success() => new(false, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) 
        => new(value, false, Error.None);

    public static Result Failure(Error error) 
        => new(true, error);

    public static Result<TValue> Failure<TValue>(Error error) 
        => new(default!, true, error);
}

public class Result<TValue>(TValue value, bool isFailure, Error error) : Result(isFailure, error)
{
    public TValue Value => IsFailure
        ? value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue)!;
}