using System.Text;

namespace GnomeStack.Text.DotEnv.Tokens;

public class DotEnvReader
{
    private readonly TextReader reader;

    private readonly DotEnvSerializerOptions options;

    private readonly StringBuilder buffer = new();

    private bool eof;

    private Capture capture = Capture.None;

    private TokenKind tokenKind = TokenKind.None;

    private int lineNumber = 1;

    private int multiLineNumber = 0;

    private int multiLineColumn = 0;

    private int columnNumber = 1;

    private int tokenStartColumnNumber = -1;

    public DotEnvReader(TextReader reader, DotEnvSerializerOptions options)
    {
        this.reader = reader;
        this.options = options;
    }

    public EnvToken Current { get; protected set; } = EnvToken.None;

    public EnvTokenType Type { get; protected set; }

    public bool Read()
    {
        if (this.eof)
            return false;

        bool keyTerminated = false;
        bool valueTerminated = false;

        while (true)
        {
            var c = (char)this.reader.Read();
            if (c is char.MinValue or char.MaxValue)
            {
                this.eof = true;
                if (this.buffer.Length == 0)
                    return false;
            }

            if (this.HandleLineBreak(c))
                return true;

            this.columnNumber++;

            switch (this.tokenKind)
            {
                case TokenKind.Key:
                case TokenKind.None:
                    {
                        if (c is '#' && this.buffer.Length == 0)
                        {
                            this.tokenKind = TokenKind.Comment;
                            this.tokenStartColumnNumber = this.columnNumber;
                            continue;
                        }

                        if (c is '=')
                        {
                            // environment name can't be empty.
                            if (this.buffer.Length == 0)
                            {
                                throw new ParseException(
                                    $"The environment variable name is missing on line {this.lineNumber}");
                            }

                            this.tokenStartColumnNumber = this.columnNumber + 1;
                            this.tokenKind = TokenKind.Value;
                            this.Current = new EnvNameToken(this.buffer.ToArray(), this.lineNumber, this.columnNumber);
                            this.buffer.Clear();
                            this.Type = EnvTokenType.Name;
                            this.multiLineNumber = 1;
                            this.multiLineColumn = this.columnNumber;
                            return true;
                        }

                        // allowed name characters
                        if (char.IsLetterOrDigit(c) || c is '_')
                        {
                            // if already terminated by whitespace, then we have an error.
                            if (keyTerminated)
                            {
                                throw new ParseException(
                                    $"Unexpected character '{c}' on line {this.lineNumber}, col {this.columnNumber} after environment variable name was terminated with a space");
                            }

                            this.buffer.Append(c);
                            continue;
                        }

                        // whitespace is allowed before and after environment name.
                        if (char.IsWhiteSpace(c))
                        {
                            if (this.buffer.Length > 0)
                                keyTerminated = true;

                            continue;
                        }

                        // invalid character if we get here.
                        throw new ParseException(
                            $"Unexpected character '{c}' on line {this.lineNumber}, col {this.columnNumber} for environment variable name");
                    }

                case TokenKind.Comment:
                    {

                        this.buffer.Append(c);
                        continue;
                    }

                case TokenKind.Value:
                    {
                        // empty value after '=', so we don't know if its a multiline value or not.
                        // only run this check if we haven't already captured a value and the buffer is 0.
                        if (this.capture == Capture.None && this.buffer.Length == 0)
                        {
                            switch (c)
                            {
                                case '\t':
                                case '\n':
                                    continue;
                                case '\"':
                                    this.capture = Capture.DoubleQuote;
                                    this.multiLineNumber = this.lineNumber;
                                    this.multiLineColumn = this.columnNumber;
                                    continue;

                                case '\'':
                                    this.capture = Capture.SingleQuote;
                                    this.multiLineNumber = this.lineNumber;
                                    this.multiLineColumn = this.columnNumber;
                                    continue;
                                case '`':
                                    this.capture = Capture.Backtick;
                                    this.multiLineNumber = this.lineNumber;
                                    this.multiLineColumn = this.columnNumber;
                                    continue;
                                case '{':
                                    // if we don't allow json, then append and continue
                                    if (!this.options.AllowJson)
                                    {
                                        this.buffer.Append(c);
                                        continue;
                                    }

                                    this.buffer.Append(c);
                                    this.capture = Capture.Brackets;
                                    this.multiLineNumber = this.lineNumber;
                                    this.multiLineColumn = this.columnNumber;
                                    continue;
                                case '-':
                                    // if we don't allow yaml, then append and continue
                                    if (!this.options.AllowYaml)
                                    {
                                        this.buffer.Append(c);
                                        continue;
                                    }

                                    var j = this.reader.Peek();

                                    // next value (+1) isn't '-' so append current and continue
                                    if (j != '-')
                                    {
                                        this.buffer.Append(c);
                                        continue;
                                    }

                                    this.reader.Read();
                                    j = this.reader.Peek();

                                    // next value +2 isn't '-' so append first char, current and continue
                                    if (j != '-')
                                    {
                                        this.buffer.Append(c);
                                        this.buffer.Append('-');
                                        continue;
                                    }

                                    this.reader.Read();

                                    j = this.reader.Peek();
                                    if (j is '\r')
                                    {
                                        this.reader.Read();
                                        j = this.reader.Peek();
                                        if (j is '\n')
                                        {
                                            this.reader.Read();
                                        }
                                        else
                                        {
                                            this.buffer.Append(c, 3)
                                                .Append('\r');
                                            continue;
                                        }
                                    }
                                    else if (j is not '\n')
                                    {
                                        this.buffer.Append(c, 3);
                                        continue;
                                    }

                                    this.capture = Capture.FrontMatter;
                                    this.multiLineNumber = this.lineNumber;
                                    this.multiLineColumn = this.columnNumber;
                                    continue;

                                default:
                                    if (char.IsWhiteSpace(c))
                                        continue;

                                    this.buffer.Append(c);
                                    continue;
                            }
                        }

                        // single line value
                        if (this.capture is Capture.None)
                        {
                            if (this.eof)
                            {
                                this.tokenKind = TokenKind.None;
                                var slice = this.buffer.ToArray();
                                this.buffer.Clear();
                                this.VisitScalarNode(slice);
                                return true;
                            }

                            // if we've already terminated by whitespace.
                            if (valueTerminated)
                            {
                                // if we get a comment, then we're done.
                                if (c is '#')
                                {
                                    this.tokenKind = TokenKind.Comment;
                                    this.tokenStartColumnNumber = this.columnNumber;
                                    var slice = this.buffer.ToArray();
                                    this.buffer.Clear();
                                    this.VisitScalarNode(slice);
                                    return true;
                                }

                                // whitespace is allowed after value;
                                if (char.IsWhiteSpace(c))
                                {
                                    continue;
                                }

                                throw new ParseException(
                                    $"Unexpected character {c} after environment variable value was terminated with whitespace");
                            }

                            // terminate value
                            if (char.IsWhiteSpace(c) && this.buffer.Length > 0)
                            {
                                valueTerminated = true;
                                continue;
                            }

                            // append if not whitespace
                            this.buffer.Append(c);
                            continue;
                        }

                        // if the multiline capture is terminated, then we're done.
                        if (this.HandleMultiLineCapture(c))
                            return true;

                        if (this.buffer.Length == 0 && char.IsWhiteSpace(c))
                            continue;

                        this.buffer.Append(c);
                        continue;
                    }
            }
        }
    }

    private bool HandleMultiLineCapture(char c)
    {
        switch (this.capture)
        {
            case Capture.None:
                return false;

            case Capture.DoubleQuote:
                if (c == '"')
                {
                    if (this.buffer.Length > 0 && this.buffer[^1] == '\\')
                    {
                        this.buffer.Remove(this.buffer.Length - 1, 1);
                        break;
                    }

                    this.capture = Capture.None;
                    this.tokenKind = TokenKind.None;
                    var slice = this.buffer.ToArray();
                    this.buffer.Clear();
                    this.Current = new EnvStringToken(slice, this.lineNumber, this.columnNumber);
                    this.Type = EnvTokenType.String;
                    return true;
                }

                break;

            case Capture.SingleQuote:
                if (c == '\'')
                {
                    if (this.buffer.Length > 0 && this.buffer[^1] == '\\')
                    {
                        this.buffer.Remove(this.buffer.Length - 1, 1);
                        break;
                    }

                    this.capture = Capture.None;
                    this.tokenKind = TokenKind.None;
                    this.Current = new EnvStringToken(
                        this.buffer.ToArray(),
                        this.lineNumber,
                        this.columnNumber);
                    this.buffer.Clear();
                    this.Type = EnvTokenType.String;
                    return true;
                }

                break;

            case Capture.Backtick:
                if (c == '`')
                {
                    // escaped backtick
                    if (this.buffer.Length > 0 && this.buffer[^1] == '\\')
                    {
                        this.buffer.Remove(this.buffer.Length - 1, 1);
                        break;
                    }

                    this.capture = Capture.None;
                    this.tokenKind = TokenKind.None;
                    this.Current = new EnvStringToken(
                        this.buffer.ToArray(),
                        this.lineNumber,
                        this.columnNumber);
                    this.buffer.Clear();
                    this.Type = EnvTokenType.String;
                    return true;
                }

                break;

            case Capture.Brackets:
                if (c == '}' && this.buffer.Length > 0 && this.buffer[^1] == '\n')
                {
                    this.buffer.Append(c);
                    this.capture = Capture.None;
                    this.tokenKind = TokenKind.None;
                    this.Current = new EnvJsonToken(
                        this.buffer.ToArray(),
                        this.lineNumber,
                        this.columnNumber);
                    this.buffer.Clear();
                    this.Type = EnvTokenType.Json;
                    return true;
                }

                break;

            case Capture.FrontMatter:
                // likely yaml.
                if (c == '-' && (this.buffer.Length != 0 && this.buffer[^1] == '\n') && this.reader.Peek() == '-')
                {
                    var d = (char)this.reader.Read();

                    // since its not yaml, then append read character and continue.
                    if (this.reader.Peek() != '-')
                    {
                        this.buffer.Append(d);
                        return false;
                    }

                    // consume last dash
                    this.reader.Read();

                    // peek for end of line or end of file.
                    d = (char)this.reader.Read();
                    while (d is not char.MinValue and not char.MaxValue and not '\n')
                    {
                        d = (char)this.reader.Read();
                    }

                    this.capture = Capture.None;
                    this.tokenKind = TokenKind.None;
                    this.Current = new EnvYamlToken(
                        this.buffer.ToArray(),
                        this.lineNumber,
                        this.columnNumber);
                    this.buffer.Clear();
                    this.Type = EnvTokenType.Yaml;
                    return true;
                }

                break;
        }

        return false;
    }

    private bool HandleLineBreak(char c)
    {
        // if we're capturing a multi-line string, then we need to handle line breaks differently.
        if (this.capture != Capture.None)
        {
            if (c is char.MinValue or char.MaxValue)
            {
                throw new ParseException("Unexpected end of file while capturing multi-line string");
            }

            return false;
        }

        bool lineBreak = false;
        if (c is '\n' or char.MaxValue or char.MinValue)
        {
            lineBreak = true;
        }
        else if (c is '\r' && this.reader.Peek() == '\n')
        {
            this.reader.Read();
            lineBreak = true;
        }

        // no line break, continue;
        if (!lineBreak)
            return false;

        // line break increment counters.
        this.lineNumber++;
        this.columnNumber = 1;
        this.multiLineNumber++;
        this.multiLineColumn++;

        // if the buffer is empty, we haven't read a token, so return false to continue;
        if (this.buffer.Length == 0)
        {
            return false;
        }

        var slice = this.buffer.ToArray();
        this.buffer.Clear();

        switch (this.tokenKind)
        {
            case TokenKind.Key:
            case TokenKind.None:
                if (this.Current is EnvNameToken)
                {
                    throw new ParseException($"Unexpected {new string(slice)} token on {this.lineNumber} at {this.columnNumber}, Missing value or assigment token '='");
                }

                this.Current = new EnvNameToken(slice, this.lineNumber, this.columnNumber);

                this.Type = EnvTokenType.Name;
                this.tokenKind = TokenKind.None;
                return true;
            case TokenKind.Comment:
                this.Current = new EnvCommentToken(slice, this.lineNumber, this.columnNumber);

                this.Type = EnvTokenType.Comment;
                this.tokenKind = TokenKind.None;
                return true;
            default:
                if (this.Current is EnvScalarToken)
                {
                    throw new ParseException($"Unexpected scalar token on {this.lineNumber} at {this.columnNumber}, missing name");
                }

                this.VisitScalarNode(slice);
                this.tokenKind = TokenKind.None;
                return true;
        }
    }

    private void VisitScalarNode(char[] slice)
    {
        if (EnvNullToken.TryParse(slice, this.lineNumber, this.columnNumber, out var nullToken)
            && nullToken is not null)
        {
            this.Current = nullToken;
            this.Type = EnvTokenType.Null;
            return;
        }

        if (EnvBooleanToken.TryParse(slice, this.lineNumber, this.columnNumber, out var booleanToken)
            && booleanToken is not null)
        {
            this.Current = booleanToken;
            this.Type = EnvTokenType.Boolean;
            return;
        }

        if (EnvNumberToken.TryParse(slice, this.lineNumber, this.columnNumber, out var numberToken)
            && numberToken is not null)
        {
            this.Current = numberToken;
            this.Type = EnvTokenType.Number;
            return;
        }

        this.Current = new EnvStringToken(slice, this.lineNumber, this.columnNumber);
        this.Type = EnvTokenType.String;
    }
}