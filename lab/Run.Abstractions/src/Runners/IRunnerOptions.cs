using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Runners;

public interface IRunnerOptions
{
    IReadOnlyList<string> Commands { get; }

    IReadOnlyList<string> RemainingArguments { get; }

    IDictionary<string, object?> Options { get; }

    IReadOnlyDictionary<string, string?> Env { get; set; }

    Verbosity Verbosity { get; }

    bool SkipDeps { get; }

    bool List { get; }

    int? Timeout { get; }

    bool Help { get; }

    string? Cwd { get; }

    bool Version { get; }
}