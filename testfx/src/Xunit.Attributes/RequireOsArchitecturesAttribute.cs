using System;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

namespace Xunit;
#pragma warning disable REFL016
[System.AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    Inherited = false,
    AllowMultiple = true)]
public sealed class RequireOsArchitecturesAttribute : SkippableTraitAttribute
{
    private static readonly TestOsArchitectures s_arch = SetOsArch();

    public RequireOsArchitecturesAttribute(params string[] osArchitectures)
    {
        TestOsArchitectures arches = TestOsArchitectures.None;
        foreach (var arch in osArchitectures)
        {
            switch (arch)
            {
                case "Any":
                    this.TestOsArchitectures = TestOsArchitectures.None;
                    return;

                default:
                    if (Enum.TryParse<TestOsArchitectures>(arch, true, out var next))
                    {
                        if (arches == TestOsArchitectures.None)
                            arches = next;
                        else
                            arches |= next;
                    }

                    break;
            }
        }

        this.TestOsArchitectures = arches;
    }

    public RequireOsArchitecturesAttribute(TestOsArchitectures osArchitectures)
    {
        if (osArchitectures == TestOsArchitectures.None)
            return;

        this.TestOsArchitectures = osArchitectures;
    }

    public TestOsArchitectures TestOsArchitectures { get; set; }

    public override string? GetSkipReason(IMessageSink sink, ITestMethod testMethod, IAttributeInfo attributeInfo)
    {
        if (this.TestOsArchitectures == 0)
            return null;

        if (this.TestOsArchitectures.HasFlag(s_arch))
            return null;

        return $"Requires OS Architectures: {this.TestOsArchitectures}";
    }

    private static TestOsArchitectures SetOsArch()
    {
        string? arch = Environment.Is64BitOperatingSystem ? "X64" : "X86";
        try
        {
            var t = typeof(System.Runtime.InteropServices.RuntimeInformation);
            var osArchProp = t.GetProperty("OSArchitecture", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (osArchProp != null)
            {
                var runtimeArch = osArchProp.GetValue(null);
                if (runtimeArch != null)
                {
                    arch = runtimeArch.ToString();
                }
            }
        }
        catch
        {
            arch = Environment.Is64BitOperatingSystem ? "X64" : "X86";
        }

        if (Enum.TryParse<TestOsArchitectures>(arch, true, out var next))
        {
            return next;
        }
        else
        {
            return Environment.Is64BitOperatingSystem ? TestOsArchitectures.X64 : TestOsArchitectures.X86;
        }
    }
}