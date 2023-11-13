namespace GnomeStack.Run.Execution;

public abstract class ExecutionDescriptor : IExecutionDescriptor
{
    protected ExecutionDescriptor(string id)
    {
        this.Id = id;
        this.Name = id;
    }

    public string Id { get; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public IList<string> Deps { get; set; } = new List<string>();

    public EvaluateAsync<int>? Timeout { get; set; }

    public EvaluateAsync<bool>? Force { get; set; }

    public EvaluateAsync<bool>? Skip { get; set; }

    public void AddDeps(params string[] deps)
    {
        foreach (var dep in deps)
        {
            this.Deps.Add(dep);
        }
    }

    public void AddDeps(IEnumerable<string> deps)
    {
        foreach (var dep in deps)
        {
            this.Deps.Add(dep);
        }
    }
}