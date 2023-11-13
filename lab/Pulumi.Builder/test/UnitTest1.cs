namespace GomeStack.Xunit.Library;

public class UnitTest1
{
    [UnitTest]
    public void Test1()
    {
        int j = 0, k = 1;
        Assert.NotEqual(j, k);
    }
}