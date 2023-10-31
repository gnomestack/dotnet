namespace GnomeStack.Extensions.Logging;

// this is a separate class because of the way loading static fields work.
public static class RootCategory
{
    public static string Name { get; set; } = "App";
}