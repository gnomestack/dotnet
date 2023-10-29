namespace GnomeStack.Extensions.Search.Abstractions;

public struct SearchResult<T>
{
    public IReadOnlyList<T> Results { get; set; }

    public int Hits { get; set; }
}