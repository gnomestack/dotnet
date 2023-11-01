using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public interface ITask : IExecutionDescriptor
{
    Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default);
}