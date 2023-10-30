# GnomeStack.Extensions.Application

Provides application environment and application paths objects that can be
with or without dependency injection to get basic information about the
application, the environment for the application, and well known paths for
the application

Microsoft.Extensions.Hosting has strong requirements around what dependencies
it has.  This library only as a dependency on 
Microsoft.Extensions.DependencyInjection.Abstractions.

The application environment is similar to the hosting environment and includes
a global dictionary to store properties and allows more flexibility in determining
what environment is production, test, qa, etc. 

It also can detect if the application is running for a test host for .NET Core
which is useful for determining if the application is running in in a set of tests.

## Usage

```csharp
using GnomeStack.Extensions.Application;

// the environment name is pulled from the first match of the following
// environment variables:
// - GNOMESTACK_ENVIRONMENT
// - APP_ENVIRONMENT
// - ASPNETCORE_ENVIRONMENT
// - DOTNET_ENVIRONMENT

// application name and version is pull from the entry assembly
// these can be overridden by passing an options object with
// the settings explicitly set.
var appEnv = new ApplicationEnvironment();
var appPaths = new ApplicationPaths(null, appEnv);
ApplicationEnvironment.Current = appEnv;
ApplicationPaths.Current = appPaths;

// write an extension method to determine if a feature is enabled
appEnv.Properties["MyFeature"] = true;


if (appEnv.IsProduction()) {
    // do something    
}

if (appEnv.IsTestHost()) {
    // do something else for automated testing. 
}

Console.WriteLine(appEnv.Name);
Console.WriteLine(appEnv.Version);
Console.WriteLine(appEnv.EnvironmentName);
Console.WriteLine(appEnv.ContentRootPath);
Console.WriteLine(appEnv.InstanceName);

// ${HOME}/.config/appname or %APPDATA%\appname
Console.WriteLine(appPaths.UserConfigDirectory);

// ${HOME}/.local/share/appname or %LOCALAPPDATA%\appname\share
Console.WriteLine(appPaths.UserDataDirectory);

// ${HOME}/.local/state/appname or %LOCALAPPDATA%\appname\state
Console.WriteLine(appPaths.UserStateDirectory);

// ${HOME}/.local/state/appname/logs or %LOCALAPPDATA%\appname\state\logs
Console.WriteLine(appPaths.UserLogsDirectory);

// /etc/appname or %ProgramData%\appname\etc
Console.WriteLine(appPaths.MachineConfigDirectory);

// /var/lib/appname or %ProgramData%\appname\share
Console.WriteLine(appPaths.MachineDataDirectory);

// /var/log/appname or %ProgramData%\appname\log
Console.WriteLine(appPaths.MachineLogsDirectory);
```

MIT License
