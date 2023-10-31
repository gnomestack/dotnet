namespace GnomeStack.Text.Serialization;

/// <summary>
///  The string style to use when serializing a string. Folded and Plain may be
///  ignored and translated to the closest equivalent such as multi line
///  text.
/// </summary>
public enum StringStyle
{
    Default = 0,
    SingleQuoted = 1,
    DoubleQuoted = 2,
    Literal = 3,
    Plain = 4,
    Folded = 5,
}