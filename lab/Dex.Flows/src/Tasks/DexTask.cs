using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public abstract class DexTask : ExecutionDescriptor, ITask
{
    protected DexTask(string id)
        : base(id)
    {
    }
    public abstract Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default);
}