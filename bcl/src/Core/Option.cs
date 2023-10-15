using System.Diagnostics.CodeAnalysis;

namespace GnomeStack;

public enum OptionState
{
    None = 0,
    Some = 1,
}

public static class Option
{
    private static readonly Option<ValueTuple> s_none = new();

    public static Option<ValueTuple> None()
        => s_none;

    public static Option<T> None<T>()
        => new(OptionState.None);

    public static Option<TValue> Some<TValue>(TValue value)
        => new(value);
}

public sealed class Option<TValue> : IEquatable<Option<TValue>>, IEquatable<TValue>
{
    private readonly TValue? value;

    private readonly OptionState state = OptionState.Some;

    public Option()
        : this(OptionState.Some)
    {
    }

    internal Option(OptionState state)
    {
        this.value = default;
        if (state == OptionState.None)
        {
            this.state = OptionState.None;
            return;
        }

        this.value = default;
        if (this.value is null or ValueTuple or DBNull)
            this.state = OptionState.None;
    }

    internal Option(TValue value)
    {
        this.value = value;
        if (this.value is null or ValueTuple or DBNull)
            this.state = OptionState.None;
    }

    public bool IsSome => this.state == OptionState.Some;

    public bool IsNone => this.state == OptionState.None;

    public static implicit operator Option<TValue>(TValue value)
        => new Option<TValue>(value);

    public static implicit operator TValue(Option<TValue> option)
    {
        return option.value!;
    }

    public static Option<TValue> Some(TValue value)
    {
        return new Option<TValue>(value);
    }

    public static Option<TValue> None()
    {
        return new Option<TValue>(OptionState.None);
    }

    public void Deconstruct(out TValue? value)
    {
        value = this.value!;
    }

    public bool Equals(TValue? other)
    {
        if (ReferenceEquals(this.value, other))
            return true;

        return object.Equals(this.value, other);
    }

    public bool Equals(Option<TValue>? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        return this.state == other.state &&
               (this.IsNone ||
               (this.IsSome && this.value!.Equals(other.value)));
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

    public TValue Expect(string message)
    {
        if (this.IsNone)
            throw new InvalidOperationException(message);

        return this.value!;
    }

    public TValue Expect(string message, Exception inner)
    {
        if (this.IsNone)
            throw new InvalidOperationException(message, inner);

        return this.value!;
    }

    public TValue Expect(string message, Func<Exception> inner)
    {
        if (this.IsNone)
            throw new InvalidOperationException(message, inner());

        return this.value!;
    }

    public Option<TValue> Filter(Func<TValue, bool> predicate)
    {
        if (this.IsNone || !predicate(this.value!))
            return new Option<TValue>();

        return this;
    }

    public Option<TOther> Map<TOther>(Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>();

        return new Option<TOther>(map(this.value!));
    }

    public Option<TOther> MapOr<TOther>(TOther defaultValue, Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>(defaultValue);

        return new Option<TOther>(map(this.value!));
    }

    public Option<TOther> MapOrElse<TOther>(Func<TOther> defaultValue, Func<TValue, TOther> map)
    {
        if (this.IsNone)
            return new Option<TOther>(defaultValue());

        return new Option<TOther>(map(this.value!));
    }

    public Option<TValue> Or(TValue other)
        => this.IsNone ? new Option<TValue>(other) : this;

    public Option<TValue> OrElse(Func<TValue> factory)
        => this.IsNone ? new Option<TValue>(factory()) : this;

    public TValue Unwrap()
        => this.IsNone ? this.value! : throw new InvalidOperationException("Option is None");

    public TValue UnwrapOr(TValue defaultValue)
        => this.IsNone ? defaultValue : this.value!;

    public TValue UnwrapOrElse(Func<TValue> defaultValue)
        => this.IsNone ? defaultValue() : this.value!;

    public Option<(TValue, TOther)> Zip<TOther>(Option<TOther> other)
    {
        if (this.IsNone || other.IsNone)
            return new Option<(TValue, TOther)>();

        return new Option<(TValue, TOther)>((this.value!, other.value!));
    }
}