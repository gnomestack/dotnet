using static GnomeStack.Buffers.ArrayPool;

namespace Tests;

public static class ArrayPool_Tests
{
    [UnitTest]
    public static void Verify_Rent_And_Return(IAssert assert)
    {
        var array = Rent<int>(10);
        assert.True(array.Length >= 10);
        assert.Equal(0, array[0]);

        array[0] = 1;
        Return(array);
        assert.Equal(1, array[0]);

        array = Rent<int>(10);
        assert.True(array.Length >= 10);
        array[0] = 1;

        Return(array, true);
        assert.Equal(0, array[0]);
    }
}