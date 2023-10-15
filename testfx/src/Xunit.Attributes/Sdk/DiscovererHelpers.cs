// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using NuGet.Frameworks;

namespace Xunit.Sdk;

// based on dotnet arcade.
internal static class DiscovererHelpers
{
    public static bool TestPlatformApplies(TestOsPlatforms platforms) =>
        platforms == TestOsPlatforms.None ||
        (platforms.HasFlag(TestOsPlatforms.Windows) && PlatformInformation.IsWindows) ||
        (platforms.HasFlag(TestOsPlatforms.Linux) && PlatformInformation.IsLinux) ||
        (platforms.HasFlag(TestOsPlatforms.OSX) && PlatformInformation.IsOSX) ||
        (platforms.HasFlag(TestOsPlatforms.IOS) && PlatformInformation.IsIOS) ||
        (platforms.HasFlag(TestOsPlatforms.FreeBSD) && PlatformInformation.IsFreeBSD) ||
        (platforms.HasFlag(TestOsPlatforms.Android) && PlatformInformation.IsAndroid) ||
        (platforms.HasFlag(TestOsPlatforms.MacCatalyst) && PlatformInformation.IsOsPlatform(OSPlatforms.MacCatalyst)) ||
        (platforms.HasFlag(TestOsPlatforms.Browser) && PlatformInformation.IsOsPlatform(OSPlatforms.Browser)) ||
        (platforms.HasFlag(TestOsPlatforms.NetBSD) && PlatformInformation.IsOsPlatform(OSPlatforms.NetBSD)) ||
        (platforms.HasFlag(TestOsPlatforms.Illumos) && PlatformInformation.IsOsPlatform(OSPlatforms.Illumos)) ||
        (platforms.HasFlag(TestOsPlatforms.Solaris) && PlatformInformation.IsOsPlatform(OSPlatforms.Solaris)) ||
        (platforms.HasFlag(TestOsPlatforms.TVOS) && PlatformInformation.IsOsPlatform(OSPlatforms.TVOS));

    public static NuGetFramework GetNuGetFramework(Assembly? assembly = null)
    {
        assembly ??= System.Reflection.Assembly.GetEntryAssembly();
        var frameworkDescription = assembly?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

        if (frameworkDescription != null)
        {
            var split = frameworkDescription.Split(',');
            if (split.Length == 2)
            {
                var label = split[1].Split('=');
                if (label.Length == 2)
                {
                    return new NuGetFramework(split[0], new Version(label[1].Trim('v')));
                }
            }
        }

        // disable reflection warning as this will get invoked on the full framework when TargetFrameworkAttribute does not exist.
#pragma warning disable REFL003, REFL016
        var setupProp = typeof(AppDomain).GetProperty("SetupInformation", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        if (setupProp != null)
        {
            var setup = setupProp.GetValue(AppDomain.CurrentDomain);
            var targetFrameworkProp = setup?.GetType().GetProperty("TargetFrameworkName");
            if (targetFrameworkProp != null)
            {
                frameworkDescription = targetFrameworkProp.GetValue(setup) as string;
                if (!string.IsNullOrWhiteSpace(frameworkDescription))
                {
                    return new NuGetFramework(frameworkDescription);
                }
            }
        }

        return NuGetFramework.UnsupportedFramework;
    }

    public static bool RuntimeConfigurationApplies(RuntimeConfigurations configurations) =>
        configurations.HasFlag(RuntimeConfigurations.None) ||
        (configurations.HasFlag(RuntimeConfigurations.Release) && IsReleaseRuntime()) ||
        (configurations.HasFlag(RuntimeConfigurations.Debug) && IsDebugRuntime()) ||
        (configurations.HasFlag(RuntimeConfigurations.Checked) && IsCheckedRuntime());

    private static bool IsCheckedRuntime() => AssemblyConfigurationEquals("Checked");

    private static bool IsReleaseRuntime() => AssemblyConfigurationEquals("Release");

    private static bool IsDebugRuntime() => AssemblyConfigurationEquals("Debug");

    private static bool AssemblyConfigurationEquals(string configuration)
    {
        var assemblyConfigurationAttribute = typeof(string).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();

        return assemblyConfigurationAttribute != null &&
               string.Equals(assemblyConfigurationAttribute.Configuration, configuration, StringComparison.InvariantCulture);
    }
}