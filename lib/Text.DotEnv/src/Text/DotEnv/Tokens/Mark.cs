using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnomeStack.Text.DotEnv.Tokens;

public readonly struct Mark
{
    public Mark(int lineNumber, int columnNumber)
    {
        this.LineNumber = lineNumber;
        this.ColumnNumber = columnNumber;
    }

    public static Mark Undefined { get; } = new Mark(-1, -1);

    public int LineNumber { get; }

    public int ColumnNumber { get; }
}