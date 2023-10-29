using GnomeStack.Extras.YamlDotNet;

using YamlDotNet.Serialization;

namespace GnomeStack.Extras.YamlDotNet;

public static class SerializerBuilderExtensions
{
    public static SerializerBuilder WithStringQuotingEmitter(this SerializerBuilder builder)
        => builder.WithEventEmitter((inner) => new StringQuotingEmitter(inner));
}