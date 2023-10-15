using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
    AttributeTargets.Method,
    Inherited = false)]
[XunitTestCaseDiscoverer(Util.TestDiscoverer, Util.AssemblyName)]
public class IntegrationTestAttribute : TestAttribute
{
    public IntegrationTestAttribute()
        : base(TestCategories.Integration)
    {
    }
}