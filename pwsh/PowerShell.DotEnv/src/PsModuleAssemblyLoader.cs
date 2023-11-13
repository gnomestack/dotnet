using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace GnomeStack.PowerShell.DotEnv;

public class PsModuleAssemblyLoader : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    private static readonly HashSet<string> Dependencies;
    private static readonly string DependencyFolder;
    private static readonly AssemblyLoadContextProxy? Proxy;

#pragma warning disable S3963
    static PsModuleAssemblyLoader()
#pragma warning restore S3963
    {
        var assembly = typeof(PsModuleAssemblyLoader).Assembly;
        DependencyFolder = Path.Combine(Path.GetDirectoryName(assembly.Location));
        Dependencies = new(StringComparer.Ordinal);
        foreach (string filePath in Directory.EnumerateFiles(DependencyFolder, "*.dll"))
        {
            Dependencies.Add(AssemblyName.GetAssemblyName(filePath).FullName);
        }

        Proxy = AssemblyLoadContextProxy.Create(assembly.FullName);
    }

    public void OnImport()
    {
        
        
        AppDomain.CurrentDomain.AssemblyResolve += ResolvingHandler;
    }

    public void OnRemove(PSModuleInfo psModuleInfo)
    {
        AppDomain.CurrentDomain.AssemblyResolve -= ResolvingHandler;
    }

    internal static Assembly? ResolvingHandler(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        if (IsAssemblyMatching(assemblyName, args.RequestingAssembly))
        {
            string fileName = assemblyName.Name + ".dll";
            string filePath = Path.Combine(DependencyFolder, fileName);

            if (File.Exists(filePath))
            {
                Console.WriteLine($"<*** Fall in 'ResolvingHandler': Newtonsoft.Json, Version=13.0.0.0  -- Loaded! ***>");

                // - In .NET, load the assembly into the custom assembly load context.
                // - In .NET Framework, assembly conflict is not a problem, so we load the assembly
                //   by 'Assembly.LoadFrom', the same as what powershell.exe would do.
                return Proxy is not null
                    ? Proxy.LoadFromAssemblyPath(filePath)
#pragma warning disable S3885
                    : Assembly.LoadFrom(filePath);
#pragma warning restore S3885
            }
        }

        return null;
    }

    private static bool IsAssemblyMatching(AssemblyName assemblyName, Assembly? requestingAssembly)
    {
        // The requesting assembly is always available in .NET, but could be null in .NET Framework.
        // - When the requesting assembly is available, we check whether the loading request came from this
        //   module (the 'conflict' assembly in this case), so as to make sure we only act on the request
        //   from this module.
        // - When the requesting assembly is not available, we just have to depend on the assembly name only.
        return requestingAssembly is not null
            ? requestingAssembly.FullName.StartsWith("conflict,") && Dependencies.Contains(assemblyName.FullName)
            : Dependencies.Contains(assemblyName.FullName);
    }

    internal class AssemblyLoadContextProxy
    {
        private readonly object customContext;
        private readonly MethodInfo loadFromAssemblyPath;

        private AssemblyLoadContextProxy(Type alc, string loadContextName)
        {
            var ctor = alc.GetConstructor(new[] { typeof(string), typeof(bool) });
            if (ctor is null)
                throw new MissingMethodException(alc.FullName, ".ctor(string, bool)");
            var load = alc.GetMethod("LoadFromAssemblyPath", new[] { typeof(string) });

            this.loadFromAssemblyPath = load ?? throw new MissingMethodException(alc.FullName, "LoadFromAssemblyPath(string)");
            this.customContext = ctor.Invoke(new object[] { loadContextName, false });
        }

        internal static AssemblyLoadContextProxy? Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var alc = typeof(object).Assembly.GetType("System.Runtime.Loader.AssemblyLoadContext");
            return alc is not null
                ? new AssemblyLoadContextProxy(alc, name)
                : null;
        }

        internal Assembly LoadFromAssemblyPath(string assemblyPath)
        {
            return (Assembly)this.loadFromAssemblyPath.Invoke(
                this.customContext,
                new object[] { assemblyPath });
        }
    }
}