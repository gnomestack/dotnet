using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class ListTaskCommandMessage : CommandMessage
{
    public ListTaskCommandMessage(ITaskCollection tasks)
        : base("list")
    {
        this.Tasks = tasks;
    }

    public ITaskCollection Tasks { get; }
}