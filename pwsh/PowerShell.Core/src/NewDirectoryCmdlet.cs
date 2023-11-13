using System;
using System.IO;
using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "Directory")]
[Alias("Make-Directory", "new_directory")]
[OutputType(typeof(DirectoryInfo), typeof(DirectoryInfo[]))]
public class NewDirectoryCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var path in this.Path)
        {
            try
            {
                var next = Env.Expand(path);
                next = System.IO.Path.GetFullPath(next);

                if (Directory.Exists(next) || File.Exists(next))
                {
                    this.WriteVerbose($"File or Directory '{path}' already exists.");
                    continue;
                }

                this.WriteVerbose($"Creating directory '{path}'");
                Directory.CreateDirectory(path);
                this.WriteObject(new DirectoryInfo(path));
            }
            catch (Exception ex)
            {
                this.WriteError(ex, path);
            }
        }

        base.ProcessRecord();
    }
}