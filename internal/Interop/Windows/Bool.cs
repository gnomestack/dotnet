using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
internal static partial class Interop
{
    /// <summary>
    /// Blittable version of Windows BOOL type. It is convenient in situations where
    /// manual marshalling is required, or to avoid overhead of regular bool marshalling.
    /// </summary>
    /// <remarks>
    /// Some Windows APIs return arbitrary integer values although the return type is defined
    /// as BOOL. It is best to never compare BOOL to TRUE. Always use bResult != BOOL.FALSE
    /// or bResult == BOOL.FALSE .
    /// </remarks>
#pragma warning disable S1939
    // ReSharper disable once InconsistentNaming
    internal enum BOOL : int
    {
        FALSE = 0,
        TRUE = 1,
    }
}