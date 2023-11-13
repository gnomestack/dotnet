using System.Management.Automation;

using GnomeStack.Secrets;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsCommon.Add, "MaskedSecret")]
public class AddMaskedSecretCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    public string[] InputObject { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var secret in this.InputObject)
        {
            SecretMasker.Default.Add(secret);
        }
    }
}