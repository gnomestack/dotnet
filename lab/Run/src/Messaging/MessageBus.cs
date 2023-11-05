using System.Collections.Concurrent;

namespace GnomeStack.Run.Messaging;

public class MessageBus : IObservable<Message>, IMessageBus
{
    public MessageBus()
    {
    }

    private ConcurrentBag<IObserver<Message>> sinks = new();

    public void Send(Message message)
    {
        foreach (var sink in sinks)
        {
            sink.OnNext(message);
        }
    }

    public ValueTask DisposeAsync()
    {
        foreach (var sink in this.sinks)
        {
            sink.OnCompleted();
        }

        return new ValueTask(Task.CompletedTask);
    }

    public IDisposable Subscribe(IMessageSink observer)
    {
        return new Subscription(observer, this);
    }

    IDisposable IObservable<Message>.Subscribe(IObserver<Message> observer)
    {
        return new Subscription(observer, this);
    }

    private void RegisterSink(IObserver<Message> sink)
    {
        this.sinks.Add(sink);
    }

    private void UnregisterSink(IObserver<Message> sink)
    {
        this.sinks.TryTake(out sink);
    }

    private sealed class Subscription : IDisposable
    {
        private readonly IObserver<Message> observer;

        private readonly MessageBus bus;

        public Subscription(IObserver<Message> observer, MessageBus bus)
        {
            this.bus = bus;
            this.observer = observer;
            this.bus.RegisterSink(this.observer);
        }

        public void Dispose()
        {
            this.bus.UnregisterSink(this.observer);
        }
    }
}