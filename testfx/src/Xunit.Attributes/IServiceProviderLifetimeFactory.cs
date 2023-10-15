using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.Sdk;

public interface IServiceProviderLifetimeFactory
{
    IServiceProviderLifetime CreateLifetime();
}