using System.Management.Automation;

using GnomeStack.Extras;
using GnomeStack.Extras.IO;

namespace GnomeStack.PowerShell.Core;

[OutputType(typeof(string))]
[Cmdlet(VerbsCommon.Format, "FileLength")]
public class FormatFileLengthCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "length")]
    public long Length { get; set; }

    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "fi")]
    public FileInfo? FileInfo { get; set; }

    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "length")]
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "fi")]
    [ValidateSet("Auto", "B", "KB", "MB", "GB", "TB", "PB")]
    public string Format { get; set; } = "Auto";

    public static string FormatBytes(long size, string format)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = size;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;

            if (!format.Equals("Auto", StringComparison.OrdinalIgnoreCase) && sizes[order] == format)
            {
                break;
            }
        }

        return $"{len:0.##} {sizes[order]}";
    }

    protected override void ProcessRecord()
    {
        if (this.FileInfo != null)
        {
            this.Length = this.FileInfo.Length;
        }

        if (Enum.TryParse(this.Format, true, out ByteMeasurement format))
        {
            this.WriteObject(this.Length.FormatByteSize(format));
            return;
        }

        this.WriteObject(FormatBytes(this.Length, this.Format.ToUpperInvariant()));
    }
}