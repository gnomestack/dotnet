using GnomeStack.Dex.Flows.Jobs;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Runners;

public interface IRunnerContext : IExecutionContext
{
    ITaskCollection Tasks { get; }

    IRunnerOptions Options { get; }

    IJobCollection Jobs { get; }
}