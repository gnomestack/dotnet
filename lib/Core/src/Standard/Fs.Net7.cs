using System.Runtime.CompilerServices;

using Microsoft.Win32.SafeHandles;

#if NET7_0_OR_GREATER

namespace GnomeStack.Standard;

public static partial class Fs
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAsyncEnumerable<string> EnumerateTextFileAsync(string path, CancellationToken cancellationToken = default)
    {
        return File.ReadLinesAsync(path, cancellationToken);
    }

    public static UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle)
        => File.GetUnixFileMode(fileHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MakeDirectory(string path, UnixFileMode mode)
        => Directory.CreateDirectory(path, mode);

    public static void EnsureDirectory(string path, UnixFileMode mode)
    {
        if (!DirectoryExists(path))
            Directory.CreateDirectory(path, mode);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Chmod(string path, UnixFileMode mode)
        => File.SetUnixFileMode(path, mode);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Chmod(SafeFileHandle fileHandle, UnixFileMode mode)
        => File.SetUnixFileMode(fileHandle, mode);

    public static void SetUnixFileMode(string path, UnixFileMode mode)
    {
        File.SetUnixFileMode(path, mode);
    }

    public static void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode)
        => File.SetUnixFileMode(fileHandle, mode);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken = default)
        => File.WriteAllBytesAsync(path, data, cancellationToken);
}
#endif