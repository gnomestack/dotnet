namespace GnomeStack.Dex.Flows.Messaging;

public class GroupStartMessage : Message
{
    public GroupStartMessage(string groupName)
    {
        this.GroupName = groupName;
    }

    public string GroupName { get; }
}