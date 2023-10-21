namespace GnomeStack.KeePass.Xml;

public class SerializerContext
{
    public Dictionary<Type, Type> Mappings { get; internal set; } = new();
}