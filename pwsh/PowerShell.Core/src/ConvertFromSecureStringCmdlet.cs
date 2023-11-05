using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;

namespace GnomeStack.PowerShell.Core;

[Alias("convert_from_secure_string")]
[Cmdlet(VerbsData.ConvertFrom, "SecureString")]
[OutputType(typeof(string), typeof(byte[]), typeof(char[]))]
public class ConvertFromSecureStringCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public SecureString? Secret { get; set; }

    [Parameter(Position = 0)]
    [ValidateSet("ascii", "unicode", "utf8", "utf8NoBom", "utf8Bom", "utf7", "utf32", "bigendianunicode", "bigendianutf32")]
    public string Encoding { get; set; } = "UTF8";

    [Parameter]
    public SwitchParameter AsString { get; set; }

    [Parameter]
    public SwitchParameter AsBytes { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Secret is null)
        {
            this.ThrowTerminatingError(new PSArgumentNullException(nameof(this.Secret)));
            return;
        }

        var bstr = IntPtr.Zero;
        var chars = new char[this.Secret.Length];
        try
        {
            bstr = Marshal.SecureStringToBSTR(this.Secret);
            Marshal.Copy(bstr, chars, 0, chars.Length);

            if (this.AsBytes.ToBool())
            {
                var encoding = PsEncodings.GetEncoding(this.Encoding);
                var bytes = encoding.GetBytes(chars);
                this.WriteObject(bytes, false);
                Array.Clear(chars, 0, chars.Length);
                return;
            }

            if (this.AsString.ToBool())
            {
                this.WriteObject(new string(chars));
                Array.Clear(chars, 0, chars.Length);
                return;
            }

            this.WriteObject(chars, false);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(bstr);
        }
    }
}