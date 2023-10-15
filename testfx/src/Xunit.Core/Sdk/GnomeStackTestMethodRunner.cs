using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

/// <summary>
/// GnomeStack test method runner for xunit v2 tests. The test method runner creates
/// the test case runner and invokes it.
/// </summary>
public class GnomeStackTestMethodRunner : TestMethodRunner<IXunitTestCase>
{
    private readonly IServiceProvider? serviceProvider;
    private readonly IMessageSink diagnosticMessageSink;
    private readonly object?[] constructorArguments;

    /// <summary>
    /// Initializes a new instance of the <see cref="GnomeStackTestMethodRunner"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for injecting dependencies.</param>
    /// <param name="testMethod">The test method to be run.</param>
    /// <param name="class">The test class that contains the test method.</param>
    /// <param name="method">The test method that contains the tests to be run.</param>
    /// <param name="testCases">The test cases to be run.</param>
    /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages to.</param>
    /// <param name="messageBus">The message bus to report run status to.</param>
    /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
    /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
    /// <param name="constructorArguments">The constructor arguments for the test class.</param>
    public GnomeStackTestMethodRunner(
        IServiceProvider? serviceProvider,
        ITestMethod testMethod,
        IReflectionTypeInfo @class,
        IReflectionMethodInfo method,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        object?[] constructorArguments)
        : base(testMethod, @class, method, testCases, messageBus, aggregator, cancellationTokenSource)
    {
        this.serviceProvider = serviceProvider;
        this.diagnosticMessageSink = diagnosticMessageSink;
        this.constructorArguments = constructorArguments;
    }

    /// <inheritdoc />
    protected override Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
    {
        if (testCase is GnomeStackTestCase gnomeStackTestCase)
        {
            return gnomeStackTestCase.RunAsync(
                this.serviceProvider,
                GnomeStackTestDiscoverer.ServiceProviderLocator,
                this.diagnosticMessageSink,
                this.MessageBus,
                this.constructorArguments,
                new ExceptionAggregator(this.Aggregator),
                this.CancellationTokenSource);
        }

        return testCase.RunAsync(
            this.diagnosticMessageSink,
            this.MessageBus,
            this.constructorArguments,
            new ExceptionAggregator(this.Aggregator),
            this.CancellationTokenSource);
    }
}