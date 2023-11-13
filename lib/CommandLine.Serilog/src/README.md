# GnomeStack.CommandLine.Serilog

Provides extension methods for Serilog LoggerConfiguration to enable
logging to the console and to a file.

There is an extension method that uses Serilog expressions to omit
logging formatting for "Information" level messages to the console
logger.  This is useful for logging to the console in a way that
is easy to read and to log to a file in a way that is easy to parse.

It will use the `args` from the `Main` method to determine the log level
by looking for flags like "--trace", "--debug" and "--verbose" and set
the log level for the console logger.

In the future, the library may emmit the extension methods in a given
requiring a hard dependency on the library.

## Usage Examples

```csharp
var config = new LoggerConfiguration()
    .UseMicrosoftFilter() // filters out Microsoft and System messages
    .UseConsoleExpression(args, theme: TemplateTheme.Code) // uses an expression to omit formatting for Information messages
    .UseFileLog("/path/to/logs"); // this will create two log files. One for errors and one for the rest of the messages.

var log = config.Build();
```

MIT License
