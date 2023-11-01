using GnomeStack.Dex.Flows.Messaging;

namespace GnomeStack.Dex.Flows;

public interface IExecutionContext
{
    IDictionary<string, string?> Env { get; }

    IDictionary<string, string?> Secrets { get; }

    IMessageBus MessageBus { get; }
}