using GnomeStack.Functional;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Jobs;

public interface IJob : IExecutionDescriptor
{
    ITaskCollection Tasks { get; }

    Task<Result<object, Error>> RunAsync(IJobContext context, CancellationToken cancellationToken = default);
}