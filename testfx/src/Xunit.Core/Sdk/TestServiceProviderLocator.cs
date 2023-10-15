using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Sdk;

public class TestServiceProviderLocator : ITestServiceProviderLocator
{
    internal static readonly IServiceProvider DefaultServiceProvider =
        new DefaultServiceProviderFactory().CreateServiceProvider();

    private readonly ConcurrentDictionary<Type, IServiceProvider?> cache = new();

    public IServiceProvider? GetServiceProvider(IAssemblyInfo assemblyInfo)
    {
        var attributes = assemblyInfo.GetCustomAttributes(typeof(UseServiceProviderFactoryAttribute));
        var attribute = attributes.FirstOrDefault();

        if (attribute == null)
            return DefaultServiceProvider;

        return this.GetServiceProvider(attribute);
    }

    public IServiceProvider? GetServiceProvider(ITestClass @class)
    {
        var attributes = @class.Class.GetCustomAttributes(typeof(UseServiceProviderFactoryAttribute));
        var attribute = attributes.FirstOrDefault();

        if (attribute == null)
            return null;

        return this.GetServiceProvider(attribute);
    }

    public IServiceProvider? GetServiceProvider(ITestMethod method)
    {
        var attributes = method.Method.GetCustomAttributes(typeof(UseServiceProviderFactoryAttribute));
        var attribute = attributes.FirstOrDefault();
        if (attribute == null)
        {
            attributes = method.TestClass.Class.GetCustomAttributes(typeof(UseServiceProviderFactoryAttribute));
            attribute = attributes.FirstOrDefault();

            if (attribute == null)
                return null;
        }

        return this.GetServiceProvider(attribute);
    }

    public IServiceProvider? GetServiceProvider(IAttributeInfo attributeInfo)
    {
        var serviceProviderFactoryType = attributeInfo.GetNamedArgument<Type>(nameof(UseServiceProviderFactoryAttribute.FactoryType));
        return this.GetServiceProvider(serviceProviderFactoryType);
    }

    public IServiceProvider? GetServiceProvider(Type? serviceProviderFactoryType)
        => this.GetServiceProvider(serviceProviderFactoryType, null);

    public IServiceProvider? GetServiceProvider(Type? serviceProviderFactoryType, IMessageSink? sink)
    {
        if (serviceProviderFactoryType is null)
            return DefaultServiceProvider;

        if (this.cache.TryGetValue(serviceProviderFactoryType, out var serviceProvider) && serviceProvider != null)
            return serviceProvider;

        try
        {
            if (Activator.CreateInstance(serviceProviderFactoryType) is ITestServiceProviderFactory factory)
            {
                serviceProvider = factory.CreateServiceProvider();
                this.cache.TryAdd(serviceProviderFactoryType, serviceProvider);
                return serviceProvider;
            }
        }
        catch (Exception ex)
        {
            sink?.OnMessage(new DiagnosticMessage(
                $"exception thrown when creating serviceProvider from {serviceProviderFactoryType.FullName} {ex.Message} {ex.StackTrace}"));
        }

        this.cache.TryAdd(serviceProviderFactoryType, DefaultServiceProvider);
        return DefaultServiceProvider;
    }

    public IServiceProvider? GetServiceProvider(ITestMethod method, IServiceProvider? currentServiceProvider)
    {
        var serviceProviderOverride = this.GetServiceProvider(method);
        IServiceProvider? serviceProvider = null;

        if (serviceProviderOverride != null)
        {
            serviceProvider = serviceProviderOverride;
        }

        if (serviceProvider == null)
        {
            serviceProviderOverride = this.GetServiceProvider(method.TestClass);
            if (serviceProviderOverride != null)
                serviceProvider = serviceProviderOverride;
        }

        serviceProvider ??= currentServiceProvider;

        return serviceProvider;
    }
}