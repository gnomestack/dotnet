using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using GnomeStack.Fmt.Ansi;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Messaging;
using GnomeStack.Standard;

using static GnomeStack.Standard.Ansi;

namespace GnomeStack.Run.Runners;

[SuppressMessage("ReSharper", "SpecifyACultureInStringConversionExplicitly")]
public class DefaultConsoleMessageSink : IMessageSink
{
    private readonly IAnsiWriter writer;

    [CLSCompliant(false)]
    public DefaultConsoleMessageSink(IAnsiWriter? writer = null)
    {
        this.writer = writer ?? new AnsiWriter();
    }

    public string SinkName => "ConsoleMessageSink";

    public void OnCompleted()
    {
        // do nothing
    }

    public void OnError(Exception error)
    {
        // do nothing
    }

    public void OnNext(Message value)
    {
        switch (value)
        {
            case GroupStartMessage groupStartMessage:
                this.writer.StartGroup(groupStartMessage.GroupName);
                break;

            case GroupEndMessage _:
                this.writer.EndGroup();
                break;

            case TaskSkippedMessage taskSkippedMessage:
                this.writer.StartGroup($"{taskSkippedMessage.Task.Name} (skipped)");
                this.writer.EndGroup();
                break;

            case TaskTimeoutMessage taskTimeoutMessage:
                {
                    var msg =
                        $"Task {taskTimeoutMessage.Task.Name} timed out after {taskTimeoutMessage.Timeout} seconds";
                    this.writer.Write(Standard.Ansi.Red(msg));
                }

                break;

            case TaskStartMessage taskStartMessage:
                this.writer.StartGroup(taskStartMessage.Task.Name);
                break;

            case ListTaskCommandMessage list:
                {
                    var maxPad = list.Tasks.Max(o => o.Name.Length);
                    foreach (var task in list.Tasks)
                    {
                        this.writer.WriteLine($"    {Magenta(task.Name.PadRight(maxPad))} ${task.Description}");
                    }
                }

                break;

            case HelpCommandMessage help:
                {
                    var msg = $"""
                               USAGE:
                                    dex [OPTIONS] [TASKS] [ARGS...]
                                    
                               OPTIONS
                                    --help, -h         Prints help information
                                    --list, -l         Lists all available tasks
                                    --version, -v      Prints version information
                                    --env, -e          Sets an environment variable e.g. NAME=VALUE
                                    --env-file --ef    Sets environment variables from a dotenv file
                                    --cwd              Sets the current working directory
                                    --debug            Enables debug logging
                                    --trace            Enables trace logging
                                    --timeout, -t      Sets the timeout in seconds for the overall task runner.
                               """;

                    this.writer.WriteLine(msg);
                    var maxPad = help.Tasks.Max(o => o.Name.Length);
                    foreach (var task in help.Tasks)
                    {
                        this.writer.WriteLine($"    {Magenta(task.Name.PadRight(maxPad))} ${task.Description}");
                    }
                }

                break;

            case VersionCommandMessage version:
                {
                    this.writer.WriteLine(version.Version);
                }

                break;
            case TaskEndMessage taskEndMessage:
                {
                    if (taskEndMessage.TaskResult.Status == ExecutionStatus.Ok)
                    {
                        var msg = $"Task {taskEndMessage.Task.Name} completed successfully";
                        this.writer.WriteLine(Green(msg));
                    }

                    if (taskEndMessage.TaskResult.Status == ExecutionStatus.Failed)
                    {
                        // todo: get stack trace
                        if (taskEndMessage.TaskResult.Error is ExceptionError err)
                        {
                            this.writer.WriteError($"{err.Exception}");
                        }
                        else
                        {
                            this.writer.WriteError($"{taskEndMessage.TaskResult.Error?.Message}");
                        }

                        var msg = $"Task {taskEndMessage.Task.Name} failed";
                        this.writer.WriteLine(Red(msg));
                    }

                    this.writer.EndGroup();
                }

                break;

            case UnhandledErrorMessage unhandledErrorMessage:
                {
                    var msg = $"Unexpected Error";
                    this.writer.Write(Red(msg));
                    if (unhandledErrorMessage.Error is ExceptionError err)
                    {
                        this.writer.WriteError($"{err.Exception}");
                    }
                    else
                    {
                        this.writer.WriteError($"{unhandledErrorMessage.Error.Message}");
                    }
                }

                break;

            case UnhandledExceptionMessage unhandledExceptionMessage:
                {
                    var msg = $"Unexpected Exception: ";
                    this.writer.Write(Standard.Ansi.Red(msg));
                    this.writer.WriteError($"{unhandledExceptionMessage.Exception}");
                }

                break;

            case TasksSummaryMessage tasksSummaryMessage:
                {
                    var maxTaskLength = tasksSummaryMessage.TaskResultsResults.Max(o => o.Task.Name.Length);
                    foreach (var result in tasksSummaryMessage.TaskResultsResults)
                    {
                        var task = result.Task;
                        var duration = result.EndedAt - result.StartedAt;
                        string msg = string.Empty;
                        switch (result.Status)
                        {
                            case ExecutionStatus.Ok:
                                {
                                    msg =
                                        $"{task.Name.PadRight(maxTaskLength)} {Green("completed")} ({Blue(duration.TotalMilliseconds.ToString())} ms)";
                                }

                                break;

                            case ExecutionStatus.Failed:
                                {
                                    msg =
                                        $"{task.Name.PadRight(maxTaskLength)} {Red("failed")} ({Blue(duration.TotalMilliseconds.ToString())} ms)";
                                }

                                break;

                            case ExecutionStatus.Skipped:
                                {
                                    msg =
                                        $"{task.Name.PadRight(maxTaskLength)} {Yellow("skipped")} ({Blue(duration.TotalMilliseconds.ToString())} ms)";
                                }

                                break;

                            case ExecutionStatus.Timeout:
                                {
                                    msg =
                                        $"{task.Name.PadRight(maxTaskLength)} {Red("timed out")} ({Blue(duration.TotalMilliseconds.ToString())} ms)";
                                }

                                break;
                            case ExecutionStatus.Cancelled:
                                {
                                    msg =
                                        $"{task.Name.PadRight(maxTaskLength)} {Red("cancelled")} ({Blue(duration.TotalMilliseconds.ToString())} ms)";
                                }

                                break;
                        }

                        this.writer.WriteLine(msg);
                    }
                }

                break;

            default:
                this.writer.WriteLine($"Unknown message: {value.GetType().FullName}");
                break;
        }
    }
}