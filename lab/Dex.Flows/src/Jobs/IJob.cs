using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Jobs;

public interface IJob : IExecutionDescriptor
{
    Task<Result<object, Error>> RunAsync(IJobContext context, CancellationToken cancellationToken = default);
}