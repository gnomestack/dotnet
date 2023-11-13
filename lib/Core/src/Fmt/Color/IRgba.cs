namespace GnomeStack.Fmt.Colors;

public interface IRgba : IEquatable<IRgba>
{
    byte R { get; }

    byte G { get; }

    byte B { get; }

    Alpha A { get; }

    void Deconstruct(out byte r, out byte g, out byte b, out Alpha a);
}