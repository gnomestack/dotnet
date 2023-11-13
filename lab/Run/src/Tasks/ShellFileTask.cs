using GnomeStack.Diagnostics;
using GnomeStack.Functional;

using RunShell = GnomeStack.Standard.Shell;

namespace GnomeStack.Run.Tasks;

public class ShellFileTask : BaseTask, IShellFileTask
{
    public ShellFileTask(string id, string file)
        : base(id)
    {
        this.File = file;
    }

    public string File { get; set; }

    public string? WorkingDirectory { get; set; }

    public string? Shell { get; set; }

    public IDictionary<string, string?> Env { get; set; } = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

    public override async Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var si = new PsStartInfo();
            if (!this.WorkingDirectory.IsNullOrWhiteSpace())
                si.WithCwd(this.WorkingDirectory);

            si.WithEnv(context.Env);

            if (this.Shell.IsNullOrWhiteSpace())
            {
                var output = await RunShell.RunFileAsResultAsync(
                        this.File,
                        si,
                        cancellationToken)
                    .NoCap();

                if (output.IsError)
                    return output.UnwrapError();

                return output.Unwrap();
            }

            var output2 = await RunShell.RunFileAsResultAsync(
                    this.Shell,
                    this.File,
                    si,
                    cancellationToken)
                .NoCap();

            if (output2.IsError)
                return output2.UnwrapError();

            return output2.Unwrap();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}