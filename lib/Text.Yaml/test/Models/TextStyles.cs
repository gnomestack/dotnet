using GnomeStack.Text.Serialization;

namespace Models;

public class TextStyles
{
    [Serialization("single", StringStyle = StringStyle.SingleQuoted)]
    public string? SingleQuoted { get; set; } = "single";

    [Serialization("double", StringStyle = StringStyle.DoubleQuoted)]
    public string? DoubleQuoted { get; set; } = "double";

    [Serialization("literal", StringStyle = StringStyle.Literal)]
    public string? Literal { get; set; } = "literal";

    [Serialization("plain", StringStyle = StringStyle.Plain)]
    public string? Plain { get; set; } = "line1\nline2";

    [Serialization("folded", StringStyle = StringStyle.Folded)]
    public string Folded { get; set; } = "line1\nline2";
}