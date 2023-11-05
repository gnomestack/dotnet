using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public abstract class BaseTask : ExecutionDescriptor, ITask
{
    protected BaseTask(string id)
        : base(id)
    {
    }

    public abstract Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default);
}