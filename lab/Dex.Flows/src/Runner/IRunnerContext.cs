using GnomeStack.Dex.Flows.Jobs;
using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Runner;

public interface IRunnerContext : IExecutionContext
{
    ITaskCollection Tasks { get; }

    IRunnerOptions Options { get; }

    IJobCollection Jobs { get; }
}