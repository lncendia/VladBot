using VladBot.Core.Interfaces;

namespace VladBot.BLL;

public class Result<T> : IResult<T>
{
    public bool Succeeded { get; }
    public string? ErrorMessage { get; }
    public T? Value { get; }

    private Result(bool succeeded, string? errorMessage, T? value)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
        Value = value;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, null, value);
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T>(false, message, default);
    }
}