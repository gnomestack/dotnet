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

    public static Result<TValue, Error> OkNoError<TValue>(TValue value)
        where TValue : notnull
        => new(ResultState.Ok, value, default!);

    public static Result<TValue, Exception> OkNoException<TValue>(TValue value)
        where TValue : notnull
        => new(ResultState.Ok, value, default!);

    public static Result<Nil, TException> None<TException>(TException error)
        where TException : Exception
        => new(ResultState.Err, default!, error);

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

    public static Result<TValue, Exception> Try<TValue>(Func<TValue> func)
        where TValue : notnull
    {
        try
        {
            return func();
        }
        catch (Exception ex)
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

    public static Result<Nil, Exception> Try(Action action)
    {
        try
        {
            action();
            return default;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<TValue, TException>> TryAsync<TValue, TException>(Func<Task<TValue>> func)
        where TException : Exception
        where TValue : notnull
    {
        try
        {
            return await func();
        }
        catch (TException ex)
        {
            return ex;
        }
    }

    public static async Task<Result<TValue, TException>> TryAsync<TValue, TException>(
        Func<CancellationToken, Task<TValue>> func,
        CancellationToken cancellationToken)
        where TException : Exception
        where TValue : notnull
    {
        try
        {
            return await func(cancellationToken);
        }
        catch (TException ex)
        {
            return ex;
        }
    }

    public static async Task<Result<TValue, Exception>> TryAsync<TValue>(Func<Task<TValue>> func)
        where TValue : notnull
    {
        try
        {
            return await func();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<TValue, Exception>> TryAsync<TValue>(Func<CancellationToken, Task<TValue>> func, CancellationToken cancellationToken)
        where TValue : notnull
    {
        try
        {
            return await func(cancellationToken).NoCap();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}