#if NETLEGACY || NET6_0

// ReSharper disable once CheckNamespace
namespace System.IO;

/// <summary>
///   <para>Represents the Unix filesystem permissions.</para>
///   <para>This enumeration supports a bitwise combination of its member values.</para>
/// </summary>
[Flags]
public enum UnixFileMode
{
    /// <summary>No permissions.</summary>
    None = 0,

    /// <summary>Execute permission for others.</summary>
    OtherExecute = 1,

    /// <summary>Write permission for others.</summary>
    OtherWrite = 2,

    /// <summary>Read permission for others.</summary>
    OtherRead = 4,

    /// <summary>Execute permission for group.</summary>
    GroupExecute = 8,

    /// <summary>Write permission for group.</summary>
    GroupWrite = 16, // 0x00000010

    /// <summary>Read permission for group.</summary>
    GroupRead = 32, // 0x00000020

    /// <summary>Execute permission for owner.</summary>
    UserExecute = 64, // 0x00000040

    /// <summary>Write permission for owner.</summary>
    UserWrite = 128, // 0x00000080

    /// <summary>Read permission for owner.</summary>
    UserRead = 256, // 0x00000100

    /// <summary>Sticky bit permission.</summary>
    StickyBit = 512, // 0x00000200

    /// <summary>Set group permission.</summary>
    SetGroup = 1024, // 0x00000400

    /// <summary>Set user permission.</summary>
    SetUser = 2048, // 0x00000800
}

#endif