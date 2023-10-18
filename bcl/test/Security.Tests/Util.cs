namespace Tests;

public static class Util
{
    private static string? s_location;

    public static string Location
    {
        get
        {
            if (s_location is not null)
                return s_location;

            var assembly = typeof(Util).Assembly;
            var codeBase = assembly.Location;
            if (codeBase.StartsWith("file:///"))
                codeBase = codeBase.Substring(8);

            s_location = Path.GetDirectoryName(codeBase);
            return s_location!;
        }
    }
}