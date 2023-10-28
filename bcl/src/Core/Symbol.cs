namespace GnomeStack;

public readonly struct Symbol : IEquatable<Symbol>, IEquatable<string>, IComparable<string>, IComparable<Symbol>
{
    private readonly string value;

    internal Symbol(string value)
    {
        this.value = value;
    }

    public int Length => this.value.Length;

    public ReadOnlySpan<char> Span => this.value.AsSpan();

    public static implicit operator string(Symbol symbol) => symbol.value;

    public static implicit operator Symbol(string value) => For(value);

    public static bool operator ==(Symbol left, Symbol right) => left.Equals(right);

    public static bool operator !=(Symbol left, Symbol right) => !left.Equals(right);

    public static bool operator >(Symbol left, Symbol right) => left.CompareTo(right) > 0;

    public static bool operator <(Symbol left, Symbol right) => left.CompareTo(right) < 0;

    public static bool operator >=(Symbol left, Symbol right) => left.CompareTo(right) >= 0;

    public static bool operator <=(Symbol left, Symbol right) => left.CompareTo(right) <= 0;

    public static Symbol For(string value)
        => SymbolStore.For(value);

    public static bool Has(string value)
        => SymbolStore.Has(value);

    public static IReadOnlyCollection<Symbol> List() => SymbolStore.List();

    public bool Equals(Symbol other) => this.value == other.value;

    public bool Equals(string? other) => this.value.Equals(other, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj)
    {
        if (obj is string value)
            return this.Equals(value);

        return obj is Symbol other && this.Equals(other);
    }

    public override int GetHashCode() => HashCode.Combine(this.value);

    public override string ToString() => this.value;

    public int CompareTo(string? other)
        => string.Compare(this.value, other, StringComparison.OrdinalIgnoreCase);

    public int CompareTo(Symbol other)
    {
        return this.CompareTo(other.value);
    }
}