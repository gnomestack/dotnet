using System.Collections;

using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public class ShellTaskBuilder : IShellTaskBuilder
{
    private readonly IShellTask task;

    public ShellTaskBuilder(IShellTask task)
    {
        this.task = task;
    }

    public IShellTaskBuilder AddDeps(params string[] deps)
    {
        foreach (var dep in deps)
        {
            this.task.Deps.Add(dep);
        }

        return this;
    }

    public IShellTaskBuilder AddDeps(IEnumerable<string> deps)
    {
        foreach (var dep in deps)
        {
            this.task.Deps.Add(dep);
        }

        return this;
    }

    public IShellTaskBuilder WithShell(string shell)
    {
        this.task.Shell = shell;
        return this;
    }

    public IShellTaskBuilder WithWorkingDirectory(string shell)
    {
        this.task.Shell = shell;
        return this;
    }

    public IShellTaskBuilder WithEnvVariables(IDictionary<string, string?> variables)
    {
        this.task.Env = variables;
        return this;
    }

    public IShellTaskBuilder AddEnvVariables(IDictionary<string, string?> variables)
    {
        foreach (var kvp in variables)
        {
            this.task.Env[kvp.Key] = kvp.Value;
        }

        return this;
    }


    public IShellTaskBuilder Set(Action<ITask> update)
    {
        update(this.task);
        return this;
    }

    public IShellTaskBuilder WithName(string name)
    {
        this.task.Name = name;
        return this;
    }

    public IShellTaskBuilder WithDescription(string description)
    {
        this.task.Description = description;
        return this;
    }

    public IShellTaskBuilder WithTimeout(Evaluate<int> eval)
    {
        this.task.Timeout = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IShellTaskBuilder WithTimeout(EvaluateAsync<int> eval)
    {
        this.task.Timeout = eval;
        return this;
    }

    public IShellTaskBuilder WithTimeout(int timeout)
    {
        this.task.Timeout = (_, _) => Task.FromResult(Result.Ok<int, Error>(timeout));
        return this;
    }

    public IShellTaskBuilder WithForce(Evaluate<bool> eval)
    {
        this.task.Force = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IShellTaskBuilder WithForce(EvaluateAsync<bool> eval)
    {
        this.task.Force = eval;
        return this;
    }

    public IShellTaskBuilder WithForce(bool force = true)
    {
        this.task.Force = (_, _) => Task.FromResult(Result.Ok<bool, Error>(force));
        return this;
    }

    public IShellTaskBuilder WithSkip(Evaluate<bool> eval)
    {
        this.task.Skip = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IShellTaskBuilder WithSkip(EvaluateAsync<bool> eval)
    {
        this.task.Skip = eval;
        return this;
    }

    public IShellTaskBuilder WithSkip(bool skip = true)
    {
        this.task.Force = (_, _) => Task.FromResult(Result.Ok<bool, Error>(skip));
        return this;
    }

    public IShellTaskBuilder WithDeps(params string[] deps)
    {
        this.task.Deps = new List<string>(deps);
        return this;
    }

    public IShellTaskBuilder WithDeps(IEnumerable<string> deps)
    {
        this.task.Deps = new List<string>(deps);
        return this;
    }
}