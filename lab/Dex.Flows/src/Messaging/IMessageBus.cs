namespace GnomeStack.Dex.Flows.Messaging;

public interface IMessageBus : IAsyncDisposable
{
    IDisposable Subscribe(IMessageSink sink);

    void Send(Message message);
}