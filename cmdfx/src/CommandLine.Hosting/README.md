# GnomeStack.Extensions.Hosting.CommandLine

Provides a CommandLineApplicationBuilder fashioned after the HostApplicationBuilder
to make it easier to build a command line application.

## Usage

```csharp
var builder = new CommandLineApplicationBuilder();
builder.UseDefaults();

// these are all available for configuration similar to 
// the generic host app builder and web app builder.

// builder.Services
// builder.Configuration
// builder.Environment
// builder.Logging
// builder.RootCommand 

builder.Services.AddLogging(lb =>
{
    lb.ClearProviders();
    lb.AddSerilog(appLog);
})

builder.Configuration.AddJsonFile(
    Path.Join(Paths.ConfigDirectory, "config.json"), true, false);

// addes this commands to the root command
builder.AddCommand(new ComposeCommand());
builder.AddCommand(new TasksCommand());
builder.AddCommand(new InitCommand());

// registers a handler class that implements ICommandHandler
builder.UseCommandHandler<SubCommand, SubCommandHandler>();

var app = builder.Build();

return await app.Parse(args).InvokeAsync();
```

MIT License
