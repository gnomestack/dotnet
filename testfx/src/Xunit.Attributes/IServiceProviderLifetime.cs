using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.Sdk;

public interface IServiceProviderLifetime : IDisposable
{
    IServiceProvider ServiceProvider { get; }
}