using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Runners;

public interface ICodeFirstRunner
{
    Task<int> RunAsync(IRunnerOptions options, ITaskCollection tasks);
}