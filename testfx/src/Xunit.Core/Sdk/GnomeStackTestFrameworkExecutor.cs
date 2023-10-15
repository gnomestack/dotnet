using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestFrameworkExecutor : XunitTestFrameworkExecutor
{
    public GnomeStackTestFrameworkExecutor(
        AssemblyName assemblyName,
        ISourceInformationProvider sourceInformationProvider,
        IMessageSink diagnosticMessageSink)
        : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
    {
        GnomeStackTestDiscoverer.ServiceProviderLocator ??= new TestServiceProviderLocator();
    }

    /// <inheritdoc />
    [SuppressMessage("AsyncUsage", "AsyncFixer03:Fire-and-forget async-void methods or delegates", Justification = "Required by xunit api")]
    protected override async void RunTestCases(
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
    {
        var locator = GnomeStackTestDiscoverer.ServiceProviderLocator;
        IServiceProvider? serviceProvider = null;
        var exceptionList = new List<Exception>();
        try
        {
            serviceProvider = locator?.GetServiceProvider(this.TestAssembly.Assembly);
        }
        catch (Exception ex)
        {
            exceptionList.Add(ex);
        }

        using var runner = new GnomeStackTestAssemblyRunner(
            serviceProvider,
            this.TestAssembly,
            testCases,
            this.DiagnosticMessageSink,
            executionMessageSink,
            executionOptions,
            exceptionList);

        await runner.RunAsync().ConfigureAwait(false);
    }
}