using System.Runtime.CompilerServices;

namespace ClosedXML.Excel;

public static class ExcelRangeBaseExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase FormatNumber(this IXLRangeBase range, string format)
    {
        range.Style.NumberFormat.SetFormat(format);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase FormatNumber(this IXLRangeBase range, int id)
    {
        range.Style.NumberFormat.SetNumberFormatId(id);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase FormatDate(this IXLRangeBase range, string format)
    {
        range.Style.DateFormat.SetFormat(format);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase FormatDate(this IXLRangeBase range, int id)
    {
        range.Style.DateFormat.SetNumberFormatId(id);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFontColor(this IXLRangeBase range, XLColor color)
    {
        range.Style.Font.SetFontColor(color);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFillColor(this IXLRangeBase range, XLColor color)
    {
        range.Style.Fill.SetBackgroundColor(color);
        return range;
    }

    public static IXLRangeBase Bold(this IXLRangeBase range, bool value = true)
    {
        range.Style.Font.SetBold(value);
        return range;
    }

    public static IXLRangeBase Underline(this IXLRangeBase range, XLFontUnderlineValues value = XLFontUnderlineValues.Single)
    {
        range.Style.Font.SetUnderline(value);
        return range;
    }

    public static IXLRangeBase Strikethrough(this IXLRangeBase range, bool value = true)
    {
        range.Style.Font.SetStrikethrough(value);
        return range;
    }

    public static IXLRangeBase Italicize(this IXLRangeBase range, bool value = true)
    {
        range.Style.Font.SetItalic(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UsePatternColor(this IXLRangeBase range, XLColor? color = null, XLFillPatternValues? pattern = null)
    {
        if (color is not null)
            range.Style.Fill.SetPatternColor(color);
        if (pattern is not null)
            range.Style.Fill.SetPatternType(pattern.Value);

        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase AlignTopToBottom(this IXLRangeBase range, bool value = true)
    {
        range.Style.Alignment.SetTopToBottom(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase WrapText(this IXLRangeBase range, bool value = true)
    {
        range.Style.Alignment.SetWrapText(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase ShrinkToFit(this IXLRangeBase range, bool value = true)
    {
        range.Style.Alignment.SetShrinkToFit(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase AlignVertical(this IXLRangeBase range, XLAlignmentVerticalValues value)
    {
        range.Style.Alignment.SetVertical(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase AlignHorizontal(this IXLRangeBase range, XLAlignmentHorizontalValues value)
    {
        range.Style.Alignment.SetHorizontal(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase Indent(this IXLRangeBase range, int value)
    {
        range.Style.Alignment.SetIndent(value);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFont(this IXLRangeBase range, string fontName)
    {
        range.Style.Font.SetFontName(fontName);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFontSize(this IXLRangeBase range, double fontSize)
    {
        range.Style.Font.SetFontSize(fontSize);
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFormulaA1(this IXLRangeBase range, string formula)
    {
        range.FormulaA1 = formula;
        return range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IXLRangeBase UseFormulaR1C1(this IXLRangeBase range, string formula)
    {
        range.FormulaR1C1 = formula;
        return range;
    }
}