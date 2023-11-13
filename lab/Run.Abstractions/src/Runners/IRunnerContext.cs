using GnomeStack.Run.Execution;
using GnomeStack.Run.Jobs;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Runners;

public interface IRunnerContext : IExecutionContext
{
    ITaskCollection Tasks { get; }

    IRunnerOptions Options { get; }

    IJobCollection Jobs { get; }
}