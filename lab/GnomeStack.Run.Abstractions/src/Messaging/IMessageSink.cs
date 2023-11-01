namespace GnomeStack.Run.Messaging;

public interface IMessageSink : IObserver<Message>
{
    public string SinkName { get; }
}