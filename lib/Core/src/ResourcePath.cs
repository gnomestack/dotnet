using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack;

[SuppressMessage("ReSharper", "ParameterHidesMember")]
public class ResourcePath : IEnumerable<string>
{
    private readonly char[] path;

    private char delimiter = '/';

    public ResourcePath(ReadOnlySpan<char> path)
    {
        this.path = path.ToArray();
    }

    public ResourcePath(ReadOnlySpan<char> path, char delimiter)
    {
        this.path = path.ToArray();
        this.delimiter = delimiter;
    }

    public ResourcePath SetDelimiter(char delimiter)
    {
        this.delimiter = delimiter;
        return this;
    }

    public IReadOnlyList<string> Split()
    {
        var sb = StringBuilderCache.Acquire();
        var list = new List<string>();
        foreach (var c in this.path)
        {
            if (c == this.delimiter)
            {
                var segment = sb.ToString();
                if (segment.Length > 0)
                    list.Add(sb.ToString());

                sb.Clear();
            }

            sb.Append(c);
        }

        if (sb.Length > 0)
        {
            list.Add(sb.ToString());
        }

        sb.Clear();
        StringBuilderCache.Release(sb);
        return list;
    }

    public IEnumerable<Memory<char>> GetMemoryEnumerator()
    {
        var sb = StringBuilderCache.Acquire();
        var list = new List<Memory<char>>();
        foreach (var c in this.path)
        {
            if (c == this.delimiter)
            {
                var segment = sb.ToString();
                if (segment.Length > 0)
                    list.Add(new Memory<char>(sb.ToArray()));

                sb.Clear();
            }

            sb.Append(c);
        }

        if (sb.Length > 0)
        {
            list.Add(new Memory<char>(sb.ToArray()));
        }

        sb.Clear();
        StringBuilderCache.Release(sb);
        foreach (var next in list)
        {
            yield return next;
        }
    }

    public IEnumerator<string> GetEnumerator()
        => this.Split().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}