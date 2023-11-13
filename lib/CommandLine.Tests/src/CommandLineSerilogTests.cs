using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

using Xunit.Abstractions;

namespace Tests;

public class CommandLineSerilogTests
{
    [UnitTest]
    public void TestExpression(ITestOutputHelper writer)
    {
        var exp = "{#if @l <> 'Information'}[{@l:u3}]{#end} {@m}\n{@x}";
        var logger = new LoggerConfiguration()
            .UseMicrosoftFilter()
            .UseConsoleExpression(new[] { "--debug" })
            .WriteTo.TestOutput(writer, new ExpressionTemplate(exp, theme: TemplateTheme.Literate, applyThemeWhenOutputIsRedirected: true), restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        logger.Debug("Debugging");
        logger.Verbose("Verbose");
        logger.Warning("Warning");
        logger.Information("Hello, world!");
        Assert.NotNull(logger);
    }
}