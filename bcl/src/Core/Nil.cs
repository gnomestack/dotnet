#pragma warning disable SA1313 // Possible null reference argument.
using System.Diagnostics.CodeAnalysis;

using GnomeStack.Functional;

namespace GnomeStack;

/// <summary>
/// Nil is a singleton type that represents the absence of a value
/// and can be used to check for null, Nil, DBNull, or ValueTask.
/// </summary>
public readonly struct Nil : IEquatable<Nil>
{
    public Nil()
    {
    }

    public static Nil Value => default;

    public static implicit operator Nil(ValueTuple _) => default;

    public static implicit operator Nil(DBNull _) => default;

    public static bool IsNil([NotNullWhen(false)] object? obj)
    {
        if (obj is IOptional optional)
            return optional.IsNone;

        return obj is null or Nil or DBNull or ValueTuple;
    }

    public bool Equals(Nil other) => true;

    public override bool Equals(object? obj) => IsNil(obj);

    public override int GetHashCode() => 0;
}