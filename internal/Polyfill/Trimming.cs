#if NETLEGACY
#pragma warning disable SA1649
namespace System.Diagnostics.CodeAnalysis;

[Flags]
[SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"")]
internal enum DynamicallyAccessedMemberTypes
{
    /// <summary>Specifies no members.</summary>
    None = 0,

    /// <summary>Specifies the default, parameterless public constructor.</summary>
    PublicParameterlessConstructor = 1,

    /// <summary>Specifies all public constructors.</summary>
    PublicConstructors = 3,

    /// <summary>Specifies all non-public constructors.</summary>
    NonPublicConstructors = 4,

    /// <summary>Specifies all public methods.</summary>
    PublicMethods = 8,

    /// <summary>Specifies all non-public methods.</summary>
    NonPublicMethods = 16, // 0x00000010

    /// <summary>Specifies all public fields.</summary>
    PublicFields = 32, // 0x00000020

    /// <summary>Specifies all non-public fields.</summary>
    NonPublicFields = 64, // 0x00000040

    /// <summary>Specifies all public nested types.</summary>
    PublicNestedTypes = 128, // 0x00000080

    /// <summary>Specifies all non-public nested types.</summary>
    NonPublicNestedTypes = 256, // 0x00000100

    /// <summary>Specifies all public properties.</summary>
    PublicProperties = 512, // 0x00000200

    /// <summary>Specifies all non-public properties.</summary>
    NonPublicProperties = 1024, // 0x00000400

    /// <summary>Specifies all public events.</summary>
    PublicEvents = 2048, // 0x00000800

    /// <summary>Specifies all non-public events.</summary>
    NonPublicEvents = 4096, // 0x00001000

    /// <summary>Specifies all interfaces implemented by the type.</summary>
    Interfaces = 8192, // 0x00002000

    /// <summary>Specifies all members.</summary>
    All = -1, // 0xFFFFFFFF
}

/// <summary>Indicates that certain members on a specified <see cref="T:System.Type" /> are accessed dynamically, for example, through <see cref="N:System.Reflection" />.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, Inherited = false)]

internal sealed class DynamicallyAccessedMembersAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute" /> class with the specified member types.</summary>
    /// <param name="memberTypes">The types of the dynamically accessed members.</param>
    public DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes) => this.MemberTypes = memberTypes;

    /// <summary>Gets the <see cref="T:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes" /> that specifies the type of dynamically accessed members.</summary>
    public DynamicallyAccessedMemberTypes MemberTypes { get; }
}

/// <summary>Indicates that the specified method requires dynamic access to code that is not referenced statically, for example, through <see cref="N:System.Reflection" />.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
internal sealed class RequiresUnreferencedCodeAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute" /> class with the specified message.</summary>
    /// <param name="message">A message that contains information about the usage of unreferenced code.</param>
    public RequiresUnreferencedCodeAttribute(string message)
    {
        this.Message = message;
    }

    /// <summary>Gets a message that contains information about the usage of unreferenced code.</summary>
    public string Message { get; }

    /// <summary>Gets or sets an optional URL that contains more information about the method, why it requires unreferenced code, and what options a consumer has to deal with it.</summary>
    public string? Url { get; set; }
}
#endif