namespace GnomeStack.Diagnostics;

public readonly struct PsOutput
{
    public PsOutput()
    {
        this.ExitCode = 0;
        this.FileName = string.Empty;
        this.StdOut = Array.Empty<string>();
        this.StdError = Array.Empty<string>();
        this.StartTime = DateTime.MinValue;
        this.ExitTime = DateTime.MinValue;
    }

    public PsOutput(
        string fileName,
        int exitCode,
        IReadOnlyList<string>? stdOut = null,
        IReadOnlyList<string>? stdError = null,
        DateTime? startTime = null,
        DateTime? exitTime = null)
    {
        this.FileName = fileName;
        this.ExitCode = exitCode;
        this.StdOut = stdOut ?? Array.Empty<string>();
        this.StdError = stdError ?? Array.Empty<string>();
        this.StartTime = startTime ?? DateTime.MinValue;
        this.ExitTime = exitTime ?? DateTime.MinValue;
    }

    public static PsOutput Empty { get; } = new();

    public int ExitCode { get; }

    public string FileName { get; }

    public IReadOnlyList<string> StdOut { get; }

    public IReadOnlyList<string> StdError { get; }

    public DateTime StartTime { get; }

    public DateTime ExitTime { get; }

    public void ThrowOnInvalidExitCode(Func<int, bool>? validate = null)
    {
        if (validate is null)
        {
            if (this.ExitCode != 0)
            {
                throw new ProcessException(this.ExitCode, this.FileName);
            }

            return;
        }

        if (!validate(this.ExitCode))
        {
            throw new ProcessException(this.ExitCode, this.FileName);
        }
    }

/*
    public Result<PsOutput, ProcessException> ToResult(Func<int, bool>? validate = null)
    {
        if (validate is null)
        {
            if (this.ExitCode != 0)
                return Result.Err<PsOutput, ProcessException>(new ProcessException(this.ExitCode, this.FileName));

            return Result.Ok<PsOutput, ProcessException>(this);
        }

        if (validate(this.ExitCode))
            return Result.Ok<PsOutput, ProcessException>(this);

        return Result.Err<PsOutput, ProcessException>(new ProcessException(this.ExitCode, this.FileName));
    }
    */
}