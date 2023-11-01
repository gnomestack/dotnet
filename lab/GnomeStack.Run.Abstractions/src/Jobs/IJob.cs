using GnomeStack.Functional;
using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Jobs;

public interface IJob : IExecutionDescriptor
{
    Task<Result<object, Error>> RunAsync(IJobContext context, CancellationToken cancellationToken = default);
}