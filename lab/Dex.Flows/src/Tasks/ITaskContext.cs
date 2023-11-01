using System.Collections;

namespace GnomeStack.Dex.Flows.Tasks;

public interface ITaskContext : IExecutionContext
{
    IDictionary<string, ITaskState?> Tasks { get; }

    ITaskState State { get; }
}