namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Iterates through the array and calls the action for each element.
    /// </summary>
    /// <param name="array">The array to iterate.</param>
    /// <param name="action">The action to apply to each item.</param>
    public static void ForEach(this Array array, Action<Array, int[]> action)
    {
        if (array.LongLength == 0)
            return;

        var walker = new ArrayTraverse(array);
        do action(array, walker.Position);
        while (walker.Step());
    }
}

internal sealed class ArrayTraverse
{
    private readonly int[] maxLengths;

    public ArrayTraverse(Array array)
    {
        this.maxLengths = new int[array.Rank];
        for (int i = 0; i < array.Rank; ++i)
        {
            this.maxLengths[i] = array.GetLength(i) - 1;
        }

        this.Position = new int[array.Rank];
    }

    public int[] Position { get; }

    public bool Step()
    {
        for (int i = 0; i < this.Position.Length; ++i)
        {
            if (this.Position[i] >= this.maxLengths[i])
            {
                continue;
            }

            this.Position[i]++;
            for (int j = 0; j < i; j++)
            {
                this.Position[j] = 0;
            }

            return true;
        }

        return false;
    }
}