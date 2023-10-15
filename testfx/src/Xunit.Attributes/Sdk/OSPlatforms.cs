using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xunit.Sdk;

// ReSharper disable InconsistentNaming
internal static class OSPlatforms
{
    public static OSPlatform Windows => OSPlatform.Windows;

    public static OSPlatform Linux => OSPlatform.Linux;

    public static OSPlatform OSX => OSPlatform.OSX;

    public static OSPlatform IOS { get; } = OSPlatform.Create("IOS");

    public static OSPlatform MacCatalyst { get; } = OSPlatform.Create("MACCATALYST");

    public static OSPlatform FreeBSD { get; } = OSPlatform.Create("FREEBSD");

    public static OSPlatform Android { get; } = OSPlatform.Create("ANDROID");

    public static OSPlatform NetBSD { get; } = OSPlatform.Create("NETBSD");

    public static OSPlatform Illumos { get; } = OSPlatform.Create("ILLUMOS");

    public static OSPlatform Solaris { get; } = OSPlatform.Create("SOLARIS");

    public static OSPlatform TVOS { get; } = OSPlatform.Create("TVOS");

    public static OSPlatform Browser { get; } = OSPlatform.Create("BROWSER");
}