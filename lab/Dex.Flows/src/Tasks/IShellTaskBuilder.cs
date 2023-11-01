using System.Collections;

namespace GnomeStack.Dex.Flows.Tasks;

public interface IShellTaskBuilder
{
    IShellTaskBuilder AddDeps(params string[] deps);

    IShellTaskBuilder Set(Action<ITask> update);

    IShellTaskBuilder WithName(string name);

    IShellTaskBuilder AddEnvVariables(IDictionary<string, string?> env);

    IShellTaskBuilder WithWorkingDirectory(string workingDirectory);

    IShellTaskBuilder WithShell(string shell);

    IShellTaskBuilder WithEnvVariables(IDictionary<string, string?> env);

    IShellTaskBuilder WithDescription(string description);

    IShellTaskBuilder WithTimeout(int timeout);

    IShellTaskBuilder WithForce(bool force = true);

    IShellTaskBuilder WithSkip(bool skip = true);

    IShellTaskBuilder WithDeps(params string[] deps);

    IShellTaskBuilder WithDeps(IEnumerable<string> deps);
}