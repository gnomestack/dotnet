using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public class TaskState : ITaskState
{
    public TaskState(ITask task)
    {
        this.Id = task.Id;
        this.Name = task.Name;
        this.Description = task.Description;
        this.Deps = new List<string>(task.Deps);
        this.Status = ExecutionStatus.Ok;
    }

    internal TaskState()
    {
        this.Id = "Default";
        this.Name = "Default";
        this.Deps = new List<string>();
    }

    public static TaskState Default { get; } = new TaskState();

    public string Id { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<string> Deps { get; }

    public int? Timeout { get; set; }

    public bool Force { get; set; }

    public bool Skip { get; set; }

    public ExecutionStatus Status { get; set; }

    public IDictionary<string, object?> Outputs { get; } =
        new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
}