using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class HelpCommandMessage : CommandMessage
{
    public HelpCommandMessage(ITaskCollection tasks)
        : base("help")
    {
        this.Tasks = tasks;
    }

    public ITaskCollection Tasks { get; }
}