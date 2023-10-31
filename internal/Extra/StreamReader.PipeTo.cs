using System.Buffers;
using System.IO;
using System.Text;

namespace GnomeStack.Extras.IO;

#if DFX_CORE
public
#else
internal
#endif
   static partial class StreamReaderExtensions
{
    public static void PipeTo(this StreamReader sr, Stream stream, int bufferSize = 0)
    {
        if (sr is null)
            throw new ArgumentNullException(nameof(sr));

        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        try
        {
            if (bufferSize < 1)
            {
                sr.BaseStream.CopyTo(stream);
            }
            else
            {
                sr.BaseStream.CopyTo(stream, bufferSize);
            }
        }
        catch (Exception ex)
        {
            if (!ex.IsInputIOException())
                throw;
        }
    }
}