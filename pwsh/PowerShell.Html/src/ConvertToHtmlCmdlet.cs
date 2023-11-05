using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Html;

[Alias("to_html", "as_html")]
[Cmdlet(VerbsData.ConvertTo, "Html")]
public class ConvertToHtmlCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public string InputObject { get; set; } = default!;

    protected override void ProcessRecord()
    {
        if (this.InputObject.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.InputObject)));
            return;
        }

        try
        {
            this.WriteObject(new HtmlDocument(this.InputObject));
        }
        catch (Exception ex)
        {
            this.WriteError(ex);
        }
    }
}