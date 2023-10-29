using System.Text.RegularExpressions;

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace GnomeStack.Extras.YamlDotNet;

public sealed class StringQuotingEmitter : ChainedEventEmitter
{
    // Patterns from https://yaml.org/spec/1.2/spec.html#id2804356
    private static Regex quotedRegex = new Regex(
        @"^(\~|null|true|false|on|off|yes|no|y|n|[-+]?(\.[0-9]+|[0-9]+(\.[0-9]*)?)([eE][-+]?[0-9]+)?|[-+]?(\.inf))?$`",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public StringQuotingEmitter(IEventEmitter next)
        : base(next)
    {
    }

    public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
    {
        var value = eventInfo.Source.Value;
        if (value is null)
        {
            base.Emit(eventInfo, emitter);
            return;
        }

        var typeCode = eventInfo.Source.Value != null
            ? Type.GetTypeCode(eventInfo.Source.Type)
            : TypeCode.Empty;

        switch (typeCode)
        {
            case TypeCode.Char:
                if (char.IsDigit((char)value))
                {
                    eventInfo.Style = ScalarStyle.DoubleQuoted;
                }

                break;
            case TypeCode.String:
                var val = value.ToString();
                if (val is null)
                {
                    base.Emit(eventInfo, emitter);
                    return;
                }

                if (quotedRegex.IsMatch(val))
                {
                    eventInfo.Style = ScalarStyle.DoubleQuoted;
                }
                else if (val.IndexOf('\n') > -1)
                {
                    eventInfo.Style = ScalarStyle.Literal;
                }

                break;
        }

        base.Emit(eventInfo, emitter);
    }
}