namespace GnomeStack.Data;

public class InvalidDbIdentifierException : Exception
{
    public InvalidDbIdentifierException()
        : base()
    {
    }

    public InvalidDbIdentifierException(string message)
        : base(message)
    {
    }

    public InvalidDbIdentifierException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}