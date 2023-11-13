namespace GnomeStack.Run.Messaging;

public class CommandMessage : Message
{
    public CommandMessage(string command)
    {
        this.Command = command;
    }

    public string Command { get; }
}