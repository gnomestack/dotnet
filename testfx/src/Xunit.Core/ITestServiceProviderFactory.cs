using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit;

public interface ITestServiceProviderFactory
{
    IServiceProvider CreateServiceProvider();
}