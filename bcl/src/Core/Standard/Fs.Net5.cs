#if NET5_0_OR_GREATER

using System.Runtime.CompilerServices;
using System.Text;

using GnomeStack.Text;

using Microsoft.Win32.SafeHandles;

namespace GnomeStack.Standard;

public static partial class Fs
{
    public static Task<byte[]> ReadFileAsync(string path, CancellationToken cancellationToken = default)
        => File.ReadAllBytesAsync(path, cancellationToken);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> ReadDirectory(string path, string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, enumerationOptions);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LinkDirectory(string path, string pathToTarget)
        => Directory.CreateSymbolicLink(path, pathToTarget);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LinkFile(string path, string pathToTarget)
        => File.CreateSymbolicLink(path, pathToTarget);

    public static FileSystemInfo? ResolveLinked(string linkPath, bool returnFinalTarget = false)
        => File.ResolveLinkTarget(linkPath, returnFinalTarget);

    public static FileSystemInfo? ResolveLinkedDirectory(string linkPath, bool returnFinalTarget = false)
        => Directory.ResolveLinkTarget(linkPath, returnFinalTarget);

    public static Task<string> ReadTextFileAsync(string path, Encoding? encoding = null, CancellationToken cancellationToken = default)
        => File.ReadAllTextAsync(path, encoding ?? Encoding.UTF8, cancellationToken);

    public static Task WriteTextFileAsync(string path, string contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
    {
        if (append)
        {
            if (encoding is not null)
            {
                return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
            }

            return File.AppendAllTextAsync(path, contents, cancellationToken);
        }

        if (encoding is not null)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }

        return File.WriteAllTextAsync(path, contents, cancellationToken);
    }

    public static Task WriteTextFileAsync(string path, IEnumerable<string> contents, Encoding? encoding = null, bool append = false, CancellationToken cancellationToken = default)
    {
        if (append)
        {
            if (encoding is not null)
            {
                return File.AppendAllLinesAsync(path, contents, encoding, cancellationToken);
            }

            return File.AppendAllLinesAsync(path, contents, cancellationToken);
        }

        if (encoding is not null)
        {
            return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        return File.WriteAllLinesAsync(path, contents, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SafeFileHandle OpenFileHandle(string path)
        => File.OpenHandle(path);
}
#endif