using Xunit;

namespace Tests;

public class AttributeTests
{
    private static readonly IAssert s_assert = FlexAssert.Default;

    [UnitTest]
    public void Verify_UnitTestAttributeWorks()
    {
        const int left = 10, right = 20;
        s_assert.NotEqual(left, right);
    }

    [IntegrationTest]
    public void Verify_IntegrationTestAttributeWorks()
    {
        const int left = 11, right = 21;
        s_assert.NotEqual(left, right);
    }

    [FunctionalTest]
    public void Verify_FunctionalTestAttributeWorks()
    {
        const int left = 13, right = 23;
        s_assert.NotEqual(left, right);
    }

    [UITest]
    public void Verify_UITestAttributeWorks()
    {
        const int left = 13, right = 24;
        s_assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireOsArchitectures(TestOsArchitectures.Arm64)]
    public void Verify_SkipOnArchAttributeWorks()
    {
        const int left = 14, right = 24;
        s_assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireTargetFrameworks("net472")]
    public void Verify_SkipOnTargetFrameworkAttributeWorks()
    {
        const int left = 14, right = 25;
        s_assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireTargetFrameworks("> net5.0")]
    public void Verify_SkipOnTargetFrameworkAttributeWithCompare()
    {
        const int left = 14, right = 28;
        s_assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Linux)]
    public void Verify_SkipOnNonLinuxSystems()
    {
        const int left = 8, right = 30;
        s_assert.NotEqual(left, right);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Windows)]
    public void Verify_SkipOnNonWindowsSystems()
    {
        const int left = 8, right = 33;
        s_assert.NotEqual(left, right);
    }

    [Fact]
    public void Verify_FactAttributeWorks()
    {
        const int left = 10, right = 21;
        s_assert.NotEqual(left, right);
    }

    [InlineData(1, 11)]
    [Theory]
    public void Verify_TheoryAttributeWorks(int left, int right)
    {
        s_assert.NotEqual(left, right);
    }
}