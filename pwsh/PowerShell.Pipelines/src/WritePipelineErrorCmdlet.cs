using System.Diagnostics;
using System.Management.Automation;
using System.Text;

using GnomeStack.Extras.Strings;
using GnomeStack.Standard;
using GnomeStack.Text;

namespace GnomeStack.PowerShell.Pipelines;

[OutputType(typeof(void))]
[Alias("write_pipeline_error")]
[Cmdlet(VerbsCommunications.Write, "PipelineError")]
public class WritePipelineErrorCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
    public object Message { get; set; } = null!;

    [Parameter]
    public string? SourceFile { get; set; }

    [Parameter]
    public int? LineNumber { get; set; }

    [Parameter]
    public int? ColumnNumber { get; set; }

    protected override void ProcessRecord()
    {
        Exception? ex = null;
        if (this.Message is Exception exception)
        {
            ex = exception;
        }
        else if (this.Message is ErrorRecord errorRecord)
        {
            ex = errorRecord.Exception;
        }

        if (ex is not null)
        {
            var t = new StackTrace(ex);
            if (t.FrameCount > 0)
            {
                var frame = t.GetFrame(0);
                if (frame?.GetFileLineNumber() != null)
                {
                    var source = frame.GetFileName();
                    var line = frame.GetFileLineNumber();
                    var column = frame.GetFileColumnNumber();
                    var add = string.Empty;

                    if (Util.IsTfBuild)
                    {
                        if (!source.IsNullOrWhiteSpace())
                        {
                            add = $";sourcepath={source}";
                        }

                        if (line != null)
                        {
                            add += $";linenumber={line}";
                        }

                        if (column != null)
                        {
                            add += $";columnnumber={column}";
                        }

                        Console.WriteLine($"##vso[task.logissue type=error{add}]{this.Message}");
                        return;
                    }

                    if (Util.IsGitHubActions)
                    {
                        if (!source.IsNullOrWhiteSpace())
                        {
                            add = $" file={source}";
                        }

                        if (line != null)
                        {
                            if (add.Length > 0)
                            {
                                add += $",line={line}";
                            }
                            else
                            {
                                add += $" line={line}";
                            }
                        }

                        if (column != null)
                        {
                            if (add.Length > 0)
                            {
                                add += $",col={column}";
                            }
                            else
                            {
                                add += $" col={column}";
                            }
                        }

                        Console.WriteLine($"::error{add}::{this.Message}");
                        return;
                    }

                    this.WriteError(ex);
                    return;
                }
            }

            if (!this.SourceFile.IsNullOrWhiteSpace())
            {
                if (Util.IsTfBuild)
                {
                    var add = string.Empty;
                    if (!this.SourceFile.IsNullOrWhiteSpace())
                    {
                        add = $";sourcepath={this.SourceFile}";
                    }

                    if (this.LineNumber.HasValue)
                    {
                        add += $";linenumber={this.LineNumber}";
                    }

                    if (this.ColumnNumber.HasValue)
                    {
                        add += $";columnnumber={this.ColumnNumber}";
                    }

                    Console.WriteLine($"##vso[task.logissue type=error{add}]{this.Message}");
                    return;
                }

                if (Util.IsGitHubActions)
                {
                    var add = string.Empty;
                    if (!this.SourceFile.IsNullOrWhiteSpace())
                    {
                        add = $" file={this.SourceFile}";
                    }

                    if (this.LineNumber.HasValue)
                    {
                        if (add.Length > 0)
                        {
                            add += $",line={this.LineNumber}";
                        }
                        else
                        {
                            add += $" line={this.LineNumber}";
                        }
                    }

                    if (this.ColumnNumber.HasValue)
                    {
                        if (add.Length > 0)
                        {
                            add += $",col={this.ColumnNumber}";
                        }
                        else
                        {
                            add += $" col={this.ColumnNumber}";
                        }
                    }

                    Console.WriteLine($"::error{add}::{this.Message}");
                }

                var sb = new StringBuilder();
                sb.Append("[ERROR]: ");
                sb.Append(this.Message);
                sb.Append(" in ").Append(this.SourceFile);
                if (this.LineNumber.HasValue)
                    sb.Append(" on line ").Append(this.LineNumber);

                if (this.ColumnNumber.HasValue)
                    sb.Append(" at column ").Append(this.ColumnNumber);

                var msg = sb.ToString();
                sb.Clear();
                Console.WriteLine(Standard.Ansi.Red(msg));
                return;
            }
        }

        if (Util.IsTfBuild)
        {
            Console.WriteLine($"##[error]{this.Message}");
            return;
        }

        if (Util.IsGitHubActions)
        {
            Console.WriteLine($"::error::{this.Message}");
            return;
        }

        Console.WriteLine(Standard.Ansi.Red($"[ERROR]: {this.Message}"));
    }
}