using System.Text;

namespace GnomeStack.Data.SqlServer.Management;

public static class Utils
{
    public static bool IsValidIdentifer(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return false;
        }

        if (identifier.Length > 128)
        {
            return false;
        }

        if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
        {
            return false;
        }

        foreach (var c in identifier)
        {
            if (!char.IsLetterOrDigit(c) && c is not '_')
            {
                return false;
            }
        }

        return true;
    }
}