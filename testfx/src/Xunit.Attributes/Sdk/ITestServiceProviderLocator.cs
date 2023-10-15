using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

/// <summary>
/// The test <see cref="IServiceProvider"/> locator which looks for custom service providers
/// at the <see cref="ITestMethod"/>, <see cref="ITestClass"/>, and <see cref="ITestAssembly"/>
/// levels.
/// </summary>
public interface ITestServiceProviderLocator
{
    IServiceProvider? GetServiceProvider(IAssemblyInfo assemblyInfo);

    IServiceProvider? GetServiceProvider(ITestClass @class);

    IServiceProvider? GetServiceProvider(ITestMethod method);

    IServiceProvider? GetServiceProvider(ITestMethod method, IServiceProvider? currentServiceProvider);
}