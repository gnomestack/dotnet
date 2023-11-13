namespace GnomeStack.Run.Messaging;

public class GroupEndMessage : Message
{
    public GroupEndMessage(string groupName)
    {
        this.GroupName = groupName;
    }

    public string GroupName { get; }
}