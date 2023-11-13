namespace GnomeStack.PowerShell.Pipelines;

public class PipelineException : Exception
{
    public PipelineException()
    {
    }

    public PipelineException(string message)
        : base(message)
    {
    }

    public PipelineException(string message, Exception inner)
        : base(message, inner)
    {
    }
}