using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests;

internal class DIFactoryA : ITestServiceProviderFactory
{
    public IServiceProvider CreateServiceProvider()
    {
        var services = new SimpleServiceProvider();
        services.AddTransient(typeof(ICustomService), _ => new MyCustomService());

        return services;
    }
}