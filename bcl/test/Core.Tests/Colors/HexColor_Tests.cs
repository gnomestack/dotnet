using GnomeStack.Colors;

namespace Tests;

public class HexColor_Tests
{
    public static IEnumerable<object[]> HexColor_TestData()
    {
        yield return new object[] { new HexColor(0x000000), 0, 0, 0, 255 };
        yield return new object[] { new HexColor(0x00000000), 0, 0, 0, 0 };
        yield return new object[] { new HexColor(0x000000FF), 0, 0, 0, 255 };
        yield return new object[] { new HexColor(0x0000FF), 0, 0, 255, 0 };
        yield return new object[] { new HexColor(0x0000FFFF), 0, 0, 255, 255 };
        yield return new object[] { new HexColor(0x00FFFFFF), 0, 255, 255, 255 };
        yield return new object[] { new HexColor(0x00FFFF00), 0, 255, 255, 0 };
        yield return new object[] { new HexColor(0xFFFFFF), 255, 255, 255, 255 };
        yield return new object[] { new HexColor(0xFFFFFFFF), 255, 255, 255, 255 };
        yield return new object[] { new HexColor(0xFFFFFF00), 255, 255, 255, 0 };
        yield return new object[] { new HexColor(0x000000FF), 0, 0, 0, 255 };
        yield return new object[] { new HexColor(0x0000FF00), 0, 0, 255, 0 };
        yield return new object[] { new HexColor(0x0000FFFF), 0, 0, 255, 255 };
        yield return new object[] { new HexColor(0x00FF0000), 0, 255, 0, 0 };
    }

    [Theory]
    [MemberData(nameof(HexColor_TestData))]
    public void HexColor_Test(HexColor hexColor, byte r, byte g, byte b, byte a)
    {
        var rgba = hexColor.ToRgba();
        Assert.Equal(r, rgba.R);
        Assert.Equal(g, rgba.G);
        Assert.Equal(b, rgba.B);
        Assert.Equal(a, (byte)rgba.A);
    }
}