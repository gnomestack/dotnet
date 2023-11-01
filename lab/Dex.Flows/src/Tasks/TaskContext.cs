using GnomeStack.Dex.Flows.Messaging;

namespace GnomeStack.Dex.Flows.Tasks;

public class TaskContext : ITaskContext
{
    public TaskContext(IMessageBus bus)
    {
        this.MessageBus = bus;
        this.State = TaskState.Default;
    }

    public TaskContext(ITaskContext context)
    {
        this.Env = new Dictionary<string, string?>(context.Env, StringComparer.OrdinalIgnoreCase);
        this.Secrets = new Dictionary<string, string?>(context.Secrets, StringComparer.OrdinalIgnoreCase);
        this.MessageBus = context.MessageBus;
        this.Tasks = new Dictionary<string, ITaskState?>(context.Tasks, StringComparer.OrdinalIgnoreCase);
        this.State = TaskState.Default;
    }

    public IDictionary<string, string?> Env { get; set; } = new Dictionary<string, string?>();

    public IDictionary<string, string?> Secrets { get; set; } = new Dictionary<string, string?>();

    public IMessageBus MessageBus { get; set; }

    public IDictionary<string, ITaskState?> Tasks { get; set; } = new Dictionary<string, ITaskState?>();

    public ITaskState State { get; set; }
}