using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestAssemblyRunner : XunitTestAssemblyRunner
{
    private readonly IServiceProvider? serviceProvider;

    public GnomeStackTestAssemblyRunner(
        IServiceProvider? serviceProvider,
        ITestAssembly testAssembly,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions,
        IEnumerable<Exception> exceptions)
        : base(
            testAssembly,
            testCases,
            diagnosticMessageSink,
            executionMessageSink,
            executionOptions)
    {
        this.serviceProvider = serviceProvider;

        foreach (var exception in exceptions)
            this.Aggregator.Add(exception);
    }

    /// <inheritdoc />
    protected override Task<RunSummary> RunTestCollectionAsync(
        IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource)
    {
        return new GnomeStackTestCollectionRunner(
                this.serviceProvider,
                testCollection,
                testCases,
                this.DiagnosticMessageSink,
                messageBus,
                this.TestCaseOrderer,
                new ExceptionAggregator(this.Aggregator),
                cancellationTokenSource)
            .RunAsync();
    }
}