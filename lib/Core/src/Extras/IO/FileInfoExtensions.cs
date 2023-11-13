using System.Globalization;

namespace GnomeStack.Extras.IO;

public static class FileInfoExtensions
{
    public static string FormatByteSize(
        this long length,
        ByteMeasurement format = ByteMeasurement.Auto)
    {
        string[] sizes = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
        double l = length;
        int order = 0;
        const int interval = 1024;
        while (l >= interval && order < (sizes.Length - 1))
        {
            l /= interval;
            order++;
            if (format != ByteMeasurement.Auto && order == (int)format)
            {
                break;
            }
        }

        return string.Format(CultureInfo.InvariantCulture, "{0:0.##} {1}", l, sizes[order]);
    }

    public static string FormatFileLength(
        this FileInfo info,
        ByteMeasurement format = ByteMeasurement.Auto)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        return FormatByteSize(info.Length, format);
    }
}