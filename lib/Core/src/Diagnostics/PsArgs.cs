using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Diagnostics;

public partial class PsArgs : List<string>
{
    public PsArgs()
    {
    }

    public PsArgs(IEnumerable<string> args)
        : base(args)
    {
    }

    private enum Quote
    {
        None = 0,

        Single = 1,

        Double = 2,
    }

    public bool Escape { get; set; } = true;

    public static implicit operator PsArgs(PsArgsBuilder builder)
        => builder.BuildPsArgs();

    public static implicit operator PsArgs(string args)
        => From(args);

    public static implicit operator PsArgs(StringBuilder args)
        => From(args);

    public static implicit operator PsArgs(string[] args)
        => new(args);

    public static implicit operator PsArgs(Collection<string> args)
        => new(args);

    public static implicit operator PsArgs(StringCollection args)
        => From(args);

    public static implicit operator string(PsArgs args)
        => args.ToString();

    public static implicit operator PsArgs(Splattable obj)
        => Splat(obj);

    public static implicit operator Collection<string>(PsArgs args)
        => new Collection<string>(args);

    public static IReadOnlyList<string> SplitArguments(ReadOnlySpan<char> args)
    {
        var token = StringBuilderCache.Acquire();
        var quote = Quote.None;
        var tokens = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            var c = args[i];

            if (quote != Quote.None)
            {
                switch (quote)
                {
                    case Quote.Single:
                        if (c == '\'')
                        {
                            quote = Quote.None;
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                        }
                        else
                        {
                            token.Append(c);
                        }

                        continue;

                    case Quote.Double:
                        if (c == '\"')
                        {
                            quote = Quote.Double;
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                        }
                        else
                        {
                            token.Append(c);
                        }

                        continue;
                }

                token.Append(c);
                continue;
            }

            if (c == ' ')
            {
                // handle backtick (`) and backslash (\) to notate a new line and different argument.
                var remaining = args.Length - 1 - i;
                if (remaining > 2)
                {
                    var j = args[i + 1];
                    var k = args[i + 2];

                    if ((j == '\\' || j == '`') && k == '\n')
                    {
                        i += 2;
                        if (token.Length > 0)
                        {
                            tokens.Add(token.ToString());
                        }

                        token.Clear();
                        continue;
                    }

                    if (remaining > 3)
                    {
                        var l = args[i + 3];
                        if (k == '\r' && l == '\n')
                        {
                            i += 3;
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                            }

                            token.Clear();
                            continue;
                        }
                    }
                }

                if (token.Length > 0)
                {
                    tokens.Add(token.ToString());
                    token.Clear();
                }

                continue;
            }

            if (token.Length == 0)
            {
                switch (c)
                {
                    case '\'':
                        quote = Quote.Single;
                        continue;

                    case '\"':
                        quote = Quote.Double;
                        continue;
                }
            }

            token.Append(c);
        }

        if (token.Length > 0)
            tokens.Add(token.ToString());

        StringBuilderCache.Release(token);

        return tokens;
    }

    public static PsArgs From(IPsArgsBuilder builder)
    {
        return builder.BuildPsArgs();
    }

    public static PsArgs From(StringBuilder builder)
    {
        return From(builder.ToString());
    }

    public static PsArgs From(StringCollection collection)
    {
        var next = new PsArgs();
        foreach (var item in collection)
        {
            if (item is null)
                continue;

            next.Add(item);
        }

        return next;
    }

    public static PsArgs From(string args)
    {
        return new PsArgs(SplitArguments(args.AsSpan()));
    }

    public static PsArgs From(IEnumerable<string> args)
    {
        return new PsArgs(args);
    }

    public static PsArgs From(params string[] args)
    {
        return new PsArgs(args);
    }

    [SuppressMessage("", "SA1100:Do not prefix calls with base unless local implementation exists", Justification = "required")]
    public void Add(ReadOnlySpan<char> item)
        => base.Add(item.ToString());

    public void Add(string item1, string item2)
    {
        this.Add(item1);
        this.Add(item2);
    }

    public void Add(string item1, string item2, string item3)
    {
        this.Add(item1);
        this.Add(item2);
        this.Add(item3);
    }

    public void Add(string item1, string item2, string item3, string item4)
    {
        this.Add(item1);
        this.Add(item2);
        this.Add(item3);
        this.Add(item4);
    }

    public void Add(string item1, string item2, string item3, string item4, string item5)
    {
        this.Add(item1);
        this.Add(item2);
        this.Add(item3);
        this.Add(item4);
        this.Add(item5);
    }

    public override string ToString()
    {
        var sb = StringBuilderCache.Acquire();

        foreach (var argument in this)
        {
            if (sb.Length > 0)
                sb.Append(' ');

            sb.AppendCliArgument(argument);
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}

#pragma warning disable SA1201
public interface IPsArgsBuilder
{
    PsArgs BuildPsArgs();
}

public abstract class PsArgsBuilder : IPsArgsBuilder
{
    public abstract PsArgs BuildPsArgs();
}