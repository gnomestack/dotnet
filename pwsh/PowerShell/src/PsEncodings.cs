using System.Text;

namespace GnomeStack.PowerShell;

public static class PsEncodings
{
    private static UTF8Encoding s_utf8NoBom = new(false, true);

    public static Encoding GetEncoding(string? encodingName)
    {
        if (string.IsNullOrEmpty(encodingName))
        {
            return System.Text.Encoding.UTF8;
        }

        if (encodingName.Equals("utf8nobom", StringComparison.OrdinalIgnoreCase))
        {
            return s_utf8NoBom;
        }

        if (encodingName.Equals("utf8bom", StringComparison.OrdinalIgnoreCase))
        {
            return System.Text.Encoding.UTF8;
        }

        if (encodingName.Equals("bigendianutf32", StringComparison.OrdinalIgnoreCase))
        {
            return System.Text.Encoding.BigEndianUnicode;
        }

        try
        {
            return Encoding.GetEncoding(encodingName);
        }
        catch (ArgumentException)
        {
            return Encoding.UTF8;
        }
    }
}