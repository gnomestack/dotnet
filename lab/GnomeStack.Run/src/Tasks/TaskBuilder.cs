using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public class TaskBuilder : ITaskBuilder
{
    private readonly ITask task;

    public TaskBuilder(ITask task)
    {
        this.task = task;
    }

    public ITaskBuilder AddDeps(params string[] deps)
    {
        foreach (var dep in deps)
            this.task.Deps.Add(dep);

        return this;
    }

    public ITaskBuilder Set(Action<ITask> update)
    {
        update(this.task);
        return this;
    }

    public ITaskBuilder WithName(string name)
    {
        this.task.Name = name;
        return this;
    }

    public ITaskBuilder WithDescription(string description)
    {
        this.task.Description = description;
        return this;
    }

    public ITaskBuilder WithTimeout(EvaluateAsync<int> eval)
    {
        this.task.Timeout = eval;
        return this;
    }

    public ITaskBuilder WithTimeout(Evaluate<int> eval)
    {
        this.task.Timeout = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public ITaskBuilder WithTimeout(int timeout)
    {
        this.task.Timeout = (_, _) => Task.FromResult(Result.Ok<int, Error>(timeout));
        return this;
    }

    public ITaskBuilder WithForce(EvaluateAsync<bool> eval)
    {
        this.task.Force = eval;
        return this;
    }

    public ITaskBuilder WithForce(Evaluate<bool> eval)
    {
        this.task.Force = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public ITaskBuilder WithForce(bool force = true)
    {
        this.task.Force = (_, _) => Task.FromResult(Result.Ok<bool, Error>(force));
        return this;
    }

    public ITaskBuilder WithSkip(EvaluateAsync<bool> eval)
    {
        this.task.Skip = eval;
        return this;
    }

    public ITaskBuilder WithSkip(Evaluate<bool> eval)
    {
        this.task.Skip = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public ITaskBuilder WithSkip(bool skip = true)
    {
        this.task.Skip = (_, _) => Task.FromResult(Result.Ok<bool, Error>(skip));
        return this;
    }

    public ITaskBuilder WithDeps(params string[] deps)
    {
        this.task.Deps = deps;
        return this;
    }

    public ITaskBuilder WithDeps(IEnumerable<string> deps)
    {
        this.task.Deps = new List<string>(deps);
        return this;
    }
}