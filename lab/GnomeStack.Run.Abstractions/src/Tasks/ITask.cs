using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public interface ITask : IExecutionDescriptor
{
    Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default);
}