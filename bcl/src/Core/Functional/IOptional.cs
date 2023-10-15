namespace GnomeStack.Functional;

/// <summary>
/// The core contract for an optional value.
/// </summary>
public interface IOptional
{
    bool IsSome { get; }

    bool IsNone { get; }
}