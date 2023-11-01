using GnomeStack.Dex.Flows.Runner;
using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class ListTaskCommandMessage : CommandMessage
{
    public ListTaskCommandMessage(ITaskCollection tasks)
        : base("list")
    {
        this.Tasks = tasks;
    }

    public ITaskCollection Tasks { get; }
}