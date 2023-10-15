using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
    AttributeTargets.Method,
    Inherited = false)]
[XunitTestCaseDiscoverer(Util.TestDiscoverer, Util.AssemblyName)]
public class FunctionalTestAttribute : TestAttribute
{
    public FunctionalTestAttribute()
        : base(TestCategories.Functional)
    {
    }
}