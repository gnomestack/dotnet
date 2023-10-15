using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Xunit.Sdk;

// ReSharper disable InconsistentNaming
internal static class PlatformInformation
{
    /// <summary>
    ///  Gets a value indicating whether the device's platform is 64 bit architecture.
    /// </summary>
    public static bool Is64Bit { get; } = RuntimeInformation.OSArchitecture is Architecture.Arm64 or Architecture.X64;

    /// <summary>
    ///  Gets a value indicating whether the device's platform is ARM architecture.
    /// </summary>
    public static bool IsArm { get; } = RuntimeInformation.OSArchitecture is Architecture.Arm or Architecture.Arm64;

    /// <summary>
    ///  Gets a value indicating whether the device's platform is a Windows operating system.
    /// </summary>
    public static bool IsWindows { get; } = IsOsPlatform(OSPlatforms.Windows);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is a Linux operating system.
    /// </summary>
    public static bool IsLinux { get; } = IsOsPlatform(OSPlatforms.Linux);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is an Apple OSX operating system.
    /// </summary>
    public static bool IsOSX { get; } = IsOsPlatform(OSPlatforms.OSX);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is a FreeBSD operating system.
    /// </summary>
    public static bool IsFreeBSD { get; } = IsOsPlatform(OSPlatforms.FreeBSD);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is an Apple IOS operating system.
    /// </summary>
    public static bool IsIOS { get; } = IsOsPlatform(OSPlatforms.IOS) && !IsOsPlatform(OSPlatforms.MacCatalyst);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is an Android operating system.
    /// </summary>
    public static bool IsAndroid { get; } = IsOsPlatform(OSPlatforms.Android);

    /// <summary>
    ///  Gets a value indicating whether the device's platform is running within WSL.
    /// </summary>
    public static bool IsWsl { get; } = GetIsWsl();

    /// <summary>
    /// Indicates whether the current application is running on the specified platform.
    /// </summary>
    /// <param name="platform">The platform to test.</param>
    /// <returns><see langword="true" /> if the current app is running on the specified platform; otherwise, <see langword="false"/>.</returns>
    public static bool IsOsPlatform(OSPlatform platform)
    {
        return RuntimeInformation.IsOSPlatform(platform);
    }

    // https://github.com/nuke-build/nuke/blob/5505791f65e542a719d8481900e8a59876ea4821/source/Nuke.Common/EnvironmentInfo.Platform.cs
    private static bool GetIsWsl()
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
    }
}