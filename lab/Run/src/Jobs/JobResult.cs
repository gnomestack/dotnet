using GnomeStack.Functional;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Jobs;

public class JobResult : IJobResult
{
    public JobResult(IJob job)
    {
        this.Job = job;
    }

    public JobResult(IJob job, Error error)
    {
        this.Job = job;
        this.Error = error;
        this.Status = ExecutionStatus.Failed;
    }

    public ExecutionStatus Status { get; set; }

    public Error? Error { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset EndedAt { get; set; }

    public IJob Job { get; }
}