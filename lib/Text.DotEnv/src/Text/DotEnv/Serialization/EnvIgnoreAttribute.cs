namespace GnomeStack.Text.DotEnv.Serialization;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class EnvIgnoreAttribute : Attribute
{
}