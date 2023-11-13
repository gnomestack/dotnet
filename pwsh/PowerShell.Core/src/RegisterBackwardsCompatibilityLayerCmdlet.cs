using System.Management.Automation;
using System.Reflection;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("register_backwards_compatibility")]
[Cmdlet(VerbsLifecycle.Register, "BackwardsCompatibilityLayer")]
public class RegisterBackwardsCompatibilityLayerCmdlet : PSCmdlet
{
    private static bool s_backwardsCompatibilityLayerRegistered;

    protected override void ProcessRecord()
    {
        if (s_backwardsCompatibilityLayerRegistered)
            return;

        s_backwardsCompatibilityLayerRegistered = true;
        var hasIsWindows = this.GetGlobalVariable("IsWindows") != null;
        var hasIsLinux = this.GetGlobalVariable("IsLinux") != null;
        var hasIsMacOS = this.GetGlobalVariable("IsMacOS") != null;
        var isCoreClr = this.GetGlobalVariable("IsCoreClr") != null;
        var hasProcess64Bit = this.GetGlobalVariable("IsProcess64Bit") != null;
        var hasOs64Bit = this.GetGlobalVariable("IsOs64Bit") != null;
        var hasCommandActionPreference = this.GetGlobalVariable("CommandActionPreference") != null;

        if (!hasIsWindows)
            this.SetVariable(new PSVariable("Global:IsWindows", Env.IsWindows, ScopedItemOptions.Constant));

        if (!hasIsLinux)
            this.SetVariable(new PSVariable("Global:IsLinux", Env.IsLinux, ScopedItemOptions.Constant));

        if (!hasIsMacOS)
            this.SetVariable(new PSVariable("Global:IsMacOs", Env.IsMacOS, ScopedItemOptions.Constant));

        if (!isCoreClr)
            this.SetVariable(new PSVariable("Global:IsCoreCLR", !hasIsWindows, ScopedItemOptions.Constant));

        if (!hasCommandActionPreference)
        {
            this.SetVariable(
                new PSVariable(
                    "Global:CommandActionPreference",
                    ActionPreference.Continue,
                    ScopedItemOptions.Constant));
        }

        if (!hasProcess64Bit)
            this.SetVariable(new PSVariable("Global:Is64BitProcess", Env.Is64BitProcess, ScopedItemOptions.Constant));

        if (!hasOs64Bit)
            this.SetVariable(new PSVariable("Global:Is64BitOs", Env.Is64BitProcess, ScopedItemOptions.Constant));

#if NETLEGACY
        var type = typeof(PSObject).Assembly.GetType("System.Management.Automation.TypeAccelerators");
        if (type is not null)
        {
#pragma warning disable S3011
            var userTypeAccelerators =
                type.GetField("userTypeAccelerators", BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore S3011

            if (userTypeAccelerators != null &&
                userTypeAccelerators.GetValue(null) is Dictionary<string, Type> dictionary &&
                !dictionary.ContainsKey("semver"))
            {
                type.GetMethod(
                        "Add",
                        BindingFlags.Public | BindingFlags.Static)
                    ?.Invoke(null, new object[] { "semver", typeof(SemanticVersion) });
            }
        }
#endif
    }
}