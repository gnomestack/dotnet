# GnomeStack.Core

Provides extended functionality for the .NET BCL such as invoking child processes
painlessly, copying directories, environment variable substitution, password
generation, useful extensions to string builder, option, and result monads.

The `GnomeStack.Standard` namespace is the std namespace where key classes
are, most of which are static classes that can be imported like a module or
use like a normal C# static class.

The `GnomeStack.Standard` makes it easier for C# scripting or a more functional 
approach to programming in C#.

The `GnomeStack.Extra` contains extension and convenience methods for the BCL and
are isolated to avoid collisions with other libraries. 

Both namespaces are meant to be used with the global `Usings.cs` file.

```csharp
using static GnomeStack.Standard.Fs;

MakeDirectory("path/to/directory");
```

## Fs

The Fs class contains methods for File and Directory with additional
methods:

- CatFiles - reads on or file files and returns a string
- CopyDirectory - recursively copies directories and files.
- EnumerateDirectoryMatches - evaluates glob patterns against directories.
- EnumerateFileMatches - evaluates glob patterns against files.
- EnumerateFileSystemInfoMatches - evaluates glob patterns against directories.
- EnsureDirectory - creates a directory if it doesn't exist.
- EnsureFile - creates a file it doesn't exist.
- Match - evaluates a glob for matches.

## Ps

The Ps class invokes child processes and has helper methods such as `Which` that
determines if an executable is on the path.

```csharp
using GnomeStack.Standard;

// elsewhere 
// executes and sends out standard out and standard error.
Ps.Exec("dotnet", "--version");
Ps.Exec("dotnet", new[] { "build", "-c", "Configuration"});

// captures standard out and standard error.
var output = Ps.Capture("dotnet", "build -c 'Configuration'");
Console.Log(output);
Console.Log(output.ExitCode);
Console.Log(output.StdOut); // readonly list of string
Console.Log(Ps.Which("git"));

// use more of a builder pattern
var output2 = Ps.New("git")
    .WithArgs("--version")
    .WithStdOut(Stdio.Piped) // captures standard out
    .WithCwd("/path/to/repo")
    .Output()
    .ThrowOnInvalidExitCode();

Console.Log(output2);
Console.Log(output2.ExitCode);

// chain command line calls
Ps.New("echo", "my test")
    .Pipe("grep", "test")
    .Pipe("cat")
    .Output();
```

## Env

Environment is an extended version of System.Environment.

- IsWindows
- IsLinux
- IsMacOS
- IsWsl
- Is64BitOs
- IsPrivilegedProcess
- ProcessArch
- OsArch
- Expand  - Expand a template string with environment variables.
- Vars    - A variable property that is enumerable and has index get and set access.
- Get     - Gets an environment variable
- TryGet  - Tries to get the environment variable.
- GetRequired - Gets the environment variable or throws.
- Set     - Sets the environment variable.
- Has     - Has the environment variable.
- Remove  - Removes the environment variable.
- AddPath - Adds a path to the environment path variable.
- GetPath - Gets the path environment value.
- SetPath - Sets the path environment value;
- HasPath - Determines if a path already exists in the environment path variable.
- RemovePath - Removes a path from the environment path variable.

```csharp
Environment.SetEnvironmentVariable("WORD", "World");

var result = Env.Expand("Hello %WORD%");
assert.Equal("Hello World", result);


var result2 = Env.Expand("Hello ${WORD}");
assert.Equal("Hello World", result2);
```
