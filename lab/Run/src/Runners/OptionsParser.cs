using GnomeStack.Functional;
using GnomeStack.Run.Execution;
using GnomeStack.Run.Messaging;
using GnomeStack.Standard;

namespace GnomeStack.Run.Runners;

public class OptionsParser : IOptionsParser
{
    public IRunnerOptions Parse(string[] args, IMessageBus bus)
    {
        var env = new Dictionary<string, string?>();
        var commands = new List<string>();
        foreach (var key in Env.Vars.Keys)
        {
            env[key] = Env.Vars[key];
        }

        var options = new RunnerOptions();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            switch (arg)
            {
                case "-h":
                case "--help":
                    options.Help = true;
                    break;

                case "--trace":
                    options.Verbosity = Verbosity.Trace;
                    break;

                case "--debug":
                    options.Verbosity = Verbosity.Debug;
                    break;

                case "--":
                    options.Env = env;
                    if (commands.Count == 0)
                        commands.Add("default");
                    options.Commands = commands;
                    options.RemainingArguments = args.Skip(i + 1).ToList();

                    return options;

                case "-v":
                case "--version":
                    options.Version = true;
                    break;

                case "-l":
                case "--list":
                    options.List = true;
                    break;

                case "--skip-deps":
                case "-s":
                    options.SkipDeps = true;
                    break;

                case "-t":
                case "--timeout":
                    if (i + 1 < args.Length)
                    {
                        var timeoutArg = args[++i];
                        if (int.TryParse(timeoutArg, out var timeout))
                        {
                            options.Timeout = timeout;
                        }
                        else
                        {
                            bus.Send(new UnhandledErrorMessage(new Error($"Unable to parse timeout {timeoutArg} as int")));
                        }
                    }

                    break;

                case "-e":
                case "--env":
                    if (i + 1 < args.Length)
                    {
                        var envArg = args[++i];
                        var parts = envArg.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            env[parts[0]] = parts[1].Trim(new[] { '\'', '"' });
                        }
                    }

                    break;

                case "--env-file":
                case "--ef":
                    if (i + 1 < args.Length)
                    {
                        var envFile = args[++i];
                        if (File.Exists(envFile))
                        {
                            var content = Fs.ReadTextFile(envFile);
                            var envDoc = DotEnv.ParseDocument(content.AsSpan());
                            foreach (var kvp in envDoc.AsNameValuePairEnumerator())
                            {
                                var key = kvp.Name;
                                var value = kvp.Value;
                                Env.Expand(value, new EnvExpandOptions()
                                {
                                    GetVariable = (name) =>
                                    {
                                        if (env.TryGetValue(name, out var next))
                                            return next;

                                        if (Env.TryGet(name, out next))
                                            return next;

                                        return null;
                                    },
                                });

                                env[key] = value;
                            }
                        }
                        else
                        {
                            bus.Send(new UnhandledErrorMessage(new Error($"Unable to find env file {envFile}")));
                        }
                    }

                    break;

                case "--cwd":
                    if (i + 1 < args.Length)
                    {
                        var cwd = args[++i];
                        if (Directory.Exists(cwd))
                        {
                            options.Cwd = cwd;
                        }
                        else
                        {
                            bus.Send(new UnhandledErrorMessage(new Error($"Unable to find cwd {cwd}")));
                        }
                    }

                    break;

                default:
                    {
                        if (arg[0] != '-')
                        {
                            commands.Add(arg);
                            continue;
                        }

                        if (i + 1 < args.Length)
                        {
                            var next = args[i + 1];
                            if (next[0] != '-')
                            {
                                options.Options[arg] = next;
                                i++;
                            }
                            else
                            {
                                options.Options[arg] = true;
                            }
                        }
                        else
                        {
                            options.Options[arg] = true;
                        }
                    }

                    break;
            }
        }

        if (commands.Count == 0)
            commands.Add("default");
        options.Env = env;
        options.Commands = commands;
        return options;
    }

    private sealed class RunnerOptions : IRunnerOptions
    {
        public IReadOnlyList<string> Commands { get; set;  }
            = new List<string>();

        public IReadOnlyList<string> RemainingArguments { get; set; }
            = new List<string>();

        public IDictionary<string, object?> Options { get; set; } =
            new Dictionary<string, object?>();

        public IReadOnlyDictionary<string, string?> Env { get; set; } =
            new Dictionary<string, string?>();

        public Verbosity Verbosity { get; set; }

        public bool SkipDeps { get; set; }

        public bool List { get; set; }

        public int? Timeout { get; set; }

        public bool Help { get; set; }

        public string? Cwd { get; set; }

        public bool Version { get; set; }
    }
}