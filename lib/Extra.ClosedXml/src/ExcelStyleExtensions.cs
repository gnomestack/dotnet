using System.Drawing;
using System.Runtime.CompilerServices;

namespace ClosedXML.Excel;

public static class ExcelStyleExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle FormatNumber(this IXLStyle style, string format)
    {
        style.NumberFormat.SetFormat(format);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle FormatNumber(this IXLStyle style, int id)
    {
        style.NumberFormat.SetNumberFormatId(id);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle FormatDate(this IXLStyle style, string format)
    {
        style.DateFormat.SetFormat(format);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle FormatDate(this IXLStyle style, int id)
    {
        style.DateFormat.SetNumberFormatId(id);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle UseFontColor(this IXLStyle style, XLColor color)
    {
        style.Font.SetFontColor(color);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle UseFillColor(this IXLStyle style, XLColor color)
    {
        style.Fill.SetBackgroundColor(color);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle UsePatternColor(this IXLStyle style, XLColor? color = null, XLFillPatternValues? pattern = null)
    {
        if (color is not null)
            style.Fill.SetPatternColor(color);
        if (pattern is not null)
            style.Fill.SetPatternType(pattern.Value);

        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle AlignTopToBottom(this IXLStyle style, bool value = true)
    {
        style.Alignment.SetTopToBottom(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle WrapText(this IXLStyle style, bool value = true)
    {
        style.Alignment.SetWrapText(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle ShrinkToFit(this IXLStyle style, bool value = true)
    {
        style.Alignment.SetShrinkToFit(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle AlignVertical(this IXLStyle style, XLAlignmentVerticalValues value)
    {
        style.Alignment.SetVertical(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle AlignHorizontal(this IXLStyle style, XLAlignmentHorizontalValues value)
    {
        style.Alignment.SetHorizontal(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle Indent(this IXLStyle style, int value)
    {
        style.Alignment.SetIndent(value);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle UseFont(this IXLStyle style, string fontName)
    {
        style.Font.SetFontName(fontName);
        return style;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLStyle UseFontSize(this IXLStyle style, double fontSize)
    {
        style.Font.SetFontSize(fontSize);
        return style;
    }
}