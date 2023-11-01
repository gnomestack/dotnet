using GnomeStack.Dex.Flows.Runner;

namespace GnomeStack.Dex.Flows.Messaging;

public class CommandMessage : Message
{
    public CommandMessage(string command)
    {
        this.Command = command;
    }

    public string Command { get; }
}