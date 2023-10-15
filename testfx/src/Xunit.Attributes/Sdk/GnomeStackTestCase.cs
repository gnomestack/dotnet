using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

/// <summary>Wraps another test case that should be skipped.</summary>
public class GnomeStackTestCase : XunitTestCase
{
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public GnomeStackTestCase()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GnomeStackTestCase"/> class.
    /// </summary>
    /// <param name="skipReason">The reason to skip the test case.</param>
    /// <param name="traits">The traits (attributes) for the test case.</param>
    /// <param name="serviceProviderLocator">The service provider locator.</param>
    /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages.</param>
    /// <param name="defaultMethodDisplay">Default method display to use (when not customized).</param>
    /// <param name="defaultMethodDisplayOptions">Default method display options to use (when not customized).</param>
    /// <param name="testMethod">The test method this test case belongs to.</param>
    /// <param name="testMethodArguments">The arguments for the test method.</param>
    public GnomeStackTestCase(
        string? skipReason,
        IEnumerable<KeyValuePair<string, List<string?>>>? traits,
        ITestServiceProviderLocator? serviceProviderLocator,
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod,
        object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
        this.SkipReason = skipReason;
        this.ServiceProvider = serviceProviderLocator?.GetServiceProvider(this.TestMethod);

        if (traits is null)
            return;

        foreach (var kvp in traits)
        {
            this.Traits.Add(kvp.Key, kvp.Value);
        }
    }

    protected IServiceProvider? ServiceProvider { get; private set; }

    public Task<RunSummary> RunAsync(
        IServiceProvider? serviceProvider,
        ITestServiceProviderLocator? locator,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object?[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
    {
        this.ServiceProvider = locator?.GetServiceProvider(this.TestMethod, serviceProvider);

        return this.RunAsync(
            diagnosticMessageSink,
            messageBus,
            constructorArguments,
            aggregator,
            cancellationTokenSource);
    }

    public override async Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object?[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
    {
        // use the SkippedTestMessageBus here to intercept any exceptions
        // that use the DynamicSkipToken.Value.
        var messageBusInterceptor = new SkippedTestMessageBus(messageBus);
        RunSummary summary;

        // DI is not enable, fallback to xunit style tests but keep the
        // benefit of skippable tests.
        if (GnomeStackTestDiscoverer.ServiceProviderLocator is null)
        {
            summary = await base.RunAsync(
                    diagnosticMessageSink,
                    messageBusInterceptor,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource)
                .ConfigureAwait(false);

            // swap the failed/skipped count if any tests DynamicSkipToken.Values were found.
            summary.Failed -= messageBusInterceptor.SkippedTestCount;
            summary.Skipped += messageBusInterceptor.SkippedTestCount;
            return summary;
        }

        using var runner = new GnomeStackTestCaseRunner(
            this.ServiceProvider,
            this,
            this.DisplayName,
            this.SkipReason,
            constructorArguments,
            this.TestMethodArguments,
            messageBusInterceptor,
            aggregator,
            cancellationTokenSource);

        summary = await runner
            .RunAsync()
            .ConfigureAwait(false);

        // swap the failed/skipped count if any tests DynamicSkipToken.Values were found.
        summary.Failed -= messageBusInterceptor.SkippedTestCount;
        summary.Skipped += messageBusInterceptor.SkippedTestCount;

        return summary;
    }

    public override void Deserialize(IXunitSerializationInfo data)
    {
        base.Deserialize(data);
        this.SkipReason = data.GetValue<string>(nameof(this.SkipReason));

        var keys = data.GetValue<string[]>("traitNames");

        foreach (var key in keys)
        {
            var values = data.GetValue<string[]>(key);
            this.Traits.Add(key, new List<string>(values));
        }
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
        base.Serialize(data);
        data.AddValue(nameof(this.SkipReason), this.SkipReason);

        data.AddValue("traitNames", this.Traits.Keys.ToArray());
        foreach (var trait in this.Traits.Keys)
        {
            data.AddValue(trait, this.Traits[trait].ToArray());
        }
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
        => this.SkipReason ?? base.GetSkipReason(factAttribute);
}