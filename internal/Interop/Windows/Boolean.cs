internal static partial class Interop
{
    /// <summary>
    /// Blittable version of Windows BOOLEAN type. It is convenient in situations where
    /// manual marshalling is required, or to avoid overhead of regular bool marshalling.
    /// </summary>
    /// <remarks>
    /// Some Windows APIs return arbitrary integer values although the return type is defined
    /// as BOOLEAN. It is best to never compare BOOLEAN to TRUE. Always use bResult != BOOLEAN.FALSE
    /// or bResult == BOOLEAN.FALSE .
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    internal enum BOOLEAN : byte
    {
        FALSE = 0,
        TRUE = 1,
    }
}