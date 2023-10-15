using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit;

[AttributeUsage(
    AttributeTargets.Assembly |
    AttributeTargets.Class |
    AttributeTargets.Method,
    Inherited = false)]
public sealed class UseServiceProviderFactoryAttribute : Attribute
{
    public UseServiceProviderFactoryAttribute(Type serviceProviderFactoryType)
    {
        this.FactoryType = serviceProviderFactoryType;
    }

    public Type FactoryType { get; set; }
}