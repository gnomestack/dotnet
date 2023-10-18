using GnomeStack.Collections.Generic;

namespace GnomeStack.Diagnostics;

public class SplatOptions
{
    public static SplatOptions Default { get; } = new();

    public StringList Command { get; set; } = new();

    public Dictionary<string, string> Aliases { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public StringList Arguments { get; set; } = new();

    public StringList Excluded { get; set; } = new();

    public StringList Included { get; set; } = new();

    public string Prefix { get; set; } = "--";

    public string Assignment { get; set; } = " ";

    public bool PreserveCase { get; set; }

    public string ExtraArgumentsName { get; set; } = "ExtraArguments";

    public string SeparateArgumentsName { get; set; } = "SeparateArguments";

    public string SeparateArgumentsPrefix { get; set; } = "--";

    public bool ShortFlag { get; set; }

    public bool AppendArguments { get; set; }

    public SplatOptions WithCommand(params string[] command)
    {
        this.Command.AddRange(command);
        return this;
    }

    public SplatOptions WithAliases(IEnumerable<KeyValuePair<string, string>> aliases)
    {
        foreach (var kvp in aliases)
        {
            this.Aliases[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public SplatOptions WithAliases(IEnumerable<(string, string)> aliases)
    {
        foreach (var (key, value) in aliases)
        {
            this.Aliases[key] = value;
        }

        return this;
    }

    public SplatOptions WithArguments(params string[] arguments)
    {
        this.Arguments.AddRange(arguments);
        return this;
    }

    public SplatOptions WithExcluded(params string[] excluded)
    {
        this.Excluded.AddRange(excluded);
        return this;
    }

    public SplatOptions WithIncluded(params string[] included)
    {
        this.Included.AddRange(included);
        return this;
    }

    public SplatOptions WithExtraArgumentsName(string name)
    {
        this.ExtraArgumentsName = name;
        return this;
    }

    public SplatOptions WithSeparateArgumentsName(string name)
    {
        this.SeparateArgumentsName = name;
        return this;
    }

    public SplatOptions WithSeparateArgumentsPrefix(string prefix)
    {
        this.SeparateArgumentsPrefix = prefix;
        return this;
    }

    public SplatOptions WithPrefix(string prefix)
    {
        this.Prefix = prefix;
        return this;
    }

    public SplatOptions WithAssignment(string assignment)
    {
        this.Assignment = assignment;
        return this;
    }

    public SplatOptions WithPreserveCase(bool preserveCase)
    {
        this.PreserveCase = preserveCase;
        return this;
    }

    public SplatOptions WithShortFlag(bool shortFlag)
    {
        this.ShortFlag = shortFlag;
        return this;
    }

    public SplatOptions WithAppendArguments(bool appendArguments)
    {
        this.AppendArguments = appendArguments;
        return this;
    }
}