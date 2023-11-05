using System;
using System.Collections;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsCommon.Set, "ModuleConfig")]
[OutputType(typeof(void))]
public class SetModuleConfigCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string? ModuleName { get; set; }

    [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
    public string? Query { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    public object? Value { get; set; }

    protected override void ProcessRecord()
    {
        var moduleName = this.ModuleName ?? this.MyInvocation.MyCommand.ModuleName;

        if (this.Query.IsNullOrWhiteSpace())
        {
            if (this.Value is null)
            {
                this.WriteError(new PSArgumentException("Value cannot be null when Query is null or empty."));
                return;
            }

            if (this.Value is not Hashtable ht)
            {
                this.WriteError(new PSArgumentException("-Value must be a hashtable when -Query is null or empty."));
                return;
            }

            PsModuleConfig.SetModuleConfig(moduleName, ht);
            return;
        }

        try
        {
            PsModuleConfig.SetModuleConfig(moduleName, this.Value, this.Query);
        }
        catch (Exception ex)
        {
            this.WriteError(ex);
        }
    }
}