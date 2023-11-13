using System.Diagnostics;
using System.Management.Automation;
using System.Text;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Pipelines;

public class WritePipelineWarningCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public object Message { get; set; } = null!;

    [Parameter]
    public string? SourceFile { get; set; }

    [Parameter]
    public int? LineNumber { get; set; }

    [Parameter]
    public int? ColumnNumber { get; set; }

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
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

                        Console.WriteLine($"##vso[task.logissue type=warning{add}]{this.Message}");
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
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
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

                        Console.WriteLine($"::warning{add}::{this.Message}");
                        return;
                    }

                    this.WriteWarning(ex.ToString());
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

                    Console.WriteLine($"##vso[task.logissue type=warning{add}]{this.Message}");
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

                    Console.WriteLine($"::warning{add}::{this.Message}");
                }

                var sb = new StringBuilder();
                sb.Append("[WARNING]: ");
                sb.Append(this.Message);
                sb.Append(" in ").Append(this.SourceFile);
                if (this.LineNumber.HasValue)
                    sb.Append(" on line ").Append(this.LineNumber);

                if (this.ColumnNumber.HasValue)
                    sb.Append(" at column ").Append(this.ColumnNumber);

                var msg = sb.ToString();
                sb.Clear();
                Console.WriteLine(Standard.Ansi.Yellow(msg));
                return;
            }
        }

        if (Util.IsTfBuild)
        {
            Console.WriteLine($"##[warning]{this.Message}");
            return;
        }

        if (Util.IsGitHubActions)
        {
            Console.WriteLine($"::warning::{this.Message}");
            return;
        }

        this.WriteWarning(Standard.Ansi.Yellow($"[WARNING]: {this.Message}"));
    }
}