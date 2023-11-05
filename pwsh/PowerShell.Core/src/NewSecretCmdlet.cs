using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;

using GnomeStack.Extras.Strings;
using GnomeStack.Secrets;

namespace GnomeStack.PowerShell.Core;

[Alias("new_secret")]
[Cmdlet(VerbsCommon.New, "Secret")]
[OutputType(typeof(string), typeof(char[]), typeof(SecureString), typeof(byte[]))]
public class NewSecretCmdlet : PSCmdlet
{
    public int Length { get; set; } = 16;

    [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "CharacterList")]
    public IEnumerable<char>? CharacterList { get; set; }

    [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
    public ScriptBlock? Validator { get; set; } = null!;

    [Parameter]
    public SwitchParameter AsString { get; set; }

    [Parameter]
    public SwitchParameter AsBytes { get; set; }

    [Parameter]
    public SwitchParameter AsSecureString { get; set; }

    protected override void ProcessRecord()
    {
        var pg = new SecretGenerator();

        if (this.CharacterList is not null)
        {
            var list = this.CharacterList.ToList();
            if (list.Count == 0)
            {
                pg.AddDefaults();
            }
            else
            {
                pg.Add(list);
            }
        }
        else
        {
            pg.AddDefaults();
        }

        if (this.Validator is not null)
        {
            pg.SetValidator((chars) =>
            {
                var r = this.Validator.Invoke(chars);
                if (r.Count == 1)
                {
                    return r[0].BaseObject is bool and true;
                }

                return false;
            });
        }

        if (this.AsString.ToBool())
        {
            this.WriteObject(pg.GenerateAsString(this.Length));
            return;
        }

        if (this.AsBytes.ToBool())
        {
            var bytes = pg.GenerateAsBytes(this.Length);
            this.WriteObject(bytes, false);
            return;
        }

        if (this.AsSecureString.ToBool())
        {
            this.WriteObject(pg.GenerateAsSecureString(this.Length));
            return;
        }

        var secret = pg.Generate(this.Length);
        this.WriteObject(secret, true);
    }
}