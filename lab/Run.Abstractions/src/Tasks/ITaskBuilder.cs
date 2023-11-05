using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public interface ITaskBuilder : IExecutionDescriptorBuilder<ITaskBuilder, ITask>
{
}