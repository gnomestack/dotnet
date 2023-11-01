namespace GnomeStack.Dex.Flows.Messaging;

public interface IMessageSink : IObserver<Message>
{
    public string SinkName { get; }
}