using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public class TaskResult : ITaskResult
{
    public ExecutionStatus Status { get; set; }

    public Error? Error { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset EndedAt { get; set; }

    public ITask Task { get; set; }
}