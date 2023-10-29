using System;
using System.Runtime.InteropServices;

#if NETLEGACY
using GnomeStack.Extras.Strings;
#endif

namespace GnomeStack.Standard;

public static partial class Env
{
    public static readonly Lazy<bool> IsElevatedUser = new Lazy<bool>(() =>
    {
        if (IsWindows)
        {
            return Interop.Shell32.IsUserAnAdmin();
        }

        return Interop.Sys.GetEUid() == 0;
    });

    private static readonly Lazy<bool> isWsl = new(() =>
    {
        if (!IsLinux)
            return false;

        try
        {
            var version = File.ReadAllText("/proc/version");
            return version.Contains("Microsoft", StringComparison.OrdinalIgnoreCase);
        }
        catch (IOException)
        {
            return false;
        }
    });

    private static readonly Lazy<bool> isWindows = new Lazy<bool>(() =>
    {
#if !NETLEGACY
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
        var platform = (int)Environment.OSVersion.Platform;
        return (platform != 4) && (platform != 6) && (platform != 128);
#endif
    });

    private static readonly Lazy<bool> isLinux = new Lazy<bool>(() =>
    {
#if !NETLEGACY
        // This API does work on full framework but it requires a newer nuget client (RID aware)
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||

               // The OSPlatform.FreeBSD property only exists in .NET Core 3.1 and higher, whereas this project is also
               // compiled for .NET Standard and .NET Framework, where an OSPlatform for FreeBSD must be created manually
               RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD"));

#else
        var platform = (int)Environment.OSVersion.Platform;
        return platform == 4;
#endif
    });

    private static readonly Lazy<bool> isMacOS = new Lazy<bool>(() =>
    {
#if !NETLEGACY
        // This API does work on full framework but it requires a newer nuget client (RID aware)
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
#else
        var buf = IntPtr.Zero;

        try
        {
            buf = Marshal.AllocHGlobal(8192);

            // This is a hacktastic way of getting sysname from uname ()
            if (Uname(buf) == 0)
            {
                var os = Marshal.PtrToStringAnsi(buf);

                if (os == "Darwin")
                {
                    return true;
                }
            }
        }
        catch
        {
            // eating the exception because if it fails we just assume it isn't a mac
        }
        finally
        {
            if (buf != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(buf);
            }
        }

        return false;
#endif
    });

    public static bool IsWindows => isWindows.Value;

    public static bool IsLinux => isLinux.Value;

    public static bool IsMacOS => isMacOS.Value;

    public static bool IsWsl => isWsl.Value;

    public static bool Is64BitOs => Environment.Is64BitOperatingSystem;

    public static bool IsPrivilegedProcess => IsElevatedUser.Value;

    public static Architecture ProcessArch => RuntimeInformation.ProcessArchitecture;

    public static Architecture OsArch => RuntimeInformation.OSArchitecture;

    [DllImport("libc", EntryPoint = "uname")]
    private static extern int Uname(IntPtr buf);
}