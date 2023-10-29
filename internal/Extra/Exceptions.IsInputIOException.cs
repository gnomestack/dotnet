#pragma warning disable CS8601
namespace GnomeStack.Extras.IO;

#if DFX_CORE
public
#else
internal
#endif
   static partial class ExceptionExtensions
{
    public static bool IsInputIOException(this Exception ex)
    {
        if (ex is AggregateException aggregateException)
        {
            return aggregateException.InnerExceptions.All(IsInputIOException);
        }

        if (ex is IOException ioException)
        {
            // this occurs when a head-like process stops reading from the input before we're done writing to it
            // see http://stackoverflow.com/questions/24876580/how-to-distinguish-programmatically-between-different-ioexceptions/24877149#24877149
            // see http://msdn.microsoft.com/en-us/library/cc231199.aspx
            return unchecked((uint)ioException.HResult) == 0x8007006D;
        }

        return ex.InnerException != null && IsInputIOException(ex.InnerException);
    }
}