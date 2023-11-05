using GnomeStack.Run.Messaging;

namespace GnomeStack.Run.Execution;

public interface IExecutionContext
{
    IDictionary<string, string?> Env { get; }

    IDictionary<string, string?> Secrets { get; }

    IMessageBus MessageBus { get; }
}