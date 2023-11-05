namespace GnomeStack.Fmt.Colors;

public interface IRgb : IEquatable<IRgb>
{
    byte R { get; }

    byte G { get; }

    byte B { get; }

    void Deconstruct(out byte r, out byte g, out byte b);
}