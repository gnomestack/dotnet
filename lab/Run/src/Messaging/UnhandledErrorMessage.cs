using GnomeStack.Functional;

namespace GnomeStack.Run.Messaging;

public class UnhandledErrorMessage : Message
{
    public UnhandledErrorMessage(Error error)
    {
        this.Error = error;
    }

    public Error Error { get; set; }
}