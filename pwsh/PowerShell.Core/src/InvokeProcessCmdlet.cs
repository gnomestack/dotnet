using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

using GnomeStack.Diagnostics;
using GnomeStack.Extras.Collections;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("invoke_process")]
[Cmdlet(VerbsLifecycle.Invoke, "Process")]
[OutputType(typeof(PsOutput))]
public class InvokeProcessCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string? Executable { get; set; }

    [Parameter]
    public PsArgs? Arguments { get; set; }

    [Parameter]
    public Stdio StdOut { get; set; } = Stdio.Inherit;

    [Parameter]
    public Stdio StdError { get; set; } = Stdio.Inherit;

    [Parameter]
    public SwitchParameter AsSudo { get; set; }

    [Parameter]
    public ActionPreference? CommandAction { get; set; }

    [Alias("Env", "e")]
    [Parameter]
    public IDictionary? Environment { get; set; }

    [Alias("Cwd", "Wd")]
    [Parameter]
    public string? WorkingDirectory { get; set; }

    [Parameter]
    public SwitchParameter PassThru { get; set; }

    [Parameter]
    public IEnumerable<IPsCapture> StdOutCapture { get; set; } = Array.Empty<IPsCapture>();

    [Parameter]
    public IEnumerable<IPsCapture> StdErrorCapture { get; set; } = Array.Empty<IPsCapture>();

    protected override void ProcessRecord()
    {
        Dictionary<string, string?>? env = null;

        if (this.Executable.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Executable));

        var args = this.Arguments;
        args ??= new PsArgs();

        var exe = Ps.Which(this.Executable);
        if (exe is null)
            throw new NotFoundOnPathException(this.Executable);

        if (this.AsSudo.ToBool() && !Env.IsWindows && !Env.IsPrivilegedProcess)
        {
            args.Insert(0, exe);
            exe = "sudo";
        }

        if (this.Environment != null)
        {
            env = new Dictionary<string, string?>();
            foreach (var key in this.Environment.Keys)
            {
                if (key is string name)
                {
                    var value = this.Environment[name];
                    if (value is null)
                    {
                        env[name] = null;
                        continue;
                    }

                    if (value is string str)
                    {
                        env[name] = str;
                    }
                }
            }
        }

        var ci = new PsStartInfo()
        {
            Args = args,
            Env = env,
            Cwd = this.WorkingDirectory,
            StdOut = this.StdOut,
            StdErr = this.StdError,
        };

        foreach (var capture in this.StdOutCapture)
        {
            ci.Capture(capture);
        }

        foreach (var capture in this.StdErrorCapture)
        {
            ci.Capture(capture);
        }

        var cmd = new Ps(exe, ci);

        this.WriteCommand(this.Executable, this.Arguments, this.CommandAction);

        var result = cmd.Output();
        this.SessionState.PSVariable.Set("LASTEXITCODE", result.ExitCode);

        if (this.PassThru.ToBool() || this.StdOut == Stdio.Piped || this.StdError == Stdio.Piped)
        {
            this.WriteObject(result);
        }
    }
}