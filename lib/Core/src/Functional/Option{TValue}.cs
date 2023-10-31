using GnomeStack.Standard;

namespace GnomeStack.Functional;

/// <summary>
///   Represents an optional value that may or may not be present and provides
///   methods to deal with non present values to avoid throwing exceptions.
/// </summary>
/// <remarks>
///     <para>
///         This type is similar to <see cref="Nullable{TValue}" /> but
///         is not restricted to value types and is a monad that can be
///         chained methods to handle the presence or absence of a value.
///     </para>
///     <para>
///         The implementation is influenced by Rust rather than F#. It is
///         purposefully lightweight and does not provide a lot of the
///         functionality that F# that other implementation provides to allow
///         users to create their own extensions.
///     </para>
/// </remarks>
/// <typeparam name="TValue">The type of the value.</typeparam>
public readonly struct Option<TValue> :
    IEquatable<Option<TValue>>,
    IOptional<TValue>
    where TValue : notnull
{
    private readonly TValue value;

    private readonly OptionState state = OptionState.Some;

    internal Option(OptionState state, TValue value)
    {
        this.state = state;
        this.value = value;
    }

    /// <summary>
    ///  Gets a value indicating whether this instance has a value.
    /// </summary>
    public bool IsSome => this.state == OptionState.Some;

    /// <summary>
    /// Gets a value indicating whether this instance has no value.
    /// </summary>
    public bool IsNone => this.state == OptionState.None;

    /// <summary>
    /// Implicitly converts <typeparamref name="TValue"/> to <see cref="Option{TValue}"/>.
    /// </summary>
    /// <param name="value">The type of the value.</param>
    public static implicit operator Option<TValue>(TValue value)
        => new(OptionState.Some, value);

#pragma warning disable SA1313
    /// <summary>
    /// Implicitly converts <see cref="ValueTask"/> to <see cref="Option{TValue}"/>.
    /// </summary>
    /// <param name="_">The discarded value.</param>
    public static implicit operator Option<TValue>(ValueTask _)
        => Option.None<TValue>();

    /// <summary>
    /// Implicitly converts <see cref="Nil"/> to <see cref="Option{TValue}"/>.
    /// </summary>
    /// <param name="_">The discarded value.</param>
    public static implicit operator Option<TValue>(Nil _)
        => Option.None<TValue>();

    public static implicit operator Option<TValue>(DBNull _)
        => Option.None<TValue>();

    public static bool operator ==(Option<TValue> left, Option<TValue> right)
        => left.Equals(right);

    public static bool operator !=(Option<TValue> left, Option<TValue> right)
        => !left.Equals(right);

    public static bool operator ==(Option<TValue> left, TValue right)
        => left.Equals(right);

    public static bool operator !=(Option<TValue> left, TValue right)
        => !left.Equals(right);

    /// <summary>
    /// Deconstructs the <see cref="Option{TValue}"/> into
    /// a <typeparamref name="TValue"/> and a <see cref="bool"/> that
    /// is <see langword="true"/> when there is a value and
    /// <see landword="false" /> when the value is none.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="some">A value indicating whether the value is some or none.</param>
    public void Deconstruct(out TValue value, out bool some)
    {
        value = this.value!;
        some = this.IsSome;
    }

    /// <summary>
    /// Determines if the object is equal to the value.
    /// </summary>
    /// <param name="other">The other value.</param>
    /// <remarks>
    ///     <para>
    ///       If the option is none and the other value is <c>null</c>,
    ///       <c>Nil</c>, <c>DBNull</c>, or <c>ValueTuple</c>, then the result is
    ///       is true. Otherwise, the result is true if the inner
    ///       value is equal to the other value, otherwise, false.
    ///     </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> when the values are
    /// equal; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TValue? other)
    {
        if (Option.IsNone(other))
            return this.IsNone;

        return EqualityComparer<TValue>.Default.Equals(this.value, other);
    }

    /// <summary>
    /// Determines if the object is equal to the value.
    /// </summary>
    /// <param name="other">The other value.</param>
    /// <remarks>
    ///     <para>
    ///       If the option is none and the other value is <c>null</c>,
    ///       <c>Nil</c>, <c>DBNull</c>, or <c>ValueTuple</c>, then the result is
    ///       is true. Otherwise, the result is true if the inner
    ///       value is equal to the other value, otherwise, false.
    ///     </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> when the values are
    /// equal; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(IOptional<TValue>? other)
    {
        if (Option.IsNone(other))
            return this.IsNone;

        var (value, _) = other;
        return EqualityComparer<TValue>.Default.Equals(this.value, value);
    }

    /// <summary>
    /// Determines if the object is equal to the value.
    /// </summary>
    /// <param name="other">The other value.</param>
    /// <remarks>
    ///     <para>
    ///       Returns true if both options have the none value, otherwise
    ///       a equality comparison is performed to determine if both values
    ///       match.
    ///     </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> when the values are
    /// equal; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Option<TValue> other)
    {
        return this.state == other.state &&
               (this.IsNone ||
               (this.IsSome && EqualityComparer<TValue>.Default.Equals(this.value, other.value)));
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {
        if (obj is Option<TValue> other)
            return this.Equals(other);

        if (obj is TValue value)
            return this.Equals(value);

        return false;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(this.state, this.value);
    }

    /// <summary>
    /// Returns <c>None</c> if the option is <c>None</c>, otherwise,
    /// returns <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other option.</typeparam>
    /// <param name="other">The other value to return when this option has a value.</param>
    /// <returns>An option of <paramref name="other"/>.</returns>
    public Option<TOther> And<TOther>(Option<TOther> other)
        where TOther : notnull
    {
        if (this.IsNone)
            return Option.None<TOther>();

        return other;
    }

    /// <summary>
    /// Returns <c>None</c> if the option is <c>None</c>, otherwise,
    /// returns <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="other">The other value to return when this option has a value.</param>
    /// <returns>An option of <paramref name="other"/>.</returns>
    public Option<TOther> And<TOther>(TOther other)
        where TOther : notnull
    {
        if (this.IsNone)
            return Option.None<TOther>();

        return other;
    }

    /// <summary>
    /// Returns <c>None</c> if the option is <c>None</c>, otherwise,
    /// returns the result of lazily evaluated <paramref name="generateValue"/>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other option.</typeparam>
    /// <param name="generateValue">The other value to return when this option has a value.</param>
    /// <returns>An option of <paramref name="generateValue"/>.</returns>
    public Option<TOther> And<TOther>(Func<TOther> generateValue)
        where TOther : notnull
    {
        if (this.IsNone)
            return Option.None<TOther>();

        return generateValue();
    }

    /// <summary>
    /// Returns <c>None</c> if the option is <c>None</c>, otherwise,
    /// returns the result of lazily evaluated <paramref name="generateOption"/>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other option.</typeparam>
    /// <param name="generateOption">The other value to return when this option has a value.</param>
    /// <returns>An option of <paramref name="generateOption"/>.</returns>
    public Option<TOther> And<TOther>(Func<Option<TOther>> generateOption)
        where TOther : notnull
    {
        if (this.IsNone)
            return Option.None<TOther>();

        return generateOption();
    }

    /// <summary>
    /// Returns the value if the option is <c>Some</c>, otherwise,
    /// throws an exception.
    /// </summary>
    /// <param name="exception">The exception to throw.</param>
    /// <returns>The value or throws.</returns>
    /// <exception cref="Exception">
    /// The exception to throw when the option is <c>None</c>.
    /// </exception>
    public TValue Expect(Exception exception)
    {
        if (this.IsNone)
            throw exception;

        return this.value;
    }

    /// <summary>
    /// Returns the value if the option is <c>Some</c>, otherwise,
    /// generates and throws an exception.
    /// </summary>
    /// <param name="factory">The factory to create an exception.</param>
    /// <returns>The value or throws.</returns>
    /// <exception cref="Exception">
    /// The exception to throw when the option is <c>None</c>.
    /// </exception>
    public TValue Expect(Func<Exception> factory)
    {
        return this.IsNone ? throw factory() : this.value;
    }

    /// <summary>
    /// Returns the value if the option is <c>Some</c>, otherwise,
    /// throws a <see cref="OptionException"/>.
    /// </summary>
    /// <param name="message">The message for the exception.</param>
    /// <returns>The value or throws.</returns>
    /// <exception cref="OptionException">
    /// The exception to throw when the option is <c>None</c>.
    /// </exception>
    public TValue Expect(string message)
    {
        if (this.IsNone)
            throw new OptionException(message);

        return this.value!;
    }

    /// <summary>
    /// Returns the value if the option is <c>Some</c> and matches
    /// the predicate, otherwise returns <c>None</c>.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>An option that is <c>Some</c> and matches
    /// the predicate, otherwise returns <c>None</c>.</returns>
    public Option<TValue> Filter(Func<TValue, bool> predicate)
    {
        if (this.IsNone || !predicate(this.value))
            return Option.None<TValue>();

        return this;
    }

    /// <summary>
    /// Maps the value to a new value if the option is <c>Some</c>,
    /// otherwise, returns <c>None</c>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="map">The map function that projects the value.</param>
    /// <returns>The new option.</returns>
    public Option<TOther> Map<TOther>(Func<TValue, TOther> map)
        where TOther : notnull
        => this.IsNone ? Option.None<TOther>() : Option.Some(map(this.value));

    /// <summary>
    /// Maps the value to a new value if the option is <c>Some</c>,
    /// otherwise, returns an option with the default value.
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="map">The map function that projects the value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The new option.</returns>
    public Option<TOther> MapOr<TOther>(Func<TValue, TOther> map, TOther defaultValue)
        where TOther : notnull
        => this.IsNone ? defaultValue : map(this.value);

    /// <summary>
    /// Maps the value to a new value if the option is <c>Some</c>,
    /// otherwise, returns an option with the default value that is
    /// lazily created.
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="map">The map function that projects the value.</param>
    /// <param name="defaultValue">The factory that generates the other value.</param>
    /// <returns>The new option.</returns>
    public Option<TOther> MapOr<TOther>(Func<TValue, TOther> map, Func<TOther> defaultValue)
        where TOther : notnull
        => this.IsNone ? defaultValue() : map(this.value);

    /// <summary>
    /// Returns the option if it is <c>Some</c>, otherwise, returns
    /// the <paramref name="other"/> option.
    /// </summary>
    /// <param name="other">The other option.</param>
    /// <returns>An option.</returns>
    public Option<TValue> Or(Option<TValue> other)
        => this.IsNone ? other : this;

    /// <summary>
    /// Returns the option if it is <c>Some</c>, otherwise, returns
    /// the <paramref name="other"/> option.
    /// </summary>
    /// <param name="other">The other option.</param>
    /// <returns>An option.</returns>
    public Option<TValue> Or(TValue other)
        => this.IsNone ? other : this;

    /// <summary>
    /// Returns the option if it is <c>Some</c>, otherwise, returns
    /// the the lazily created option.
    /// </summary>
    /// <param name="factory">The factory to create the other value.</param>
    /// <returns>An option.</returns>
    public Option<TValue> Or(Func<TValue> factory)
        => this.IsNone ? factory() : this;

    /// <summary>
    /// Returns the option if it is <c>Some</c>, otherwise, returns
    /// the the lazily created option.
    /// </summary>
    /// <param name="factory">The factory to create the other value.</param>
    /// <returns>An option.</returns>
    public Option<TValue> Or(Func<Option<TValue>> factory)
        => this.IsNone ? factory() : this;

    /// <summary>
    /// Returns the underlying value if it is <c>Some</c>, otherwise, throws
    /// an exception.
    /// </summary>
    /// <returns>The value or throws.</returns>
    /// <exception cref="OptionException">
    /// Thrown when the value is <c>None</c>.
    /// </exception>
    public TValue Unwrap()
    {
        this.ThrowIfNone();
        return this.value;
    }

    /// <summary>
    /// Returns the underlying value if it is <c>Some</c>; otherwise,
    /// returns the <paramref name="defaultValue"/>.
    /// </summary>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The value or default value.</returns>
    public TValue UnwrapOr(TValue defaultValue)
        => this.IsNone ? defaultValue : this.value!;

    /// <summary>
    /// Returns the underlying value if it is <c>Some</c>; otherwise,
    /// returns the lazily created <paramref name="factory"/>.
    /// </summary>
    /// <param name="factory">The factory to create the default value.</param>
    /// <returns>The value or default value.</returns>
    public TValue UnwrapOr(Func<TValue> factory)
        => this.IsNone ? factory() : this.value!;

    /// <summary>
    /// Zips the option with another option and returns a tuple of the
    /// values if both options are <c>Some</c>; otherwise, returns <c>None</c>.
    /// </summary>
    /// <typeparam name="TOther">The type of the other option.</typeparam>
    /// <param name="other">The other option to zip.</param>
    /// <returns>The zipped tupple option.</returns>
    public Option<(TValue, TOther)> Zip<TOther>(Option<TOther> other)
        where TOther : notnull
    {
        if (this.IsNone || other.IsNone)
            return Option.None<(TValue, TOther)>();

        return Option.Some((this.value!, other.value!));
    }

    /// <summary>
    /// Throws an exception if the option is <c>None</c>.
    /// </summary>
    /// <returns>The option if an exception is not thrown.</returns>
    /// <exception cref="OptionException">
    /// Throws when the value is <c>None</c>.
    /// </exception>
    public Option<TValue> ThrowIfNone()
    {
        OptionException.ThrowIfNone(this);
        return this;
    }
}