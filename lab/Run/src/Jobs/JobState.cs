using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Jobs;

public class JobState : IJobState
{
    public JobState(IJob task)
    {
        this.Id = task.Id;
        this.Name = task.Name;
        this.Description = task.Description;
        this.Deps = new List<string>(task.Deps);
        this.Status = ExecutionStatus.Ok;
    }

    internal JobState()
    {
        this.Id = "empty_default";
        this.Name = "Empty Default";
        this.Deps = new List<string>();
    }

    public static JobState Default { get; } = new JobState();

    public string Id { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<string> Deps { get; }

    public int? Timeout { get; set; }

    public bool Force { get; set; }

    public bool Skip { get; set;  }

    public ExecutionStatus Status { get; set; }

    public IDictionary<string, object?> Outputs { get; set; } =
        new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
}