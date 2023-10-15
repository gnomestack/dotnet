using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestClassRunner : XunitTestClassRunner
{
    private readonly IServiceProvider? serviceProvider;

    private readonly IDictionary<Type, object?> collectionFixtureMappings;

    private IDisposable? scope;

    public GnomeStackTestClassRunner(
        IServiceProvider? serviceProvider,
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IDictionary<Type, object?> collectionFixtureMappings)
        : base(
            testClass,
            @class,
            testCases,
            diagnosticMessageSink,
            messageBus,
            testCaseOrderer,
            aggregator,
            cancellationTokenSource,
            collectionFixtureMappings)
    {
        this.serviceProvider = serviceProvider;
        var locator = GnomeStackTestDiscoverer.ServiceProviderLocator;
        var localServiceProvider = locator?.GetServiceProvider(testClass);
        if (localServiceProvider != null)
        {
            this.serviceProvider = localServiceProvider;
        }

        this.collectionFixtureMappings = collectionFixtureMappings;
    }

    /// <inheritdoc />
    protected override object?[] CreateTestClassConstructorArguments()
    {
        if (this.serviceProvider?.GetService(typeof(ITestOutputHelperAccessor))
            is ITestOutputHelperAccessor container)
        {
            container.TestOutputHelper = new TestOutputHelper();
        }

        var classType = this.Class.Type.GetTypeInfo();

        if (classType.IsAbstract && classType.IsSealed)
            return Array.Empty<object>();

        var constructor = this.SelectTestClassConstructor();
        if (constructor == null)
            return Array.Empty<object?>();

        var parameters = constructor.GetParameters();

        var unusedArguments = new List<Tuple<int, ParameterInfo>>();
        object?[] constructorArguments = new object?[parameters.Length];
        for (var idx = 0; idx < parameters.Length; ++idx)
        {
            var parameter = parameters[idx];

            if (this.TryGetConstructorArgument(constructor, idx, parameter, out var argumentValue))
                constructorArguments[idx] = argumentValue;
            else if (parameter.HasDefaultValue)
                constructorArguments[idx] = parameter.DefaultValue;
            else if (parameter.IsOptional)
                constructorArguments[idx] = parameter.ParameterType.GetTypeInfo().GetDefaultValue();
            else if (parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
                constructorArguments[idx] = Array.CreateInstance(parameter.ParameterType, 0);
            else
                unusedArguments.Add(Tuple.Create(idx, parameter));
        }

        if (unusedArguments.Count > 0)
            this.Aggregator.Add(new TestClassException(this.FormatConstructorArgsMissingMessage(constructor, unusedArguments)));

        return constructorArguments;
    }

    /// <inheritdoc />
    protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object? argumentValue)
    {
        if (parameter.ParameterType == typeof(ITestOutputHelper))
        {
            if (this.serviceProvider?.GetService(typeof(ITestOutputHelperAccessor))
                is ITestOutputHelperAccessor container)
            {
                argumentValue = container.TestOutputHelper;
                return true;
            }

            if (this.serviceProvider?.GetService(typeof(ITestOutputHelper))
                is TestOutputHelper helper)
            {
                argumentValue = helper;
            }
            else
            {
                argumentValue = new TestOutputHelper();
            }

            return true;
        }

        if (parameter.ParameterType == typeof(IMessageSink))
        {
            argumentValue = this.DiagnosticMessageSink;
            return true;
        }

        if (parameter.ParameterType == typeof(CancellationToken))
        {
            argumentValue = this.CancellationTokenSource.Token;
            return true;
        }

        if (!base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue))
        {
            var value = this.serviceProvider?.GetService(parameter.ParameterType);
            return value is not null;
        }

        return true;
    }

    /// <inheritdoc />
    protected override void CreateClassFixture(Type fixtureType)
    {
        var scopedServiceProvider = this.serviceProvider;
        if (this.scope is null && this.serviceProvider?.GetService(typeof(IServiceProviderLifetimeFactory))
                is IServiceProviderLifetimeFactory factory)
        {
            var lifetime = factory.CreateLifetime();
            scopedServiceProvider = lifetime.ServiceProvider;
        }

        var fixture = scopedServiceProvider?.GetService(fixtureType);

        if (fixture != null)
        {
            this.Aggregator.Run(() => this.ClassFixtureMappings[fixtureType] = fixture);
            return;
        }

        var ctors = fixtureType.GetTypeInfo()
            .DeclaredConstructors
            .Where(ci => !ci.IsStatic && ci.IsPublic)
            .ToList();

        if (ctors.Count != 1)
        {
            this.Aggregator.Add(
                new TestClassException($"Class fixture type '{fixtureType.FullName}' may only define a single public constructor."));
            return;
        }

        var ctor = ctors[0];
        var missingParameters = new List<ParameterInfo>();
        var i = 0;
        var ctorArgs = ctor.GetParameters().Select(p =>
        {
            if (!this.collectionFixtureMappings.TryGetValue(p.ParameterType, out var arg))
            {
                this.TryGetConstructorArgument(ctor, i, p, out arg);
            }

            if (arg == null)
                missingParameters.Add(p);

            i++;
            return arg;
        }).ToArray();

        if (missingParameters.Count > 0)
        {
            this.Aggregator.Add(new TestClassException(
                $"Class fixture type '{fixtureType.FullName}' had one or more unresolved constructor arguments:"
                + $" {string.Join(", ", missingParameters.Select(p => $"{p.ParameterType.Name} {p.Name}"))}"));
        }
        else
        {
            this.Aggregator.Run(() => this.ClassFixtureMappings[fixtureType] = ctor.Invoke(ctorArgs));
        }
    }

    /// <inheritdoc />
    protected override async Task BeforeTestClassFinishedAsync()
    {
        await base.BeforeTestClassFinishedAsync().ConfigureAwait(false);

        this.scope?.Dispose();
        this.scope = null;
    }

    /// <inheritdoc />
    protected override Task<RunSummary> RunTestMethodAsync(
        ITestMethod testMethod,
        IReflectionMethodInfo method,
        IEnumerable<IXunitTestCase> testCases,
        object?[] constructorArguments)
    {
        return new GnomeStackTestMethodRunner(
                this.serviceProvider,
                testMethod,
                this.Class,
                method,
                testCases,
                this.DiagnosticMessageSink,
                this.MessageBus,
                new ExceptionAggregator(this.Aggregator),
                this.CancellationTokenSource,
                constructorArguments)
            .RunAsync();
    }
}