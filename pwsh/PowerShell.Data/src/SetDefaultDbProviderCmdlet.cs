using System.Management.Automation;

namespace GnomeStack.PowerShell.Data;

[Alias("set_default_db_provider")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Set, "DefaultDbProvider")]
public class SetDefaultDbProviderCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true, Position = 0, Mandatory = true)]
    public string Name { get; set; } = null!;

    protected override void ProcessRecord()
    {
        if (ProviderRegistry.Get(this.Name) is null)
        {
            var msg = $"The provider {this.Name} was not found";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        ProviderRegistry.DefaultProviderName = this.Name;
    }
}