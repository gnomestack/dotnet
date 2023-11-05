using System.Collections;
using System.Management.Automation;
using System.Text.Json;

using Path = System.IO.Path;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsData.Save, "ModuleConfig")]
[OutputType(typeof(void))]
public class SaveModuleConfigCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string? ModuleName { get; set; }

    [Parameter(ValueFromPipeline = true)]
    public object? Config { get; set; }

    [Parameter]
    public string? Path { get; set; }

    protected override void ProcessRecord()
    {
        Hashtable? config = null;
        if (this.Config is PSObject)
        {
            var converted = ConvertToHashtableCmdlet.Convert(this.Config);
            if (converted is Hashtable convertedHt)
                config = convertedHt;
        }
        else if (this.Config is Hashtable ht)
        {
            config = ht;
        }
        else if (this.Config is IDictionary dictionary)
        {
            config = new Hashtable(dictionary);
        }

        var moduleName = this.ModuleName ?? this.MyInvocation.MyCommand.ModuleName;
        PsModuleConfig.SaveModuleConfig(moduleName, this.Path, config);
    }
}