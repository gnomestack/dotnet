namespace GnomeStack.Functional;

/// <summary>
/// The core contract for a generic optional value.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IOptional<TValue> : IOptional,
    IEquatable<IOptional<TValue>>,
    IEquatable<TValue>
    where TValue : notnull
{
    /// <summary>
    /// Deconstructs the <see cref="IOptional{TValue}"/> into
    /// a <typeparamref name="TValue"/> and a <see cref="bool"/> that
    /// is <see langword="true"/> when there is a value and
    /// <see landword="false" /> when the value is none.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="some">A value indicating whether the value is some or none.</param>
    void Deconstruct(out TValue value, out bool some);

    /// <summary>
    /// Returns the underlying value if it is <c>Some</c>, otherwise, throws
    /// an exception.
    /// </summary>
    /// <returns>The value or throws.</returns>
    /// <exception cref="OptionException">
    /// Thrown when the value is <c>None</c>.
    /// </exception>
    TValue Unwrap();
}