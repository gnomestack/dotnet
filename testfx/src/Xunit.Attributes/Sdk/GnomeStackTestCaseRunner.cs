using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class GnomeStackTestCaseRunner : XunitTestCaseRunner, IDisposable
{
    private readonly IServiceProvider? serviceProvider;

    private readonly IDisposable? scope;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GnomeStackTestCaseRunner" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for injecting dependencies.</param>
    /// <param name="testCase">The test case to be run.</param>
    /// <param name="displayName">The display name of the test case.</param>
    /// <param name="skipReason">The skip reason, if the test is to be skipped.</param>
    /// <param name="constructorArguments">The arguments to be passed to the test class constructor.</param>
    /// <param name="testMethodArguments">The arguments to be passed to the test method.</param>
    /// <param name="messageBus">The message bus to report run status to.</param>
    /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
    /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
    public GnomeStackTestCaseRunner(
        IServiceProvider? serviceProvider,
        IXunitTestCase testCase,
        string displayName,
        string skipReason,
        object?[] constructorArguments,
        object[] testMethodArguments,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
        : base(
            testCase,
            displayName,
            skipReason,
            constructorArguments,
            testMethodArguments,
            messageBus,
            aggregator,
            cancellationTokenSource)
    {
        this.DisplayName = displayName;
        this.SkipReason = skipReason;
        this.TestClass = this.TestCase.TestMethod.TestClass.Class.ToRuntimeType();
        this.TestMethod = this.TestCase.Method.ToRuntimeMethod();
        this.serviceProvider = serviceProvider;

        if (this.serviceProvider?
                .GetService(typeof(IServiceProviderLifetimeFactory))
            is IServiceProviderLifetimeFactory factory)
        {
            var lifetime = factory.CreateLifetime();
            this.serviceProvider = lifetime.ServiceProvider;
            this.scope = lifetime;
        }

        var ctor = this.TestClass.GetConstructors().FirstOrDefault();
        Type[]? parameterTypes;
        object?[]? args;

        ParameterInfo[]? parameters;
        if (constructorArguments.Length > 0)
        {
            parameters = ctor?.GetParameters() ?? Array.Empty<ParameterInfo>();
            parameterTypes = new Type[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                parameterTypes[i] = parameters[i].ParameterType;

            args = constructorArguments ?? Array.Empty<object>();
            var ctorArgs = new object?[parameters.Length];
            Array.Copy(args, ctorArgs, args.Length);

            for (var i = 0; i < parameters.Length; i++)
            {
                var obj = ctorArgs[i] ?? this.serviceProvider?.GetService(parameters[i].ParameterType);

                ctorArgs[i] = obj;
            }

            this.ConstructorArguments = Reflector.ConvertArguments(ctorArgs, parameterTypes);
        }
        else
        {
            this.ConstructorArguments = constructorArguments;
        }

        parameters = this.TestMethod.GetParameters();
        parameterTypes = new Type[parameters?.Length ?? 0];
        args = testMethodArguments ?? Array.Empty<object>();
        if (parameters != null && parameters.Length != args.Length)
        {
            for (var i = 0; i < parameters.Length; i++)
                parameterTypes[i] = parameters[i].ParameterType;

            var methodArgs = new object?[parameters.Length];
            Array.Copy(args, methodArgs, args.Length);
            for (var i = 0; i < parameters.Length; i++)
            {
                var obj = methodArgs[i];
                if (obj == null)
                {
                    var pt = parameters[i].ParameterType;
                    obj = pt.FullName switch
                    {
                        "System.IServiceProvider" => this.serviceProvider,
                        "Xunit.Sdk.GnomeStackTestCase" => (GnomeStackTestCase)this.TestCase,
                        "Xunit.Sdk.IXunitTestCase" => this.TestCase,
                        _ => this.serviceProvider?.GetService(pt),
                    };
                }

                methodArgs[i] = obj;
            }

            args = methodArgs;
        }

        this.TestMethodArguments = Reflector.ConvertArguments(args, parameterTypes);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override XunitTestRunner CreateTestRunner(
        ITest test,
        IMessageBus messageBus,
        Type testClass,
        object?[] constructorArguments,
        MethodInfo testMethod,
        object[] testMethodArguments,
        string skipReason,
        IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
    {
        return new GnomeStackTestRunner(
            this.serviceProvider,
            test,
            messageBus,
            testClass,
            constructorArguments,
            testMethod,
            testMethodArguments,
            skipReason,
            beforeAfterAttributes,
            aggregator,
            cancellationTokenSource);
    }

    /// <inheritdoc />
    protected override Task<RunSummary> RunTestAsync()
    {
        var test = this.CreateTest(this.TestCase, this.DisplayName);
        var runner = this.CreateTestRunner(
            test,
            this.MessageBus,
            this.TestClass,
            this.ConstructorArguments,
            this.TestMethod,
            this.TestMethodArguments,
            this.SkipReason,
            this.BeforeAfterAttributes,
            this.Aggregator,
            this.CancellationTokenSource);

        return runner.RunAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        this.scope?.Dispose();
    }
}