using System.Buffers;
using System.IO;
using System.Text;

namespace GnomeStack.Extras.IO;

#if DFX_CORE
public
#else
internal
#endif
   static partial class TextWriterExtensions
{
    /// <summary>
    /// Writes the contents of the <paramref name="reader"/> to the <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The text writer.</param>
    /// <param name="reader">The text reader to read from.</param>
    /// <param name="bufferSize">The size of the buffer to use. Defaults to 4096.</param>
    /// <exception cref="ArgumentNullException">Thrown when the writer or reader is null.</exception>
    public static void Write(this TextWriter writer, TextReader reader, int bufferSize = -1)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (reader is null)
            throw new ArgumentNullException(nameof(reader));

        if (bufferSize < 0)
            bufferSize = 4096;

        var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
        try
        {
            var span = new Span<char>(buffer);
            int read;
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

    /// <summary>
    ///  Writes the contents of the <paramref name="stream"/> to the <paramref name="writer"/>. The the method will close
    /// the stream unless <paramref name="leaveOpen"/> is true.
    /// </summary>
    /// <param name="writer">The text writer.</param>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="encoding">The encoding to use. Defaults to UTF8.</param>
    /// <param name="bufferSize">The size of the buffer to use. Defaults to 4096.</param>
    /// <param name="leaveOpen">Instructs to leave the stream open.</param>
    /// <exception cref="ArgumentNullException">Thrown when writer or stream is null.</exception>
    public static void Write(this TextWriter writer, Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, true, bufferSize, leaveOpen);
        writer.Write(reader, bufferSize);
    }

    /// <summary>
    /// Writes the contents of the file to the <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The text writer.</param>
    /// <param name="file">The file to read from.</param>
    /// <param name="encoding">The encoding to use. Defaults to UTF8.</param>
    /// <param name="bufferSize">The size of the buffer to use. Defaults to 4096.</param>
    /// <exception cref="ArgumentNullException">Thrown when writer or file is null.</exception>
    public static void Write(this TextWriter writer, FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (file is null)
            throw new ArgumentNullException(nameof(file));

        using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, true, bufferSize, false);
        writer.Write(reader, bufferSize);
    }
}