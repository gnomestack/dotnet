using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestCollectionRunner : XunitTestCollectionRunner
{
    private readonly IServiceProvider? serviceProvider;
    private readonly IMessageSink diagnosticMessageSink;
    private IDisposable? scope;

    public GnomeStackTestCollectionRunner(
        IServiceProvider? serviceProvider,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
        : base(
            testCollection,
            testCases,
            diagnosticMessageSink,
            messageBus,
            testCaseOrderer,
            aggregator,
            cancellationTokenSource)
    {
        this.serviceProvider = serviceProvider;
        this.diagnosticMessageSink = diagnosticMessageSink;
    }

    /// <inheritdoc />
    protected override void CreateCollectionFixture(Type fixtureType)
    {
        var scopedServiceProvider = this.serviceProvider;
        if (this.scope is null && this.serviceProvider?.GetService(typeof(IServiceProviderLifetimeFactory))
                is IServiceProviderLifetimeFactory factory)
        {
            var lifetime = factory.CreateLifetime();
            scopedServiceProvider = lifetime.ServiceProvider;
        }

        var fixture = scopedServiceProvider?.GetService(fixtureType);

        if (fixture != null)
        {
            this.Aggregator.Run(() => this.CollectionFixtureMappings[fixtureType] = fixture);
            return;
        }

        var ctors = fixtureType.GetTypeInfo()
            .DeclaredConstructors
            .Where(ci => !ci.IsStatic && ci.IsPublic)
            .ToList();

        if (ctors.Count != 1)
        {
            this.Aggregator.Add(
                new TestClassException($"Collection fixture type '{fixtureType.FullName}' may only define a single public constructor."));
            return;
        }

        var ctor = ctors[0];
        var missingParameters = new List<ParameterInfo>();
        var ctorArgs = ctor.GetParameters().Select(p =>
        {
            object? arg = null;
            if (p.ParameterType == typeof(IMessageSink))
            {
                arg = this.DiagnosticMessageSink;
            }
            else
            {
                var obj = this.serviceProvider?.GetService(p.ParameterType);
                if (obj == null)
                    missingParameters.Add(p);
            }

            return arg;
        }).ToArray();

        if (missingParameters.Count > 0)
        {
            this.Aggregator.Add(new TestClassException(
                $"Collection fixture type '{fixtureType.FullName}' had one or more unresolved constructor arguments:"
                + $" {string.Join(", ", missingParameters.Select(p => $"{p.ParameterType.Name} {p.Name}"))}"));
        }
        else
        {
            this.Aggregator.Run(() => this.CollectionFixtureMappings[fixtureType] = ctor.Invoke(ctorArgs));
        }
    }

    /// <inheritdoc/>
    protected override async Task BeforeTestCollectionFinishedAsync()
    {
        await base.BeforeTestCollectionFinishedAsync().ConfigureAwait(false);

        this.scope?.Dispose();
        this.scope = null;
    }

    /// <inheritdoc />
    protected override Task<RunSummary> RunTestClassAsync(
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases)
    {
        return new GnomeStackTestClassRunner(
            this.serviceProvider,
            testClass,
            @class,
            testCases,
            this.diagnosticMessageSink,
            this.MessageBus,
            this.TestCaseOrderer,
            new ExceptionAggregator(this.Aggregator),
            this.CancellationTokenSource,
            this.CollectionFixtureMappings).RunAsync();
    }
}