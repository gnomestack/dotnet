using System.Text;

namespace GnomeStack.Data.Mssql.Management;

public static class MssqlValidate
{
    public static bool UserName(ReadOnlySpan<char> name)
    {
        name = name.Trim();
        if (name.IsEmpty || name.IsWhiteSpace())
        {
            return false;
        }

        if (name.Length > 128)
        {
            return false;
        }

        if (name[0] is '[')
        {
            if (name[^1] is not ']')
                return false;

            name = name.Slice(1, name.Length - 2);
        }

        if (!char.IsLetter(name[0]))
        {
            return false;
        }

        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c) || c is '_' or '\\' or '-' or '.' or '@')
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public static bool ElasticPoolName(ReadOnlySpan<char> name)
    {
        if (name.IsEmpty || name.IsWhiteSpace())
        {
            return false;
        }

        if (name.Length > 128)
        {
            return false;
        }

        if (!char.IsLetter(name[0]) && name[0] != '_')
        {
            return false;
        }

        foreach (var c in name)
        {
            if (!char.IsLetterOrDigit(c) && c is not '_' && c is not '-')
            {
                return false;
            }
        }

        return true;
    }

    public static bool PermissionName(ReadOnlySpan<char> name)
    {
        name = name.Trim();
        if (name.IsEmpty || name.IsWhiteSpace())
        {
            return false;
        }

        if (name.Length > 128)
        {
            return false;
        }

        if (!char.IsLetter(name[0]))
        {
            return false;
        }

        foreach (var c in name)
        {
            if (!char.IsLetterOrDigit(c) && c is not ' ')
            {
                return false;
            }
        }

        return true;
    }

    public static bool OperationName(ReadOnlySpan<char> name)
    {
        name = name.Trim();
        if (name.IsEmpty || name.IsWhiteSpace())
        {
            return false;
        }

        if (!char.IsLetter(name[0]))
        {
            return false;
        }

        foreach (var c in name)
        {
            if (!char.IsLetterOrDigit(c) && c is not '_' && c is not ' ')
            {
                return false;
            }
        }

        return true;
    }

    public static bool GrantIdentifier(ReadOnlySpan<char> identifier)
    {
        if (identifier.IsEmpty || identifier.IsWhiteSpace())
        {
            return false;
        }

        var index = identifier.IndexOf("::".AsSpan());
        if (index > -1)
        {
            var seg = identifier.Slice(0, index);
            foreach (var c in seg)
            {
                if (char.IsLetterOrDigit(c) || c is ' ')
                    continue;

                return false;
            }

            identifier = identifier.Slice(index + 2);
            return Identifier(identifier);
        }

        return Identifier(identifier);
    }

    public static bool Identifier(ReadOnlySpan<char> identifier)
    {
        identifier = identifier.Trim();
        if (identifier.IsEmpty || identifier.IsWhiteSpace())
        {
            return false;
        }

        int index = identifier.IndexOf('.');
        if (index > -1)
        {
            while (index > -1 && !identifier.IsEmpty)
            {
                var part = identifier.Slice(0, index);
                identifier = identifier.Slice(index + 1);
                if (part.IsEmpty)
                    return false;

                if (!Identifier(part))
                {
                    return false;
                }

                index = identifier.IndexOf('.');
                if (index == -1)
                {
                    return Identifier(identifier);
                }
            }
        }

        if (identifier[0] is '[')
        {
            if (identifier[^1] is not ']')
                return false;

            identifier = identifier.Slice(1, identifier.Length - 2);
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