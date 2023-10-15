namespace GnomeStack.Diagnostics;

[System.Serializable]
public class NotFoundOnPathException : System.Exception
{
    public NotFoundOnPathException()
    {
    }

    public NotFoundOnPathException(string message)
        : base(message)
    {
    }

    public NotFoundOnPathException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    protected NotFoundOnPathException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
}