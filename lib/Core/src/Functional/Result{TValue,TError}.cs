using System.Diagnostics.CodeAnalysis;

using GnomeStack.Standard;

namespace GnomeStack.Functional;

[SuppressMessage("ReSharper", "ParameterHidesMember")]
public readonly struct Result<TValue, TError> : IResult<TValue, TError>
    where TError : notnull
    where TValue : notnull
{
    private readonly ResultState state;

    private readonly TValue value;

    private readonly TError error;

    internal Result(ResultState state, TValue value, TError error)
    {
        this.state = state;
        this.value = value;
        this.error = error;
    }

    public bool IsOk => this.state == ResultState.Ok;

    public bool IsError => this.state == ResultState.Err;

    public static implicit operator Task<Result<TValue, TError>>(Result<TValue, TError> result)
        => Task.FromResult(result);

    public static implicit operator Result<TValue, TError>(TValue value)
        => Result.Ok<TValue, TError>(value);

    public static implicit operator Result<TValue, TError>(Option<TValue> value)
    {
        var valueName = typeof(TValue).FullName;
        var errorName = typeof(TError).FullName;
        var inner = value.Expect(
            $"Implicit conversion from Option<{valueName}> to Result<{valueName}," +
            $" {errorName}> failed because the option was None.");

        return Result.Ok<TValue, TError>(inner);
    }

    public static implicit operator Result<TValue, TError>(TError error)
        => Result.Error<TValue, TError>(error);

    public static implicit operator Result<TValue, TError>(Option<TError> error)
    {
        var valueName = typeof(TValue).FullName;
        var errorName = typeof(TError).FullName;
        var inner = error.Expect(
            $"Implicit conversion from Option<{errorName}> to Result<{valueName}," +
            $" {errorName}> failed because the option was None.");

        return Result.Error<TValue, TError>(inner);
    }

    public static Result<TValue, TError> Ok(TValue value)
        => new(ResultState.Ok, value, default!);

    public static Result<TValue, TError> Err(TError error)
        => new(ResultState.Err, default!, error);

    public Result<TOtherValue, TError> And<TOtherValue>(Result<TOtherValue, TError> other)
        where TOtherValue : notnull
    {
        if (this.IsError)
            return this.error;

        return other;
    }

    public Result<TOtherValue, TError> And<TOtherValue>(TOtherValue other)
        where TOtherValue : notnull
    {
        if (this.IsError)
            return this.error;

        return other;
    }

    public Result<TOtherValue, TError> And<TOtherValue>(Func<TOtherValue> other)
        where TOtherValue : notnull
    {
        if (this.IsError)
            return this.error;

        return other();
    }

    public Result<TOtherValue, TError> And<TOtherValue>(Func<Result<TOtherValue, TError>> other)
        where TOtherValue : notnull
    {
        if (this.IsError)
            return this.error;

        return other();
    }

    public void Deconstruct(out TValue value)
    {
        value = this.ThrowIfError().Unwrap();
    }

    public void Deconstruct(out bool ok, out TValue value)
    {
        value = this.value!;
        ok = this.IsOk;
    }

    public void Deconstruct(out bool ok, out TValue value, out TError error)
    {
        value = this.value!;
        ok = this.IsOk;
        error = this.error;
    }

    public Option<TError> Error()
        => this.IsError ? this.error : Option.None<TError>();

    public bool Equals(IResult<TValue, TError>? other)
    {
        if (other is null)
            return false;

        if (this.IsOk != other.IsOk || this.IsError != other.IsError)
            return false;

        var (ok, v, e) = other;

        if (this.IsOk && ok)
            return this.value!.Equals(v);

        if (this.IsError)
            return this.error.Equals(e);

        return false;
    }

    public bool Equals(TValue? other)
    {
        if (this.IsOk)
            return this.value!.Equals(other);

        return false;
    }

    public TValue Expect(string message)
    {
        if (this.IsError)
            throw new ResultException(message);

        return this.value;
    }

    public TValue Expect(Func<string> message)
    {
        if (this.IsError)
            throw new ResultException(message());

        return this.value;
    }

    public TValue Expect(Exception exception)
    {
        if (this.IsError)
            throw exception;

        return this.value;
    }

    public TError ExpectError(string message)
    {
        if (!this.IsError)
            throw new ResultException(message);

        return this.error;
    }

    public TError ExpectError(Func<string> message)
    {
        if (!this.IsError)
            throw new ResultException(message());

        return this.error;
    }

    public TError ExpectError(Exception exception)
    {
        if (!this.IsError)
            throw exception;

        return this.error;
    }

    public bool IsOkAnd(Func<TValue, bool> predicate)
        => this.IsOk && predicate(this.value!);

    public bool IsErrorAnd(Func<TError, bool> predicate)
        => this.IsError && predicate(this.error);

    public Option<TValue> Ok()
        => this.IsOk ? this.value : Option.None<TValue>();

    public Result<TOther, TError> Map<TOther>(Func<TValue, TOther> map)
        where TOther : notnull
    {
        if (this.IsError)
            return this.error;

        return map(this.value!);
    }

    public Result<TValue, TOtherError> MapError<TOtherError>(Func<TError, TOtherError> map)
        where TOtherError : notnull
    {
        if (this.IsError)
            return map(this.error);

        return this.value!;
    }

    public Result<TValue, TError> ThrowIfError()
    {
        ResultException.ThrowIfError(this);

        return this;
    }

    public TValue Unwrap()
    {
        ResultException.ThrowIfError(this);
        return this.value!;
    }

    public TValue UnwrapOr(TValue defaultValue)
    {
        if (this.IsError)
            return defaultValue;

        return this.value!;
    }

    public TValue UnwrapOr(Func<TValue> defaultValue)
    {
        if (this.IsError)
            return defaultValue();

        return this.value!;
    }

    public TError UnwrapError()
    {
        if (!this.IsError)
            throw new InvalidOperationException($"UnwrapError is invalid when result has value: {this.value}.");

        return this.error;
    }

    public TError UnwrapErrorOr(TError defaultError)
    {
        if (!this.IsError)
            return defaultError;

        return this.error;
    }

    public TError UnwrapErrorOr(Func<TError> defaultError)
    {
        if (!this.IsError)
            return defaultError();

        return this.error;
    }
}