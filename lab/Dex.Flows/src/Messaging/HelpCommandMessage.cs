using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class HelpCommandMessage : CommandMessage
{
    public HelpCommandMessage(ITaskCollection tasks)
        : base("help")
    {
        this.Tasks = tasks;
    }

    public ITaskCollection Tasks { get; }
}