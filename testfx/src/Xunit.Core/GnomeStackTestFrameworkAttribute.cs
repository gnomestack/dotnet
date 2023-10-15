using System;

using Xunit.Sdk;

namespace Xunit;

[TestFrameworkDiscoverer(
    "Xunit.Sdk.GnomeStackTestFrameworkTypeDiscoverer",
    "GnomeStack.Xunit.Core")]
[AttributeUsage(
    System.AttributeTargets.Assembly,
    Inherited = false,
    AllowMultiple = false)]
public sealed class GnomeStackTestFrameworkAttribute : System.Attribute,
    ITestFrameworkAttribute
{
    public GnomeStackTestFrameworkAttribute()
    {
    }

    public Type? CustomFrameworkType { get; set; }
}