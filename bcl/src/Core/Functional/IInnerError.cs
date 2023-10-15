namespace GnomeStack.Functional;

public interface IInnerError
{
    public string Message { get; }

    public string? Code { get; }

    public IInnerError? InnerError { get; }
}