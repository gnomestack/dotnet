using GnomeStack.Dex.Flows.Jobs;
using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Jobs;

public abstract class BaseJob : ExecutionDescriptor, IJob
{
    protected BaseJob(string id)
        : base(id)
    {
    }

    public abstract Task<Result<object, Error>> RunAsync(
        IJobContext context,
        CancellationToken cancellationToken = default);
}