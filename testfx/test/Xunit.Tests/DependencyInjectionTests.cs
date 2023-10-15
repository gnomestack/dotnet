using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class DependencyInjectionTests
{
    [UnitTest]
    public static void Verify_TestDependencyInjection_WithStatic(IAssert assert)
    {
        const int h = 0, i = 22;
        assert.NotEqual(h, i);
    }

    [UnitTest]
    public void Verify_TestDependencyInjection_WithInstance(IAssert assert)
    {
        const int h = 0, i = 15;
        assert.NotEqual(h, i);
    }

    [UnitTest]
    public void Verify_TestDependencyInjection_ForTestOutputHelper(IAssert assert, ITestOutputHelper writer)
    {
        writer.WriteLine("Console output!");
        const int h = 0, i = 18;
        assert.NotEqual(h, i);
    }

    [UnitTest]
    [UseServiceProviderFactory(typeof(DIFactoryA))]
    public void Verify_TestDependencyInjection_CustomFactory(IAssert assert, ICustomService myService)
    {
        assert.NotNull(myService);
        assert.Equal("My", myService.Name);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Linux)]
    public void Verify_SkipOnNonLinuxSystems()
    {
        const int left = 8, right = 30;
        var assert = FlexAssert.Default;
        assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Windows)]
    public void Verify_SkipOnNonWindowsSystems()
    {
        const int left = 8, right = 33;
        var assert = FlexAssert.Default;
        assert.NotEqual(left, right);
    }
}