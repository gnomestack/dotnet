using System;
using System.IO;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Core;

[Alias("is_directory")]
[Cmdlet(VerbsDiagnostic.Test, "Directory")]
[OutputType(typeof(bool))]
public class TestDirectoryCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "fi")]
    public object? InputObject { get; set; }

    protected override void ProcessRecord()
    {
        try
        {
            if (this.InputObject is null)
            {
                this.WriteObject(false);
                return;
            }

            if (this.InputObject is string path)
            {
                try
                {
                    if (path.IsNullOrWhiteSpace())
                    {
                        this.WriteObject(false);
                        return;
                    }

                    path = System.IO.Path.GetFullPath(path);
                    var attr = File.GetAttributes(path);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        this.WriteObject(true);
                        return;
                    }

                    this.WriteObject(false);
                    return;
                }
                catch
                {
                    this.WriteObject(false);
                    return;
                }
            }

            if (this.InputObject is FileInfo fi)
            {
                this.WriteObject(fi.Attributes.HasFlag(FileAttributes.Directory));
                return;
            }

            if (this.InputObject is DirectoryInfo)
            {
                this.WriteObject(true);
                return;
            }

            if (this.InputObject is FileSystemInfo fsi)
            {
                this.WriteObject(fsi.Attributes.HasFlag(FileAttributes.Directory));
                return;
            }

            this.WriteObject(false);
        }
        catch (Exception ex)
        {
            this.WriteError(ex, this.InputObject);
            this.WriteObject(false);
        }
    }
}