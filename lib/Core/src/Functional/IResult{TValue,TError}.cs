namespace GnomeStack.Functional;

public interface IResult<TValue, TError> : IResult,
    IEquatable<IResult<TValue, TError>>,
    IEquatable<TValue>
    where TError : notnull
    where TValue : notnull
{
    void Deconstruct(out TValue value);

    void Deconstruct(out bool ok, out TValue value);

    void Deconstruct(out bool ok, out TValue value, out TError error);
}