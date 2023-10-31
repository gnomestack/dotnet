namespace GnomeStack.Diagnostics;

internal class RubyExecutor : ShellExecutor
{
    public RubyExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("ruby", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"C:\Ruby32-x64\ruby.exe",
                @"%USERPROFILE%\Ruby32-x64\ruby.exe",
            });
        });
    }

    public override string Shell => "ruby";

    public override string Extension => ".rb";
}