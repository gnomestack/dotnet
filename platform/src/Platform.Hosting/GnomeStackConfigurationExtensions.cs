using GnomeStack.Platform.App;
using GnomeStack.Text;

namespace Microsoft.Extensions.Configuration;

public static class GnomeStackConfigurationExtensions
{
    public static IConfigurationBuilder Clear(
        this IConfigurationBuilder builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.Sources.Clear();
        return builder;
    }

    public static IConfigurationBuilder AddUserJsonFile(
        this IConfigurationBuilder builder,
        IAppPaths appPaths,
        string? path = null,
        bool optional = true,
        bool reloadOnChange = false)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        if (string.IsNullOrWhiteSpace(path))
            path = "appsettings.json";

        if (Path.IsPathFullyQualified(path))
            return builder.AddJsonFile(path, optional, reloadOnChange);

        path = Path.Join(appPaths.UserConfigDirectory, path);

        return builder.AddJsonFile(path, optional, reloadOnChange);
    }

    public static IConfigurationBuilder AddMachineJsonFile(
        this IConfigurationBuilder builder,
        IAppPaths appPaths,
        string? path = null,
        bool optional = true,
        bool reloadOnChange = false)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        if (string.IsNullOrWhiteSpace(path))
            path = "appsettings.json";

        if (Path.IsPathFullyQualified(path))
            return builder.AddJsonFile(path, optional, reloadOnChange);

        path = Path.Join(appPaths.MachineConfigDirectory, path);

        return builder.AddJsonFile(path, optional, reloadOnChange);
    }

    public static IConfigurationBuilder AddDockerSecretJsonFile(
        this IConfigurationBuilder builder,
        string? path = null,
        bool optional = true,
        bool reloadOnChange = false)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        if (string.IsNullOrWhiteSpace(path))
            path = "appsettings.json";

        if (Path.IsPathFullyQualified(path))
            return builder.AddJsonFile(path, optional, reloadOnChange);

        path = Path.Join("/var/data/secrets", path);

        return builder.AddJsonFile(path, optional, reloadOnChange);
    }

    public static IConfigurationBuilder AddDefaultJsonFiles(
        this IConfigurationBuilder builder,
        string environment,
        string baseName = "appsettings",
        bool optional = true,
        bool reloadOnChange = false)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.Sources.Clear();
        builder.AddJsonFile($"{baseName}.json", optional, reloadOnChange);
        builder.AddJsonFile($"{baseName}.{environment}.json", optional, reloadOnChange);
        builder.AddJsonFile($"{baseName}.secrets.json", optional, reloadOnChange);
        builder.AddJsonFile($"{baseName}.{environment}.secrets.json", optional, reloadOnChange);

        return builder;
    }

    public static IConfigurationBuilder AddJsonFromEnvironmentVariable(
        this IConfigurationBuilder configurationBuilder,
        string environmentVariableName,
        bool optional = true)
    {
        string? appsettingsEnvValue = Environment.GetEnvironmentVariable(environmentVariableName);
        var isEmpty = appsettingsEnvValue.IsNullOrWhiteSpace();
        if (!optional && isEmpty)
        {
            throw new InvalidOperationException(
                $"The Environment variable '{environmentVariableName}' value for" +
                " injecting json app settings is empty");
        }

        if (!isEmpty)
        {
            appsettingsEnvValue = appsettingsEnvValue!.Trim();
            if (!appsettingsEnvValue.StartsWith("{") || !appsettingsEnvValue.EndsWith("}"))
            {
                throw new InvalidOperationException(
                    $"The Environment variable '{environmentVariableName}' value for" +
                    " injecting json app settings does not start with '{' or end with '}'");
            }

            var stream = new MemoryStream(Encodings.Utf8.GetBytes(appsettingsEnvValue));
            configurationBuilder.AddJsonStream(stream);
        }

        return configurationBuilder;
    }
}