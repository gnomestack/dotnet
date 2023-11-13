using System.Management.Automation;
using System.Text;

using GnomeStack.Diagnostics;

namespace GnomeStack.PowerShell.Core;

[Alias("new_process_capture")]
[OutputType(typeof(IPsCapture))]
[Cmdlet(VerbsCommon.New, "ProcessCapture")]
public class NewProcessCaptureCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public object InputObject { get; set; } = null!;

    protected override void ProcessRecord()
    {
        if (this.InputObject is Array)
        {
            var ex1 = new PSArgumentException(
                $"Arrays are not supported for capturing standard io for processes");

            this.WriteError(ex1);
            return;
        }

        if (this.InputObject is ICollection<string> collection)
        {
            this.WriteObject(new PsCollectionCapture(collection));
            return;
        }

        if (this.InputObject is FileInfo fi)
        {
            this.WriteObject(new PsTextWriterCapture(fi));
            return;
        }

        if (this.InputObject is TextWriter writer)
        {
            this.WriteObject(new PsTextWriterCapture(writer));
            return;
        }

        if (this.InputObject is Stream stream)
        {
            this.WriteObject(new PsTextWriterCapture(stream, Encoding.UTF8));
            return;
        }

        if (this.InputObject is ScriptBlock scriptBlock)
        {
            Action<string?> action = (s) => scriptBlock.Invoke(s);
            this.WriteObject(new PsActionCapture(action));
            return;
        }

        var ex = new PSArgumentException(
            $"{this.InputObject.GetType().FullName} is not supported for capturing standard io for processes");

        this.WriteError(ex);
    }
}