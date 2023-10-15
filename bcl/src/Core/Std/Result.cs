namespace GnomeStack.Functional;

public static class Result
{
    public static Result<TValue, TError> Ok<TValue, TError>(TValue value)
        where TError : notnull
        where TValue : notnull
        => new(ResultState.Ok, value, default!);

    public static async Task<Result<TValue, TError>> OkAsync<TValue, TError>(Task<TValue> task)
        where TError : notnull
        where TValue : notnull
        => Ok<TValue, TError>(await task);

    public static Result<TValue, Error> NoError<TValue>(TValue value)
        where TValue : notnull
        => new(ResultState.Ok, value, default!);

    public static Result<TValue, Exception> NoException<TValue>(TValue value)
        where TValue : notnull
        => new(ResultState.Ok, value, default!);

    public static Result<TValue, TError> Error<TValue, TError>(TError error)
        where TError : notnull
        where TValue : notnull
        => new(ResultState.Err, default!, error);

    public static Result<TValue, Error> Error<TValue>(Error error)
        where TValue : notnull
        => new(ResultState.Err, default!, error);

    public static Result<TValue, Exception> Error<TValue>(Exception error)
        where TValue : notnull
        => new(ResultState.Err, default!, error);

    public static Result<TValue, Error> TryError<TValue>(Func<TValue> func)
        where TValue : notnull
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            return GnomeStack.Functional.Error.Convert(ex);
        }
    }

    public static Result<TValue, TException> Try<TValue, TException>(Func<TValue> func)
        where TException : Exception
        where TValue : notnull
    {
        try
        {
            return func();
        }
        catch (TException ex)
        {
            return ex;
        }
    }

    public static Result<Nil, TException> Try<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
            return default;
        }
        catch (TException ex)
        {
            return ex;
        }
    }
}