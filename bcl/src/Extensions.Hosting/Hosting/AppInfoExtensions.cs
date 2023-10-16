using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Hosting;

namespace GnomeStack.Extensions.Hosting;

public static class AppInfoExtensions
{
    public static bool IsProduction(this IAppInfo applicationInfo)
    {
        return applicationInfo.IsEnvironment("production");
    }

    public static bool IsDevelopment(this IAppInfo applicationInfo)
    {
        return applicationInfo.IsEnvironment("development");
    }

    public static bool IsTest(this IAppInfo applicationInfo)
    {
        return applicationInfo.IsEnvironment("test");
    }

    public static bool IsStaging(this IAppInfo applicationInfo)
    {
        return applicationInfo.IsEnvironment("staging");
    }

    public static bool IsQualityAssurance(this IAppInfo applicationInfo)
    {
        return applicationInfo.IsEnvironment("qa");
    }
}