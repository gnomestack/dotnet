using System;

using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
    AttributeTargets.Method,
    Inherited = false)]
[XunitTestCaseDiscoverer(Util.TestDiscoverer, Util.AssemblyName)]
public class TestAttribute : FactAttribute
{
    public TestAttribute()
        : this(TestCategories.Test)
    {
    }

    protected TestAttribute(string categoryName)
    {
        this.Category = categoryName;
    }

    public bool LongRunning { get; set; }

    public bool LocalOnly { get; set; }

    public bool Unsafe { get; set; }

    public string? TicketId { get; set; }

    public string[]? Tags { get; set; }

    protected internal string? Category { get; set; }
}