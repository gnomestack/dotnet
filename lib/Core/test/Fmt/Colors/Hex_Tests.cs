using GnomeStack.Fmt.Colors;

namespace Test.Colors;

public class Hex_Tests
{
    [Theory]
    [InlineData("#000000", 0, 0, 0)]
    [InlineData("#000", 0, 0, 0)]
    [InlineData("#FF0000", 255, 0, 0)]
    [InlineData("#00FF00", 0, 255, 0)]
    [InlineData("#0000FF", 0, 0, 255)]
    [InlineData("FFF", 255, 255, 255)]
    [InlineData("FF00FF", 255, 0, 255)]
    public void Ctr_String(string hex, byte r, byte g, byte b)
    {
        var color = new Hex(hex);
        Assert.Equal(r, color.R);
        Assert.Equal(g, color.G);
        Assert.Equal(b, color.B);
    }

    [Theory]
    [InlineData(0x000000, 0, 0, 0)]
    [InlineData(0xFF0000, 255, 0, 0)]
    [InlineData(0x00FF00, 0, 255, 0)]
    [InlineData(0x0000FF, 0, 0, 255)]
    [InlineData(0xFF00FF, 255, 0, 255)]
    public void Ctr_Uint(uint hex, byte r, byte g, byte b)
    {
        var color = new Hex(hex);
        Assert.Equal(r, color.R);
        Assert.Equal(g, color.G);
        Assert.Equal(b, color.B);
    }

    [UnitTest]
    public void ToStringShouldBeHex()
    {
        var color = new Hex(0xFF0000);
        Assert.Equal("FF0000", color.ToString());
    }
}