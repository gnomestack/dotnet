using System.Collections.Concurrent;

namespace GnomeStack;

internal static class SymbolStore
{
    private static readonly ConcurrentDictionary<string, Symbol> s_symbols = new(StringComparer.OrdinalIgnoreCase);

    public static Symbol For(string value)
    {
        value = string.Intern(value);
        return s_symbols.GetOrAdd(value, (k) => new Symbol(k));
    }

    public static bool Has(string value) => s_symbols.ContainsKey(value);

    public static IReadOnlyCollection<Symbol> List() => s_symbols.Values.ToList();
}