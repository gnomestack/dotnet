using GnomeStack.Run.Messaging;

namespace GnomeStack.Run.Runners;

public interface IOptionsParser
{
    public IRunnerOptions Parse(string[] args, IMessageBus bus);
}