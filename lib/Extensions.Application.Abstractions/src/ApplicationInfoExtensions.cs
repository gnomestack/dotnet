namespace GnomeStack.Extensions.Application;

public static class ApplicationInfoExtensions
{
    public static bool IsProduction(this IApplicationInfo applicationInfo)
    {
        if (!applicationInfo.Properties.TryGetValue("ProductionEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("production");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("production");
    }

    public static bool IsDevelopment(this IApplicationInfo applicationInfo)
    {
        if (!applicationInfo.Properties.TryGetValue("DevelopmentEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("development");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("development");
    }

    public static bool IsTest(this IApplicationInfo applicationInfo)
    {
        if (!applicationInfo.Properties.TryGetValue("TestEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("test");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("test");
    }

    public static bool IsTestHost(this IApplicationInfo applicationInfo)
    {
        if (applicationInfo.Properties.TryGetValue("TestHost", out var isTestHostValue) && isTestHostValue is bool isTestHost)
            return isTestHost;

        if (!applicationInfo.Properties.TryGetValue("TestHostEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("testhost");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("testhost");
    }

    public static bool IsStaging(this IApplicationInfo applicationInfo)
    {
        if (!applicationInfo.Properties.TryGetValue("StagingEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("staging");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("staging");
    }

    public static bool IsQualityAssurance(this IApplicationInfo applicationInfo)
    {
        if (!applicationInfo.Properties.TryGetValue("QaEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationInfo.IsEnvironment("qa");
        }

        foreach (var test in tests)
        {
            if (applicationInfo.IsEnvironment(test))
                return true;
        }

        return applicationInfo.IsEnvironment("qa");
    }
}