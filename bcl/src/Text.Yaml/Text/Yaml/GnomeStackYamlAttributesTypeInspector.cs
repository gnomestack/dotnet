using GnomeStack.Text.Serialization;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace GnomeStack.Text.Yaml;

/// <summary>
/// Applies the Yaml* attributes to another <see cref="ITypeInspector"/>.
/// </summary>
public sealed class GnomeStackYamlAttributesTypeInspector : TypeInspectorSkeleton
{
    private readonly ITypeInspector innerTypeDescriptor;

    public GnomeStackYamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
    {
        this.innerTypeDescriptor = innerTypeDescriptor;
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
    {
        var props = this.innerTypeDescriptor.GetProperties(type, container);
        var list = new List<PropertyDescriptor>();

        // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        foreach (var prop in props)
        {
            if (prop.GetCustomAttribute<YamlIgnoreAttribute>() is not null ||
                prop.GetCustomAttribute<IgnoreAttribute>() is not null)
            {
                continue;
            }

            var descriptor = new PropertyDescriptor(prop);
            var memberAttr = prop.GetCustomAttribute<YamlMemberAttribute>();
            if (memberAttr is not null)
            {
                if (memberAttr.SerializeAs != null)
                {
                    descriptor.TypeOverride = memberAttr.SerializeAs;
                }

                descriptor.Order = memberAttr.Order;
                descriptor.ScalarStyle = memberAttr.ScalarStyle;

                if (memberAttr.Alias != null)
                {
                    descriptor.Name = memberAttr.Alias;
                }

                list.Add(descriptor);
                continue;
            }

            var serializationAttr = prop.GetCustomAttribute<SerializationAttribute>();
            if (serializationAttr is not null)
            {
                if (serializationAttr.Order > 0)
                    descriptor.Order = serializationAttr.Order;

                if (serializationAttr.Name is not null)
                    descriptor.Name = serializationAttr.Name;
            }

            list.Add(descriptor);
        }

        list.Sort((x, y) => x.Order.CompareTo(y.Order));
        return list;
    }
}