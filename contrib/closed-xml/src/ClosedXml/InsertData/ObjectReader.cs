using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using GnomeStack.ComponentModel.DataAnnotations;

using ClosedXML.Attributes;

namespace GnomeStack.ClosedXml.InsertData;

internal class ObjectReader : IInsertDataReader
{
    private const BindingFlags MemberBindingFlags = BindingFlags.Public
                                                    | BindingFlags.Instance
                                                    | BindingFlags.Static;

    private readonly IReadOnlyCollection<object?> data;
    private readonly IReadOnlyList<ColumnInfo> members;

    public ObjectReader(IEnumerable data)
    {
        if (data is IReadOnlyCollection<object> list)
        {
            this.data = list;
        }
        else if (data is IEnumerable<object> enumerable)
        {
            this.data = enumerable.ToArray();
        }
        else
        {
            this.data = data.Cast<object>().ToArray();
        }

        var itemType = this.data.GetItemType();
        if (itemType is null)
            throw new ArgumentException("Unable to get the item type of the data collection", nameof(data));

        if (itemType.IsNullableType())
            itemType = itemType.GetUnderlyingType();

        this.members = this.ProcessMembers(itemType);
    }

    public IEnumerable<IEnumerable<object?>> GetData()
    {
        foreach (var item in this.data)
            yield return this.GetItemData(item);
    }

    public int GetPropertiesCount()
    {
        return this.members.Count;
    }

    public string GetPropertyName(int propertyIndex)
    {
        if (propertyIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(propertyIndex), "Property index must be non-negative");

        if (propertyIndex >= this.members.Count)
            throw new ArgumentOutOfRangeException($"{propertyIndex} exceeds the number of the object properties");

        var memberInfo = this.members[propertyIndex];

        return memberInfo.Name;
    }

    public int GetRecordsCount()
    {
        return this.data.Count;
    }

    private static Delegate CreateGetDelegate(FieldInfo fieldInfo)
    {
        if (fieldInfo.IsStatic)
        {
            var invokeGet = Expression.Field(null, fieldInfo);
            return Expression
                .Lambda(Expression.Block(invokeGet))
                .Compile();
        }
        else
        {
            if (fieldInfo.DeclaringType is null)
                throw new InvalidOperationException($"Nonstatic field {fieldInfo.Name} is missing DeclaringType");

            var oVariable = Expression.Parameter(fieldInfo.DeclaringType, "o");
            var invokeGet = Expression.Field(oVariable, fieldInfo);
            return Expression
                .Lambda(Expression.Block(invokeGet), oVariable)
                .Compile();
        }
    }

    private static Delegate CreateGetDelegate(PropertyInfo propertyInfo)
    {
        if (!propertyInfo.CanRead)
            throw new InvalidOperationException($"Property {propertyInfo.Name} prohibits reading the value.");

        var isStatic = propertyInfo.GetMethod?.IsStatic == true;

        if (isStatic)
        {
            var invokeGet = Expression.Property(null, propertyInfo);
            return Expression
                .Lambda(Expression.Block(invokeGet))
                .Compile();
        }
        else
        {
            if (propertyInfo.DeclaringType is null)
            {
                throw new InvalidOperationException(
                    $"Nonstatic property {propertyInfo.Name} is missing DeclaringType");
            }

            var indexExpressions = new List<Expression>();
            var argumentsExpression = Expression.Parameter(typeof(object?[]), "arguments");

            foreach (var parameter in propertyInfo.GetIndexParameters())
            {
                indexExpressions.Add(
                    Expression.Convert(
                        Expression.ArrayIndex(
                            argumentsExpression,
                            Expression.Constant(parameter.Position)),
                        parameter.ParameterType));
            }

            var oVariable = Expression.Parameter(propertyInfo.DeclaringType, "o");
            Expression invokeGet = indexExpressions.Count > 0
                ? Expression.Property(oVariable, propertyInfo, indexExpressions)
                : Expression.Property(oVariable, propertyInfo);
            var b = Expression.Block(invokeGet);
            return Expression
                .Lambda(b, oVariable)
                .Compile();
        }
    }

    private IReadOnlyList<ColumnInfo> ProcessMembers(Type itemType)
    {
        var members = new List<ColumnInfo>();
        var fields = itemType.GetFields(MemberBindingFlags);
        var properties = itemType.GetProperties(MemberBindingFlags);
        foreach (var field in fields)
        {
            var xlColumn = field.GetCustomAttribute<XLColumnAttribute>();
            if (xlColumn is not null)
            {
                if (xlColumn.Ignore)
                    continue;

                var name = xlColumn.Header.IsNullOrWhiteSpace() ? field.Name : xlColumn.Header;
                members.Add(
                    new ColumnInfo(name, CreateGetDelegate(field))
                    {
                        Static = field.IsStatic, Order = xlColumn.Order,
                    });

                continue;
            }

            var hidden = field.GetCustomAttribute<IgnoreAttribute>();
            if (hidden is not null)
                continue;

            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute is not null)
            {
                var name = displayAttribute.Name.IsNullOrWhiteSpace() ? field.Name : displayAttribute.Name;
                members.Add(new ColumnInfo(name!, CreateGetDelegate(field))
                {
                    Static = field.IsStatic, Order = displayAttribute.Order,
                });
                continue;
            }

            members.Add(new ColumnInfo(field.Name, CreateGetDelegate(field)) { Static = field.IsStatic, });
        }

        foreach (var prop in properties)
        {
            if (prop.GetIndexParameters().Length > 0 || prop.GetMethod is null)
                continue;

            var xlColumn = prop.GetCustomAttribute<XLColumnAttribute>();
            if (xlColumn is not null)
            {
                if (xlColumn.Ignore)
                    continue;

                var name = xlColumn.Header.IsNullOrWhiteSpace() ? prop.Name : xlColumn.Header;
                members.Add(
                    new ColumnInfo(name, CreateGetDelegate(prop))
                    {
                        Static = prop.GetMethod.IsStatic, Order = xlColumn.Order,
                    });

                continue;
            }

            var hidden = prop.GetCustomAttribute<IgnoreAttribute>();
            if (hidden is not null)
                continue;

            var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute is not null)
            {
                var name = displayAttribute.Name.IsNullOrWhiteSpace() ? prop.Name : displayAttribute.Name;
                members.Add(new ColumnInfo(name, CreateGetDelegate(prop))
                {
                    Static = prop.GetMethod.IsStatic, Order = displayAttribute.Order,
                });
                continue;
            }

            members.Add(new ColumnInfo(prop.Name, CreateGetDelegate(prop)) { Static = prop.GetMethod.IsStatic, });
        }

        members.Sort((x, y) => x.Order.CompareTo(y.Order));

        return members;
    }

    private IEnumerable<object?> GetItemData(object? item)
    {
        foreach (var t in this.members)
        {
            if (item == null)
            {
                yield return null;
                continue;
            }

            var memberInfo = t;
            if (memberInfo.Static)
                yield return memberInfo.Getter.DynamicInvoke();

            yield return memberInfo.Getter.DynamicInvoke(item);
        }
    }

    private sealed class ColumnInfo
    {
        public ColumnInfo(string name, Delegate getter)
        {
            this.Getter = getter;
            this.Name = name;
        }

        public string Name { get; }

        public int Order { get; set; }

        public Delegate Getter { get; }

        public bool Static { get; set; }
    }
}