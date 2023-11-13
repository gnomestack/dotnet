using System.Reflection;

using GnomeStack.Diagnostics;
using GnomeStack.Fmt.Ansi;
using GnomeStack.Functional;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Messaging;
using GnomeStack.Run.Tasks;
using GnomeStack.Standard;

namespace GnomeStack.Run.Runners;

public class ConsoleRunner
{
    public async Task<int> RunAsync(
        string[] args,
        ITaskCollection tasks,
        IAnsiWriter? writer = null,
        CancellationToken cancellationToken = default)
    {
        var messageSink = new DefaultConsoleMessageSink(writer);
        await using var messageBus = new MessageBus();
        messageBus.Subscribe(messageSink);
        var options = new OptionsParser().Parse(args, messageBus);

        if (options.List)
        {
            var list = new ListTaskCommandMessage(tasks);
            messageBus.Send(list);
            return 0;
        }

        if (options.Help)
        {
            var help = new HelpCommandMessage(tasks);
            messageBus.Send(help);
            return 0;
        }

        if (options.Version)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var v = executingAssembly.GetName().Version;
            var msg = new VersionCommandMessage(v?.ToString() ?? "0.0.0");
            messageBus.Send(msg);
            return 0;
        }

        var dest = new TaskCollection();
        foreach (var cmd in options.Commands)
        {
            var task = tasks[cmd];
            if (task is null)
            {
                messageBus.Send(new UnhandledErrorMessage(new Error($"Unable to find task {cmd}")));
                return 1;
            }

            FlattenTasks(task, tasks, dest, messageBus);
            if (!dest.Contains(task))
                dest.Add(task);
        }

        if (DetectCyclicalDeps(dest, messageBus))
            return 1;

        var context = new TaskContext(messageBus);
        var results = await this.HandleTasksAsync(dest, context, messageBus, options.Timeout ?? 0, cancellationToken)
            .NoCap();

        messageBus.Send(new TasksSummaryMessage(results));
        var failed = results.Any(x => x.Status == ExecutionStatus.Failed);
        var cancelled = results.Any(x => x.Status == ExecutionStatus.Cancelled);
        var timeout = results.Any(x => x.Status == ExecutionStatus.Timeout);
        var skipped = results.Any(x => x.Status == ExecutionStatus.Skipped);
        var ok = results.Any(x => x.Status == ExecutionStatus.Ok);
        var exitCode = 0;
        if (failed)
        {
            exitCode = 1;
        }
        else if (cancelled)
        {
            exitCode = 2;
        }
        else if (timeout)
        {
            exitCode = 3;
        }
        else if (skipped)
        {
            exitCode = 4;
        }
        else if (ok)
        {
            exitCode = 0;
        }

        return exitCode;
    }

    private static bool DetectCyclicalDeps(ITaskCollection dest, IMessageBus bus)
    {
        var stack = new Stack<string>();
        bool Resolve(ITask task)
        {
            if (stack.Contains(task.Id))
            {
                bus.Send(new UnhandledErrorMessage(new Error($"Cyclical dependency detected: {string.Join(" -> ", stack)}")));
                return false;
            }

            stack.Push(task.Id);
            foreach (var dep in task.Deps)
            {
                if (dest.Contains(dep))
                {
                    var depTask = dest[dep];
                    if (!Resolve(depTask!))
                        return false;
                }
                else
                {
                    bus.Send(new UnhandledErrorMessage(new Error($"Unable to find task {dep}")));
                    return false;
                }
            }

            stack.Pop();
            return true;
        }

        foreach (var task in dest)
        {
            if (!Resolve(task))
                return true;
        }

        return false;
    }

    private static void FlattenTasks(ITask task, ITaskCollection src, TaskCollection dest, IMessageBus bus)
    {
        foreach (var dep in task.Deps)
        {
            if (src.Contains(dep))
            {
                var depTask = src[dep];
                if (depTask is null)
                {
                    bus.Send(new UnhandledErrorMessage(new Error($"Unable to find task {dep}")));
                    continue;
                }

                if (!dest.Contains(depTask))
                    dest.Add(depTask);

                FlattenTasks(depTask, src, dest, bus);
            }
            else
            {
                bus.Send(new UnhandledErrorMessage(new Error($"Unable to find task {dep}")));
            }
        }
    }

    private async Task<IReadOnlyList<ITaskResult>> HandleTasksAsync(
        ITaskCollection tasks,
        ITaskContext context,
        IMessageBus messageBus,
        int timeout = 0,
        CancellationToken cancellationToken = default)
    {
        var nextState = new TaskContext(context);
        var results = new List<ITaskResult>();
        var failed = false;
        foreach (var task in tasks)
        {
            var lastState = nextState;
            var nextContext = new TaskContext(lastState);
            var state = new TaskState(task);
            state.Timeout = 0;
            if (task.Timeout is not null)
            {
                var r = await task.Timeout(nextContext, cancellationToken).NoCap();
                if (r.IsError)
                {
                    nextContext.MessageBus.Send(new UnhandledErrorMessage(r.UnwrapError()));
                }
                else
                {
                    state.Timeout = r.Unwrap();
                }
            }

            if (task.Force is not null)
            {
                var r = await task.Force(nextContext, cancellationToken).NoCap();
                if (r.IsError)
                {
                    nextContext.MessageBus.Send(new UnhandledErrorMessage(r.UnwrapError()));
                }
                else
                {
                    state.Force = r.Unwrap();
                }
            }

            if (task.Skip is not null)
            {
                var r = await task.Skip(nextContext, cancellationToken).NoCap();
                if (r.IsError)
                {
                    nextContext.MessageBus.Send(new UnhandledErrorMessage(r.UnwrapError()));
                }
                else
                {
                    state.Skip = r.Unwrap();
                }
            }

            nextContext.State = state;
            nextContext.Tasks[task.Id] = state;

            var force = state.Force;
            if (cancellationToken.IsCancellationRequested && !force)
            {
                state.Status = ExecutionStatus.Cancelled;
                var result = new TaskResult()
                {
                    Status = ExecutionStatus.Cancelled,
                    StartedAt = DateTimeOffset.UtcNow,
                    EndedAt = DateTimeOffset.UtcNow,
                    Task = task,
                };

                results.Add(result);
                messageBus.Send(new TaskCancellationMessage(result, null));
                return results;
            }

            if (state.Skip)
            {
                var result = new TaskResult()
                {
                    Status = ExecutionStatus.Skipped,
                    StartedAt = DateTimeOffset.UtcNow,
                    EndedAt = DateTimeOffset.UtcNow,
                    Task = task,
                };

                state.Status = ExecutionStatus.Skipped;
                results.Add(result);
                messageBus.Send(new TaskSkippedMessage(result, null));
                continue;
            }

            if (failed && !force)
            {
                var result = new TaskResult()
                {
                    Status = ExecutionStatus.Skipped,
                    StartedAt = DateTimeOffset.UtcNow,
                    EndedAt = DateTimeOffset.UtcNow,
                    Task = task,
                };
                state.Status = ExecutionStatus.Skipped;
                results.Add(result);
                messageBus.Send(new TaskSkippedMessage(result, "Previous task failed"));
                continue;
            }

            context.MessageBus.Send(new TaskStartMessage(task));
            var to = state.Timeout ?? timeout;
            var ctc = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if (to > 0)
            {
                ctc.CancelAfter(to * 1000);
            }

            var ct = ctc.Token;

            try
            {
                var result = await this.HandleTaskAsync(task, context, ct).NoCap();
                result.Task = task;
                var envFile = Env.Get("DEX_ENV");
                try
                {
                    if (envFile is not null && Fs.FileExists(envFile))
                    {
#if !NETLEGACY
                        var content = await Fs.ReadTextFileAsync(envFile, cancellationToken: ct)
                            .NoCap();

                        if (content.Length > 0)
                        {
                            var envDoc = DotEnv.ParseDocument(content);
                            foreach (var (key, value) in envDoc.AsNameValuePairEnumerator())
                            {
                                nextContext.Env[key] = value;
                                Env.Set(key, value);
                            }

                            await Fs.WriteTextFileAsync(envFile, string.Empty, cancellationToken: ct);
                        }
#else
                        var content = Fs.ReadTextFile(envFile);

                        if (content.Length > 0)
                        {
                            var envDoc = DotEnv.ParseDocument(content.AsSpan());
                            foreach (var (key, value) in envDoc.AsNameValuePairEnumerator())
                            {
                                nextContext.Env[key] = value;
                                Env.Set(key, value);
                            }

                            Fs.WriteTextFile(envFile, string.Empty);
                        }
#endif
                    }
                }
                catch (Exception ex)
                {
                    context.MessageBus.Send(new UnhandledExceptionMessage(ex));
                }

                results.Add(result);
                state.Status = result.Status;
                switch (result.Status)
                {
                    case ExecutionStatus.Timeout:
                        context.MessageBus.Send(new TaskTimeoutMessage(result, to));
                        break;

                    case ExecutionStatus.Cancelled:
                        context.MessageBus.Send(new TaskCancellationMessage(result, null));
                        break;

                    default:
                        if (result.Status == ExecutionStatus.Failed)
                            failed = true;

                        messageBus.Send(new TaskEndMessage(result));
                        break;
                }
            }
            catch (Exception ex)
            {
                failed = true;
                var result = new TaskResult()
                {
                    Status = ExecutionStatus.Failed,
                    StartedAt = DateTimeOffset.UtcNow,
                    EndedAt = DateTimeOffset.UtcNow,
                    Task = task,
                    Error = new ExceptionError(ex),
                };

                results.Add(result);
                state.Status = result.Status;
                messageBus.Send(new TaskEndMessage(result));
            }
        }

        return results;
    }

    private async Task<TaskResult> HandleTaskAsync(ITask task, ITaskContext context, CancellationToken cancellationToken)
    {
        var start = DateTimeOffset.UtcNow;
        var result = new TaskResult()
        {
            Status = ExecutionStatus.Ok,
            StartedAt = start,
            EndedAt = DateTimeOffset.UtcNow,
        };

        if (cancellationToken.IsCancellationRequested)
        {
            result.Status = ExecutionStatus.Cancelled;
            return result;
        }

        try
        {
            var r2 = await task.RunAsync(context, cancellationToken)
                .NoCap();

            result.EndedAt = DateTimeOffset.UtcNow;
            if (r2.IsOk)
            {
                result.Status = ExecutionStatus.Ok;
                var output = r2.Unwrap();
                switch (output)
                {
                    case null:
                    case Nil:
                        return result;
                    case PsOutput psOutput when psOutput.ExitCode != 0:
                        {
                            result.Status = ExecutionStatus.Failed;
                            var ex = new ProcessException(psOutput.ExitCode, psOutput.FileName);
                            result.Error = new ExceptionError(ex);
                        }

                        break;
                    case PsOutput psOutput:
                        context.State.Outputs["ExitCode"] = psOutput.ExitCode;
                        context.State.Outputs["StdOut"] = psOutput.StdOut;
                        context.State.Outputs["StdErr"] = psOutput.StdError;
                        context.State.Outputs["FileName"] = psOutput.FileName;
                        break;
                    case IDictionary<string, object?> data:
                        {
                            foreach (var kvp in data)
                            {
                                context.State.Outputs[kvp.Key] = kvp.Value;
                            }

                            break;
                        }

                    case IReadOnlyDictionary<string, object?> data:
                        {
                            foreach (var kvp in data)
                            {
                                context.State.Outputs[kvp.Key] = kvp.Value;
                            }

                            break;
                        }

                    default:
                        context.State.Outputs["Result"] = output;
                        break;
                }
            }
            else
            {
                result.Status = ExecutionStatus.Failed;
                result.Error = r2.UnwrapError();
            }
        }
        catch (AggregateException ex)
        {
            var handled = false;
            foreach (var next in ex.InnerExceptions)
            {
                if (next is TimeoutException)
                {
                    result.Status = ExecutionStatus.Timeout;
                    result.Error = new ExceptionError(next);
                    handled = true;
                    break;
                }

                if (next is TaskCanceledException)
                {
                    result.Status = ExecutionStatus.Cancelled;
                    result.Error = new ExceptionError(next);
                    handled = true;
                    break;
                }
            }

            if (!handled)
            {
                result.Status = ExecutionStatus.Failed;
                result.Error = new ExceptionError(ex);
            }
        }
        catch (TaskCanceledException ex)
        {
            result.Status = ExecutionStatus.Cancelled;
            result.Error = new ExceptionError(ex);
        }
        catch (TimeoutException ex)
        {
            result.Status = ExecutionStatus.Timeout;
            result.Error = new ExceptionError(ex);
        }
        catch (Exception ex)
        {
            result.Status = ExecutionStatus.Failed;
            result.Error = new ExceptionError(ex);
        }

        return result;
    }
}