using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using NuGet.Frameworks;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit;

[System.AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    Inherited = false,
    AllowMultiple = true)]
public sealed class RequireTargetFrameworksAttribute : SkippableTraitAttribute
{
    private static readonly ConcurrentDictionary<string, NuGetFramework> s_assemblies = new();

    public RequireTargetFrameworksAttribute(string targetFrameworks)
    {
        if (targetFrameworks.IsNullOrWhiteSpace())
        {
            this.TargetFrameworks = Array.Empty<string>();
            return;
        }

        this.TargetFrameworks = targetFrameworks.Split(';')
            .Where(o => !string.IsNullOrWhiteSpace(o))
            .ToArray();
    }

    public string[] TargetFrameworks { get; set; }

    public override string? GetSkipReason(IMessageSink sink, ITestMethod testMethod, IAttributeInfo attributeInfo)
    {
        var name = testMethod.TestClass.Class.Assembly.Name;
        if (!s_assemblies.TryGetValue(name, out var targetFramework))
        {
            try
            {
                var assemblyName = new AssemblyName(testMethod.TestClass.Class.Assembly.Name);
                var assembly = Assembly.Load(assemblyName);
                targetFramework = DiscovererHelpers.GetNuGetFramework(assembly);
                s_assemblies.TryAdd(name, targetFramework);
            }
            catch (Exception ex)
            {
                sink.OnMessage(new DiagnosticMessage(ex.Message + Environment.NewLine + ex.StackTrace));
                return $"Require target frameworks error: {ex.Message}";
            }
        }

        if (this.TargetFrameworks.Length == 0)
            return null;

        foreach (var target in this.TargetFrameworks)
        {
            if (string.IsNullOrWhiteSpace(target))
                continue;

            if (targetFramework.IsAny)
                return null;

            var parts = target.Split(' ');
            var match = "==";
            var nextTarget = target;

            try
            {
                if (parts.Length == 2)
                {
                    match = parts[0];
                    nextTarget = parts[1];
                }
                else
                {
                    var framework1 = NuGet.Frameworks.NuGetFramework.Parse(nextTarget);
                    if (framework1.Framework == targetFramework.Framework && framework1.Version == targetFramework.Version)
                        return null;
                }

                var framework = NuGetFramework.Parse(nextTarget);

                switch (match)
                {
                    case "==":
                        if (framework.Framework == targetFramework.Framework && framework.Version == targetFramework.Version)
                            return null;

                        break;
                    case ">=":
                        if (framework.Framework == targetFramework.Framework
                            && targetFramework.Version >= framework.Version)
                        {
                            return null;
                        }

                        break;
                    case ">":
                        if (framework.Framework == targetFramework.Framework
                            && targetFramework.Version > framework.Version)
                        {
                            return null;
                        }

                        break;
                    case "!=":
                        if (framework.Framework != targetFramework.Framework
                            || targetFramework.Version != framework.Version)
                        {
                            return null;
                        }

                        break;
                    case "<=":
                        if (framework.Framework == targetFramework.Framework
                            && targetFramework.Version <= framework.Version)
                        {
                            return null;
                        }

                        break;
                    case "<":
                        if (framework.Framework == targetFramework.Framework
                            && targetFramework.Version >= framework.Version)
                        {
                            return null;
                        }

                        break;
                    default:
                        if (framework.Framework == targetFramework.Framework && framework.Version == targetFramework.Version)
                            return null;
                        break;
                }
            }
            catch (Exception ex)
            {
                sink.OnMessage(new DiagnosticMessage(ex.Message + Environment.NewLine + ex.StackTrace));
                return $"Require target frameworks error: {ex.Message}";
            }
        }

        return $"Requires target frameworks: {string.Join(",", this.TargetFrameworks)}";
    }
}