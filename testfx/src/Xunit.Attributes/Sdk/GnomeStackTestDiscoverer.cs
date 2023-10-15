using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestDiscoverer : FactDiscoverer
{
    private static bool? isCi = null;

    private static bool? isContainer = null;

    public GnomeStackTestDiscoverer(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    public static ITestServiceProviderLocator? ServiceProviderLocator { get; set; }

    public static bool IsContainer
    {
        get
        {
            if (isContainer is not null)
            {
                return isContainer.Value;
            }

            if (Environment.GetEnvironmentVariable("TEST_CONTAINER")
                    ?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
            {
                isContainer = true;
                return true;
            }

            if (System.IO.File.Exists("/proc/1/cgroup"))
            {
                var cgroup = System.IO.File.ReadAllText("/proc/1/cgroup");
                if (cgroup.Contains("docker") || cgroup.Contains("kubepods"))
                {
                    isContainer = true;
                    return true;
                }
            }

            isContainer = false;
            return false;
        }
    }

    public static bool IsCi
    {
        get
        {
            if (isCi is not null)
            {
                return isCi.Value;
            }

            var ci = Environment.GetEnvironmentVariable("CI") ??
                     Environment.GetEnvironmentVariable("TF_BUILD");
            if (ci?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
            {
                isCi = true;
                return true;
            }

            if (Environment.GetEnvironmentVariable("JENKINS_URL") != null)
            {
                isCi = true;
                return true;
            }

            isCi = false;

            return isCi.Value;
        }
    }

    public override IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo factAttribute)
    {
        if (factAttribute is not ReflectionAttributeInfo { Attribute: TestAttribute })
            return base.Discover(discoveryOptions, testMethod, factAttribute);

        IXunitTestCase testCase;

        if (testMethod.Method.IsGenericMethodDefinition)
        {
            testCase = this.ErrorTestCase(
                discoveryOptions,
                testMethod,
                "[Fact] methods are not allowed to be generic.");
        }
        else
        {
            testCase = this.CreateTestCase(discoveryOptions, testMethod, factAttribute);
        }

        return new[] { testCase };
    }

    protected override IXunitTestCase CreateTestCase(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo factAttribute)
    {
        var category = factAttribute.GetNamedArgument<string?>("Category");
        var tags = factAttribute.GetNamedArgument<string[]?>("Tags");
        var ticketId = factAttribute.GetNamedArgument<string?>("TicketId");
        var longRunning = factAttribute.GetNamedArgument<bool>("LongRunning");
        var localOnly = factAttribute.GetNamedArgument<bool>("LocalOnly");
        var unsafeTest = factAttribute.GetNamedArgument<bool>("Unsafe");
        var traits = new Dictionary<string, List<string?>>();

        if (!category.IsNullOrWhiteSpace())
            traits.Add("tags", category);

        if (localOnly)
            traits.Add("tags", "local-only");

        if (unsafeTest)
            traits.Add("tags", "unsafe");

        if (tags is { Length: > 0 })
        {
            foreach (var tag in tags)
            {
                if (string.IsNullOrEmpty(tag))
                    continue;

                traits.Add("tags", tag);
            }
        }

        if (longRunning)
            traits.Add("tags", "long-running");

        if (!ticketId.IsNullOrWhiteSpace())
            traits.Add("ticketId", ticketId);

        var attrs = testMethod.Method.GetCustomAttributes(typeof(SkippableTraitAttribute));
        var sb = new StringBuilder();
        foreach (var skippableAttr in attrs)
        {
            if (skippableAttr is not ReflectionAttributeInfo reflect)
            {
                continue;
            }

            if (reflect.Attribute is not SkippableTraitAttribute attr)
                continue;

            var nextReason = attr.GetSkipReason(this.DiagnosticMessageSink, testMethod, factAttribute);
            if (!string.IsNullOrWhiteSpace(nextReason))
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(nextReason);
            }
        }

        if (localOnly && !IsCi)
        {
            if (sb.Length > 0)
                sb.Append(". ");

            sb.Append("Local only test is skipped in the CI pipeline");
        }

        if (unsafeTest && !IsContainer)
        {
            if (sb.Length > 0)
                sb.Append(". ");

            sb.Append("Unsafe test is skipped unless it runs in a container");
        }

        var skipReason = sb.ToString();
        var test = new GnomeStackTestCase(
            skipReason,
            traits,
            ServiceProviderLocator,
            this.DiagnosticMessageSink,
            discoveryOptions.MethodDisplayOrDefault(),
            discoveryOptions.MethodDisplayOptionsOrDefault(),
            testMethod);

        return test;
    }

    private ExecutionErrorTestCase ErrorTestCase(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        string message) =>
        new(
            this.DiagnosticMessageSink,
            discoveryOptions.MethodDisplayOrDefault(),
            discoveryOptions.MethodDisplayOptionsOrDefault(),
            testMethod,
            message);
}