using System.Diagnostics;

namespace GnomeStack.Identity.Actions;

internal static class Otel
{
    public static readonly ActivitySource ActivitySource = new("GnomeStack.Identity", "1.0.0");
}