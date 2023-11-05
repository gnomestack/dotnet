using GnomeStack.Diagnostics;
using GnomeStack.Functional;

namespace GnomeStack.PackageManager;

public class PackageManagerResult
{
    public PackageManagerResult()
    {
    }

    public PackageManagerResult(string operation, Result<PsOutput, Error> result)
    {
        this.Operation = operation;
        if (result.IsOk)
        {
            var output = result.Unwrap();
            if (output.ExitCode != 0)
            {
                this.IsError = true;
                this.Error = Error.Convert(new ProcessException(output.ExitCode, output.FileName));
                return;
            }
        }

        this.IsOk = result.IsOk;
        this.IsError = result.IsError;
        if (result.IsError)
            this.Error = result.UnwrapError();
    }

    public bool IsOk { get; set; }

    public bool IsError { get; set; }

    public Error? Error { get; set; }

    public string Operation { get; set; } = string.Empty;
}