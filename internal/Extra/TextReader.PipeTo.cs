using System.Buffers;
using System.IO;
using System.Text;

namespace GnomeStack.Extras.IO;

#if DFX_CORE
public
#else
internal
#endif
   static partial class TextReaderExtensions
{
    public static void PipeTo(
        this TextReader reader,
        TextWriter writer,
        int bufferSize = -1)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (bufferSize == -1)
            bufferSize = 4096;

        var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
        try
        {
            int read;
            var span = new Span<char>(buffer);

            while ((read = reader.Read(span)) > 0)
            {
                writer.Write(span.Slice(0, read));
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsInputIOException())
                throw;
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }

    public static void PipeTo(
        this TextReader reader,
        Stream stream,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        using var sw = new StreamWriter(stream, encoding ?? Encoding.UTF8, bufferSize, leaveOpen);
        reader.PipeTo(sw, bufferSize);
    }

    public static void PipeTo(
        this TextReader reader,
        FileInfo file,
        Encoding? encoding = null,
        int bufferSize = -1)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
        reader.PipeTo(stream, encoding, bufferSize, false);
    }

    public static void PipeTo(
        this TextReader reader,
        ICollection<string> lines)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        try
        {
            while (reader.ReadLine() is { } line)
            {
                lines.Add(line);
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsInputIOException())
                throw;
        }
    }

    public static Task PipeToAsync(
        this TextReader reader,
        FileInfo file,
        Encoding? encoding = null,
        int bufferSize = -1,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        return InnerPipeToAsync(reader, file, encoding, bufferSize, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        ICollection<string> lines,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        return InnerPipeToAsync(reader, lines, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        TextWriter writer,
        int bufferSize = -1,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        return InnerPipeToAsync(reader, writer, bufferSize, cancellationToken);
    }

    public static Task PipeToAsync(
        this TextReader reader,
        Stream stream,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false,
        CancellationToken cancellationToken = default)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        return InnerPipeToAsync(reader, stream, encoding, bufferSize, leaveOpen, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        TextWriter writer,
        int bufferSize,
        CancellationToken cancellationToken)
    {
        if (bufferSize == -1)
            bufferSize = 4096;

        char[] buffer = ArrayPool<char>.Shared.Rent(bufferSize);
        try
        {
            int read;
            var memory = new Memory<char>(buffer);

            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            while ((read = await reader.ReadAsync(memory, cancellationToken)) > 0)
            {
                await writer.WriteAsync(memory.Slice(0, read), cancellationToken);
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsInputIOException())
                throw;
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        Stream stream,
        Encoding? encoding = null,
        int bufferSize = -1,
        bool leaveOpen = false,
        CancellationToken cancellationToken = default)
    {
#if NETLEGACY
        using var sw = new StreamWriter(stream, encoding ?? Encoding.UTF8, bufferSize, leaveOpen);
#else
        await using var sw = new StreamWriter(stream, encoding, bufferSize, leaveOpen);
#endif
        await reader.PipeToAsync(sw, bufferSize, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        FileInfo file,
        Encoding? encoding,
        int bufferSize,
        CancellationToken cancellationToken)
    {
#if NETLEGACY
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
#else
        await using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);
#endif
        await reader.PipeToAsync(stream, encoding, bufferSize, false, cancellationToken);
    }

    private static async Task InnerPipeToAsync(
        TextReader reader,
        ICollection<string> lines,
        CancellationToken cancellationToken)
    {
        try
        {
#if !NET7_0_OR_GREATER
            while (await reader.ReadLineAsync().ConfigureAwait(false) is { } line)
            {
                lines.Add(line);
            }
#else
            while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
            {
                lines.Add(line);
            }
#endif
        }
        catch (Exception ex)
        {
            if (!ex.IsInputIOException())
                throw;
        }
    }
}