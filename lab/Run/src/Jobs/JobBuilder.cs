using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Jobs;

public class JobBuilder : IJobBuilder
{
    private readonly IJob job;

    public JobBuilder(IJob job)
    {
        this.job = job;
    }

    public IJobBuilder AddDeps(params string[] deps)
    {
        foreach (var dep in deps)
            this.job.Deps.Add(dep);

        return this;
    }

    public IJobBuilder Set(Action<IJob> update)
    {
        update(this.job);
        return this;
    }

    public IJobBuilder WithName(string name)
    {
        this.job.Name = name;
        return this;
    }

    public IJobBuilder WithDescription(string description)
    {
        this.job.Description = description;
        return this;
    }

    public IJobBuilder WithTimeout(EvaluateAsync<int> eval)
    {
        this.job.Timeout = eval;
        return this;
    }

    public IJobBuilder WithTimeout(Evaluate<int> eval)
    {
        this.job.Timeout = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IJobBuilder WithTimeout(int timeout)
    {
        this.job.Timeout = (_, _) => Task.FromResult(Result.Ok<int, Error>(timeout));
        return this;
    }

    public IJobBuilder WithForce(EvaluateAsync<bool> eval)
    {
        this.job.Force = eval;
        return this;
    }

    public IJobBuilder WithForce(Evaluate<bool> eval)
    {
        this.job.Force = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IJobBuilder WithForce(bool force = true)
    {
        this.job.Force = (_, _) => Task.FromResult(Result.Ok<bool, Error>(force));
        return this;
    }

    public IJobBuilder WithSkip(EvaluateAsync<bool> eval)
    {
        this.job.Skip = eval;
        return this;
    }

    public IJobBuilder WithSkip(Evaluate<bool> eval)
    {
        this.job.Skip = (s, _) => Task.FromResult(eval(s));
        return this;
    }

    public IJobBuilder WithSkip(bool skip = true)
    {
        this.job.Skip = (_, _) => Task.FromResult(Result.Ok<bool, Error>(skip));
        return this;
    }

    public IJobBuilder WithDeps(params string[] deps)
    {
        this.job.Deps = deps;
        return this;
    }

    public IJobBuilder WithDeps(IEnumerable<string> deps)
    {
        this.job.Deps = new List<string>(deps);
        return this;
    }
}