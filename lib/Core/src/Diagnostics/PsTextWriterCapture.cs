using System.Diagnostics;
using System.Text;

namespace GnomeStack.Diagnostics;

public class PsTextWriterCapture : IPsCapture
{
    private readonly TextWriter writer;

    private readonly bool shouldDispose;

    public PsTextWriterCapture(TextWriter writer, bool dispose = false)
    {
        this.writer = writer;
        this.shouldDispose = dispose;
    }

    public PsTextWriterCapture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        if (bufferSize < 1)
            bufferSize = 4096;

        this.writer = new StreamWriter(file.Open(FileMode.Create, FileAccess.Write, FileShare.Read), encoding ?? Encoding.UTF8, bufferSize);
        this.shouldDispose = true;
    }

    public PsTextWriterCapture(Stream stream, Encoding? encoding, int bufferSize = -1, bool leaveOpen = false)
    {
        if (bufferSize < 1)
            bufferSize = 4096;

        this.writer = new StreamWriter(stream, encoding ?? Encoding.UTF8, bufferSize, leaveOpen);
        this.shouldDispose = true;
    }

    public void OnStart(Process process)
    {
        // do nothing
    }

    public void WriteLine(string line)
    {
        this.writer.WriteLine(line);
    }

    public void OnExit()
    {
        this.writer.Flush();

        if (this.shouldDispose)
            this.writer.Dispose();
    }
}