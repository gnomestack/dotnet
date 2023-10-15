# GnomeStack.Xunit.Core

Provides dependency injection for Xunit test class constructors and test methods
that use attributes from `GnomeStack.Xunit.Attributes`.

To enable DI, the `Xunit.GnomeStackTestFramework` must be added to your test
assembly so that Xunit knows which test framework is responsible for test
execution.

The attribute may be added through a csproj file using the following:

```xml
<ItemGroup>
    <AssemblyAttribute Include="Xunit.GnomeStackTestFramework" />
</ItemGroup>
```

The package comes with its own simple dependency injector that wires up an
`IAssert` instance and `ITestOutputHelper`.  The benefits of using `IAssert`
is adding extension methods.

```csharp
[UnitTest]
public void Verify_TestDependencyInjection_ForTestOutputHelper(
    IAssert assert, 
    ITestOutputHelper writer)
{
        writer.WriteLine("Console output!");
        const int h = 0, i = 18;
        assert.NotEqual(h, i);
}
```

A test class or a test case may use the `UseServiceProviderFactoryAttribute` to
replace the built in simple dependency injector with its own implementation.
The attribute requires an type that implements `ITestServiceProviderFactory` and
it must return an object that implements `IServiceProvider`.

```csharp
internal class DIFactoryA : ITestServiceProviderFactory
{
    public IServiceProvider CreateServiceProvider()
    {
        // SimpleServiceProvider auto adds IAssert and ITestOutputHelper
        var services = new SimpleServiceProvider();
        services.AddTransient(typeof(ICustomService), _ => new MyCustomService());

        return services;
    }
}

public class MyTest
{
    [UnitTest]
    [UseServiceProviderFactory(typeof(DIFactoryA))]
    public void Verify_TestDependencyInjection_CustomFactory(
        IAssert assert, 
        ICustomService myService)
    {
        assert.NotNull(myService);
        assert.Equal("My", myService.Name);
    }
}
```

These ideas are not new and come from other existing projects like
Xunit Skippable Fact & dotnet arcade.  Huge shoutout to Xunit for enabling
extension points.

MIT License (project).

Apache License (code from Xunit).
