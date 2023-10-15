using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

internal class DefaultServiceProviderFactory : ITestServiceProviderFactory
{
    public IServiceProvider CreateServiceProvider()
    {
        var serviceProvider = new SimpleServiceProvider();
        return serviceProvider;
    }
}