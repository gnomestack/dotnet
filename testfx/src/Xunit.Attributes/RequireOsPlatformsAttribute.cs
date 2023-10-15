using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit;

[System.AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    Inherited = false,
    AllowMultiple = true)]
public sealed class RequireOsPlatformsAttribute : SkippableTraitAttribute
{
    public RequireOsPlatformsAttribute(TestOsPlatforms osPlatforms)
    {
        if (osPlatforms == 0)
        {
            this.TestOsPlatforms = TestOsPlatforms.None;
            return;
        }

        this.TestOsPlatforms = osPlatforms;
    }

    public RequireOsPlatformsAttribute(params string[] osPlatforms)
    {
        TestOsPlatforms platforms = TestOsPlatforms.None;
        foreach (var platform in osPlatforms)
        {
            switch (platform)
            {
                case "Any":
                    this.TestOsPlatforms = TestOsPlatforms.None;
                    return;

                default:
                    if (Enum.TryParse<TestOsPlatforms>(platform, out var next))
                    {
                        if (platforms == TestOsPlatforms.None)
                            platforms = next;
                        else
                            platforms |= next;
                    }

                    break;
            }
        }

        this.TestOsPlatforms = platforms;
    }

    public TestOsPlatforms TestOsPlatforms { get; set; }

    public override string? GetSkipReason(IMessageSink sink, ITestMethod testMethod, IAttributeInfo attributeInfo)
    {
        return !DiscovererHelpers.TestPlatformApplies(this.TestOsPlatforms) ? $"Requires OS: {this.TestOsPlatforms}" : null;
    }
}