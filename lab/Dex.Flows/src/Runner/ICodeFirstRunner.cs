using GnomeStack.Dex.Flows.Jobs;
using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Runner;

public interface ICodeFirstRunner
{
    Task<int> RunAsync(IRunnerOptions options, ITaskCollection tasks);
}