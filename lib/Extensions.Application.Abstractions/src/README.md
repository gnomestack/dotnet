# GnomeStack.Extensions.Application.Abstractions

Provides contracts and abstractions for the ApplicationInfo and ApplicationPaths.

The ApplicationInfo provides application information and is a lightweight version of
the HostingEnvironment class without any dependencies on System.Extensions.Hosting and
provides ways to extend `IsProduction()`, `IsTest()`, etc by setting properties.  The ApplicationInfo
can be used with `IsTestHost()` to determine if code is running in dotnet's test host for
testing purposes.

The ApplicationPaths for getting application specific well known paths to folders like
`/home/<user>/.config/<myAppName>` or `C:\Users\<user>\AppData\Roaming\<myAppName>` which 
is useful for console, worker, and desktop applications.  ApplicationPaths has a cache
for storing other well known paths as needed by the application.

The only dependency is on Microsoft.Extensions.FileProviders.Abstractions.

MIT License
