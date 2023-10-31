namespace GnomeStack.Diagnostics;

internal static class Extensions
{
    public static HashSet<T> AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            set.Add(value);
        }

        return set;
    }
}