using GnomeStack.Functional;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Jobs;

public abstract class BaseJob : ExecutionDescriptor, IJob
{
    protected BaseJob(string id)
        : base(id)
    {
    }

    public abstract ITaskCollection Tasks { get; }

    public abstract Task<Result<object, Error>> RunAsync(
        IJobContext context,
        CancellationToken cancellationToken = default);
}