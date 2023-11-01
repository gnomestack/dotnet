using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Jobs;

public abstract class DexJob : ExecutionDescriptor, IJob
{
    protected DexJob(string id)
        : base(id)
    {
    }

    public abstract Task<Result<object, Error>> RunAsync(
        IJobContext context,
        CancellationToken cancellationToken = default);
}