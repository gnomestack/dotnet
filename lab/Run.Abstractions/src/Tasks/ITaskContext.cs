using System.Collections;

using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public interface ITaskContext : IExecutionContext
{
    IDictionary<string, ITaskState?> Tasks { get; }

    ITaskState State { get; }
}