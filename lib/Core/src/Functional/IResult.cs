namespace GnomeStack.Functional;

public interface IResult
{
    bool IsOk { get; }

    bool IsError { get; }
}